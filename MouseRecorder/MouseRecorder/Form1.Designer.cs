namespace MouseRecorder
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Record_Button = new System.Windows.Forms.Button();
            this.Stop_Button = new System.Windows.Forms.Button();
            this.Play_Button = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.X = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Y = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.relX = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.relY = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Action = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Save_Button = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.Clear_Button = new System.Windows.Forms.Button();
            this.Load_Button = new System.Windows.Forms.Button();
            this.Scale_Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Record_Button
            // 
            this.Record_Button.Location = new System.Drawing.Point(12, 12);
            this.Record_Button.Name = "Record_Button";
            this.Record_Button.Size = new System.Drawing.Size(140, 60);
            this.Record_Button.TabIndex = 0;
            this.Record_Button.Text = "Record";
            this.Record_Button.UseVisualStyleBackColor = true;
            this.Record_Button.Click += new System.EventHandler(this.Record_Button_Click);
            // 
            // Stop_Button
            // 
            this.Stop_Button.Location = new System.Drawing.Point(12, 78);
            this.Stop_Button.Name = "Stop_Button";
            this.Stop_Button.Size = new System.Drawing.Size(140, 60);
            this.Stop_Button.TabIndex = 1;
            this.Stop_Button.Text = "Stop";
            this.Stop_Button.UseVisualStyleBackColor = true;
            this.Stop_Button.Click += new System.EventHandler(this.Stop_Button_Click);
            // 
            // Play_Button
            // 
            this.Play_Button.Location = new System.Drawing.Point(169, 12);
            this.Play_Button.Name = "Play_Button";
            this.Play_Button.Size = new System.Drawing.Size(140, 60);
            this.Play_Button.TabIndex = 2;
            this.Play_Button.Text = "Play";
            this.Play_Button.UseVisualStyleBackColor = true;
            this.Play_Button.Click += new System.EventHandler(this.Play_Button_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.X,
            this.Y,
            this.relX,
            this.relY,
            this.Action});
            this.listView1.Location = new System.Drawing.Point(12, 186);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(888, 401);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // X
            // 
            this.X.Text = "Absolute X";
            this.X.Width = 120;
            // 
            // Y
            // 
            this.Y.Text = "Absolute Y";
            this.Y.Width = 119;
            // 
            // relX
            // 
            this.relX.Text = "Relutive X";
            this.relX.Width = 112;
            // 
            // relY
            // 
            this.relY.Text = "Relutive Y";
            this.relY.Width = 116;
            // 
            // Action
            // 
            this.Action.Text = "Action";
            this.Action.Width = 110;
            // 
            // Save_Button
            // 
            this.Save_Button.Location = new System.Drawing.Point(325, 12);
            this.Save_Button.Name = "Save_Button";
            this.Save_Button.Size = new System.Drawing.Size(140, 60);
            this.Save_Button.TabIndex = 4;
            this.Save_Button.Text = "Save";
            this.Save_Button.UseVisualStyleBackColor = true;
            this.Save_Button.Click += new System.EventHandler(this.Save_Button_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 10;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // Clear_Button
            // 
            this.Clear_Button.Location = new System.Drawing.Point(169, 78);
            this.Clear_Button.Name = "Clear_Button";
            this.Clear_Button.Size = new System.Drawing.Size(140, 60);
            this.Clear_Button.TabIndex = 5;
            this.Clear_Button.Text = "Clear";
            this.Clear_Button.UseVisualStyleBackColor = true;
            this.Clear_Button.Click += new System.EventHandler(this.Clear_Button_Click);
            // 
            // Load_Button
            // 
            this.Load_Button.Location = new System.Drawing.Point(325, 78);
            this.Load_Button.Name = "Load_Button";
            this.Load_Button.Size = new System.Drawing.Size(140, 60);
            this.Load_Button.TabIndex = 6;
            this.Load_Button.Text = "Load";
            this.Load_Button.UseVisualStyleBackColor = true;
            this.Load_Button.Click += new System.EventHandler(this.Load_Button_Click);
            // 
            // Scale_Label
            // 
            this.Scale_Label.AutoSize = true;
            this.Scale_Label.Location = new System.Drawing.Point(12, 163);
            this.Scale_Label.Name = "Scale_Label";
            this.Scale_Label.Size = new System.Drawing.Size(49, 20);
            this.Scale_Label.TabIndex = 7;
            this.Scale_Label.Text = "Scale";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 599);
            this.Controls.Add(this.Scale_Label);
            this.Controls.Add(this.Load_Button);
            this.Controls.Add(this.Clear_Button);
            this.Controls.Add(this.Save_Button);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.Play_Button);
            this.Controls.Add(this.Stop_Button);
            this.Controls.Add(this.Record_Button);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Record_Button;
        private System.Windows.Forms.Button Stop_Button;
        private System.Windows.Forms.Button Play_Button;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button Save_Button;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ColumnHeader X;
        private System.Windows.Forms.ColumnHeader Y;
        private System.Windows.Forms.Button Clear_Button;
        private System.Windows.Forms.Button Load_Button;
        private System.Windows.Forms.ColumnHeader relX;
        private System.Windows.Forms.ColumnHeader relY;
        private System.Windows.Forms.Label Scale_Label;
        private System.Windows.Forms.ColumnHeader Action;
    }
}

