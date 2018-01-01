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
using System.IO;

namespace TouchpadService {
    public partial class TouchpadService : ServiceBase {
        private const string path = "C:\\Users\\Maxim2\\Documents\\Touchpad\\TouchpadServer\\TouchpadController\\bin\\Debug\\TouchpadController.exe";//path of tray icon app
        private TouchpadServer server;

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
            this.server = new TouchpadServer();
            this.server.Start();
            SetServiceStatus(ServiceState.SERVICE_RUNNING);
        }

        protected override void OnStop() {
            SetServiceStatus(ServiceState.SERVICE_STOP_PENDING);
            this.server.Stop();
            SetServiceStatus(ServiceState.SERVICE_STOPPED);
        }

        public void SetServiceStatus(ServiceState serviceState, int dwWaitHint = 10000) {
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = serviceState;
            serviceStatus.dwWaitHint = dwWaitHint;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }
    }
}
