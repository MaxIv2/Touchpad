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
            this.blacklistView = new System.Windows.Forms.ListView();
            this.deviceNameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.addressHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            // blacklistView
            // 
            this.blacklistView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.deviceNameHeader,
            this.addressHeader});
            this.blacklistView.View = System.Windows.Forms.View.Details;
            this.blacklistView.FullRowSelect = true;
            this.blacklistView.Location = new System.Drawing.Point(13, 12);
            this.blacklistView.Name = "blacklistView";
            this.blacklistView.Size = new System.Drawing.Size(259, 208);
            this.blacklistView.TabIndex = 2;
            this.blacklistView.UseCompatibleStateImageBehavior = false;
            // 
            // deviceNameHeader
            // 
            this.deviceNameHeader.Text = "Device Name";
            this.deviceNameHeader.Width = -2;//Auto sizing
            // 
            // addressHeader
            // 
            this.addressHeader.Text = "Address";
            this.addressHeader.Width = -2;//Auto sizing
            // 
            // BlacklistWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.blacklistView);
            this.Controls.Add(this.removeButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "BlacklistWindow";
            this.Text = "Blacklist";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.ListView blacklistView;
        private System.Windows.Forms.ColumnHeader deviceNameHeader;
        private System.Windows.Forms.ColumnHeader addressHeader;

    }
}