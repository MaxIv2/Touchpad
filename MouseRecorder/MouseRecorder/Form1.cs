using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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

        #endregion

        #region Form1 Events

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            a = 0;
            b = 0;
        }

        #endregion

        #region Button Clicks

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
                        if (spl.Length == 2 && Regex.IsMatch(spl[0], @"^\d+$") && Regex.IsMatch(spl[1], @"^\d+$"))
                        {
                            lv = new ListViewItem(spl[0]);
                            lv.SubItems.Add(spl[1]);
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
                            sw.WriteLine("{0}{1}{2}", item.SubItems[0].Text, '\t', item.SubItems[1].Text);
                        }
                    }
                }
            }
        }

        #endregion

        #region Timer Ticks

        private void timer1_Tick(object sender, EventArgs e)
        {
            lv = new ListViewItem(Cursor.Position.X.ToString());
            lv.SubItems.Add(Cursor.Position.Y.ToString());
            listView1.Items.Add(lv);
            b++;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (a != b)
            {
                Cursor.Position = new Point(int.Parse(listView1.Items[a].SubItems[0].Text), int.Parse(listView1.Items[a].SubItems[1].Text));
                a++;
            }
        }

        #endregion

    }
}
