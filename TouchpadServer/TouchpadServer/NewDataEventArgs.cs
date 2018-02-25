using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouchpadServer {
    class NewDataEventArgs : EventArgs {
        public Queue<byte> info { get; private set; }
        public NewDataEventArgs(Queue<byte> info) {
            this.info = info;
        }
    }
}
