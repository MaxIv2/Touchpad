namespace TouchpadServer {
    partial class BlacklistWindow {
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
            this.removeButton = new System.Windows.Forms.Button();
            this.blacklistDisplay = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // removeButton
            // 
            this.removeButton.Location = new System.Drawing.Point(13, 226);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(259, 23);
            this.removeButton.TabIndex = 1;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            // 
            // blacklistDisplay
            // 
            this.blacklistDisplay.Location = new System.Drawing.Point(12, 12);
            this.blacklistDisplay.Name = "blacklistDisplay";
            this.blacklistDisplay.Size = new System.Drawing.Size(260, 199);
            this.blacklistDisplay.TabIndex = 2;
            // 
            // BlacklistWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.blacklistDisplay);
            this.Controls.Add(this.removeButton);
            this.MaximizeBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "BlacklistWindow";
            this.Text = "BlacklistWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.ListBox blacklistDisplay;

    }
}