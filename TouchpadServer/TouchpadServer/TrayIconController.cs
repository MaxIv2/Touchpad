using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TouchpadServer {
    class TrayIconController : IDisposable {
        private bool windowIsOpen;
        private NotifyIcon trayIcon;
        private EventHandler exitApplication;
        private string MACAddress;
        private bool disposed;

        public TrayIconController(EventHandler exitApplicaion, string MACAddress) {
            this.trayIcon = new NotifyIcon();
            MenuItem[] menuItems = { new MenuItem("Exit", exitApplicaion) };
            this.trayIcon.ContextMenu = new ContextMenu(menuItems);
            this.trayIcon.Text = "Remote Touchpad";
            this.trayIcon.Icon = Resources.mouseBlack;
            this.trayIcon.Visible = true;
            this.trayIcon.Click += this.IconClick;
            this.exitApplication += this.exitApplication;
            this.MACAddress = MACAddress;
        }

        public void IconClick(object sender, EventArgs e) {
            if (!windowIsOpen) {
                SettingsWindow t = new SettingsWindow(this.MACAddress);
                t.Show();
                this.windowIsOpen = true;
                t.FormClosed += this.FormClosed;
            }
        }

        public void FormClosed(object sender, EventArgs e) {
            this.windowIsOpen = false;
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
