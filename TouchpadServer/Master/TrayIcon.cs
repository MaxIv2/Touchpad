using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Master {
    class TrayIcon : Form{
        private PictureBox pictureBox1;
        private NotifyIcon trayIcon;

        public TrayIcon() {
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Remote touchpad settings";
            trayIcon.Icon = Master.Properties.Resources.mouseIcon;
            trayIcon.Visible = true;
            trayIcon.Click += OnIconClick;
            trayIcon.DoubleClick += this.OnDoubleClick;
        }

        protected override void OnLoad(EventArgs e) {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.
            this.ControlBox = false;
            base.OnLoad(e);
        }

        public void OnDoubleClick(object sender, EventArgs e) {
            this.Close();
        }

        private void SetVisibilty(bool visible) {
            this.Visible = visible;
            this.ShowInTaskbar = visible;
        }

        public void OnIconClick(object sender, EventArgs e) {
            SetVisibilty(!this.Visible);
        }

        private void InitializeComponent() {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(112, 58);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // TrayIcon
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.pictureBox1);
            this.Name = "TrayIcon";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
