﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System.Timers;
using System.Net.Sockets;
using System.Diagnostics;

namespace TouchpadServer {
    class BluetoothServer : Server, IDisposable {
        private Guid identifier;
        private BluetoothListener listener;
        private BluetoothClient client;
        private NetworkStream stream;

        public BluetoothServer(Guid guid) : base(){
            this.online = false;
            this.identifier = guid;
            this.listener = new BluetoothListener(guid);
        }
        protected override void BlacklistClient() {
            BlacklistManager.Insert(client.GetRemoteMachineName(client.RemoteEndPoint.Address), client.RemoteEndPoint.Address.ToString());
        } 

        #region Connectivity
        public override void Disconnect(bool notifyClient = true) {
            if (this.connected) {
                this.connectivityChecker.Enabled = false;
                this.reader.Enabled = false;
                if (notifyClient)
                    this.SendTerminateConnection();
                string address = this.client.RemoteEndPoint.Address.ToString();
                this.client.Close();
                this.client.Dispose();
                this.stream.Close();
                this.stream.Dispose();
                this.connected = false;
                OnConnectionStatusChanged(new ConnectionStatusChangedEventArgs(ConnectionStatusChangedEventArgs.ConnectionStatus.DISCONNECTED, address));
            }
        }
        public override void AcceptClient() {
            this.client = this.listener.AcceptBluetoothClient();
            string address = this.client.RemoteEndPoint.Address.ToString();
            if (BlacklistManager.Contains(address)) {
                this.client.Close();
                this.ResetGetter();
                return;
            }
            this.listener.Stop();
            this.connected = true;
            this.awaitingAcknoldegement = false;
            this.stream = client.GetStream();
            this.missing = 0;
            this.connectivityChecker.Enabled = true;
            this.reader.Enabled = true;

            this.OnConnectionStatusChanged(new ConnectionStatusChangedEventArgs(ConnectionStatusChangedEventArgs.ConnectionStatus.CONNECTED, this.client.RemoteMachineName));
        }
        #endregion

        #region Listener Methods
        protected override void StopListener() {
            this.listener.Stop();
        }
        protected override void StartListener() {
            this.listener.Start();
        }
        protected override bool GetPending() {
            return this.listener.Pending();
        }
        #endregion

        #region Send/Receive data
        protected override byte[] ReceiveData(byte len) {
            if (this.client.Available < len)
                return null;
            byte[] buffer = new byte[len];
            this.stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }
        protected override void SendData(byte[] buffer, int start = 0, int length = -1) {
            if (length == -1)
                length = buffer.Length;
            try {
                this.stream.Write(buffer, start, length);
                this.stream.Flush();
            } catch {
                //no client
            }
        }
        #endregion

        #region IDisposable implementation
        public override void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing) {
            if (this.disposed)
                return;
            if (disposing) {
                if (this.connected) {
                    this.stream.Dispose();
                    this.client.Dispose();
                }
                this.connectivityChecker.Dispose();
                this.reader.Dispose();
                ApplicationEvents.userDisconnectRequestEventHandler -= this.HandleDisconnectRequest;
                ApplicationEvents.userTurnOnOffRequestHandler -= this.HandleTurnOnOff;
            }
            this.disposed = true;
        }
        #endregion

        public override string GetEndpointRepresentation() {
            BluetoothRadio radio = BluetoothRadio.PrimaryRadio;
            if (radio == null || radio.LocalAddress == null)
                throw new Exception("Primary radio is missing, or bluetooth is off");
            return String.Format("{0:C}", radio.LocalAddress);
        }

        public static bool SupportsBluetooth() {
            BluetoothRadio radio = BluetoothRadio.PrimaryRadio;
            if (radio == null || radio.LocalAddress == null)
                return false;
            return true;
        }
    }
}
