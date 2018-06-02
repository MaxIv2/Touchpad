using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TouchpadServer {
    class MainContext : ApplicationContext {
        public enum Status { Connected, Online, Offline };
        private enum MessageType : byte {
            MouseEvent = 0,
            ConnectionCheck = 1,
            CheckAcknoledgement = 2,
            TerminateConnection = 3
        };
      
        private Server server;
        private System.Timers.Timer reader;
        private System.Timers.Timer connectivityChecker;
        private TrayIconController iconController;
        private object readerLock;
        private bool waitingForAck;
        private static string serverEndpointRepr;
        public static string ServerEndpointRepr {
            get {
                return serverEndpointRepr;
            }
        }
        private static string clientEP;
        public static string ClientEP {
            get {
                return clientEP;
            }
        }
        private static Status serverStatus;
        public static Status ServerStatus {
            get {
                return serverStatus;
            }
        }

        public MainContext() {
            serverStatus = Status.Offline;
            this.SetUpReader();
            this.SetUpConnectivityChecker();
            this.SubscribeEvents();
            this.StartServer(); 
            this.iconController = TrayIconController.Instance;
        }
        private void StartServer() {
            if (Properties.Settings.Default.bluetooth) {
                try {
                    this.server = new BluetoothServer();
                }
                catch {
                    this.server = new TcpServer();
                    Properties.Settings.Default.bluetooth = false;
                    Properties.Settings.Default.Save();
                }
            }
            else {
                this.server = new TcpServer();
            }
            serverEndpointRepr = this.server.ServerEndpointRepr();
            this.server.GoOnline();
            GlobalAppEvents.RaiseOnlineEvent(this, new EventArgs());
        }

        #region timers
        private void SetUpReader() {
            this.reader = new System.Timers.Timer(5);
            this.reader.AutoReset = true;
            this.reader.Elapsed += this.Read;
            this.readerLock = new object();
        }
        private void Read(object sender, System.Timers.ElapsedEventArgs e) {
            lock (this.readerLock) {
                byte[] b = new byte[2];
                if (!this.server.RecieveData(b))
                    return;
                switch ((MessageType)b[0]) {
                    case MessageType.MouseEvent:
                        byte length = b[1];
                        byte[] mouseData = new byte[length];
                        while (!this.server.RecieveData(mouseData))
                            Thread.Sleep(50);
                        InputHandler.HandleData(mouseData);
                        break;
                    case MessageType.ConnectionCheck:
                        this.server.SendData(new byte[] { 1, 0 });
                        break;
                    case MessageType.CheckAcknoledgement:
                        this.waitingForAck = false;
                        break;
                    case MessageType.TerminateConnection:
                        this.server.Disconnect();
                        break;
                }
            }
        }
        private void SetUpConnectivityChecker() {
            this.connectivityChecker = new System.Timers.Timer(5000);
            this.connectivityChecker.AutoReset = true;
            this.connectivityChecker.Elapsed += this.CheckConnectivity;
            this.waitingForAck = false;
        }

        private void CheckConnectivity(object sender, System.Timers.ElapsedEventArgs e) {
            if (waitingForAck) {
                this.OnDisconnectRequest(this, new EventArgs());
                return;
            }
            this.server.SendData(new byte[] { 2, 0 });
            this.waitingForAck = true;
            
        }
        #endregion

        #region event subscription
        private void SubscribeEvents() {
            GlobalAppEvents.Connected += this.OnClientConnected;
            GlobalAppEvents.Disconnected += this.OnClientDisconnected;
            GlobalAppEvents.Online += this.OnOnline;
            GlobalAppEvents.Offline += this.OnOffline;
            GlobalAppEvents.OnlineRequest += this.OnOnlineRequest;
            GlobalAppEvents.OfflineRequest += this.OnOfflineRequest;
            GlobalAppEvents.ExitRequest += this.OnExitRequest;
            GlobalAppEvents.DisconnectReqeust += this.OnDisconnectRequest;
            GlobalAppEvents.SwitchConnectionTypeRequest += this.OnSwitchConnectionTypeRequest;
        }
        private void Unsubscribe() {
            GlobalAppEvents.Connected -= this.OnClientConnected;
            GlobalAppEvents.Disconnected -= this.OnClientDisconnected;
            GlobalAppEvents.Online -= this.OnOnline;
            GlobalAppEvents.Offline -= this.OnOffline;
            GlobalAppEvents.OnlineRequest -= this.OnOnlineRequest;
            GlobalAppEvents.OfflineRequest -= this.OnOfflineRequest;
        }
        #endregion

        #region event handling
        protected void OnClientConnected(object sender, EventArgs e) {
            clientEP = server.GetClientEndpoint();
            string address = Properties.Settings.Default.bluetooth ? clientEP : clientEP.Split(':')[0];
            if (BlacklistManager.Contains(address)) {
                server.Disconnect();
                return;
            }

            serverStatus = Status.Connected;
            this.connectivityChecker.Enabled = true;
            this.reader.Enabled = true;
        }
        protected void OnClientDisconnected(object sender, EventArgs e) {
            serverStatus = Status.Online;
            this.reader.Enabled = false;
            this.connectivityChecker.Enabled = false;
            this.server.GoOnline();
        }
        protected void OnOffline(object sender, EventArgs e) {
            serverStatus = Status.Offline;
            this.reader.Enabled = false;
        }
        protected void OnOnline(object sender, EventArgs e) {
            serverStatus = Status.Online;
        }
        protected void OnOnlineRequest(object sender, EventArgs e) {
            this.server.GoOnline();
        }
        protected void OnOfflineRequest(object sender, EventArgs e) {
            this.server.GoOffline();
        }
        protected void OnDisconnectRequest(object sender, EventArgs e) {
            this.server.SendData(new byte[] { 3, 0 });
            this.server.Disconnect();
        }
        private void OnExitRequest(object sender, EventArgs e) {
            GlobalAppEvents.RaiseOfflineEvent(this, new EventArgs());
            Unsubscribe();
            this.iconController.Dispose();
            this.reader.Dispose();
            this.connectivityChecker.Dispose();
            this.ExitThread();
        }
        private void OnSwitchConnectionTypeRequest(object sender, EventArgs e) {
            this.server.GoOffline();
            if (this.server is TcpServer) {
                try {
                    this.server = new BluetoothServer();
                    Properties.Settings.Default.bluetooth = true;
                    Properties.Settings.Default.Save();
                } catch {
                    this.server = new TcpServer();
                    Properties.Settings.Default.bluetooth = false;
                    Properties.Settings.Default.Save();
                }
            }
            else {
                this.server = new TcpServer();
                Properties.Settings.Default.bluetooth = false;
                Properties.Settings.Default.Save();
            }
            serverEndpointRepr = this.server.ServerEndpointRepr();
            this.server.GoOnline();
            GlobalAppEvents.RaiseSwitchedConnectionTypeEvent(this, new EventArgs());
        }
        #endregion
    }
}
