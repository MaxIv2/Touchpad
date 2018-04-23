using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouchpadServer {
    sealed class ConnectionStatusChangedEventArgs : EventArgs {
        public enum ConnectionStatus { CONNECTED, DISCONNECTED, OFFLINE }
        public ConnectionStatus status { get; private set; }
        public string endpointRepresentation {get; private set;}

        public ConnectionStatusChangedEventArgs(ConnectionStatus status, string endpointRepresentation) {
            this.status = status;
            this.endpointRepresentation = endpointRepresentation;
        }
    }
}
