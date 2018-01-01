using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TouchpadServer {
    class TrayIconController {
        private NotifyIcon trayIcon;
        private ContextMenu contextMenu;

        public TrayIconController(EventHandler exitApplicaion) {
            MenuItem[] menuItems = { new MenuItem("Exit", exitApplicaion) };
            this.contextMenu = new ContextMenu(menuItems);
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Remote touchpad settings";
            trayIcon.ContextMenu = contextMenu;
            trayIcon.Icon = Properties.Resources.mouseIcon;
            trayIcon.Visible = true;
            trayIcon.Click += OnIconClick;
        }

        public void OnIconClick(object sender, EventArgs e) {
            SettingsWindow t = new SettingsWindow();
            t.Show();
        }

        public void Dispose() {
        }
    }
}
