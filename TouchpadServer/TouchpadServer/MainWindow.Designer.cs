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
            this.moveText = new System.Windows.Forms.Label();
            this.scrollText = new System.Windows.Forms.Label();
            this.scaleText = new System.Windows.Forms.Label();
            this.moveBar = new System.Windows.Forms.TrackBar();
            this.scrollBar = new System.Windows.Forms.TrackBar();
            this.scaleBar = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.QRCodeContainer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.moveBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scrollBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleBar)).BeginInit();
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
            this.exitButton.Location = new System.Drawing.Point(13, 289);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(162, 23);
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
            this.switchConnectionType.Location = new System.Drawing.Point(580, 289);
            this.switchConnectionType.Name = "switchConnectionType";
            this.switchConnectionType.Size = new System.Drawing.Size(149, 23);
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
            // moveText
            // 
            this.moveText.AutoSize = true;
            this.moveText.Location = new System.Drawing.Point(306, 181);
            this.moveText.Name = "moveText";
            this.moveText.Size = new System.Drawing.Size(35, 13);
            this.moveText.TabIndex = 9;
            this.moveText.Text = "Move Sensitivity";
            // 
            // scrollText
            // 
            this.scrollText.AutoSize = true;
            this.scrollText.Location = new System.Drawing.Point(306, 217);
            this.scrollText.Name = "scrollText";
            this.scrollText.Size = new System.Drawing.Size(35, 13);
            this.scrollText.TabIndex = 10;
            this.scrollText.Text = "Scroll sensitivity";
            // 
            // label3
            // 
            this.scaleText.AutoSize = true;
            this.scaleText.Location = new System.Drawing.Point(306, 249);
            this.scaleText.Name = "scaleText";
            this.scaleText.Size = new System.Drawing.Size(35, 13);
            this.scaleText.TabIndex = 11;
            this.scaleText.Text = "Scale Sensitivity";
            this.scaleBar.ValueChanged += this.ScaleSensitivityChanged;
            // 
            // moveBar
            // 
            this.moveBar.Location = new System.Drawing.Point(414, 181);
            this.moveBar.Name = "moveBar";
            this.moveBar.Size = new System.Drawing.Size(315, 45);
            this.moveBar.TabIndex = 12;
            this.moveBar.ValueChanged += this.MoveSensitivityChanged;
            // 
            // scrollBar
            // 
            this.scrollBar.Location = new System.Drawing.Point(414, 217);
            this.scrollBar.Name = "scrollBar";
            this.scrollBar.Size = new System.Drawing.Size(316, 45);
            this.scrollBar.TabIndex = 13;
            this.scrollBar.ValueChanged += this.ScrollSensitivityChanged;
            // 
            // trackBar3
            // 
            this.scaleBar.Location = new System.Drawing.Point(414, 249);
            this.scaleBar.Name = "trackBar3";
            this.scaleBar.Size = new System.Drawing.Size(315, 45);
            this.scaleBar.TabIndex = 14;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 324);
            this.Controls.Add(this.switchConnectionType);
            this.Controls.Add(this.scaleBar);
            this.Controls.Add(this.scrollBar);
            this.Controls.Add(this.moveBar);
            this.Controls.Add(this.scaleText);
            this.Controls.Add(this.scrollText);
            this.Controls.Add(this.moveText);
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
            ((System.ComponentModel.ISupportInitialize)(this.moveBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scrollBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleBar)).EndInit();
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
        private System.Windows.Forms.Label moveText;
        private System.Windows.Forms.Label scrollText;
        private System.Windows.Forms.Label scaleText;
        private System.Windows.Forms.TrackBar moveBar;
        private System.Windows.Forms.TrackBar scrollBar;
        private System.Windows.Forms.TrackBar scaleBar;
    }
}