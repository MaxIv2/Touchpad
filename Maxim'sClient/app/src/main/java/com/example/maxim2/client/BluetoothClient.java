package com.example.maxim2.client;

import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothSocket;
import android.util.Log;
import android.widget.Toast;

import java.io.IOException;
import java.util.Set;
import java.util.UUID;
import java.util.concurrent.ConcurrentLinkedQueue;

/**
 * Created by Maxim2 on 4/4/2018.
 */

public class BluetoothClient extends Client {

    private final UUID bluetoothUUID = UUID.randomUUID().fromString("8c30dbc1-60b3-4cef-a04d-39de0275d914");
    private BluetoothSocket socket;
    private BluetoothDevice serverDevice;

    public BluetoothClient(String address, SessionEndEventHandler handler) throws Exception {
        super(handler);
        BluetoothAdapter adapter =BluetoothAdapter.getDefaultAdapter();
        while(!adapter.isEnabled())
            adapter.enable();
        this.serverDevice = adapter.getRemoteDevice(address);
        if(!isPaired(this.serverDevice)) {
            if(!tryToPair(this.serverDevice)) {
                Log.d("BClient", "Couldn't pair");
                this.endSession(false);
                throw new Exception("Failed to connect");
            }
        }
        try {
            this.socket = this.serverDevice.createRfcommSocketToServiceRecord(this.bluetoothUUID);
            this.socket.connect();
        } catch (IOException e) {
            Log.d("BClient", "Something went wrong with socket");
            this.endSession();
            throw new Exception("Failed to connect");
        }
        this.commands = new ConcurrentLinkedQueue<Byte>();
        this.setTimers();
    }

    private static boolean tryToPair(BluetoothDevice device) {
        return device.createBond();
    }

    private static boolean isPaired(BluetoothDevice device) {
        Set<BluetoothDevice> paired = BluetoothAdapter.getDefaultAdapter().getBondedDevices();
        for (BluetoothDevice pairedDevice: paired) {
            if (pairedDevice.getAddress().equals(device.getAddress()))
                return true;
        }
        return false;
    }

    @Override
    protected void send(byte[] buffer) {
        try {
            this.socket.getOutputStream().write(buffer,0, buffer.length);
        } catch (IOException e) {
            Log.d("BClient", "Failed to send message, probably disconnected");
            this.endSession();
        }
    }

    @Override
    protected void receive(byte[] buffer) {
        try {
            this.socket.getInputStream().read(buffer);
        } catch (IOException e) {
            Log.d("BClient", "Failed to read message, probably disconnected");
            this.endSession();
        }
    }

    @Override
    protected void closeConnection() {
        try {
            this.socket.close();
        } catch (IOException e) {
            Log.d("BClient", "Close falied");
            this.endSession(false);
        }
    }

    @Override
    protected int getAvailableDataCount() {
        try {
            return this.socket.getInputStream().available();
        } catch (IOException e) {
            this.endSession(false);
            return 0;
        }
    }
}
