using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TouchpadServer {
    class MainContext : ApplicationContext {

        BluetoothServer server;
        TrayIconController icon;

        public delegate void NewDataEventHandler(object sender, NewDataEventArgs e);
        public delegate void DisconnectedEventHandler(object sender, EventArgs e);

        public MainContext() : base() {
            this.server = new BluetoothServer(new Guid(Resources.MyGuid), this.HandleNewData, this.DisconnectedEvent);
            this.icon = new TrayIconController(ExitApplication, BluetoothServer.GetAdaptersMACAddress());
        }

        public void HandleNewData(object sender, NewDataEventArgs e) {
            Queue<byte> actionData = e.info;
            bool notEnoughBytes = false;
            while(!notEnoughBytes || actionData.Count == 0) {
                byte actionCode = actionData.Peek();
                switch ((MouseEvent.ActionCodes)actionCode) {
                    case MouseEvent.ActionCodes.MOVE:
                        if (actionData.Count < 3) { // 2 bytes: dx,dy + 1 type byte, 3 IN TOTAL
                            actionData.Dequeue();
                            sbyte dx = (sbyte)actionData.Dequeue();
                            sbyte dy = (sbyte)actionData.Dequeue();
                            MouseController.Move(dx, dy);
                        }
                        else
                            notEnoughBytes = true;
                        break;
                    case MouseEvent.ActionCodes.LEFTBUTTON:
                        if (actionData.Count < 2) { // 1 byte: status + 1 type byte, 2 IN TOTAL
                            actionData.Dequeue();
                            byte status = actionData.Dequeue();
                            MouseController.Left(status);
                        }
                        else
                            notEnoughBytes = true;
                        break;
                    case MouseEvent.ActionCodes.RIGHTBUTTON:
                        if (actionData.Count < 2) { // 1 byte: status + 1 type byte, 2 IN TOTAL
                            actionData.Dequeue();
                            byte status = actionData.Dequeue();
                            MouseController.Right(status);
                        }
                        else
                            notEnoughBytes = true;
                        break;
                    case MouseEvent.ActionCodes.SCROLL:
                        if (actionData.Count < 2) {  // 1 byte: data + 1 type byte, 2 IN TOTAL
                            actionData.Dequeue();
                            sbyte scroll = (sbyte)actionData.Dequeue();
                            MouseController.Scroll(scroll);
                        }
                        else
                            notEnoughBytes = true;
                        break;
                    case MouseEvent.ActionCodes.ZOOM:
                        if (actionData.Count < 2) {  // 1 byte: data + 1 type byte, 2 IN TOTAL
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

        public void DisconnectedEvent(object sender, EventArgs e) {

        }

        public void ExitApplication(object sender, EventArgs e) {
            this.server.CloseServer();
            this.server.Dispose();
            this.icon.Dispose();
            this.ExitThread();
        }
    }
}
