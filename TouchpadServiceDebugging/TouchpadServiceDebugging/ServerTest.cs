using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;

namespace TouchpadServiceDebugging{
    class ServerTest {
        private TcpListener listener;
        private bool terminateThread;
        public string ip { get; private set; }
        public int port { get; private set; }
        InputHandler mc;

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, UIntPtr dwExtraInfo);

        public ServerTest() {
            SetListener();
            mc = new InputHandler();
            this.terminateThread = false;
        }

        private void Run() {
            listener.Start();
            while (!this.terminateThread) {
                if (listener.Pending()) {
                    Socket client = listener.AcceptSocket();
                    listener.Stop();
                    HandleClient(client);
                }
                Thread.Sleep(100);
            }
        }

        private void HandleClient(Socket client) {
            byte[] buffer;
            int dataLen;
            while (!this.terminateThread) {
                if(client.Available > 0){
                    dataLen = client.Available;
                    buffer = new byte[dataLen];
                    client.Receive(buffer);
                    mc.AddToQueue(buffer);
                }
                Thread.Sleep(50);
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

        private static IPAddress GetLocalIPAddress() {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList) {
                if (ip.AddressFamily == AddressFamily.InterNetwork) {
                    return ip;
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        private void SetListener() {
            IPAddress ipObject = GetLocalIPAddress();
            IPEndPoint localEP = new IPEndPoint(ipObject, 0);
            this.ip = ipObject.ToString();
            this.listener = new TcpListener(localEP);
            listener.Start();
            this.port = (listener.LocalEndpoint as IPEndPoint).Port + 1;
            listener.Stop();
        }
    }
}
