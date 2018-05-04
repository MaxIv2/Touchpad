using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Diagnostics;

namespace TouchpadServer {
    abstract class Server : IDisposable {
        protected bool online;
        protected bool disposed;
        protected bool connected;
        protected bool awaitingAcknoldegement;
        protected Timer connectivityChecker;
        protected Timer reader;
        protected Timer clientGetter;
        private object readerLock;

        protected Server() {
            this.SetConnectivityChecker();
            this.SetReader();
            this.SetClientGetter();
            this.online = false;
            this.disposed = false;
            this.connected = false;
            this.awaitingAcknoldegement = false;
            this.readerLock = new object();
            ApplicationEvents.userTurnOnOffRequestHandler += this.HandleTurnOnOff;
            ApplicationEvents.userDisconnectRequestEventHandler += this.HandleDisconnectRequest;
        }

        protected enum MessageType { MOUSE = 0, CONNECTION_CHECK = 1, CHECK_ACKNOLEDGEMENT = 2, TERMINATE_CONNECTION = 3 }

        #region Connectivity
        public void GoOnline() {
            if (online)
                return;
            this.StartListener();
            online = true;
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
                this.StopListener();
            }
            this.OnConnectionStatusChanged(new ConnectionStatusChangedEventArgs(ConnectionStatusChangedEventArgs.ConnectionStatus.OFFLINE, ""));
            online = false;
        }

        public abstract void Disconnect(bool notifyClient = true);

        public abstract void AcceptClient();
        #endregion

        #region Timers and timed methods
        protected void SetConnectivityChecker() {
            this.connectivityChecker = new Timer(5000);
            this.connectivityChecker.AutoReset = true;
            this.connectivityChecker.Elapsed += this.CheckConnection;
        }

        private void CheckConnection(Object source, ElapsedEventArgs e) {
            if (awaitingAcknoldegement) {
                Disconnect();
                this.StartListener();
                this.clientGetter.Enabled = true;
                return;
            }
            SendConnectionCheck();
            this.awaitingAcknoldegement = true;
        }

        protected void SetReader() {
            this.reader = new Timer(5);
            this.reader.AutoReset = true;
            this.reader.Elapsed += this.ReadData;
        }

        private void ReadData(Object source, ElapsedEventArgs e) {
            lock (readerLock) {
                byte[] outerHeader = this.ReceiveData(2);
                if (outerHeader == null)
                    return;
                byte type = outerHeader[0];
                byte length = outerHeader[1];
                switch ((MessageType)type) {
                    case MessageType.TERMINATE_CONNECTION:
                        Debug.WriteLine("terminate");
                        this.Disconnect(notifyClient: false);
                        this.ResetGetter();
                        break;
                    case MessageType.CONNECTION_CHECK:
                        Debug.WriteLine("CONNECTION_CHECK");
                        this.SendCheckAcknoledgement();
                        break;
                    case MessageType.CHECK_ACKNOLEDGEMENT:
                        Debug.WriteLine("CHECK_ACKNOLEDGEMENT");
                        this.awaitingAcknoldegement = false;
                        break;
                    case MessageType.MOUSE:
                        byte[] buffer = this.ReceiveData(length);
                        if (buffer == null || buffer.Length < length) {
                            return;
                        }
                        OnNewData(buffer);
                        break;
                    default:
                        Disconnect();
                        Debug.WriteLine("wut");
                        break;
                }
            }
        }

        protected void SetClientGetter() {
            this.clientGetter = new Timer(500);
            this.clientGetter.AutoReset = true;
            this.clientGetter.Elapsed += this.TryToGetClient;
        }

        private void TryToGetClient(Object source, ElapsedEventArgs e) {
            if (this.GetPending()) {
                this.clientGetter.Enabled = false;
                this.AcceptClient();
            }
        }

        protected void ResetGetter() {
            this.StartListener();
            this.clientGetter.Enabled = true;
        }
        #endregion

        #region Raise events
        protected void OnConnectionStatusChanged(ConnectionStatusChangedEventArgs args) {
            ApplicationEvents.CallConnectionStatusChangedEventHandler(this, args);
        }

        protected void OnNewData(byte[] info) {
            ApplicationEvents.CallNewDataEventHandler(this, info);
        }
        #endregion

        #region Handle events
        protected void HandleDisconnectRequest(object sender, bool blacklist) {
            if (connected) {
                if (blacklist)
                    this.BlacklistClient();
                Disconnect();
                this.StartListener();
                this.clientGetter.Enabled = true;
            }
        }

        protected void HandleTurnOnOff(object sender, EventArgs e) {
            if (online)
                GoOffline();
            else
                GoOnline();
        }
        #endregion

        #region Special Messages
        protected void SendCheckAcknoledgement() {
            byte[] buffer = { (byte)MessageType.CHECK_ACKNOLEDGEMENT, 0 };
            this.SendData(buffer);
        }

        protected void SendConnectionCheck() {
            byte[] buffer = { (byte)MessageType.CONNECTION_CHECK, 0 };
            this.SendData(buffer);
        }

        protected void SendTerminateConnection() {
            byte[] buffer = { (byte)MessageType.TERMINATE_CONNECTION, 0 };
            this.SendData(buffer);
        }

        #endregion Special Messages

        #region Listener Methods
        protected abstract void StartListener();

        protected abstract void StopListener();

        protected abstract bool GetPending();
        #endregion

        #region Send/Receive Methods
        protected abstract byte[] ReceiveData(byte len);

        protected abstract void SendData(byte[] buffer, int start = 0, int length = -1);
        #endregion

        protected abstract void BlacklistClient();

        public abstract string GetEndpointRepresentation();
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;
            if (disposing) {
                ApplicationEvents.userTurnOnOffRequestHandler -= this.HandleTurnOnOff;
                ApplicationEvents.userDisconnectRequestEventHandler -= this.HandleDisconnectRequest;
            }
            this.reader.Dispose();
            this.clientGetter.Dispose();
            this.connectivityChecker.Dispose();
            this.disposed = true;
        }

    }
}
