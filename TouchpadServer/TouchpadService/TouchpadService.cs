using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Runtime.InteropServices;

namespace TouchpadService {
    public partial class TouchpadService : ServiceBase {
        private int port;
        private string ip;
        private TcpListener server;

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
        public enum ServiceState {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        }; 

        public TouchpadService() {
            InitializeComponent();
        }

        protected override void OnStart(string[] args) {
            SetServiceStatus(ServiceState.SERVICE_START_PENDING);
            SetListener();
            TouchpadServerThread thread = new TouchpadServerThread(this.server);
            thread.Start();
            SetServiceStatus(ServiceState.SERVICE_RUNNING);
        }

        protected override void OnStop() {
            SetServiceStatus(ServiceState.SERVICE_STOP_PENDING);
            TouchpadServerThread thread = new TouchpadServerThread(this.server);
            thread.Stop();
            SetServiceStatus(ServiceState.SERVICE_STOPPED);
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

        public void SetListener() {
            this.ip = GetLocalIPAddress();
            string[] splitIP = ip.Split('.');
            byte[] address = new byte[4];
            for (int i = 0; i < 4; i++) {
                address[i] = byte.Parse(splitIP[i]);
            }
            IPEndPoint localEP = new IPEndPoint(new IPAddress(address), 0);
            this.server = new TcpListener(localEP);
            server.Start();
            this.port = (server.LocalEndpoint as IPEndPoint).Port;
            server.Stop();
            SetRegistryValue("port", this.port);
            SetRegistryValue("ip", this.ip);
        }

        public void SetServiceStatus(ServiceState serviceState, int dwWaitHint = 10000) {
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = serviceState;
            serviceStatus.dwWaitHint = dwWaitHint;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        public void SetRegistryValue(string valueName, object value) {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Touchpad");
            key.SetValue(valueName, value);
            key.Close();
        }
    }
}
