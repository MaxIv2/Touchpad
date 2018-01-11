using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TouchpadServer {
    class InputHandler {
        private Queue<byte> clientInput;
        public bool done { get; private set; }
        public enum ActionType { Move = 0, Left = 1, Right = 2, Scroll = 3, Zoom = 4 }
        public static int[] ActionParamLength = { 2, 1, 1, 1, 1 };
        private DiagnosticsReporter reporter;

        public InputHandler() {
            this.reporter = new DiagnosticsReporter("");
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
                byte type = clientInput.Dequeue();
                if (length <= clientInput.Count) {
                    switch ((ActionType)type) {
                        case ActionType.Move:
                            sbyte dx = (sbyte)clientInput.Dequeue();
                            sbyte dy = (sbyte)clientInput.Dequeue();
                            MouseController.Move(dx, dy);
                            reporter.AddItem(new Item(type, new Description.MoveDescription(dx, dy)));
                            break;
                        case ActionType.Left:
                            byte state = clientInput.Dequeue();
                            MouseController.Left(state);
                            reporter.AddItem(new Item(type, new Description.ButtonEventDescription(state)));
                            break;
                        case ActionType.Right:
                            state = clientInput.Dequeue();
                            MouseController.Right(state);
                            reporter.AddItem(new Item(type, new Description.ButtonEventDescription(state)));
                            break;
                    }
                }
                else {
                    done = true;
                }
                if (clientInput.Count == 0)
                    done = true;
            }
        }
    }
}
