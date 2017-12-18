using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseRecorder
{
    public partial class Form1 : Form
    {
        #region Parameters

        ListViewItem lv;
        int a, b;

        private enum MouseAction
        {
            Move,
            Left_Down,
            Right_Down,
            Middle_Down,
            Left_UP,
            Right_Up,
            Middle_Up
        }

        #endregion

        #region Form1 initialize

        public Form1()
        {
            InitializeComponent();

            MouseHook.Start();
            MouseHook.MouseActionLD += new EventHandler(OnLeftDown);
            MouseHook.MouseActionRD += new EventHandler(OnRightDown);
            MouseHook.MouseActionMD += new EventHandler(OnMiddleDown);
            MouseHook.MouseActionLU += new EventHandler(OnLeftUp);
            MouseHook.MouseActionRU += new EventHandler(OnRightUp);
            MouseHook.MouseActionMU += new EventHandler(OnMiddleUp);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            a = 0;
            b = 0;
            Scale_Label.Text = "Screen Size : " + SystemInformation.VirtualScreen.Width + "x" + SystemInformation.VirtualScreen.Height;
        }

        #endregion

        #region Buttons

        private void Record_Button_Click(object sender, EventArgs e)
        {
            timer2.Stop();
            timer1.Start();
        }

        private void Stop_Button_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
        }

        private void Play_Button_Click(object sender, EventArgs e)
        {
            a = 0;
            timer1.Stop();
            timer2.Start();
        }

        private void Clear_Button_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            a = 0;
            b = 0;
            timer1.Stop();
            timer2.Stop();
        }

        private void Load_Button_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
            string[] spl = null;
            string filename = "";
            OpenFileDialog sfd = new OpenFileDialog();

            sfd.Title = "LoadFileDialog ImportFromFile";
            sfd.Filter = "Text File (.txt) | *.txt";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                filename = sfd.FileName.ToString();
                if (filename != "")
                {
                    string[] lines = System.IO.File.ReadAllLines(filename);
                    foreach (string line in lines)
                    {
                        spl = line.Split('\t');
                        if (spl.Length == 3 && Regex.IsMatch(spl[0], @"^\d+$") && Regex.IsMatch(spl[1], @"^\d+$") && Enum.IsDefined(typeof(MouseAction), spl[2]))
                        {
                            lv = new ListViewItem(spl[0]);
                            lv.SubItems.Add(spl[1]);
                            if (b == 0){
                                lv.SubItems.Add("0");
                                lv.SubItems.Add("0");
                            }else{
                                lv.SubItems.Add((int.Parse(spl[0]) - int.Parse(listView1.Items[b-1].SubItems[0].Text)).ToString());
                                lv.SubItems.Add((int.Parse(spl[1]) - int.Parse(listView1.Items[b - 1].SubItems[1].Text)).ToString());
                            }
                            lv.SubItems.Add(spl[2]);
                            listView1.Items.Add(lv);
                            b++;
                        }
                    }
                }
            }
        }

        private void Save_Button_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
            string filename = "";
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Title = "SaveFileDialog Export2File";
            sfd.Filter = "Text File (.txt) | *.txt";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                filename = sfd.FileName.ToString();
                if (filename != "")
                {
                    using (StreamWriter sw = new StreamWriter(filename))
                    {
                        foreach (ListViewItem item in listView1.Items)
                        {
                            sw.WriteLine("{0}{1}{2}{3}{4}", item.SubItems[0].Text, '\t', item.SubItems[1].Text, '\t', item.SubItems[4].Text);
                        }
                    }
                }
            }
        }

        #endregion

        #region Timers

        private void timer1_Tick(object sender, EventArgs e)
        {
            lv = new ListViewItem(Cursor.Position.X.ToString());
            lv.SubItems.Add(Cursor.Position.Y.ToString());
            if (b == 0){
                lv.SubItems.Add("0");
                lv.SubItems.Add("0");
            }else{
                lv.SubItems.Add((int.Parse(lv.SubItems[0].Text) - int.Parse(listView1.Items[b - 1].SubItems[0].Text)).ToString());
                lv.SubItems.Add((int.Parse(lv.SubItems[1].Text) - int.Parse(listView1.Items[b - 1].SubItems[1].Text)).ToString());
            }
            lv.SubItems.Add(MouseAction.Move.ToString());
            listView1.Items.Add(lv);
            b++;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (a != b)
            {
                Cursor.Position = new Point(int.Parse(listView1.Items[a].SubItems[0].Text), int.Parse(listView1.Items[a].SubItems[1].Text));
                //Cursor.Position = new Point(Cursor.Position.X + int.Parse(listView1.Items[a].SubItems[2].Text), Cursor.Position.Y + int.Parse(listView1.Items[a].SubItems[3].Text));              // moution by relative coordinates works poorly for dual monitor with 2 diffrent resolutions.
                a++;
            }
        }

        #endregion

        #region OnClick Event Heandlers

        private void OnLeftDown(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
                return;
            lv = new ListViewItem(Cursor.Position.X.ToString());
            lv.SubItems.Add(Cursor.Position.Y.ToString());
            if (b == 0){
                lv.SubItems.Add("0");
                lv.SubItems.Add("0");
            }else{
                lv.SubItems.Add((int.Parse(lv.SubItems[0].Text) - int.Parse(listView1.Items[b - 1].SubItems[0].Text)).ToString());
                lv.SubItems.Add((int.Parse(lv.SubItems[1].Text) - int.Parse(listView1.Items[b - 1].SubItems[1].Text)).ToString());
            }
            lv.SubItems.Add(MouseAction.Left_Down.ToString());
            listView1.Items.Add(lv);
            b++;
        }

        private void OnRightDown(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
                return;
            lv = new ListViewItem(Cursor.Position.X.ToString());
            lv.SubItems.Add(Cursor.Position.Y.ToString());
            if (b == 0)
            {
                lv.SubItems.Add("0");
                lv.SubItems.Add("0");
            }
            else
            {
                lv.SubItems.Add((int.Parse(lv.SubItems[0].Text) - int.Parse(listView1.Items[b - 1].SubItems[0].Text)).ToString());
                lv.SubItems.Add((int.Parse(lv.SubItems[1].Text) - int.Parse(listView1.Items[b - 1].SubItems[1].Text)).ToString());
            }
            lv.SubItems.Add(MouseAction.Right_Down.ToString());
            listView1.Items.Add(lv);
            b++;
        }

        private void OnMiddleDown(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
                return;
            lv = new ListViewItem(Cursor.Position.X.ToString());
            lv.SubItems.Add(Cursor.Position.Y.ToString());
            if (b == 0)
            {
                lv.SubItems.Add("0");
                lv.SubItems.Add("0");
            }
            else
            {
                lv.SubItems.Add((int.Parse(lv.SubItems[0].Text) - int.Parse(listView1.Items[b - 1].SubItems[0].Text)).ToString());
                lv.SubItems.Add((int.Parse(lv.SubItems[1].Text) - int.Parse(listView1.Items[b - 1].SubItems[1].Text)).ToString());
            }
            lv.SubItems.Add(MouseAction.Middle_Down.ToString());
            listView1.Items.Add(lv);
            b++;
        }

        private void OnLeftUp(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
                return;
            lv = new ListViewItem(Cursor.Position.X.ToString());
            lv.SubItems.Add(Cursor.Position.Y.ToString());
            if (b == 0)
            {
                lv.SubItems.Add("0");
                lv.SubItems.Add("0");
            }
            else
            {
                lv.SubItems.Add((int.Parse(lv.SubItems[0].Text) - int.Parse(listView1.Items[b - 1].SubItems[0].Text)).ToString());
                lv.SubItems.Add((int.Parse(lv.SubItems[1].Text) - int.Parse(listView1.Items[b - 1].SubItems[1].Text)).ToString());
            }
            lv.SubItems.Add(MouseAction.Left_UP.ToString());
            listView1.Items.Add(lv);
            b++;
        }

        private void OnRightUp(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
                return;
            lv = new ListViewItem(Cursor.Position.X.ToString());
            lv.SubItems.Add(Cursor.Position.Y.ToString());
            if (b == 0)
            {
                lv.SubItems.Add("0");
                lv.SubItems.Add("0");
            }
            else
            {
                lv.SubItems.Add((int.Parse(lv.SubItems[0].Text) - int.Parse(listView1.Items[b - 1].SubItems[0].Text)).ToString());
                lv.SubItems.Add((int.Parse(lv.SubItems[1].Text) - int.Parse(listView1.Items[b - 1].SubItems[1].Text)).ToString());
            }
            lv.SubItems.Add(MouseAction.Right_Up.ToString());
            listView1.Items.Add(lv);
            b++;
        }

        private void OnMiddleUp(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
                return;
            lv = new ListViewItem(Cursor.Position.X.ToString());
            lv.SubItems.Add(Cursor.Position.Y.ToString());
            if (b == 0)
            {
                lv.SubItems.Add("0");
                lv.SubItems.Add("0");
            }
            else
            {
                lv.SubItems.Add((int.Parse(lv.SubItems[0].Text) - int.Parse(listView1.Items[b - 1].SubItems[0].Text)).ToString());
                lv.SubItems.Add((int.Parse(lv.SubItems[1].Text) - int.Parse(listView1.Items[b - 1].SubItems[1].Text)).ToString());
            }
            lv.SubItems.Add(MouseAction.Middle_Up.ToString());
            listView1.Items.Add(lv);
            b++;
        }

        #endregion
    }

    public static class MouseHook
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
            if (nCode >= 0 && MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam){
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                MouseActionLD(null, new EventArgs());
            }

            if (nCode >= 0 && MouseMessages.WM_RBUTTONDOWN == (MouseMessages)wParam){
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                MouseActionRD(null, new EventArgs());
            }

            if (nCode >= 0 && MouseMessages.WM_MBUTTONDOWN == (MouseMessages)wParam){
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

//roll up/down
//play clicks