using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TouchpadServer {
    public partial class LoginForm : Form {
        private static LoginForm form;

        public static LoginForm Form {
            get {
                if (form == null) {
                    form = new LoginForm();
                }
                return form;
            }
        }

        private LoginForm() {
            InitializeComponent();
            this.loginButton.Click += loginButton_Click;
            this.registerLink.LinkClicked += registerLink_Click;
        }

        private void registerLink_Click(object sender, LinkLabelLinkClickedEventArgs e) {
            string target = e.Link.LinkData as string;
            if (target != null)
                System.Diagnostics.Process.Start(target);
        }

        private void loginButton_Click(object sender, EventArgs e) {
            TryRegister();
        }

        protected override void OnClosed(EventArgs e) {
            form = null;
            base.OnClosed(e);
        }

        private async Task TryRegister() {
            bool result = await ReportClient.Instance.Authenticate(usernameInput.Text, passwordInput.Text);
            if (result == false) {
                MessageBox.Show("Log in failed!");
            }
            else {
                this.Close();
            }
        }
    }
}
