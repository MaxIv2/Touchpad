namespace TouchpadServer {
    partial class MainWindow {
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
            this.QRCodeContainer = new System.Windows.Forms.PictureBox();
            this.onlineSwitch = new TouchpadServer.SwitchButton();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.blackListButton = new System.Windows.Forms.Button();
            this.switchConnectionType = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.mouseSensitivity = new System.Windows.Forms.TrackBar();
            this.scrollSensitivity = new System.Windows.Forms.TrackBar();
            this.zoomSensitivity = new System.Windows.Forms.TrackBar();
            this.mouseSensLabel = new System.Windows.Forms.Label();
            this.scrollSensLabel = new System.Windows.Forms.Label();
            this.zoomSensLabel = new System.Windows.Forms.Label();
            this.sendDataCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.QRCodeContainer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mouseSensitivity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scrollSensitivity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomSensitivity)).BeginInit();
            this.SuspendLayout();
            // 
            // QRCodeContainer
            // 
            this.QRCodeContainer.Location = new System.Drawing.Point(8, 16);
            this.QRCodeContainer.Name = "QRCodeContainer";
            this.QRCodeContainer.Size = new System.Drawing.Size(270, 270);
            this.QRCodeContainer.TabIndex = 0;
            this.QRCodeContainer.TabStop = false;
            // 
            // onlineSwitch
            // 
            this.onlineSwitch.Location = new System.Drawing.Point(443, 16);
            this.onlineSwitch.Name = "onlineSwitch";
            this.onlineSwitch.Size = new System.Drawing.Size(78, 23);
            this.onlineSwitch.TabIndex = 1;
            this.onlineSwitch.Text = "onlineSwitch";
            this.onlineSwitch.UseVisualStyleBackColor = true;
            // 
            // disconnectButton
            // 
            this.disconnectButton.Location = new System.Drawing.Point(292, 75);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(229, 23);
            this.disconnectButton.TabIndex = 2;
            this.disconnectButton.Text = "Disconnect";
            this.disconnectButton.UseVisualStyleBackColor = true;
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(412, 246);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(109, 23);
            this.exitButton.TabIndex = 3;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            // 
            // blackListButton
            // 
            this.blackListButton.Location = new System.Drawing.Point(292, 246);
            this.blackListButton.Name = "blackListButton";
            this.blackListButton.Size = new System.Drawing.Size(114, 23);
            this.blackListButton.TabIndex = 4;
            this.blackListButton.Text = "Blacklist";
            this.blackListButton.UseVisualStyleBackColor = true;
            // 
            // switchConnectionType
            // 
            this.switchConnectionType.Location = new System.Drawing.Point(292, 16);
            this.switchConnectionType.Name = "switchConnectionType";
            this.switchConnectionType.Size = new System.Drawing.Size(145, 23);
            this.switchConnectionType.TabIndex = 5;
            this.switchConnectionType.Text = "Switch Connection Type";
            this.switchConnectionType.UseVisualStyleBackColor = true;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(289, 51);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(176, 13);
            this.statusLabel.TabIndex = 6;
            this.statusLabel.Text = "Connected to : 192.168.1.8 : 55555";
            // 
            // mouseSensitivity
            // 
            this.mouseSensitivity.SetRange(1, 10);
            this.mouseSensitivity.Location = new System.Drawing.Point(335, 106);
            this.mouseSensitivity.Name = "mouseSensitivity";
            this.mouseSensitivity.Size = new System.Drawing.Size(186, 45);
            this.mouseSensitivity.TabIndex = 7;
            // 
            // scrollSensitivity
            // 
            this.scrollSensitivity.Location = new System.Drawing.Point(335, 157);
            this.scrollSensitivity.Name = "scrollSensitivity";
            this.scrollSensitivity.Size = new System.Drawing.Size(186, 45);
            this.scrollSensitivity.TabIndex = 8;
            // 
            // zoomSensitivity
            // 
            this.zoomSensitivity.Location = new System.Drawing.Point(335, 208);
            this.zoomSensitivity.Name = "zoomSensitivity";
            this.zoomSensitivity.Size = new System.Drawing.Size(186, 45);
            this.zoomSensitivity.TabIndex = 9;
            // 
            // mouseSensLabel
            // 
            this.mouseSensLabel.AutoSize = true;
            this.mouseSensLabel.Location = new System.Drawing.Point(289, 107);
            this.mouseSensLabel.Name = "mouseSensLabel";
            this.mouseSensLabel.Size = new System.Drawing.Size(54, 26);
            this.mouseSensLabel.TabIndex = 10;
            this.mouseSensLabel.Text = "Mouse\nSensitivity";
            // 
            // scrollSensLabel
            // 
            this.scrollSensLabel.AutoSize = true;
            this.scrollSensLabel.Location = new System.Drawing.Point(288, 157);
            this.scrollSensLabel.Name = "scrollSensLabel";
            this.scrollSensLabel.Size = new System.Drawing.Size(54, 26);
            this.scrollSensLabel.TabIndex = 11;
            this.scrollSensLabel.Text = "Scroll\nSensitivity";
            // 
            // zoomSensLabel
            // 
            this.zoomSensLabel.AutoSize = true;
            this.zoomSensLabel.Location = new System.Drawing.Point(288, 208);
            this.zoomSensLabel.Name = "zoomSensLabel";
            this.zoomSensLabel.Size = new System.Drawing.Size(54, 26);
            this.zoomSensLabel.TabIndex = 12;
            this.zoomSensLabel.Text = "Zoom\nSensitivity";
            // 
            // sendDataCheckBox
            // 
            this.sendDataCheckBox.AutoSize = true;
            this.sendDataCheckBox.Checked = global::TouchpadServer.Properties.Settings.Default.SendToServer;
            this.sendDataCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sendDataCheckBox.Location = new System.Drawing.Point(291, 275);
            this.sendDataCheckBox.Name = "sendDataCheckBox";
            this.sendDataCheckBox.Size = new System.Drawing.Size(119, 17);
            this.sendDataCheckBox.TabIndex = 15;
            this.sendDataCheckBox.Text = "Send data to server";
            this.sendDataCheckBox.UseVisualStyleBackColor = true;
            this.sendDataCheckBox.CheckedChanged += new System.EventHandler(this.sendDataCheckBox_CheckedChanged);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 310);
            this.Controls.Add(this.sendDataCheckBox);
            this.Controls.Add(this.blackListButton);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.zoomSensLabel);
            this.Controls.Add(this.scrollSensLabel);
            this.Controls.Add(this.mouseSensLabel);
            this.Controls.Add(this.zoomSensitivity);
            this.Controls.Add(this.scrollSensitivity);
            this.Controls.Add(this.mouseSensitivity);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.switchConnectionType);
            this.Controls.Add(this.disconnectButton);
            this.Controls.Add(this.onlineSwitch);
            this.Controls.Add(this.QRCodeContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainWindow";
            this.Text = "Touchpad";
            ((System.ComponentModel.ISupportInitialize)(this.QRCodeContainer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mouseSensitivity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scrollSensitivity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomSensitivity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.PictureBox QRCodeContainer;
        private SwitchButton onlineSwitch;
        private System.Windows.Forms.Button disconnectButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button blackListButton;
        private System.Windows.Forms.Button switchConnectionType;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.TrackBar mouseSensitivity;
        private System.Windows.Forms.TrackBar scrollSensitivity;
        private System.Windows.Forms.TrackBar zoomSensitivity;
        private System.Windows.Forms.Label mouseSensLabel;
        private System.Windows.Forms.Label scrollSensLabel;
        private System.Windows.Forms.Label zoomSensLabel;
        private System.Windows.Forms.CheckBox sendDataCheckBox;
    }
}