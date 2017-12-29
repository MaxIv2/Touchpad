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
        public enum ActionType { Move = 0, Left = 1, Right = 2, Scroll = 3, Zoom = 4}

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
                byte length = clientInput.Dequeue();
                while (length > clientInput.Count)
                    Thread.Sleep(5);
                byte type = clientInput.Dequeue();
                switch ((ActionType)type) {
                    case ActionType.Move:
                        MouseController.Move(clientInput.Dequeue(), clientInput.Dequeue());
                        break;
                    case ActionType.Left:
                        MouseController.Left(Convert.ToBoolean(clientInput.Dequeue()));
                        break;
                    case ActionType.Right:
                        MouseController.Right(Convert.ToBoolean(clientInput.Dequeue()));
                        break;
                }
                if (clientInput.Count == 0)
                    done = true;
            }
        }
    }
}
