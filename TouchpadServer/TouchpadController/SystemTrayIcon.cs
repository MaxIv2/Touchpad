using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace TouchpadController {
    class SystemTrayIcon : ApplicationContext {
        private NotifyIcon trayIcon;
        private ContextMenu contextMenu;

        public SystemTrayIcon() {
            MenuItem[] menuItems = { new MenuItem("Exit", ExitOnClick) };
            this.contextMenu = new ContextMenu(menuItems);
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Remote touchpad settings";
            trayIcon.ContextMenu = contextMenu;
            trayIcon.Icon = Properties.Resources.mouseIcon;
            trayIcon.Visible = true;
            trayIcon.Click += OnIconClick;
            trayIcon.DoubleClick += this.OnDoubleClick;
        }


        public void OnDoubleClick(object sender, EventArgs e) {
            //this.Close();
        }

        private void SetVisibilty(bool visible) {
            //this.Visible = visible;
            //this.ShowInTaskbar = visible;
        }

        public void ExitOnClick(object sender, EventArgs e) {
            trayIcon.Dispose();
            this.ExitThread();
        }

        public void OnIconClick(object sender, EventArgs e) {
            TouchpadController t = new TouchpadController();
            t.Show();
        }
    }
}
