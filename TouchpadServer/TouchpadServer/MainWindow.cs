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
    public partial class MainWindow : Form {

        delegate void UpdateStatusDelegate(ConnectionStatusChangedEventArgs text);
        delegate void UpdateButtonDelegate(ConnectionStatusChangedEventArgs text);

        public MainWindow() {
            InitializeComponent();
            this.UpdateStatus(MainContext.status);
            this.diconnectButton.Enabled = MainContext.status.status == ConnectionStatusChangedEventArgs.ConnectionStatus.CONNECTED;
            this.blacklistButton.Enabled = MainContext.status.status == ConnectionStatusChangedEventArgs.ConnectionStatus.CONNECTED;
            this.switchConnectionType.Text = Properties.Settings.Default.Bluetooth ? "Switch to WiFi" : "Switch to Bluetooth";
            this.switchConnectionType.Click += this.SwitchConnectionTypeRequest;
            this.QRCodeContainer.BackgroundImage = RenderQrCode(Properties.Settings.Default.EndpointRepresentation);
            this.QRCodeContainer.Size = this.QRCodeContainer.BackgroundImage.Size;
            this.QRCodeContainer.SizeMode = PictureBoxSizeMode.StretchImage;
            this.exitButton.Click += ApplicationEvents.CallUserExitRequestEventHandler;
            this.diconnectButton.Click += this.disconnectButtonClick;
            this.blacklistButton.Click += this.blacklistButtonClick;
            this.onAndOffButtonSwitch.Click += ApplicationEvents.CallUserTurnOnOffRequestHandler;
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
            if (this.serverStatus.InvokeRequired) {
                UpdateButtonDelegate d = new UpdateButtonDelegate(UpdateButton);
                this.Invoke(d, new object[] { e });
            }
            else
                this.UpdateButton(e);
        }

        private void UpdateButton(ConnectionStatusChangedEventArgs e) {
            this.diconnectButton.Enabled = e.status == ConnectionStatusChangedEventArgs.ConnectionStatus.CONNECTED;
            this.blacklistButton.Enabled = MainContext.status.status == ConnectionStatusChangedEventArgs.ConnectionStatus.CONNECTED;
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
                UpdateStatusDelegate d = new UpdateStatusDelegate(UpdateStatus);
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



        private void MoveSensitivityChanged(object sender, EventArgs e)
        {
            int value = this.moveBar.Value + 1;
            Properties.Settings.Default.Move = value;
            Properties.Settings.Default.Save();
        }
        private void ScrollSensitivityChanged(object sender, EventArgs e)
        {
            int value = this.scrollBar.Value + 1;
            Properties.Settings.Default.Scroll = value;
            Properties.Settings.Default.Save();
        }
        private void ScaleSensitivityChanged(object sender, EventArgs e)
        {
            int value = this.scaleBar.Value + 1;
            Properties.Settings.Default.Scale = value;
            Properties.Settings.Default.Save();
        }

    }
}
