using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace TouchpadServer {
    static class Program {
        static Mutex mutex = new Mutex(true, "Touchpad one instance only");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            if (mutex.WaitOne(TimeSpan.Zero, true)) {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainContext());
                mutex.ReleaseMutex();
            }
            else {
                MessageBox.Show("Already running!");
            }
        }
    }
}
