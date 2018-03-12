using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace TouchpadServer {
    class InputHandler {
		public static void SetThreadPool() {
            if(!ThreadPool.SetMaxThreads(1, 3))
				Debug.WriteLine("Failed to set ThreadPool's max threads");
			if(!ThreadPool.SetMinThreads(0,1))
				Debug.WriteLine("Failed to set ThreadPool's min threads");
		}
		
        public static void HandleOnNewDataEvent(object sender, NewDataEventArgs e) {
            ThreadPool.QueueUserWorkItem(ProcessInput, e);
        }

        private static void ProcessInput(object e) {
            Queue<byte> actionData = ((NewDataEventArgs)e).info;
            //Debug.WriteLine("Trying to control mouse");
            bool notEnoughBytes = false;
            while(!notEnoughBytes && actionData.Count > 0) {
                byte actionCode = actionData.Peek();
                switch ((MouseEvent.ActionCode)actionCode) {
                    case MouseEvent.ActionCode.MOVE:
                        if (actionData.Count >= 3) { // 2 bytes: dx,dy + 1 type byte, 3 IN TOTAL
                            try {
                                actionData.Dequeue();
                                sbyte dx = (sbyte)actionData.Dequeue();
                                sbyte dy = (sbyte)actionData.Dequeue();
                                MouseController.Move(dx, dy);
                                System.Threading.Thread.Sleep(5);
                            } catch (Exception exception) {
                                Debug.WriteLine("Error occured: " + exception.Message);
                            }
                        }
                        else
                            notEnoughBytes = true;
                        break;
                    case MouseEvent.ActionCode.LEFTBUTTON:
                        if (actionData.Count >= 2) { // 1 byte: status + 1 type byte, 2 IN TOTAL
                            actionData.Dequeue();
                            byte status = actionData.Dequeue();
                            MouseController.Left(status);
                        }
                        else
                            notEnoughBytes = true;
                        break;
                    case MouseEvent.ActionCode.RIGHTBUTTON:
                        if (actionData.Count >= 2) { // 1 byte: status + 1 type byte, 2 IN TOTAL
                            actionData.Dequeue();
                            byte status = actionData.Dequeue();
                            MouseController.Right(status);
                        }
                        else
                            notEnoughBytes = true;
                        break;
                    case MouseEvent.ActionCode.SCROLL:
                        if (actionData.Count >= 2) {  // 1 byte: data + 1 type byte, 2 IN TOTAL
                            actionData.Dequeue();
                            sbyte scroll = (sbyte)actionData.Dequeue();
                            MouseController.Scroll(scroll);
                        }
                        else
                            notEnoughBytes = true;
                        break;
                    case MouseEvent.ActionCode.ZOOM:
                        if (actionData.Count >= 2) {  // 1 byte: data + 1 type byte, 2 IN TOTAL
                            actionData.Dequeue();
                            sbyte zoom = (sbyte)actionData.Dequeue();
                            MouseController.Zoom(zoom);
                        }
                        else
                            notEnoughBytes = true;
                        break;
                }
            }
        }
    }
}
