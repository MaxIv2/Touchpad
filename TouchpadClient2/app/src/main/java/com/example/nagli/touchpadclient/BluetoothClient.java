package com.example.nagli.touchpadclient;


import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothSocket;
import android.util.Log;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.Timer;
import java.util.TimerTask;
import java.util.UUID;

public class BluetoothClient {
    private String serverMACAddress;
    private final UUID uuid = UUID.fromString("6898db70-e890-4609-b2c4-d55c1cb40c99");
    private BluetoothDevice device;
    private BluetoothSocket client;
    private BluetoothAdapter adapter;
    private OutputStream output;
    private InputStream input;
    private Timer connectivityChecker;

    public BluetoothClient(String serverMACAddress) throws IOException {
        this.serverMACAddress = serverMACAddress;
        this.adapter = BluetoothAdapter.getDefaultAdapter();
        this.device = adapter.getRemoteDevice(serverMACAddress);
        this.client = device.createRfcommSocketToServiceRecord(uuid);
        this.client.connect();
        this.input = this.client.getInputStream();
        this.output = client.getOutputStream();
        setTimer();
    }

    private void setTimer() {
        this.connectivityChecker = new Timer();
        connectivityChecker.schedule(checkConnection, 0, 5000);
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

    TimerTask checkConnection = new TimerTask() {
        @Override
        public void run() {
            Log.println(Log.INFO, "hi", "check connection");
        }
    };
}
