using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

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
            Left_Up,
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
            timer1.Stop();
            timer2.Stop();
            a = 0;
            b = 0;
            listView1.Items.Clear();
        }

        private void Load_Button_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
            string[] spl = null;
            string filename = "";
            OpenFileDialog sfd = new OpenFileDialog();

            sfd.Title = "Load File Dialog";
            sfd.Filter = "Text file (*.txt)|*.txt|Sqlite files (*.sqlite)|*.sqlite";

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
                                lv.SubItems.Add((int.Parse(spl[0]) - int.Parse(listView1.Items[b - 1].SubItems[0].Text)).ToString());
                                lv.SubItems.Add((int.Parse(spl[1]) - int.Parse(listView1.Items[b - 1].SubItems[1].Text)).ToString());
                            }
                            lv.SubItems.Add(spl[2]);
                            listView1.Items.Add(lv);
                            b++;
                        }
                    }
                }
                if (filename.EndsWith(".sqlite"))
                {
                    SQLiteConnection m_dbConnection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", filename));
                    m_dbConnection.Open();

                    string sql = "select * from MouseRecord";
                    SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (Regex.IsMatch(reader["X"].ToString(), @"^\d+$") && Regex.IsMatch(reader["Y"].ToString(), @"^\d+$") && Enum.IsDefined(typeof(MouseAction), reader["Action"].ToString()))
                        {
                            lv = new ListViewItem(((int)reader["X"]).ToString());
                            lv.SubItems.Add(((int)reader["Y"]).ToString());
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
                            lv.SubItems.Add((string)reader["Action"]);
                            listView1.Items.Add(lv);
                            b++;
                        }
                    }
                    m_dbConnection.Close();
                }
            }
        }

        private void Save_Button_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
            string filename = "";
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Title = "Save File Dialog";
            sfd.Filter = "Text file (*.txt)|*.txt|Sqlite files (*.sqlite)|*.sqlite";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                filename = sfd.FileName.ToString();
                if (filename.EndsWith(".txt"))
                { 
                    using (StreamWriter sw = new StreamWriter(filename)){
                        foreach (ListViewItem item in listView1.Items)
                            sw.WriteLine("{0}{1}{2}{3}{4}", item.SubItems[0].Text, '\t', item.SubItems[1].Text, '\t', item.SubItems[4].Text);
                    }
                }
                if (filename.EndsWith(".sqlite"))
                {
                    SQLiteConnection.CreateFile(filename);
                    SQLiteConnection m_dbConnection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", filename));
                    m_dbConnection.Open();

                    string sql = "create table MouseRecord (X int, Y int, Action char(10))";
                    SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                    command.ExecuteNonQuery();

                    foreach (ListViewItem item in listView1.Items)
                    {
                        sql = string.Format("insert into MouseRecord (X, Y, Action) values ({0}, {1}, '{2}')", item.SubItems[0].Text, item.SubItems[1].Text, item.SubItems[4].Text);
                        command = new SQLiteCommand(sql, m_dbConnection);
                        command.ExecuteNonQuery();
                    }

                    m_dbConnection.Close();
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
            if (a == b)
                return;
            
            if (a == 0){
                Cursor.Position = new Point(int.Parse(listView1.Items[a].SubItems[0].Text), int.Parse(listView1.Items[a].SubItems[1].Text));
                a++;
                return;
            }

            MouseAction act = (MouseAction)Enum.Parse(typeof(MouseAction), listView1.Items[a].SubItems[4].Text);

            switch (act)
            {
                case MouseAction.Move:
                    //MouseController.MoveCursor(int.Parse(listView1.Items[a].SubItems[2].Text), int.Parse(listView1.Items[a].SubItems[3].Text));
                    Cursor.Position = new Point(Cursor.Position.X + int.Parse(listView1.Items[a].SubItems[2].Text), Cursor.Position.Y + int.Parse(listView1.Items[a].SubItems[3].Text));
                    break;
                case MouseAction.Left_Down:
                    MouseController.LeftDown();
                    break;
                case MouseAction.Left_Up:
                    MouseController.LeftUp();
                    break;
                case MouseAction.Right_Down:
                    MouseController.RightDown();
                    break;
                case MouseAction.Right_Up:
                    MouseController.RightUp();
                    break;
                case MouseAction.Middle_Down:
                    MouseController.MiddleDown();
                    break;
                case MouseAction.Middle_Up:
                    MouseController.MiddleUp();
                    break;
            }
            //Cursor.Position = new Point(int.Parse(listView1.Items[a].SubItems[0].Text), int.Parse(listView1.Items[a].SubItems[1].Text));
            a++;
            
        }

        #endregion

        #region OnClick Event Handlers
        
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
            lv.SubItems.Add(MouseAction.Left_Up.ToString());
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
}