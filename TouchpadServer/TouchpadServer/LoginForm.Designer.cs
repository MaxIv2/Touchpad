namespace TouchpadServer {
    partial class LoginForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.loginButton = new System.Windows.Forms.Button();
            this.passwordInput = new System.Windows.Forms.TextBox();
            this.usernameInput = new System.Windows.Forms.TextBox();
            this.registerLink = new System.Windows.Forms.LinkLabel();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(16, 58);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(181, 23);
            this.loginButton.TabIndex = 11;
            this.loginButton.Text = "loginButton";
            this.loginButton.UseVisualStyleBackColor = true;
            // 
            // passwordInput
            // 
            this.passwordInput.Location = new System.Drawing.Point(97, 32);
            this.passwordInput.Name = "passwordInput";
            this.passwordInput.PasswordChar = '*';
            this.passwordInput.Size = new System.Drawing.Size(100, 20);
            this.passwordInput.TabIndex = 10;
            // 
            // usernameInput
            // 
            this.usernameInput.Location = new System.Drawing.Point(97, 6);
            this.usernameInput.Name = "usernameInput";
            this.usernameInput.Size = new System.Drawing.Size(100, 20);
            this.usernameInput.TabIndex = 9;
            // 
            // registerLink
            // 
            this.registerLink.AutoSize = true;
            this.registerLink.Location = new System.Drawing.Point(17, 100);
            this.registerLink.Name = "registerLink";
            this.registerLink.Size = new System.Drawing.Size(70, 13);
            this.registerLink.TabIndex = 2;
            this.registerLink.TabStop = true;
            this.registerLink.Text = "Register here";
            this.registerLink.Links.Add(0, this.registerLink.Text.Length, "https://cryptic-lowlands-58542.herokuapp.com/register");

            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(13, 35);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(56, 13);
            this.passwordLabel.TabIndex = 7;
            this.passwordLabel.Text = "Password:";
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Location = new System.Drawing.Point(12, 9);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(58, 13);
            this.usernameLabel.TabIndex = 6;
            this.usernameLabel.Text = "Username:";
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(211, 120);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.passwordInput);
            this.Controls.Add(this.usernameInput);
            this.Controls.Add(this.registerLink);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.usernameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.Text = "Log in";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.TextBox passwordInput;
        private System.Windows.Forms.TextBox usernameInput;
        private System.Windows.Forms.LinkLabel registerLink;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Label usernameLabel;
    }
}