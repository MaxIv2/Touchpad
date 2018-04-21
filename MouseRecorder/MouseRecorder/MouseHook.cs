using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MouseRecorder
{
    static class MouseHook
    {
        public static event EventHandler MouseActionLD = delegate { };
        public static event EventHandler MouseActionRD = delegate { };
        public static event EventHandler MouseActionMD = delegate { };
        public static event EventHandler MouseActionLU = delegate { };
        public static event EventHandler MouseActionRU = delegate { };
        public static event EventHandler MouseActionMU = delegate { };

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,     //The left mouse button was pressed.
            WM_LBUTTONUP = 0x0202,       //The left mouse button was released.
            WM_RBUTTONDOWN = 0x0204,     //The right mouse button was pressed.
            WM_RBUTTONUP = 0x0205,       //The right mouse button was released.
            WM_MBUTTONDOWN = 0x0207,     //The middle mouse button was pressed.
            WM_MBUTTONUP = 0x0208,       //The middle mouse button was released.

            WM_MOUSEWHEEL = 0x020A,

            //most likely useless:
            MK_CONTROL = 0x0008,
            WM_MOUSEMOVE = 0x0200,
            WM_LBUTTONDBLCLK = 0x0203,   //The left mouse button was double-clicked.
            WM_RBUTTONDBLCLK = 0x0206,   //The right mouse button was double-clicked.
            WM_MBUTTONDBLCLK = 0x0209    //The middle mouse button was double-clicked.
        }

        private static IntPtr HookCallback(
          int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                MouseActionLD(null, new EventArgs());
            }

            if (nCode >= 0 && MouseMessages.WM_RBUTTONDOWN == (MouseMessages)wParam)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                MouseActionRD(null, new EventArgs());
            }

            if (nCode >= 0 && MouseMessages.WM_MBUTTONDOWN == (MouseMessages)wParam)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                MouseActionMD(null, new EventArgs());
            }

            if (nCode >= 0 && MouseMessages.WM_LBUTTONUP == (MouseMessages)wParam)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                MouseActionLU(null, new EventArgs());
            }

            if (nCode >= 0 && MouseMessages.WM_RBUTTONUP == (MouseMessages)wParam)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                MouseActionRU(null, new EventArgs());
            }

            if (nCode >= 0 && MouseMessages.WM_MBUTTONUP == (MouseMessages)wParam)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                MouseActionMU(null, new EventArgs());
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        #region

        private static LowLevelMouseProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        public static void Start()
        {
            _hookID = SetHook(_proc);
        }

        public static void stop()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc,
                  GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private const int WH_MOUSE_LL = 14;

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
          LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
          IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        #endregion
    }
}
