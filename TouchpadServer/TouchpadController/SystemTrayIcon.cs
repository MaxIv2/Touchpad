using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace TouchpadController {
    class SystemTrayIcon {
        private NotifyIcon trayIcon;

        public SystemTrayIcon() {
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Remote touchpad settings";
            //trayIcon.Icon = TouchpadController.Proper
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

        public void OnIconClick(object sender, EventArgs e) {
            //SetVisibilty(!this.Visible);
        }
    }
}
