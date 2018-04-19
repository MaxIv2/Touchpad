using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouchpadServer {
    static class ApplicationEvents {
        public static event EventHandler<byte[]> newDataEventHandler;
        public static event EventHandler<ConnectionStatusChangedEventArgs> connectionStatusChangedEventHandler;
        public static event EventHandler<bool> userDisconnectRequestEventHandler;
        public static event EventHandler<EventArgs> userExitRequestEventHandler;
        public static event EventHandler<EventArgs> userTurnOnOffRequestHandler;
        public static event EventHandler<EventArgs> connectionTypeChangeRequestHandler;
        public static void CallNewDataEventHandler(object sender, byte[] inputBatches) {
            if (newDataEventHandler != null)
                newDataEventHandler(sender, inputBatches);
        }
        public static void CallConnectionStatusChangedEventHandler(object sender, ConnectionStatusChangedEventArgs e) {
            if(connectionStatusChangedEventHandler != null)
                connectionStatusChangedEventHandler(sender, e);
        }
        public static void CallUserDisconnectRequestEventHandler(object sender, bool blacklist) {
			if(userDisconnectRequestEventHandler != null)
				userDisconnectRequestEventHandler(sender, blacklist);
        }
        public static void CallUserExitRequestEventHandler(object sender, EventArgs e) {
			if(userExitRequestEventHandler != null)
				userExitRequestEventHandler(sender, e);
        }
        public static void CallUserTurnOnOffRequestHandler(object sender, EventArgs e) {
            if (userTurnOnOffRequestHandler != null)
                userTurnOnOffRequestHandler(sender, e);
        }
        public static void CallConnectionTypeChangeRequestHandler(object sender, EventArgs e) {
            if (connectionTypeChangeRequestHandler != null)
                connectionTypeChangeRequestHandler(sender, e);
        }
    }
}
