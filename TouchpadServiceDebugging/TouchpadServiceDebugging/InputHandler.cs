using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TouchpadServiceDebugging {
    class InputHandler {
        private Queue<byte> clientInput;
        public bool done { get; private set; }
        public InputHandler() {
            this.clientInput = new Queue<byte>();
            this.done = true;
        }

        public void AddToQueue(byte[] buffer) {
            foreach (byte b in buffer) {
                clientInput.Enqueue(b);
            }
            if (done) {
                Thread th = new Thread(HandleInput);
                done = false;
                th.Start();
            }
        }

        public void HandleInput() {
            while (!done) {
                byte b = clientInput.Dequeue();
                //do something
                Console.WriteLine(b);
                if (clientInput.Count == 0)
                    done = true;
            }
        }
    }
}
