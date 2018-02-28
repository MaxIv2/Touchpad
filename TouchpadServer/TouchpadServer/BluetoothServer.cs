using System;
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
        private bool connected;
        private Queue<byte> inputData;
        private int missingDataCount;
        private bool awaitingAcknoldegement;
        public event MainContext.NewDataEventHandler newDataEventHandler;
        private bool isListening;
        private event MainContext.DisconnectedEventHandler disconnectedEventHandler;
        public bool disposed { get; private set; }

        public BluetoothServer(Guid guid, MainContext.NewDataEventHandler newDataEventHandler, MainContext.DisconnectedEventHandler disconnectedEventHandler) {
            this.identifier = guid;
            this.listener = new BluetoothListener(guid);
            this.disconnectedEventHandler += disconnectedEventHandler;
            this.newDataEventHandler += newDataEventHandler;
            this.connected = false;
            this.inputData = new Queue<byte>();
            this.awaitingAcknoldegement = false;
            this.SetConnectivityChecker();
            this.SetReader();
        }

        public void Disconnect(bool notifyClient=true) {
            if (this.connected) {
                this.connectivityChecker.Enabled = false;
                this.reader.Enabled = false;
                if(notifyClient)
                    this.sendTerminateConnection();
                this.client.Close();
                this.client.Dispose();
                this.stream.Close();
                this.stream.Dispose();
                this.connected = false;
                OnDisconnectedEvent();
            }
        }

        public void OnDisconnectedEvent() {
            disconnectedEventHandler(this, new EventArgs());
        }

        public void CloseServer() {
            this.Disconnect();
            if (this.isListening)
                listener.Stop();
        }

        private void sendTerminateConnection() {
            byte[] buffer = { (byte)MessageType.TERMINATE_CONNECTION, 0 };
            SendData(buffer);
        }

        public void StartListening() {
            if (this.isListening)
                return;
            this.listener.Start();
            this.isListening = true;
        }

        public void StopListening() {
            if (!this.isListening)
                return;
            this.listener.Stop();
            this.isListening = false;
        }

        public void AcceptClient() {
            this.client = this.listener.AcceptBluetoothClient();
            this.connected = true;
            this.stream = client.GetStream();
            this.connectivityChecker.Enabled = true;
            this.reader.Enabled = true;
        }

        private void SetConnectivityChecker() {
            this.connectivityChecker = new Timer(5000);
            this.connectivityChecker.AutoReset = true;
            this.connectivityChecker.Elapsed += this.CheckConnection;
        }

        private void CheckConnection(Object source, ElapsedEventArgs e) {
            if (awaitingAcknoldegement) {
                Disconnect();
                return;
            }
            SendConnectionCheck();
            this.awaitingAcknoldegement = true;
        }

        private void OnConnectionLost() {
            this.Disconnect();
        }

        private void SetReader() {
            this.reader = new Timer(50);
            this.reader.AutoReset = true;
            this.reader.Elapsed += this.ReadData;
        }

        private void ReadData(Object source, ElapsedEventArgs e) {
            byte[] buffer = this.RecieveData();
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
                        for (int j = 0; j < length; length--) {
                            this.inputData.Enqueue(buffer[j + 2]);
                        }
                        missingDataCount = length;
                        OnNewData(inputData);
                        break;
                    default:
                        throw new Exception("Something went wrong :(");
                        break;
                }
            }
        }

        private byte[] RecieveData() {
            if (this.client.Available < 2)
                return null;
            byte[] buffer = new byte[this.client.Available];
            this.stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        public void SendData(byte[] buffer, int start = 0, int length = -1) {
            if (length == -1)
                length = buffer.Length;
            this.stream.Write(buffer, start, length);
            this.stream.Flush();
        }

        private void SendCheckAcknoledgement() {
            byte[] buffer = { (byte) MessageType.CHECK_ACKNOLEDGEMENT, 0};
            this.SendData(buffer);
        }

        private void SendConnectionCheck() {
            byte[] buffer = { (byte)MessageType.CONNECTION_CHECK, 0};
            SendData(buffer);
        }

        private void OnNewData(Queue<byte> info) {
            this.newDataEventHandler(this, new NewDataEventArgs(info));
        }

        public static string GetAdaptersMACAddress() {
            BluetoothRadio radio = BluetoothRadio.PrimaryRadio;
            if (radio == null || radio.LocalAddress == null)
                throw new Exception("Primary radio is missing, or bluetooth is off");
            return String.Format("{0:C}", radio.LocalAddress);
        }

        public void Dispose() {
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
            }
            this.disposed = true;
        }
    }
}
