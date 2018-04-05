package com.example.maxim2.client;

import android.util.Log;

import java.util.Queue;
import java.util.Timer;
import java.util.TimerTask;

/**
 * Created by Maxim2 on 4/4/2018.
 */

public abstract class Client {
    private enum MessageCode { CHECK, ACK, END;
        public byte asByte() {
            switch (this) {
                case CHECK:
                    return 1;
                case ACK:
                    return 2;
                default://END
                    return 3;
            }
        }
        public static MessageCode toEnum(byte b) throws Exception {
            switch (b) {
                case 1:
                    return CHECK;
                case 2:
                    return ACK;
                case 3:
                    return END;
                default:
                    throw new Exception("not me");
            }
        }
    }

    protected SessionEndEventHandler sessionEndHandler;
    Queue<Byte> commands;
    private Timer messageChecker;
    private Timer connectionCheckQuerySender;
    private Timer commandSender;
    private boolean waitingForAnAck = false;
    public boolean sessionEnded;

    protected Client(SessionEndEventHandler handler) {
        this.waitingForAnAck = false;
        this.sessionEndHandler = handler;
        this.sessionEnded = false;
    }

    public void addToQueue(byte[] commandData) {
        if (commandData.length + commands.size() >= 255)
            this.sendCommands();
        for (byte b: commandData) {
            commands.add(b);
        }
    }

    private void sendCommands() {
        if(commands.size() == 0)
            return;
        byte[] buffer = new byte[commands.size() + 2];
        buffer[0] = 0;
        buffer[1] = (byte)commands.size();
        int i = 2;
        for(;i< buffer.length; i++){
            buffer[i] = commands.remove();
        }
        this.send(buffer);
    }

    protected void setTimers() {
        this.setMessageChecker();
        this.setCommandSender();
        this.setConnectionCheckQuerySender();
    }

    private void setConnectionCheckQuerySender() {
        TimerTask sendCheckQuery = new TimerTask() {
            @Override
            public void run() {
                if (waitingForAnAck)
                    endSession();
                else
                    sendConnectionCheck();
            }
        };
        this.connectionCheckQuerySender = new Timer();
        this.connectionCheckQuerySender.schedule(sendCheckQuery, 0, 5000);
    }
    private void setCommandSender() {
        TimerTask sendCommands = new TimerTask() {
            @Override
            public void run() {
                sendCommands();
            }
        };
        this.commandSender = new Timer();
        this.commandSender.schedule(sendCommands, 0, 1);
    }
    private void setMessageChecker(){
        TimerTask checkForMessages = new TimerTask() {
            @Override
            public void run() {
                byte[] data = new byte[getAvailableDataCount()];
                receive(data);
                int index = 0;
                while (index < data.length) {
                    try {
                        MessageCode code = MessageCode.toEnum(data[index]);
                        switch (code) {
                            case CHECK:
                                sendAcknowledgement();
                                Log.d("Client", "sending ack...");
                                break;
                            case ACK:
                                waitingForAnAck = false;
                                break;
                            default://END
                                closeConnection();
                                onSessionEnd();
                                break;
                        }
                        index++;
                    } catch (Exception e) {
                        //ah oh
                    }
                    index++;
                }
            }
        };
        this.messageChecker = new Timer();
        this.messageChecker.schedule(checkForMessages, 0, 2500);
    }

    private void sendTerminationMessage() {
        this.send(new byte[]{MessageCode.END.asByte(),0});
    }
    private void sendAcknowledgement() {
        this.send(new byte[]{MessageCode.ACK.asByte(),0});
    }
    private void sendConnectionCheck() {
        this.send(new byte[]{MessageCode.CHECK.asByte(), 0});
    }

    protected abstract void send(byte[] buffer);
    protected abstract void receive(byte[] buffer);
    protected abstract void closeConnection();
    protected abstract int getAvailableDataCount();

    private void onSessionEnd(){
        sessionEndHandler.HandleSessionEnd();
    }
    public void endSession() {
        if(this.sessionEnded)
            return;
        this.sessionEnded = true;
        this.sendTerminationMessage();
        this.connectionCheckQuerySender.cancel();
        this.connectionCheckQuerySender.purge();
        this.messageChecker.cancel();
        this.messageChecker.purge();
        this.commandSender.cancel();
        this.commandSender.purge();
        this.closeConnection();
        this.onSessionEnd();
    }

    public interface SessionEndEventHandler {
        void HandleSessionEnd();
    }

}
