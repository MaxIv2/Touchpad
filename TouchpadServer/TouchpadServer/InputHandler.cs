using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TouchpadServer {
    class InputHandler {
        private Queue<TouchpadRequest> clientInput;
        public bool done { get; private set; }
        private DiagnosticsReporter reporter;

        public InputHandler() {
            this.reporter = new DiagnosticsReporter("http://localhost");
            this.clientInput = new Queue<TouchpadRequest>();
            this.done = true;
        }

        public static void EnqueueRawRequests(Queue<TouchpadRequest> queue, byte[] data) {
            int nextDataSetIndex = 0;
            while (nextDataSetIndex < data.Length) {
                TouchpadRequest request = new TouchpadRequest(data, nextDataSetIndex);
                queue.Enqueue(request);
                nextDataSetIndex += request.length + 1;//+1 because length excludes itself
            }
        }

        public void AddToQueue(byte[] buffer) {
            EnqueueRawRequests(clientInput, buffer);
            if (done) {
                Thread th = new Thread(HandleInput);
                done = false;
                th.Start();
            }
        }

        public void HandleInput() {
            while (!done) {
                TouchpadRequest request = clientInput.Dequeue();
                switch ((TouchpadRequest.ActionType)request.type) {
                    case TouchpadRequest.ActionType.Move:
                        sbyte dx = (sbyte)request.GetArgumentAt(0);
                        sbyte dy = (sbyte)request.GetArgumentAt(1);
                        MouseController.Move(dx, dy);

                        reporter.AddItem(new Item((byte)request.type, new Description.MoveDescription(dx, dy)));
                        break;
                    case TouchpadRequest.ActionType.Left:
                        byte state = request.GetArgumentAt(0);
                        MouseController.Left(state);
                        reporter.AddItem(new Item((byte)request.type, new Description.ButtonEventDescription(state)));
                        break;
                    case TouchpadRequest.ActionType.Right:
                        state = request.GetArgumentAt(0);
                        MouseController.Right(state);
                        reporter.AddItem(new Item((byte)request.type, new Description.ButtonEventDescription(state)));
                        break;
                    case TouchpadRequest.ActionType.Zoom:
                        sbyte zoom = (sbyte)request.GetArgumentAt(0);;
                        MouseController.Zoom(zoom);
                        reporter.AddItem(new Item((byte)request.type, new Description.ZoomDescription(zoom)));
                        break;
                    case TouchpadRequest.ActionType.Scroll:
                        sbyte scroll = (sbyte)request.GetArgumentAt(0); ;
                        MouseController.Scroll(scroll);
                        reporter.AddItem(new Item((byte)request.type, new Description.ScrollDescription(scroll)));
                        break;
                }
                if (clientInput.Count == 0)
                    done = true;
            }
        }
    }
}
