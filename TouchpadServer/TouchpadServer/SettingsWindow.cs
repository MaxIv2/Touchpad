using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QRCoder;

namespace TouchpadServer {
    public partial class SettingsWindow : Form {

        delegate void SafeUpdateStatusDelegate(ConnectionStatusChangedEventArgs text);
        public SettingsWindow() {
            InitializeComponent();
            this.UpdateStatus(MainContext.status);
            this.switchConnectionType.Text = Properties.Settings.Default.Bluetooth ? "Switch to WiFi" : "Switch to Bluetooth";
            this.switchConnectionType.Click += this.SwitchConnectionTypeRequest;
            this.QRCodeContainer.BackgroundImage = RenderQrCode(Properties.Settings.Default.EndpointRepresentation);
            this.QRCodeContainer.Size = this.QRCodeContainer.BackgroundImage.Size;
            this.QRCodeContainer.SizeMode = PictureBoxSizeMode.StretchImage;
            this.exitButton.Click += ApplicationEvents.CallUserExitRequestEventHandler;
            this.diconnectButton.Click += this.disconnectButtonClick;
            this.blacklist.Click += this.blacklistButtonClick;
            this.onAndOffButtonSwitch.Click += ApplicationEvents.CallTurnOnOffEventHandlerHandler;
            ApplicationEvents.connectionStatusChangedEventHandler += this.HandleConnectionStatusChanged;
            this.FormClosing += this.UnsbscribeFromHandlers;
        }

        private void disconnectButtonClick(object sender, EventArgs e) {
            ApplicationEvents.CallUserDisconnectRequestEventHandler(sender, false);
        }

        private void blacklistButtonClick(object sender, EventArgs e) {
            ApplicationEvents.CallUserDisconnectRequestEventHandler(sender, true);
        }
        

        private void HandleConnectionStatusChanged(object sender, ConnectionStatusChangedEventArgs e) {
            this.SafeUpdateStatus(e);
        }

        private void UpdateStatus(ConnectionStatusChangedEventArgs status) {
            string firstRow = "Status" + (Properties.Settings.Default.Bluetooth ? "(Bluetooth based):" : "(WiFi based):") + "\n";
            switch (status.status) {
                case ConnectionStatusChangedEventArgs.ConnectionStatus.CONNECTED:
                    this.serverStatus.Text = firstRow + status.endpointRepresentation;
                    break;
                case ConnectionStatusChangedEventArgs.ConnectionStatus.DISCONNECTED:
                    this.serverStatus.Text = firstRow + "Not connected";
                    break;
                case ConnectionStatusChangedEventArgs.ConnectionStatus.OFFLINE:
                    this.serverStatus.Text = firstRow + "Status: Offline";
                    break;
            }
        }

        private void SafeUpdateStatus(ConnectionStatusChangedEventArgs status) {
            if (this.serverStatus.InvokeRequired) {
                SafeUpdateStatusDelegate d = new SafeUpdateStatusDelegate(UpdateStatus);
                this.Invoke(d, new object[] { status });
            }
            else
                this.UpdateStatus(status);
        }

        private void UnsbscribeFromHandlers(object sender, EventArgs e) {
            ApplicationEvents.connectionStatusChangedEventHandler -= this.HandleConnectionStatusChanged;
        }

        private static Bitmap RenderQrCode(string data) {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.L);
            QRCode qrCode = new QRCode(qrCodeData);
            return qrCode.GetGraphic(9);
        }

        private void SwitchConnectionTypeRequest(object sender, EventArgs e) {
            ApplicationEvents.CallConnectionTypeChangeRequestHandler(this, null);
            this.Close();
        }
    }
}
