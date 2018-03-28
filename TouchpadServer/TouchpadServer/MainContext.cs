using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace TouchpadServer {
    class MainContext : ApplicationContext {

        Server server;
        TrayIconController icon;
        public static ConnectionStatusChangedEventArgs status;

        public delegate void NewDataEventHandler(object sender, NewDataEventArgs e);
        public delegate void DisconnectedEventHandler(object sender, EventArgs e);

        public MainContext() : base() {
            status = new ConnectionStatusChangedEventArgs(ConnectionStatusChangedEventArgs.ConnectionStatus.OFFLINE, "");
            ApplicationEvents.newDataEventDataEventHandler += InputHandler.HandleOnNewDataEvent;
            ApplicationEvents.connectionStatusChangedEventHandler += this.HandleConnectionStatusChanged;
            ApplicationEvents.userExitRequestEventHandler += this.HandleUserExitRequest;
            ApplicationEvents.connectionTypeChangeRequestHandler += this.HandleConnectionTypeChangeRequest;
            if (Properties.Settings.Default.Bluetooth)
                this.server = new BluetoothServer(new Guid(Properties.Resources.Guid));
            else
                this.server = new TcpServer();
            Properties.Settings.Default.EndpointRepresentation = this.server.GetEndpointRepresentation();
            Properties.Settings.Default.Save();
            this.icon = new TrayIconController();
            this.server.GoOnline();
        }

        private void HandleConnectionTypeChangeRequest(object sender, EventArgs e) {
            this.server.GoOffline();
            this.server.Dispose();
            Properties.Settings.Default.Bluetooth = !Properties.Settings.Default.Bluetooth;
            if (Properties.Settings.Default.Bluetooth)
                this.server = new BluetoothServer(new Guid(Properties.Resources.Guid));
            else
                this.server = new TcpServer();
            Properties.Settings.Default.EndpointRepresentation = this.server.GetEndpointRepresentation();
            Properties.Settings.Default.Save();
            this.server.GoOnline();
        }

        private void HandleConnectionStatusChanged(object sender, ConnectionStatusChangedEventArgs e) {
            status = e;
        }

        public void HandleUserExitRequest(object sender, EventArgs e) {
            ApplicationEvents.newDataEventDataEventHandler -= InputHandler.HandleOnNewDataEvent;
            ApplicationEvents.userExitRequestEventHandler -= this.HandleUserExitRequest;
            ApplicationEvents.connectionStatusChangedEventHandler -= this.HandleConnectionStatusChanged;
            ApplicationEvents.connectionTypeChangeRequestHandler -= this.HandleConnectionTypeChangeRequest;
            this.server.GoOffline();
            this.server.Dispose();
            this.icon.Dispose();
            this.ExitThread();
        }
    }
}
