using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using QRCode;


namespace TouchpadServer {
    public partial class MainWindow : Form {
        private static MainWindow form;
        public static MainWindow Form {
            get {
                if (form == null)
                    form = new MainWindow();
                return form;
            }
        }
        public MainWindow() {
            InitializeComponent();
            SetLabel();
            GlobalAppEvents.ExitRequest += this.Exit;
            GlobalAppEvents.SwitchedConnectionType += OnSwitchedConnectionType;
            GlobalAppEvents.Connected += OnStatusChanged;
            GlobalAppEvents.Disconnected += OnStatusChanged;
            GlobalAppEvents.Offline += OnStatusChanged;
            GlobalAppEvents.Online += OnStatusChanged;
            this.disconnectButton.Click += GlobalAppEvents.RaiseDisconnectReqeustEvent;
            this.blackListButton.Click += blackListButton_Click;
            SetBars();
            SetUpClicks();
            SetupQR();
        }

        void blackListButton_Click(object sender, EventArgs e) {
            if (MainContext.ServerStatus != MainContext.Status.Connected)
                return;
            string address = MainContext.ClientEP;
            if (!Properties.Settings.Default.bluetooth)
                address = address.Split(':')[0];
            GlobalAppEvents.RaiseDisconnectReqeustEvent(this, new EventArgs());
            BlacklistManager.Insert(address);
        }

        private void SetLabel() {
            string text = Properties.Settings.Default.bluetooth ? "(Bluetooth)" : "(WiFi)";
            System.Diagnostics.Debug.WriteLine(Properties.Settings.Default.bluetooth);
            switch (MainContext.ServerStatus) {
                case MainContext.Status.Online:
                    this.statusLabel.Text = text + "Online";
                    break;
                case MainContext.Status.Offline:
                    this.statusLabel.Text = text + "Offline";
                    break;
                case MainContext.Status.Connected:
                    string msg = text + "Connected to: ";
                    msg += MainContext.ClientEP;
                    this.statusLabel.Text = msg;
                    break;

            }
        }
        private void sendDataCheckBox_CheckedChanged(object sender, System.EventArgs e) {
            Properties.Settings.Default.SendToServer = this.sendDataCheckBox.Checked;
            Properties.Settings.Default.Save();
        }
        delegate void a();
        private void OnStatusChanged(object sender, EventArgs e) {
            if (this.statusLabel.InvokeRequired) {
                this.Invoke(new a(SetLabel), new object[] { });
            }
            else {
                SetLabel();
            }
        }

        void OnSwitchedConnectionType(object sender, EventArgs e) {
            SetupQR();
            if (this.statusLabel.InvokeRequired) {
                this.Invoke(new a(SetLabel), new object[] { });
            }
            else {
                SetLabel();
            }
        }

        private void SetUpClicks() {
            this.exitButton.Click += GlobalAppEvents.RaiseExitRequest;
            this.disconnectButton.Click += GlobalAppEvents.RaiseDisconnectedEvent;
            this.switchConnectionType.Click += GlobalAppEvents.RaiseSwitchConnectionTypeRequestEvent;
        }

        private void SetupQR() {
            int size = QRCodeContainer.Size.Width;
            Bitmap image = QRCode.QRCode.Generate(MainContext.ServerEndpointRepr,size, QRCodeImpl.ErrorCorrectionLevels.L);
            this.QRCodeContainer.BackgroundImage = image;
            this.QRCodeContainer.Size = this.QRCodeContainer.BackgroundImage.Size;
            this.QRCodeContainer.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void Exit(object sender, EventArgs e)
        {
            this.Close();
        }
        protected override void OnClosed(EventArgs e) {
            form = null;
            base.OnClosed(e);
        }
        private void SetBars() {
            this.mouseSensitivity.Value = Properties.Settings.Default.Move;
            this.scrollSensitivity.Value = Properties.Settings.Default.Scroll;
            this.zoomSensitivity.Value = Properties.Settings.Default.Zoom;
            this.mouseSensitivity.ValueChanged += this.MoveSensitivityChanged;
            this.scrollSensitivity.ValueChanged += this.ZoomSensitivityChanged;
            this.zoomSensitivity.ValueChanged += this.ScrollSensitivityChanged;
        }
        private void MoveSensitivityChanged(object sender, EventArgs e)
        {
            int value = this.mouseSensitivity.Value;
            Properties.Settings.Default.Move = value;
            Properties.Settings.Default.Save();
        }
        private void ScrollSensitivityChanged(object sender, EventArgs e)
        {
            int value = this.scrollSensitivity.Value;
            Properties.Settings.Default.Scroll = value;
            Properties.Settings.Default.Save();
        }
        private void ZoomSensitivityChanged(object sender, EventArgs e) {
            int value = this.zoomSensitivity.Value;
            Properties.Settings.Default.Zoom = value;
            Properties.Settings.Default.Save();
        }

    }
}
