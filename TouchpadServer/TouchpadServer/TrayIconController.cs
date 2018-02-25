using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TouchpadServer {
    class TrayIconController : IDisposable {
        private NotifyIcon trayIcon;
        private ContextMenu contextMenu;
        private bool disposed;
        private EventHandler exitApplication;
        private string MACAddress;

        public TrayIconController(EventHandler exitApplicaion, string MACAddress) {
            MenuItem[] menuItems = { new MenuItem("Exit", exitApplicaion) };
            this.contextMenu = new ContextMenu(menuItems);
            this.trayIcon = new NotifyIcon();
            this.trayIcon.Text = "Remote touchpad settings";
            this.trayIcon.ContextMenu = contextMenu;
            this.trayIcon.Icon = Resources.mouseBlack;
            this.trayIcon.Visible = true;
            this.trayIcon.Click += OnIconClick;
            this.exitApplication += exitApplication;
            this.MACAddress = MACAddress;
        }

        public void OnIconClick(object sender, EventArgs e) {
            SettingsWindow t = new SettingsWindow(this.MACAddress);
            t.Show();
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;
            if (disposing) {
                //managed
                contextMenu.Dispose();
                trayIcon.Dispose();
            }
            disposed = true;
        }
    }
}
