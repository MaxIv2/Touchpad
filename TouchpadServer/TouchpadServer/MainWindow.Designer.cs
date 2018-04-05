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
            this.diconnectButton = new System.Windows.Forms.Button();
            this.serverStatus = new System.Windows.Forms.Label();
            this.exitButton = new System.Windows.Forms.Button();
            this.blacklistButton = new System.Windows.Forms.Button();
            this.switchConnectionType = new System.Windows.Forms.Button();
            this.onAndOffButtonSwitch = new TouchpadServer.SwitchButton();
            ((System.ComponentModel.ISupportInitialize)(this.QRCodeContainer)).BeginInit();
            this.SuspendLayout();
            // 
            // QRCodeContainer
            // 
            this.QRCodeContainer.Location = new System.Drawing.Point(13, 12);
            this.QRCodeContainer.MaximumSize = new System.Drawing.Size(250, 50);
            this.QRCodeContainer.MinimumSize = new System.Drawing.Size(250, 250);
            this.QRCodeContainer.Name = "QRCodeContainer";
            this.QRCodeContainer.Size = new System.Drawing.Size(250, 250);
            this.QRCodeContainer.TabIndex = 0;
            this.QRCodeContainer.TabStop = false;
            // 
            // diconnectButton
            // 
            this.diconnectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.diconnectButton.Location = new System.Drawing.Point(306, 109);
            this.diconnectButton.Name = "diconnectButton";
            this.diconnectButton.Size = new System.Drawing.Size(424, 36);
            this.diconnectButton.TabIndex = 1;
            this.diconnectButton.Text = "Disconnect";
            this.diconnectButton.UseVisualStyleBackColor = true;
            // 
            // serverStatus
            // 
            this.serverStatus.AutoSize = true;
            this.serverStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.serverStatus.Location = new System.Drawing.Point(301, 56);
            this.serverStatus.Name = "serverStatus";
            this.serverStatus.Size = new System.Drawing.Size(185, 50);
            this.serverStatus.TabIndex = 2;
            this.serverStatus.Text = "Status (___ based):\nOffline";
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(306, 242);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(124, 23);
            this.exitButton.TabIndex = 4;
            this.exitButton.Text = "Exit Remote Touchpad";
            this.exitButton.UseVisualStyleBackColor = true;
            // 
            // blacklistButton
            // 
            this.blacklistButton.Location = new System.Drawing.Point(306, 151);
            this.blacklistButton.Name = "blacklistButton";
            this.blacklistButton.Size = new System.Drawing.Size(124, 23);
            this.blacklistButton.TabIndex = 7;
            this.blacklistButton.Text = "Blacklist";
            this.blacklistButton.UseVisualStyleBackColor = true;
            // 
            // switchConnectionType
            // 
            this.switchConnectionType.Location = new System.Drawing.Point(516, 242);
            this.switchConnectionType.Name = "switchConnectionType";
            this.switchConnectionType.Size = new System.Drawing.Size(213, 23);
            this.switchConnectionType.TabIndex = 8;
            this.switchConnectionType.Text = "Switch Connection Type";
            this.switchConnectionType.UseVisualStyleBackColor = true;
            // 
            // onAndOffButtonSwitch
            // 
            this.onAndOffButtonSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.onAndOffButtonSwitch.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.onAndOffButtonSwitch.Location = new System.Drawing.Point(642, 12);
            this.onAndOffButtonSwitch.Name = "onAndOffButtonSwitch";
            this.onAndOffButtonSwitch.Size = new System.Drawing.Size(87, 23);
            this.onAndOffButtonSwitch.TabIndex = 3;
            this.onAndOffButtonSwitch.Text = "On/Off";
            this.onAndOffButtonSwitch.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 277);
            this.Controls.Add(this.switchConnectionType);
            this.Controls.Add(this.blacklistButton);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.onAndOffButtonSwitch);
            this.Controls.Add(this.serverStatus);
            this.Controls.Add(this.diconnectButton);
            this.Controls.Add(this.QRCodeContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "Touchpad";
            ((System.ComponentModel.ISupportInitialize)(this.QRCodeContainer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox QRCodeContainer;
        private System.Windows.Forms.Button diconnectButton;
        private System.Windows.Forms.Label serverStatus;
        private TouchpadServer.SwitchButton onAndOffButtonSwitch;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button blacklistButton;
        private System.Windows.Forms.Button switchConnectionType;
    }
}