using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Timers;

namespace TouchpadServer {
    sealed class TcpServer : Server {
        private TcpListener listener;
        private bool isListenening;
        private bool isConnected;
        private Socket client;
        private Timer clientGetter;

        public bool IsOnline {
            get {
                return this.isConnected || this.isListenening;
            }
        }

        public TcpServer() {
            this.isListenening = false;
            SetUpListener();
            SetUpClientGetter(500);
        }
        
        #region setup
        private void SetUpListener() {
            IPEndPoint DefaultLoopbackEndpoint = new IPEndPoint(IPAddress.Loopback, port: 0);
            int port;
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)) {
                socket.Bind(DefaultLoopbackEndpoint);
                port = ((IPEndPoint)socket.LocalEndPoint).Port;
            }
            this.listener = new TcpListener(new IPAddress(new byte[] { 0, 0, 0, 0 }), port);
        }
        private void SetUpClientGetter(int interval) {
            this.clientGetter = new Timer(interval);
            this.clientGetter.AutoReset = true;
            this.clientGetter.Elapsed += TryGetClient;
        }
        private void TryGetClient(object sender, ElapsedEventArgs e) {
            if (this.listener.Pending()) {
                this.clientGetter.Enabled = false;
                this.client = listener.AcceptSocket();
                this.isConnected = true;
                GlobalAppEvents.RaiseConnectedEvent(this, new EventArgs());
            }
        }
        #endregion
        #region connectivity
        public void GoOnline() {
            int port = ((IPEndPoint)this.listener.LocalEndpoint).Port;
            System.Diagnostics.Debug.WriteLine("listening on port : {0}", port);
            this.listener.Start();
            this.isListenening = true;
            this.clientGetter.Enabled = true;
            GlobalAppEvents.RaiseOnlineEvent(this, new EventArgs());
        }
        public void GoOffline() {
            this.clientGetter.Enabled = false;
            if(this.isListenening)
                this.listener.Stop();
            this.isListenening = false;
            if (this.isConnected)
                this.client.Close();
            this.isConnected = false;
            GlobalAppEvents.RaiseOfflineEvent(this, new EventArgs());
        }
        public void Disconnect() {
            if (this.isConnected) {
                this.client.Close();
                GlobalAppEvents.RaiseDisconnectedEvent(this, new EventArgs());
            }
        }
        #endregion
        #region send/receive
        public void SendData(byte[] data) {
            try {
                if (this.isConnected)
                    client.Send(data);
            } catch {
                Disconnect();
            }
        }
        public bool RecieveData(byte[] buffer) {
            try {
                if (!this.isConnected)
                    throw new Exception("Not connected to anyone!");
                if (client.Available < buffer.Length)
                    return false;
                client.Receive(buffer);
                return true;
            }
            catch {
                Disconnect();
                return false;
            }
        }
        #endregion
        #region endpoint stuff
        private static string GetLocalIP() {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces()) {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && ni.OperationalStatus == OperationalStatus.Up) {
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses) {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {
                            return ip.Address.ToString();
                        }
                    }
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        public string ServerEndpointRepr() {
            string ip = GetLocalIP();
            string res = ip + ":" + (listener.LocalEndpoint as IPEndPoint).Port;
            return res;
        }

        public string GetClientEndpoint() {
            if (this.client != null) {
                IPEndPoint ep = this.client.RemoteEndPoint as IPEndPoint;
                string epRepr = ep.Address.ToString() + ":" + ep.Port;
                return epRepr;
            }
            return "";
        }
        #endregion
    }
}
