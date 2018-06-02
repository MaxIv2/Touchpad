using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TouchpadServer {
    interface Server {
        bool IsOnline {
            get;
        }
        void GoOnline();
        void GoOffline();
        void Disconnect();
        /// <summary>
        /// Recieves the exact amount of data as the buffer allows.
        /// If there is not enough data to fill the buffer than returns false.
        /// Otherwise, will return true.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        bool RecieveData(byte[] buffer);
        void SendData(byte[] data);
        string ServerEndpointRepr();
        string GetClientEndpoint();
    }
}
