﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System.Timers;
using System.Net.Sockets;

namespace TouchpadServer {
    class BluetoothServer : IDisposable {//TO DO: implement a handler for termination of connection request
        private enum MessageType { MOUSE = 0, CONNECTION_CHECK = 1, CHECK_ACKNOLEDGEMENT = 2, TERMINATE_CONNECTION = 3 }
        public Guid identifier { get; private set; }
        private BluetoothListener listener;
        private BluetoothClient client;
        private NetworkStream stream;
        private Timer connectivityChecker;
        private Timer reader;
        private Timer clientGetter;
        private bool connected;
        private Queue<byte> inputData;
        private int missingDataCount;
        private bool awaitingAcknoldegement;

        public static bool online { get; private set; }
        public bool disposed { get; private set; }

        public BluetoothServer(Guid guid) {
            online = false;
            this.identifier = guid;
            this.listener = new BluetoothListener(guid);
            this.connected = false;
            this.inputData = new Queue<byte>();
            this.awaitingAcknoldegement = false;
            this.SetConnectivityChecker();
            this.SetReader();
            this.SetClientGetter();
            ApplicationEvents.turnOnOffEventHandler += this.HandleTurnOnOff;
            ApplicationEvents.userDisconnectRequestEventHandler += this.HandleDisconnectRequest;
        }

        #region Connectivity
        public void GoOnline() {
            if (online)
                return;
            online = true;
            this.listener.Start();
            this.OnConnectionStatusChanged(new ConnectionStatusChangedEventArgs(ConnectionStatusChangedEventArgs.ConnectionStatus.DISCONNECTED, ""));
            this.clientGetter.Enabled = true;
        }
        
        public void GoOffline() {
            if (!online)
                return;
            if (this.connected)
                Disconnect();
            else {
                this.clientGetter.Enabled = false;
                this.listener.Stop();
            }
            this.OnConnectionStatusChanged(new ConnectionStatusChangedEventArgs(ConnectionStatusChangedEventArgs.ConnectionStatus.OFFLINE,""));
            online = false;
        }
        
        public void Disconnect(bool notifyClient = true) {
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

        public void AcceptClient() {
            this.client = this.listener.AcceptBluetoothClient();
            this.connected = true;
            this.stream = client.GetStream();
            //this.connectivityChecker.Enabled;
            this.reader.Enabled = true;
            string address = this.client.RemoteEndPoint.Address.ToString();
            this.OnConnectionStatusChanged(new ConnectionStatusChangedEventArgs(ConnectionStatusChangedEventArgs.ConnectionStatus.CONNECTED, address));
        }
        #endregion

        #region Timers and timed methods
        private void SetConnectivityChecker() {
            this.connectivityChecker = new Timer(5000);
            this.connectivityChecker.AutoReset = true;
            this.connectivityChecker.Elapsed += this.CheckConnection;
        }

        private void CheckConnection(Object source, ElapsedEventArgs e) {
            if (awaitingAcknoldegement) {
                Disconnect();
                this.listener.Start();
                this.clientGetter.Enabled = true;
                return;
            }
            SendConnectionCheck();
            this.awaitingAcknoldegement = true;
        }

        private void SetReader() {
            this.reader = new Timer(50);
            this.reader.AutoReset = false;
            this.reader.Elapsed += this.ReadData;
        }

        private void ReadData(Object source, ElapsedEventArgs e) {
            byte[] buffer = this.ReceiveData();
            if (buffer == null)
                return;
            int index = 0;
            if (missingDataCount > 0) {
                while (missingDataCount > 0) {
                    this.inputData.Enqueue(buffer[index]);
                    index++;
                }
                OnNewData(inputData);
            }
            if(buffer.Length > index + 1) {
                switch ((MessageType)buffer[0]) {
                    case MessageType.TERMINATE_CONNECTION:
                        this.Disconnect(notifyClient: false);
                        break;
                    case MessageType.CONNECTION_CHECK:
                        this.SendCheckAcknoledgement();
                        break;
                    case MessageType.CHECK_ACKNOLEDGEMENT:
                        this.awaitingAcknoldegement = false;
                        break;
                    case MessageType.MOUSE:
                        byte length = buffer[1];
                        int j;
                        for (j = 0; j < length; j++) {
                            this.inputData.Enqueue(buffer[j + 2]);
                        }
                        missingDataCount = buffer[1] - j;
                        OnNewData(inputData);
                        break;
                    default:
                        throw new Exception("Something went wrong :(");
                        break;
                }
            }
        }

        private void SetClientGetter() {
            this.clientGetter = new Timer(500);
            this.clientGetter.AutoReset = true;
            this.clientGetter.Elapsed += this.TryToGetClient;
        }

        public void TryToGetClient(Object source, ElapsedEventArgs e) {
            if (listener.Pending()) {
                this.clientGetter.Enabled = false;
                this.AcceptClient();
                this.listener.Stop();
            }
        }
        #endregion
        
        #region Handle events
        private void HandleDisconnectRequest(object sender, EventArgs e) {
            Disconnect();
            this.listener.Start();
            this.clientGetter.Enabled = true;
        }

        private void HandleTurnOnOff(object sender, EventArgs e) {
            if (online)
                GoOffline();
            else
                GoOnline();
        }
        #endregion

        #region Raise events
        private void OnConnectionStatusChanged(ConnectionStatusChangedEventArgs args) {
            ApplicationEvents.CallConnectionStatusChangedEventHandler(this, args);
        }

        private void OnNewData(Queue<byte> info) {
            ApplicationEvents.CallNewEventDataEventHandler(this, new NewDataEventArgs(info));
        }
        #endregion

        #region Send/Receive data
        private byte[] ReceiveData() {
            if (this.client.Available < 2)
                return null;
            byte[] buffer = new byte[this.client.Available];
            this.stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        public void SendData(byte[] buffer, int start = 0, int length = -1) {
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

        #region Special Messages
        private void SendCheckAcknoledgement() {
            byte[] buffer = { (byte) MessageType.CHECK_ACKNOLEDGEMENT, 0};
            this.SendData(buffer);
        }

        private void SendConnectionCheck() {
            byte[] buffer = { (byte)MessageType.CONNECTION_CHECK, 0};
            SendData(buffer);
        }

        private void SendTerminateConnection() {
            byte[] buffer = { (byte)MessageType.TERMINATE_CONNECTION, 0 };
            SendData(buffer);
        }
        #endregion

        #region IDisposable implementation
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (this.disposed)
                return;
            ApplicationEvents.userDisconnectRequestEventHandler -= this.HandleDisconnectRequest;
            ApplicationEvents.turnOnOffEventHandler -= this.HandleTurnOnOff;
            if (disposing) {
                if (this.connected) {
                    this.stream.Dispose();
                    this.client.Dispose();
                }
                this.connectivityChecker.Dispose();
                this.reader.Dispose();
            }
            this.disposed = true;
        }
        #endregion

        #region Static methods
        public static string GetAdaptersMACAddress() {
            BluetoothRadio radio = BluetoothRadio.PrimaryRadio;
            if (radio == null || radio.LocalAddress == null)
                throw new Exception("Primary radio is missing, or bluetooth is off");
            return String.Format("{0:C}", radio.LocalAddress);
        }
        #endregion
    }
}
