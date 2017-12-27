using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouchpadServiceDebugging {
    class MouseController {
        Queue<byte> clientInput;

        public MouseController() {
            this.clientInput = new Queue<byte>();
        }

        public void AddToQueue(byte[] buffer) {
            foreach (byte b in buffer) {
                clientInput.Enqueue(b);
            }
        }
    }
}
