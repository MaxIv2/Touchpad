using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TouchpadServer {
    class GlobalAppEvents {
        public static event EventHandler Online;
        public static event EventHandler Offline;
        public static event EventHandler Disconnected;
        public static event EventHandler Connected;
        public static event EventHandler OfflineRequest;
        public static event EventHandler OnlineRequest;
        public static event EventHandler DisconnectReqeust;
        public static event EventHandler ExitRequest;
        public static event EventHandler SwitchConnectionTypeRequest;
        public static event EventHandler SwitchedConnectionType;

        public static void RaiseOnlineEvent(object sender, EventArgs e) {
            if (OnlineRequest != null) 
                Online(sender, e);
            Debug.WriteLine("Online");
        }
        public static void RaiseOfflineEvent(object sender, EventArgs e) {
            if (OnlineRequest != null) 
                Offline(sender, e);
            Debug.WriteLine("Offline");
        }
        public static void RaiseDisconnectedEvent(object sender, EventArgs e) {
            if (OnlineRequest != null) 
                Disconnected(sender, e);
            Debug.WriteLine("Disconnected");
        }
        public static void RaiseConnectedEvent(object sender, EventArgs e) {
            if (OnlineRequest != null)
                Connected(sender, e);
            Debug.WriteLine("Connected");
        }
        public static void RaiseOfflineRequestEvent(object sender, EventArgs e) {
            if (OnlineRequest != null)
                OfflineRequest(sender, e);
            Debug.WriteLine("OfflineRequest");
        }
        public static void RaiseOnlineRequestEvent(object sender, EventArgs e) {
            if(OnlineRequest != null)
                OnlineRequest(sender, e);
            Debug.WriteLine("OnlineRequest");
        }
        public static void RaiseDisconnectReqeustEvent(object sender, EventArgs e) {
            if (DisconnectReqeust != null)
                DisconnectReqeust(sender, e);
            Debug.WriteLine("DisconnectReqeust");
        }
        public static void RaiseExitRequest(object sender, EventArgs e) {
            if (ExitRequest != null)
                ExitRequest(sender, e);
            Debug.WriteLine("ExitRequest");
        }
        public static void RaiseSwitchConnectionTypeRequestEvent(object sender, EventArgs e) {
            if (SwitchConnectionTypeRequest != null) {
                SwitchConnectionTypeRequest(sender, e);
            }
            Debug.WriteLine("SwitchConnectionTypeRequest!");
        }
        public static void RaiseSwitchedConnectionTypeEvent(object sender, EventArgs e) {
            if (SwitchedConnectionType != null)
                SwitchedConnectionType(sender, e);
            Debug.WriteLine("SwitchedConnectionType");
        }
    }
}
