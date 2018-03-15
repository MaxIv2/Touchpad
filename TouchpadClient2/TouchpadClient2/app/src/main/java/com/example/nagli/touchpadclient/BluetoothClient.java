package com.example.nagli.touchpadclient;


import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothSocket;
import android.util.Log;

import java.util.Queue;
import java.util.Timer;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.TimerTask;
import java.util.UUID;
import java.util.concurrent.ConcurrentLinkedQueue;

public class BluetoothClient {
    private String serverMACAddress;
    private final UUID uuid = UUID.fromString("8c30dbc1-60b3-4cef-a04d-39de0275d914");
    private BluetoothDevice device;
    private BluetoothSocket client;
    private BluetoothAdapter adapter;
    private OutputStream output;
    private InputStream input;
    private Timer connectivityChecker;
    private Queue<Byte> bmessage;
    private Timer messagetimer,ACKtimer;
    private boolean waitForACK;

    public BluetoothClient(String serverMACAddress) throws IOException {
        this.serverMACAddress = serverMACAddress;
        this.adapter = BluetoothAdapter.getDefaultAdapter();
        this.device = adapter.getRemoteDevice(serverMACAddress);
        this.client = device.createRfcommSocketToServiceRecord(uuid);
        this.client.connect();
        this.input = this.client.getInputStream();
        this.output = client.getOutputStream();
        this.bmessage = new ConcurrentLinkedQueue<>();
    }

    public void SetMessage(byte [] buffer) {
        if (bmessage.size() + buffer.length >= 255)
            SendBmessageNow();
        String s = "{ ";
        for (byte b:buffer ) {
            bmessage.add(b);
            s += b + " ";
        }
        Log.println(Log.INFO,"message", s);
    }

    public void SendBmessageNow() {
        if(bmessage.size() == 0)
            return;
        byte[] buffer = new byte[bmessage.size()+2];
        buffer[0] = 0;
        buffer[1] = (byte) bmessage.size();
        for (int i = 2;i<buffer.length;i++)
        {
            buffer[i]=bmessage.remove();
        }
        Log.println(Log.INFO,"send", "Length: " + buffer.length);
        sendData(buffer);
    }

    public void SetMessageTimer() throws  InterruptedException
    {
        TimerTask task = new TimerTask() {
            @Override
            public void run() {
                SendBmessageNow();
            }
        };
        messagetimer = new Timer();
        messagetimer.scheduleAtFixedRate(task,0,25);
    }
    public void SetConnectivityCheckerTimer() throws InterruptedException {
        final byte[] sendbuffer = {1, 0};
        waitForACK = false;
        TimerTask task = new TimerTask() {
            @Override
            public void run() {
                if (waitForACK == false) {
                    //sendData(sendbuffer);
                    waitForACK = true;
                }
            }
        };
        connectivityChecker = new Timer();
        connectivityChecker.schedule(task, 0, 5000);
    }

    public void SetACKtimer() throws InterruptedException {//+check data
        final byte[] ACKbuffer = {2, 0};
        final byte[] getbuffer = new byte[2];
        TimerTask task = new TimerTask() {
            @Override
            public void run() {
                receiveData(getbuffer);
                if (getbuffer[0] != 0)
                    Log.d("getbuffer[0]:", getbuffer[0] + "");
                if (getbuffer[0] == 1)
                {
                    sendData(ACKbuffer);
                }
                else if (getbuffer[0] == 3)
                {
                    close();
                }
                else if (getbuffer[0] == 2)
                {
                    waitForACK =false;
                }
            }
        };
        ACKtimer = new Timer();
        ACKtimer.scheduleAtFixedRate(task, 0, 500);
    }

    public void sendData(byte[] buffer) {
        try {
            this.output.write(buffer, 0, buffer.length);
            this.output.flush();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public void receiveData(byte[] buffer) {
        try {
            this.input.read(buffer, 0, buffer.length);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
    public void close(){
        messagetimer.cancel();
        messagetimer.purge();
        ACKtimer.cancel();
        ACKtimer.purge();
        connectivityChecker.cancel();
        connectivityChecker.purge();
        try {
            this.output.close();
            this.input.close();
            this.client.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
