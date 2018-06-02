using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace TouchpadServer {
    class MouseController {
        //(mouse_event flags)
        public enum Flags { Absolute = 0x8000, LeftDown = 0x0002, LeftUp = 0x0004, Move = 0x0001, RightDown = 0x0008, RightUp = 0x0010, Wheel = 0x0800 }
        //(keybd_event flags)
        public const int KEYUP = 2;
        //user32.dll imports
        [DllImport("user32.dll")]
        public static extern void mouse_event(int flags, int dx, int dy, int dwData, int dwExtraInfo);
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public static void Move(int dx, int dy) {
            mouse_event((int)Flags.Move, dx, dy, 0, 0);
        }
        public static void Left(int up) {
            if (up == 0)
                mouse_event((int)Flags.LeftDown, 0, 0, 0, 0);
            else if (up == 1)
                mouse_event((int)Flags.LeftUp, 0, 0, 0, 0);
            else
                mouse_event((int)Flags.LeftDown | (int)Flags.LeftUp, 0, 0, 0, 0);
        }
        public static void Right(int up) {
            if (up == 0)
                mouse_event((int)Flags.RightDown, 0, 0, 0, 0);
            else if (up == 1)
                mouse_event((int)Flags.RightUp, 0, 0, 0, 0);
            else
                mouse_event((int)Flags.RightDown | (int)Flags.RightUp, 0, 0, 0, 0);
        }
        //Zoom and scroll weren't tested yet.
        public static void Zoom(int zoom) {
            keybd_event(VirtualKeys.CONTROL, 0, 0, 0);
            mouse_event((int)Flags.Wheel, 0, 0, zoom, 0);
            keybd_event(VirtualKeys.CONTROL, 0, KEYUP, 0);
        }
        public static void Scroll(int scroll) {
            mouse_event((int)Flags.Wheel, 0, 0, scroll, 0);
        }

        private static class VirtualKeys {
            public const byte CONTROL = 0X11;
        }
    }
}
