namespace TouchpadController {
    partial class TouchpadController {
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
            this.QRCodeContatiner = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.QRCodeContatiner)).BeginInit();
            this.SuspendLayout();
            // 
            // QRCodeContatiner
            // 
            this.QRCodeContatiner.Location = new System.Drawing.Point(90, 12);
            this.QRCodeContatiner.Name = "QRCodeContatiner";
            this.QRCodeContatiner.Size = new System.Drawing.Size(390, 366);
            this.QRCodeContatiner.TabIndex = 0;
            this.QRCodeContatiner.TabStop = false;
            // 
            // TouchpadController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 431);
            this.Controls.Add(this.QRCodeContatiner);
            this.Name = "TouchpadController";
            this.Text = "TouchpadController";
            ((System.ComponentModel.ISupportInitialize)(this.QRCodeContatiner)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox QRCodeContatiner;
    }
}