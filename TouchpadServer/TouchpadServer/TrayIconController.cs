using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MouseRecorder;

namespace TouchpadServer {

    sealed class TrayIconController : IDisposable {
        private NotifyIcon trayIcon;
        private bool disposed;
        private static TrayIconController instance;
        public static TrayIconController Instance {
            get {
                if (instance == null)
                    instance = new TrayIconController();
                return instance;
            }
        }

        private TrayIconController() {
            this.trayIcon = new NotifyIcon();
            this.trayIcon.Text = "Remote Touchpad";
            this.trayIcon.Icon = Properties.Resources.mouseBlack;
            this.trayIcon.Visible = true;
            this.trayIcon.Click += this.IconClick;
            this.SubscribeEvents();
            ContextMenuStrip menu = new ContextMenuStrip();
            Tuple<string, EventHandler>[] items = { new Tuple<string, EventHandler>("Blacklist", LaunchBlacklistWindow),
                                                      new Tuple<string,EventHandler>("Mouse Recorder", LaunchMouseRecorder),
                                                      new Tuple<string,EventHandler> ("Log in", LaunchLoginWindow),
                                                      new Tuple<string,EventHandler>("Exit", GlobalAppEvents.RaiseExitRequest)};
            foreach (Tuple<string, EventHandler> item in items) {
                menu.Items.Add(item.Item1, null, item.Item2);
            }
            this.trayIcon.ContextMenuStrip = menu;
        }

        #region event subscription
        private void SubscribeEvents() {
            GlobalAppEvents.Online += OnOnline;
            GlobalAppEvents.Disconnected += OnOnline;
            GlobalAppEvents.Connected += OnConnected;
            GlobalAppEvents.Offline += OnOffline;
        }
        private void UnsubscribeEvents() {
            GlobalAppEvents.Online += OnOnline;
            GlobalAppEvents.Disconnected += OnOnline;
            GlobalAppEvents.Connected += OnConnected;
            GlobalAppEvents.Offline += OnOffline;
        }

        #endregion

        private void OnOffline(object sender, EventArgs e) {
            this.trayIcon.Icon = Properties.Resources.mouseRed;
        }
        private void OnConnected(object sender, EventArgs e) {
            this.trayIcon.Icon = Properties.Resources.mouseGreen;
        }
        private void OnOnline(object sender, EventArgs e) {
            this.trayIcon.Icon = Properties.Resources.mouseBlack;
        }

        private void LaunchMouseRecorder(object sender, EventArgs e) {
            MouseRecorderWindow window = new MouseRecorderWindow();
            window.Show();
        }

        private void LaunchLoginWindow(object sender, EventArgs e) {
            LoginForm a = LoginForm.Form;
            a.Show();
        }
        private void LaunchBlacklistWindow(object sender, EventArgs e) {
            BlacklistWindow blacklistWindow = BlacklistWindow.Form;
            blacklistWindow.Show();
        }
        private void IconClick(object sender, EventArgs e) {
            MouseEventArgs eventArgs = (MouseEventArgs)e;
            if (eventArgs.Button == MouseButtons.Left) {
                MainWindow settingsWindow = MainWindow.Form;
                settingsWindow.Show();
            }
        }
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing) {
            if (disposed)
                return;
            if (disposing) {
                UnsubscribeEvents();
                trayIcon.Visible = false;
                trayIcon.Dispose();
            }
            instance = null;
            disposed = true;
        }
    }
}
