using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TouchpadServer {
    class MyApplicationContext : ApplicationContext {

        private TrayIconController trayIcon;
        private Server server;

        public MyApplicationContext() {
            this.trayIcon = new TrayIconController(ExitApp);
            this.server = new Server();
            this.server.Start();
        }

        public void ExitApp(object sender, EventArgs args) {
            this.trayIcon.Dispose();
            this.server.Stop();
            this.ExitThread();
        }
    }
}
