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
        public static ConnectionStatusChangedEventArgs status;

        public delegate void NewDataEventHandler(object sender, NewDataEventArgs e);
        public delegate void DisconnectedEventHandler(object sender, EventArgs e);

        public MainContext() : base() {
            status = new ConnectionStatusChangedEventArgs(ConnectionStatusChangedEventArgs.ConnectionStatus.OFFLINE, "");
            ApplicationEvents.connectionStatusChangedEventHandler += this.HandleConnectionStatusChanged;
            ApplicationEvents.newDataEventDataEventHandler += this.HandleNewData;
            ApplicationEvents.userExitRequestEventHandler += this.HandleUserExitRequest;
            this.server = new BluetoothServer(new Guid(Resources.MyGuid));
            this.icon = new TrayIconController(HandleUserExitRequest, BluetoothServer.GetAdaptersMACAddress());
            this.server.GoOnline();
        }

        private void HandleConnectionStatusChanged(object sender, ConnectionStatusChangedEventArgs e) {
            status = e;
        }

        //implement this
        public void HandleNewData(object sender, NewDataEventArgs e) {
            Queue<byte> actionData = e.info;
            bool notEnoughBytes = false;
            while(!notEnoughBytes || actionData.Count == 0) {
                byte actionCode = actionData.Peek();
                switch ((MouseEvent.ActionCode)actionCode) {
                    case MouseEvent.ActionCode.MOVE:
                        if (actionData.Count < 3) { // 2 bytes: dx,dy + 1 type byte, 3 IN TOTAL
                            actionData.Dequeue();
                            sbyte dx = (sbyte)actionData.Dequeue();
                            sbyte dy = (sbyte)actionData.Dequeue();
                            MouseController.Move(dx, dy);
                        }
                        else
                            notEnoughBytes = true;
                        break;
                    case MouseEvent.ActionCode.LEFTBUTTON:
                        if (actionData.Count < 2) { // 1 byte: status + 1 type byte, 2 IN TOTAL
                            actionData.Dequeue();
                            byte status = actionData.Dequeue();
                            MouseController.Left(status);
                        }
                        else
                            notEnoughBytes = true;
                        break;
                    case MouseEvent.ActionCode.RIGHTBUTTON:
                        if (actionData.Count < 2) { // 1 byte: status + 1 type byte, 2 IN TOTAL
                            actionData.Dequeue();
                            byte status = actionData.Dequeue();
                            MouseController.Right(status);
                        }
                        else
                            notEnoughBytes = true;
                        break;
                    case MouseEvent.ActionCode.SCROLL:
                        if (actionData.Count < 2) {  // 1 byte: data + 1 type byte, 2 IN TOTAL
                            actionData.Dequeue();
                            sbyte scroll = (sbyte)actionData.Dequeue();
                            MouseController.Scroll(scroll);
                        }
                        else
                            notEnoughBytes = true;
                        break;
                    case MouseEvent.ActionCode.ZOOM:
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

        public void HandleUserExitRequest(object sender, EventArgs e) {
            ApplicationEvents.newDataEventDataEventHandler -= this.HandleNewData;
            ApplicationEvents.newDataEventDataEventHandler -= this.HandleUserExitRequest;
            ApplicationEvents.connectionStatusChangedEventHandler -= this.HandleConnectionStatusChanged;
            this.server.GoOffline();
            this.server.Dispose();
            this.icon.Dispose();
            this.ExitThread();
        }
    }
}
