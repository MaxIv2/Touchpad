using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;


namespace TouchpadServerSide {
    class MouseServer {
        private int port;
        private string ip;
        private Socket serverSocket;

        public MouseServer() {
            this.serverSocket = new Socket(SocketType.Stream, ProtocolType.IP);
            this.ip = GetLocalIPAddress();
            string[] splitIP = ip.Split('.');
            byte[] address = new byte[4];
            for (int i = 0; i < 4; i++) {
                address[i] = byte.Parse(splitIP[i]);
            }
            IPEndPoint localEP = new IPEndPoint(new IPAddress(address), 0);
            this.serverSocket.Bind(localEP);
            this.port = (serverSocket.LocalEndPoint as IPEndPoint).Port;
            Thread serverThread = new Thread(ServerThread);
            serverThread.Start();
        }

        public static string GetLocalIPAddress() {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList) {
                if (ip.AddressFamily == AddressFamily.InterNetwork) {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public int GetPort() {
            return this.port;
        }

        public string GetIP() {
            return this.ip;
        }

        public Socket GetServerSocket() {
            return this.serverSocket;
        }

        private void ServerThread() {
            this.serverSocket.Listen(1);
            Socket clientSocket = this.serverSocket.Accept();
            byte[] buffer = new byte[4];
            bool alive = true;
            while (alive) {
                try {
                    clientSocket.Receive(buffer);
                    HandleMovement(buffer);
                } catch (SocketException) {
                    clientSocket.Close();
                    serverSocket.Close();
                }
            }
        }

        private void HandleMovement(byte[] buffer) {
            byte flags = buffer[0];
            sbyte dx = (sbyte)buffer[1];
            sbyte dy = (sbyte)buffer[2];
            sbyte dwData = (sbyte)buffer[3];
            MouseController.MouseEvent(flags, dx, dy, dwData);
        }
    }
}
