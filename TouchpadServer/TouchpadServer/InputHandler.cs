using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace TouchpadServer {
    class InputHandler {
        static object locker = new object();

        public static void HandleOnNewDataEvent(object sender, NewDataEventArgs e) {
            Thread th = new Thread(() => ProcessInput(e));
            th.Start();
        }

        private static void ProcessInput(object e) {
            lock (locker) {
                Queue<byte[]> actionData = ((NewDataEventArgs)e).info;
                Debug.WriteLine("Trying to control mouse");
                int lastIndx = -1;
                byte[] previousBatch = null;
                int lengthNow = actionData.Count;
                for (int i = 0; i < lengthNow; i++) {
                    byte[] batch = actionData.Dequeue();
                    lastIndx = ProccessBatch(batch, previousBatch, lastIndx);
                    previousBatch = batch;
                }
            }
        }

        public static int ProccessBatch(byte[] batch, byte[] previousBatch = null, int lastIndx = -1) {
            byte[] merged;
            if (lastIndx == -1) {
                merged = batch;
                lastIndx = 0;
            }
            else
                merged = Merge(lastIndx, previousBatch, batch);
            
            bool notEnoughBytes = false;
            int i = 0;
            while (i < merged.Length && !notEnoughBytes) {
                byte action = merged[i];
                switch ((MouseEvent.ActionCode)action) {
                    case MouseEvent.ActionCode.MOVE:
                        if (merged.Length - i >= 3) { // 2 bytes: dx,dy + 1 type byte, 3 IN TOTAL
                            sbyte dx = (sbyte)merged[i + 1];
                            sbyte dy = (sbyte)merged[i + 2];
                            Thread.Sleep(1);
                            MouseController.Move(dx, dy);
                            i += 3;
                        }
                        else {
                            notEnoughBytes = true;
                        }
                        break;
                    case MouseEvent.ActionCode.LEFTBUTTON:
                        if (merged.Length - i >= 2) { // 1 byte: status + 1 type byte, 2 IN TOTAL
                            byte status = merged[i + 1];
                            MouseController.Left(status);
                            i += 2;
                        }
                        else {
                            notEnoughBytes = true;
                        }
                        break;
                    case MouseEvent.ActionCode.RIGHTBUTTON:
                        if (merged.Length - i >= 2) { // 1 byte: status + 1 type byte, 2 IN TOTAL
                            byte status = merged[i + 1];
                            MouseController.Right(status);
                            i += 2;
                        }
                        else {
                            notEnoughBytes = true;
                        }
                        break;
                    case MouseEvent.ActionCode.SCROLL:
                        if (merged.Length - i >= 2) {  // 1 byte: data + 1 type byte, 2 IN TOTAL
                            sbyte scroll = (sbyte)merged[i + 1];
                            MouseController.Scroll(scroll);
                            i += 2;
                        }
                        else {
                            notEnoughBytes = true;
                        }
                        break;
                    case MouseEvent.ActionCode.ZOOM:
                        if (merged.Length - i >= 2) {  // 1 byte: data + 1 type byte, 2 IN TOTAL
                            sbyte zoom = (sbyte)merged[i + 1];
                            MouseController.Zoom(zoom);
                            i += 2;
                        }
                        else {
                            notEnoughBytes = true;
                        }
                        break;
                }
            }
            if (!notEnoughBytes)
                return 0;
            return i - lastIndx;
        }

        public static byte[] Merge(int startIndx, byte[] first, byte[] second) {
            byte[] merged = new byte[second.Length + first.Length - startIndx];
            Array.Copy(first, startIndx, merged, 0, first.Length - startIndx);
            Array.Copy(second, 0, merged, first.Length - startIndx, second.Length);
            return merged;
        }

    }
}
