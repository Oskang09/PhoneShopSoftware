namespace CSINVManage
{
    partial class Form2
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
            this.lockScreenControl1 = new GestureLockApp.GestureLockControl.LockScreenControl();
            this.SuspendLayout();
            // 
            // lockScreenControl1
            // 
            this.lockScreenControl1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lockScreenControl1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lockScreenControl1.Location = new System.Drawing.Point(12, 12);
            this.lockScreenControl1.Name = "lockScreenControl1";
            this.lockScreenControl1.Size = new System.Drawing.Size(153, 156);
            this.lockScreenControl1.TabIndex = 0;
            this.lockScreenControl1.Text = "lockScreenControl1";
            this.lockScreenControl1.PassCodeSubmitted += new System.EventHandler<GestureLockApp.GestureLockControl.PassCodeSubmittedEventArgs>(this.lockScreenControl1_PassCodeSubmitted);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(177, 180);
            this.Controls.Add(this.lockScreenControl1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);

        }

        #endregion

        public GestureLockApp.GestureLockControl.LockScreenControl lockScreenControl1;
    }
}