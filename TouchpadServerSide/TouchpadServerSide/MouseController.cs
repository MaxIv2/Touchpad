using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;

namespace TouchpadServerSide {
    class MouseController {
        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, UIntPtr dwExtraInfo);
        private enum Flag { ABSOLUTE, LEFTDOWN, LEFTUP, MIDDLEDOWN, MIDDLEUP, MOVE, RIGHTDOWN, RIGHTUP };
        private static int GetFlag(Flag mf) {
            switch (mf) {
                case Flag.ABSOLUTE:
                    return 0x8000;
                case Flag.LEFTDOWN:
                    return 0x0002;
                case Flag.LEFTUP:
                    return 0x0004;
                case Flag.MIDDLEDOWN:
                    return 0x0020;
                case Flag.MIDDLEUP:
                    return 0x0040;
                case Flag.MOVE:
                    return 0x0001;
                case Flag.RIGHTDOWN:
                    return 0x0008;
                case Flag.RIGHTUP:
                    return 0x0010;
                default:
                    return 0;
            }
        }
        public static void RightClick() {
            mouse_event(GetFlag(Flag.RIGHTDOWN) ^ GetFlag(Flag.RIGHTUP), 0, 0, 0, new UIntPtr(0));
        }
        public static void LeftClick() {
            mouse_event(GetFlag(Flag.LEFTDOWN) ^ GetFlag(Flag.LEFTUP), 0, 0, 0, new UIntPtr(0));
        }
        public static void MiddleCLick() {
            mouse_event(GetFlag(Flag.MIDDLEDOWN) ^ GetFlag(Flag.MIDDLEUP), 0, 0, 0, new UIntPtr(0));
        }
        public static void MoveCursor(int dx, int dy) {
            mouse_event(GetFlag(Flag.MOVE), dx, dy, 0, new UIntPtr());
        }
    }
}
