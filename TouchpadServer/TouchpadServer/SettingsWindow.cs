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
        public SettingsWindow(string MACAddress) {
            InitializeComponent();
            this.UpdateStatus(MainContext.status);
            this.QRCodeContainer.BackgroundImage = RenderQrCode(MACAddress);
            this.QRCodeContainer.Size = RenderQrCode(MACAddress).Size;
            this.QRCodeContainer.SizeMode = PictureBoxSizeMode.StretchImage;
            this.exitButton.Click += ApplicationEvents.CallUserExitRequestEventHandler;
            this.diconnectButton.Click += ApplicationEvents.CallUserDisconnectRequestEventHandler;
            this.onAndOffButtonSwitch.Click += ApplicationEvents.CallTurnOnOffEventHandlerHandler;
            ApplicationEvents.connectionStatusChangedEventHandler += this.HandleConnectionStatusChanged;
            this.FormClosing += this.UnsbscribeFromHandlers;
        }

        private void HandleConnectionStatusChanged(object sender, ConnectionStatusChangedEventArgs e) {
            UpdateStatus(e);
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
