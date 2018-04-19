using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace TouchpadServer {
    class MainContext : ApplicationContext {

        private Server server;
        private TrayIconController icon;
        public static ConnectionStatusChangedEventArgs status;

        public MainContext() : base() {
            status = new ConnectionStatusChangedEventArgs(ConnectionStatusChangedEventArgs.ConnectionStatus.OFFLINE, "");
            ApplicationEvents.newDataEventHandler += InputHandler.HandleOnNewDataEvent;
            ApplicationEvents.connectionStatusChangedEventHandler += this.HandleConnectionStatusChanged;
            ApplicationEvents.userExitRequestEventHandler += this.HandleUserExitRequest;
            ApplicationEvents.connectionTypeChangeRequestHandler += this.HandleConnectionTypeChangeRequest;
            if (Properties.Settings.Default.Bluetooth) {
                if(BluetoothServer.SupportsBluetooth())
                    this.server = new BluetoothServer(new Guid(Properties.Resources.Guid));
                else {
                    MessageBox.Show("Bluetooth is either not supported or turned off on this machine");
                    Properties.Settings.Default.Bluetooth = false;
                    this.server = new TcpServer();
                }
            }
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
            if (Properties.Settings.Default.Bluetooth) {
                if (BluetoothServer.SupportsBluetooth())
                    this.server = new BluetoothServer(new Guid(Properties.Resources.Guid));
                else {
                    MessageBox.Show("Bluetooth is either not supported or turned off on this machine");
                    Properties.Settings.Default.Bluetooth = false;
                    this.server = new TcpServer();
                }
            }
            else
                this.server = new TcpServer();
            Properties.Settings.Default.EndpointRepresentation = this.server.GetEndpointRepresentation();
            Properties.Settings.Default.Save();
            this.server.GoOnline();
        }

        private void HandleConnectionStatusChanged(object sender, ConnectionStatusChangedEventArgs e) {
            status = e;
        }

        private void HandleUserExitRequest(object sender, EventArgs e) {
            ApplicationEvents.newDataEventHandler -= InputHandler.HandleOnNewDataEvent;
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
