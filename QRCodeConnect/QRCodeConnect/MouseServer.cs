using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;


namespace QRCodeConnect {
    class MouseServer {
        private int port;
        private string ip;
        private Socket serverSocket;

        public MouseServer() {
            this.serverSocket = new Socket(SocketType.Dgram, ProtocolType.IP);
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

        public void ServerThread() {
            this.serverSocket.Listen(1);
            Socket clientSide = this.serverSocket.Accept();
            byte[] buffer = Encoding.ASCII.GetBytes("Hello World!");
            clientSide.Send(buffer);
            clientSide.Close();
            this.serverSocket.Close();
        }
    }
}
