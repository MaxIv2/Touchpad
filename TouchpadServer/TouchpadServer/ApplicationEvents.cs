using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouchpadServer {
    static class ApplicationEvents {
        public static event EventHandler<NewDataEventArgs> newDataEventDataEventHandler;
        public static event EventHandler<ConnectionStatusChangedEventArgs> connectionStatusChangedEventHandler;
        public static event EventHandler<EventArgs> userDisconnectRequestEventHandler;
        public static event EventHandler<EventArgs> userExitRequestEventHandler;
        public static event EventHandler<EventArgs> turnOnOffEventHandler;

        public static void CallNewEventDataEventHandler(object sender, NewDataEventArgs e) {
            newDataEventDataEventHandler(sender, e);
        }

        public static void CallConnectionStatusChangedEventHandler(object sender, ConnectionStatusChangedEventArgs e) {
            if(connectionStatusChangedEventHandler != null)
                connectionStatusChangedEventHandler(sender, e);
        }
        public static void CallUserDisconnectRequestEventHandler(object sender, EventArgs e) {
			if(userDisconnectRequestEventHandler != null)
				userDisconnectRequestEventHandler(sender, e);
        }

        public static void CallUserExitRequestEventHandler(object sender, EventArgs e) {
			if(userExitRequestEventHandler != null)
				userExitRequestEventHandler(sender, e);
        }
        public static void CallTurnOnOffEventHandlerHandler(object sender, EventArgs e) {
            if(turnOnOffEventHandler != null)
				turnOnOffEventHandler(sender, e);
        }
    }
}
