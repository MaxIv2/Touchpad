using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TouchpadServer {
    class TrayIconController : IDisposable {
        private bool settingsWindowIsOpen;
        private bool blackistWindowIsOpen;
        private NotifyIcon trayIcon;
        private string MACAddress;
        private bool menuOpen = false;
        private bool disposed;
        private SettingsWindow settingsWindow;
        private BlacklistWindow blacklistWindow;

        public TrayIconController(EventHandler exitApplicaion, string MACAddress) {
            this.trayIcon = new NotifyIcon();
            MenuItem[] menuItems = { new MenuItem("Exit", ApplicationEvents.CallUserExitRequestEventHandler), new MenuItem("Blacklist",LaunchBlacklistWindow) };
            this.trayIcon.ContextMenu = new ContextMenu(menuItems);
            this.trayIcon.Text = "Remote Touchpad";
            this.trayIcon.Icon = Properties.Resources.mouseBlack;
            this.trayIcon.Visible = true;
            this.trayIcon.Click += this.IconClick;
            this.MACAddress = MACAddress;
            ApplicationEvents.connectionStatusChangedEventHandler += ChangeAppearance;
            this.trayIcon.ContextMenu.Popup += setMenuOpen;
            this.trayIcon.ContextMenu.Collapse += setMenuClosed;
        }

        private void setMenuOpen(object sender, EventArgs e) {
            this.menuOpen = true;
        }
        private void setMenuClosed(object sender, EventArgs e) {
            this.menuOpen = false;
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
            this.menuOpen = false;
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
            if (!menuOpen) {
                if (!settingsWindowIsOpen) {
                    settingsWindow = new SettingsWindow(this.MACAddress);
                    settingsWindow.Show();
                    this.settingsWindowIsOpen = true;
                    settingsWindow.FormClosed += this.FormClosed;
                } else {
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

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;
            if (disposing) {
                trayIcon.Dispose();
            }
            disposed = true;
        }
    }
}
