using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;


namespace QRCodeConnect {
    class MouseServer {
        private int port;
        private string ip;
        public MouseServer() {
            Socket serverSocket = new Socket(SocketType.Dgram, ProtocolType.IP);
            this.ip = GetLocalIPAddress();
            string[] splitIP = ip.Split('.');
            byte[] address = new byte[4];
            for (int i = 0; i < 4; i++) {
                address[i] = byte.Parse(splitIP[i]);
            }
            IPEndPoint localEP = new IPEndPoint(new IPAddress(address), 0);
            serverSocket.Bind(localEP);
            this.port = (serverSocket.LocalEndPoint as IPEndPoint).Port;
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
    }
}
