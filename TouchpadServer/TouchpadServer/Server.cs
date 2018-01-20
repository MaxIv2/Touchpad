using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace TouchpadServer {
    class Server {
        private TcpListener listener;
        private bool terminateThread;
        public string ip { get; private set; }
        public int port { get; private set; }
        InputHandler inputHandler;

        public Server() {
            SetListener();
            inputHandler = new InputHandler();
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
                Thread.Sleep(1000);
            }
        }

        private void HandleClient(Socket client)
        {
            byte[] buffer;
            int dataLen;
            while (!this.terminateThread) {
                if (client.Available > 0) {
                    dataLen = client.Available;
                    buffer = new byte[dataLen];
                    client.Receive(buffer);
                    inputHandler.AddToQueue(buffer);
                }
                if (!client.Connected)
                    break;
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
            Properties.Settings.Default.ip = this.ip;
            Properties.Settings.Default.port = this.port;
        }
    }
}
