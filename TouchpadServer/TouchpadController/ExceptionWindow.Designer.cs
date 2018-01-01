namespace TouchpadController {
    partial class ExceptionWindow {
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
            this.ExceptionText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ExceptionText
            // 
            this.ExceptionText.Cursor = System.Windows.Forms.Cursors.SizeNESW;
            this.ExceptionText.Location = new System.Drawing.Point(12, 12);
            this.ExceptionText.Name = "ExceptionText";
            this.ExceptionText.ReadOnly = true;
            this.ExceptionText.Size = new System.Drawing.Size(653, 20);
            this.ExceptionText.TabIndex = 0;
            // 
            // ExceptionWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 441);
            this.Controls.Add(this.ExceptionText);
            this.Name = "ExceptionWindow";
            this.Text = "ExceptionWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ExceptionText;
    }
}