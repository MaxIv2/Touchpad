using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Web.Script.Serialization;
using System.Net.Http;


namespace TouchpadServer {
    static class InputHandler {
        private static Queue<Command> commandsToExecute;
        private static Queue<Command> commandsToSend;
        private static Thread worker;

        private static void ProcessAndValidate(byte[] input) {
            if (commandsToExecute == null) {
                commandsToExecute = new Queue<Command>();
                commandsToSend = new Queue<Command>();
            }
            byte[] batch = (byte[])input;
            int i = 0;
            while (i < batch.Length) {
                byte action = batch[i];
                if (!Command.ActionCode.IsDefined(typeof(Command.ActionCode), action))
                    i = batch.Length;
                else {
                    Command c;
                    switch ((Command.ActionCode)action) {
                        case Command.ActionCode.MOVE:
                            if (batch.Length - i >= 3) { // 2 bytes: dx,dy + 1 type byte, 3 IN TOTAL
                                c = new Command.Move((sbyte)batch[i + 1], (sbyte)batch[i + 2]);
                                EnqueueCommand(c);
                                i += 3;
                            }
                            else {
                                i = batch.Length;
                            }
                            break;
                        case Command.ActionCode.LEFTBUTTON:
                            if (batch.Length - i >= 2) { // 1 byte: status + 1 type byte, 2 IN TOTAL
                                c = new Command.LeftButton(batch[i + 1]);
                                EnqueueCommand(c);
                                i += 2;
                            }
                            else {
                                i = batch.Length;
                            }
                            break;
                        case Command.ActionCode.RIGHTBUTTON:
                            if (batch.Length - i >= 2) { // 1 byte: status + 1 type byte, 2 IN TOTAL
                                c = new Command.RightButton(batch[i + 1]);
                                EnqueueCommand(c);
                                i += 2;
                            }
                            else {
                                i = batch.Length;
                            }
                            break;
                        case Command.ActionCode.SCROLL:
                            if (batch.Length - i >= 2) {  // 1 byte: data + 1 type byte, 2 IN TOTAL
                                c = new Command.Scroll((sbyte)batch[i + 1]);
                                EnqueueCommand(c);
                                i += 2;
                            }
                            else {
                                i = batch.Length;
                            }
                            break;
                        case Command.ActionCode.ZOOM:
                            if (batch.Length - i >= 2) {  // 1 byte: data + 1 type byte, 2 IN TOTAL
                                c = new Command.Zoom((sbyte)batch[i + 1]);
                                EnqueueCommand(c);
                                i += 2;
                            }
                            else {
                                i = batch.Length;
                            }
                            break;
                    }
                }
            }
            NotifyThread();
        }

        private static void EnqueueCommand(Command c) {
            commandsToExecute.Enqueue(c);
            ReportClient.Instance.EnqueueCommand(c);
        }

        private static void NotifyThread() {
            if (worker == null || !worker.IsAlive) {
                worker = new Thread(HandleInput);
                worker.Start();
            }
        }

        public static void HandleOnNewDataEvent(object sender, byte[] batch) {
            ProcessAndValidate(batch);
        }

        private static void HandleInput() {
            while (commandsToExecute.Count > 0) {
                switch (commandsToExecute.Peek().code) {
                    case Command.ActionCode.MOVE:
                        Command.Move move = (Command.Move)commandsToExecute.Dequeue();
                        MouseController.Move(move.dx * Properties.Settings.Default.Move, move.dy * Properties.Settings.Default.Move);
                        break;
                    case Command.ActionCode.LEFTBUTTON:
                        Command.LeftButton left = (Command.LeftButton)commandsToExecute.Dequeue();
                        MouseController.Left(left.state);
                        break;
                    case Command.ActionCode.RIGHTBUTTON:
                        Command.RightButton right = (Command.RightButton)commandsToExecute.Dequeue();
                        MouseController.Right(right.state);
                        break;
                    case Command.ActionCode.SCROLL:
                        Command.Scroll scroll = (Command.Scroll)commandsToExecute.Dequeue();
                        MouseController.Scroll(scroll.scroll);
                        break;
                    case Command.ActionCode.ZOOM:
                        Command.Zoom zoom = (Command.Zoom)commandsToExecute.Dequeue();
                        MouseController.Zoom(zoom.zoom);
                        break;
                }
            }
        }
    }
}

