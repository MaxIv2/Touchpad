using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;

namespace TouchpadService {
    class TouchpadServerThread {
        private TcpListener server;
        private bool terminateThread;

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, UIntPtr dwExtraInfo);

        public TouchpadServerThread(TcpListener server) {
            this.server = server;
            this.terminateThread = false;
        }

        private void Run() {
            server.Start();
            while (!this.terminateThread) {
                if (server.Pending()) {
                    Socket client = server.AcceptSocket();
                    HandleClient(client);
                    server.Stop();
                }
            }
        }

        private void HandleClient(Socket client) {
            byte[] buffer = new byte[5];
            UIntPtr u = new UIntPtr();
            while (!this.terminateThread) {
                if (client.Available >= 5) {
                    client.Receive(buffer);
                    int flags = (int)buffer[0] + ((int)buffer[1] * 256);
                    int dx = (sbyte)buffer[2];
                    int dy = (sbyte)buffer[3];
                    int dwData = (sbyte)buffer[4];
                    mouse_event(flags, dx, dy, dwData, u);
                }
            }
            client.Close();
        }

        public void Start() {
            Thread thread = new Thread(Run);
            thread.Start();
        }

        public void Stop() {
            this.terminateThread = true;
        }
    }
}
