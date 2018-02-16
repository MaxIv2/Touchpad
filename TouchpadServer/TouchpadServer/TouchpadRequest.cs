using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouchpadServer {
    class TouchpadRequest {
        public enum ActionType { Move = 0, Left = 1, Right = 2, Scroll = 3, Zoom = 4 }
        public static int[] ActionParamLength = { 2, 1, 1, 1, 1 };
        public int length { get; private set; }
        public ActionType type { get; private set; }
        private byte[] args;

        public TouchpadRequest (byte[] data, int index) {
            this.type = (ActionType)data[1];
            this.length = data[0];
            this.args = new byte[length-1];
            Array.Copy(data, index+2, args, 0, length-1);
        }

        public byte GetArgumentAt(int index) {
            return this.args[index];
        }
    }
}
