package com.example.maxim2.client;

import android.util.Log;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.InetAddress;
import java.net.Socket;
import java.net.SocketAddress;
import java.net.SocketException;
import java.util.concurrent.ConcurrentLinkedQueue;

/**
 * Created by Maxim2 on 4/4/2018.
 */

public class TcpClient extends Client {
    private Socket clientSocket;
    private OutputStream outputStream;
    private InputStream inputStream;
    Object lock = new Object();


    public TcpClient(final String ip, final int port, SessionEndEventHandler handler) throws Exception {
        super(handler);
        Thread t = new Thread(new Runnable() {
            @Override
            public void run() {
                try {
                    clientSocket = new Socket(InetAddress.getByName(ip), port);
                    outputStream = clientSocket.getOutputStream();
                    inputStream = clientSocket.getInputStream();
                    synchronized (lock){
                        lock.notifyAll();
                    }
                } catch (IOException e) {
                    Log.d("TClient: ", "failed to connect");
                    e.printStackTrace();
                    synchronized (lock){
                        lock.notifyAll();
                    }
                }
            }
        });
        t.start();
        synchronized (lock) {
            lock.wait();
        }
        if (clientSocket != null) {
            if(!clientSocket.isConnected()) {
                Log.d("TClient: ", "failed to connect");
                throw new Exception("nope");
            }
        }
        else {
            throw new Exception("nope");
        }
        this.commands = new ConcurrentLinkedQueue<>();
        this.setTimers();
    }

    @Override
    protected void send(byte[] buffer) {
        try {
            this.outputStream.write(buffer);
        } catch (IOException e) {
            Log.d("TClient", "failed to send");
            this.endSession();
        }
    }

    @SuppressWarnings("ResultOfMethodCallIgnored")
    @Override
    protected void receive(byte[] buffer) {
        try {
            this.inputStream.read(buffer);
        } catch (IOException e) {
            Log.d("TClient", "failed to receive");
            this.endSession();
        }
    }

    @Override
    protected void closeConnection() {
        try {
            this.clientSocket.close();
        } catch (IOException e) {
            Log.d("TClient", "failed to close");
            this.endSession();
        }
    }

    @Override
    protected int getAvailableDataCount() {
        try {
            return this.inputStream.available();
        } catch (IOException e) {
            Log.d("TClient", "failed to get available");
            this.endSession();
            return 0;
        }
    }

}
