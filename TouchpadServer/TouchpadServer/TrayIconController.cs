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

        public TrayIconController(EventHandler exitApplicaion) {
            MenuItem[] menuItems = { new MenuItem("Exit", exitApplicaion) };
            this.contextMenu = new ContextMenu(menuItems);
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Remote touchpad settings";
            trayIcon.ContextMenu = contextMenu;
            trayIcon.Icon = Properties.Resources.mouseIcon;
            trayIcon.Visible = true;
            trayIcon.Click += OnIconClick;
            this.exitApplication += exitApplication;
        }

        public void OnIconClick(object sender, EventArgs e) {
            SettingsWindow t = new SettingsWindow();
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
