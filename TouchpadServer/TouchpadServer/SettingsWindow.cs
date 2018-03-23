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
        public SettingsWindow(string MACAddress) {
            InitializeComponent();
            this.UpdateStatus(MainContext.status);
            this.QRCodeContainer.BackgroundImage = RenderQrCode(MACAddress);
            this.QRCodeContainer.Size = RenderQrCode(MACAddress).Size;
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
            switch (status.status) {
                case ConnectionStatusChangedEventArgs.ConnectionStatus.CONNECTED:
                    this.serverStatus.Text = "Status: Connected to: " + status.macaddress;
                    break;
                case ConnectionStatusChangedEventArgs.ConnectionStatus.DISCONNECTED:
                    this.serverStatus.Text = "Status: Not connected";
                    break;
                case ConnectionStatusChangedEventArgs.ConnectionStatus.OFFLINE:
                    this.serverStatus.Text = "Status: Offline";
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
    }
}
