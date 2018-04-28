using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Timers;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace TouchpadServer {
    sealed class TcpServer : Server, IDisposable {
        private TcpListener listener;
        private Socket client;
        private int port;
        
        public TcpServer() : base() {
            this.SetListener();
        }

        protected override void BlacklistClient() {
            string address = (this.client.RemoteEndPoint as IPEndPoint).Address.ToString();
            string name;
            try {
                IPHostEntry entry = Dns.GetHostEntry(address);
                if (entry != null)
                    name = entry.HostName;
                else
                    name = "Unknown";
            }
            catch (SocketException) {
                name = "Unknown";
            }
            BlacklistManager.Insert(name, address);
        } 

        #region Connectivity
        public override void AcceptClient() {
            this.client = this.listener.AcceptSocket();
            string address = (this.client.RemoteEndPoint as IPEndPoint).Address.ToString();
            if (BlacklistManager.Contains(address)) {
                this.client.Close();
                this.ResetGetter();
                return;
            }
            this.listener.Stop();
            this.connected = true;
            this.awaitingAcknoldegement = false;
            this.missing = 0;
            this.connectivityChecker.Enabled = true;
            this.reader.Enabled = true;
            this.OnConnectionStatusChanged(new ConnectionStatusChangedEventArgs(ConnectionStatusChangedEventArgs.ConnectionStatus.CONNECTED, this.client.RemoteEndPoint.ToString()));
        }
        public override void Disconnect(bool notifyClient = true) {
            if (this.connected) {
                this.connectivityChecker.Enabled = false;
                this.reader.Enabled = false;
                if (notifyClient)
                    this.SendTerminateConnection();
                string address = this.client.RemoteEndPoint.ToString();
                this.client.Close();
                this.client.Dispose();
                this.connected = false;
                OnConnectionStatusChanged(new ConnectionStatusChangedEventArgs(ConnectionStatusChangedEventArgs.ConnectionStatus.DISCONNECTED, address));
            }
        }
        #endregion Connectivity

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
            try {
                this.client.Receive(buffer, buffer.Length, SocketFlags.None);
                return buffer;
            }
            catch (SocketException) {
                return null;//client Disconnected
            }
        }

        protected override void SendData(byte[] buffer, int start = 0, int length = -1) {
            if (length == -1)
                length = buffer.Length;
            try {
                this.client.Send(buffer, length, SocketFlags.None);
            }
            catch {
                //no client
            }
        }
        #endregion

        #region IDisposable implementation
        public override void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (this.disposed)
                return;
            if (disposing) {
                if (this.connected) {
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
            return GetLocalIPAddress() +":" + (port - 1);
        }
        private void SetListener() {
            IPAddress ipObject = new IPAddress(new byte[] { 0, 0, 0, 0 });
            IPEndPoint localEP = new IPEndPoint(ipObject, this.GetFreePort());
            string ip = ipObject.ToString();
            this.listener = new TcpListener(localEP);
            listener.Start();
            this.port = (listener.LocalEndpoint as IPEndPoint).Port + 1;
            listener.Stop();
        }
        private static IPAddress GetLocalIPAddress() {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces()) {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && ni.OperationalStatus == OperationalStatus.Up) {
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses) {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {
                            return ip.Address;
                        }
                    }
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        private int GetFreePort() {
            int PortStartIndex = 1000;
            int PortEndIndex = 2000;
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpEndPoints = properties.GetActiveTcpListeners();

            List<int> usedPorts = tcpEndPoints.Select(p => p.Port).ToList<int>();

            for (int port = PortStartIndex; port < PortEndIndex; port++) {
                if (!usedPorts.Contains(port)) {
                    return port;
                }
            }
            return 0;
        }
    }
}
