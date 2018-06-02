using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System.Net.Sockets;
using System.Timers;

namespace TouchpadServer {
    sealed class BluetoothServer : Server {
        private BluetoothListener listener;
        private bool isListenening;
        private bool isConnected;
        private BluetoothClient client;
        private Timer clientGetter;
        private NetworkStream stream;

        public bool IsOnline {
            get {
                return this.isConnected || this.isListenening;
            }
        }

        public BluetoothServer() {
            this.isListenening = false;
            SetUpListener();
            SetUpClientGetter(500);
        }
        
        #region setup
        private void SetUpListener() {
            this.listener = new BluetoothListener(new Guid(Properties.Resources.GUID));
        }
        private void SetUpClientGetter(int interval) {
            this.clientGetter = new Timer(interval);
            this.clientGetter.AutoReset = true;
            this.clientGetter.Elapsed += TryGetClient;
        }
        private void TryGetClient(object sender, ElapsedEventArgs e) {
            if (this.listener.Pending()) {
                this.clientGetter.Enabled = false;
                this.client = listener.AcceptBluetoothClient();
                this.stream = client.GetStream();
                this.isConnected = true;
                GlobalAppEvents.RaiseConnectedEvent(this, new EventArgs());
            }
        }
        #endregion
        #region connectivity
        public void GoOnline() {
            System.Diagnostics.Debug.WriteLine("listening bluetooth");
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
                if (this.isConnected) {
                    this.stream.Write(data, 0, data.Length);
                }
            }
            catch {
                Disconnect();
            }
        }
        public bool RecieveData(byte[] buffer) {
            try {
                if (!this.isConnected)
                    throw new Exception("Not connected to anyone!");
                if (client.Available < buffer.Length)
                    return false;
                this.stream.Read(buffer, 0, buffer.Length);
                return true;
            }
            catch {
                Disconnect();
                return false;
            }
        }
        #endregion
        #region endpoint stuff
        public string ServerEndpointRepr() {
            BluetoothRadio radio = BluetoothRadio.PrimaryRadio;
            if (radio == null || radio.LocalAddress == null)
                throw new Exception("Primary radio is missing, or bluetooth is off");
            return String.Format("{0:C}", radio.LocalAddress);
        }
        public string GetClientEndpoint() {
            if (this.client != null)
                return this.client.RemoteEndPoint.Address.ToString();
            return "";
        }
        #endregion
    }
}
