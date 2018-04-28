using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            ApplicationEvents.connectionStatusChangedEventHandler += ChangeAppearance;
            ContextMenuStrip menu = new ContextMenuStrip();
            Tuple<string, EventHandler>[] items = { new Tuple<string, EventHandler>("Blacklist", LaunchBlacklistWindow),
                                                      new Tuple<string,EventHandler> ("Log in", LaunchLoginWindow),
                                                      new Tuple<string,EventHandler>("Exit", ApplicationEvents.CallUserExitRequestEventHandler)};
            foreach (Tuple<string, EventHandler> item in items) {
                menu.Items.Add(item.Item1, null, item.Item2);
            }
            this.trayIcon.ContextMenuStrip = menu;
        }

        private void LaunchLoginWindow(object sender, EventArgs e) {
            LoginForm a = LoginForm.Form;
            a.Show();
        }

        private void ChangeAppearance(object sender, ConnectionStatusChangedEventArgs e) {
            switch (e.status) {
                case ConnectionStatusChangedEventArgs.ConnectionStatus.OFFLINE:
                    this.trayIcon.Icon = Properties.Resources.mouseRed;
                    break;
                case ConnectionStatusChangedEventArgs.ConnectionStatus.DISCONNECTED:
                    this.trayIcon.Icon = Properties.Resources.mouseBlack;
                    break;
                case ConnectionStatusChangedEventArgs.ConnectionStatus.CONNECTED:
                    this.trayIcon.Icon = Properties.Resources.mouseGreen;
                    break;
            } 
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
                ApplicationEvents.connectionStatusChangedEventHandler -= ChangeAppearance;
                trayIcon.Visible = false;
                trayIcon.Dispose();
            }
            instance = null;
            disposed = true;
        }
    }
}
