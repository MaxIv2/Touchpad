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
        public event MainContext.NewDataEventHandler newDataNotify;
        private bool isListening;
        public bool disposed { get; private set; }

        public BluetoothServer(Guid guid, MainContext.NewDataEventHandler newDataNotify) {
            this.identifier = guid;
            this.listener = new BluetoothListener(guid);
            this.connected = false;
            this.inputData = new Queue<byte>();
            this.awaitingAcknoldegement = false;
        }
        public void Disconnect() {
            if (client != null) {
                this.sendTerminateConnection();
                this.client.Close();
                this.client.Dispose();
                this.stream.Close();
                this.stream.Dispose();
            }
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
            this.stream = client.GetStream();
            this.SetConnectivityChecker();
            this.SetReader();
        }
        private void SetConnectivityChecker() {
            this.connectivityChecker = new Timer(5000);
            this.connectivityChecker.AutoReset = true;
            this.connectivityChecker.Elapsed += this.CheckConnection;
            this.connectivityChecker.Enabled = true;
        }
        private void CheckConnection(Object source, ElapsedEventArgs e) {
            if (awaitingAcknoldegement) {
                //disconnected
                return;
            }
            SendConnectionCheck();
            this.awaitingAcknoldegement = true;
        }
        private void OnConnectionLost() {
            this.client.Close();
            this.client.Dispose();
            this.client = null;
            this.stream.Close();
            this.stream.Dispose();
            this.stream = null;
            //lost client notify main context?
        }
        private void SetReader() {
            this.reader = new Timer(50);
            this.reader.AutoReset = true;
            this.reader.Elapsed += this.ReadData;
            this.reader.Enabled = true;
        }
        private void ReadData(Object source, ElapsedEventArgs e) {
            byte[] buffer = this.RecieveData();
            if (buffer == null)
                return;
            if (missingDataCount > 0) {
                foreach (byte b in buffer) {
                    this.inputData.Enqueue(b);
                }
                OnNewData(inputData);
            }
            else {
                switch ((MessageType)buffer[0]) {
                    case MessageType.CONNECTION_CHECK:
                        this.SendCheckAcknoledgement();
                        break;
                    case MessageType.CHECK_ACKNOLEDGEMENT:
                        this.awaitingAcknoldegement = false;
                        break;
                    case MessageType.MOUSE:
                        byte length = buffer[1];
                        for (int i = 0; i < length; length--) {
                            this.inputData.Enqueue(buffer[i + 2]);
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
            this.newDataNotify(this, new NewDataEventArgs(info));
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
                if(this.stream != null)
                    this.stream.Dispose();
                if (this.client != null)
                    this.client.Dispose();
                if (this.reader != null)
                    this.reader.Dispose();
                if (this.connectivityChecker != null)
                    this.connectivityChecker.Dispose();
            }
            this.disposed = true;
        }
    }
}
