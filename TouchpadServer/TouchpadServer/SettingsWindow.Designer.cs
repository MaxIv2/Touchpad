﻿namespace TouchpadServer {
    partial class SettingsWindow {
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
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 389);
            this.Controls.Add(this.QRCodeContainer);
            this.Name = "SettingsWindow";
            this.Text = "SettingsWindows";
            ((System.ComponentModel.ISupportInitialize)(this.QRCodeContainer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox QRCodeContainer;
    }
}