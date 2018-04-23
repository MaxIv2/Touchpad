using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TouchpadServer {
    sealed class TrayIconController : IDisposable {
        private bool settingsWindowIsOpen;
        private bool blackistWindowIsOpen;
        private NotifyIcon trayIcon;
        private bool disposed;
        private MainWindow settingsWindow;
        private BlacklistWindow blacklistWindow;

        public TrayIconController() {
            this.trayIcon = new NotifyIcon();
            this.trayIcon.Text = "Remote Touchpad";
            this.trayIcon.Icon = Properties.Resources.mouseBlack;
            this.trayIcon.Visible = true;
            this.trayIcon.Click += this.IconClick;
            ApplicationEvents.connectionStatusChangedEventHandler += ChangeAppearance;
            ContextMenuStrip menu = new ContextMenuStrip();
            Tuple<string, EventHandler>[] items = { new Tuple<string, EventHandler>("Blacklist", LaunchBlacklistWindow),
                                                     new Tuple<string,EventHandler>("Exit", ApplicationEvents.CallUserExitRequestEventHandler) };
            foreach (Tuple<string, EventHandler> item in items) {
                menu.Items.Add(item.Item1, null, item.Item2);
            }
            this.trayIcon.ContextMenuStrip = menu;
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
            if (!blackistWindowIsOpen) {
                blacklistWindow = new BlacklistWindow();
                blacklistWindow.Show();
                this.blackistWindowIsOpen = true;
                blacklistWindow.FormClosed += this.FormClosed;
            }
            else {
                blacklistWindow.WindowState = FormWindowState.Normal;
                blacklistWindow.Focus();
                blacklistWindow.BringToFront();
            }
        }

        private void IconClick(object sender, EventArgs e) {
            MouseEventArgs eventArgs = (MouseEventArgs)e;
            if (eventArgs.Button == MouseButtons.Left) {
                if (!settingsWindowIsOpen) {
                    settingsWindow = new MainWindow();
                    settingsWindow.Show();
                    this.settingsWindowIsOpen = true;
                    settingsWindow.FormClosed += this.FormClosed;
                }
                else {
                    settingsWindow.WindowState = FormWindowState.Normal;
                    settingsWindow.Focus();
                    settingsWindow.BringToFront();
                }
            }
        }

        private void FormClosed(object sender, EventArgs e) {
            if (sender is BlacklistWindow)
                this.blackistWindowIsOpen = false;
            else
                this.settingsWindowIsOpen = false;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing) {
            if (disposed)
                return;
            if (disposing) {
                ApplicationEvents.connectionStatusChangedEventHandler -= ChangeAppearance;
                trayIcon.Dispose();
            }
            disposed = true;
        }
    }
}
