namespace Personal_Expense_Tracker
{
    partial class NotificationForm
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
            this.notificationLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // notificationLabel
            // 
            this.notificationLabel.AutoSize = true;
            this.notificationLabel.Cursor = System.Windows.Forms.Cursors.SizeNESW;
            this.notificationLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.notificationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.notificationLabel.Location = new System.Drawing.Point(44, 26);
            this.notificationLabel.Name = "notificationLabel";
            this.notificationLabel.Size = new System.Drawing.Size(225, 25);
            this.notificationLabel.TabIndex = 0;
            this.notificationLabel.Text = "Example Text Right Now";
            this.notificationLabel.Click += new System.EventHandler(this.notificationLabel_Click_1);
            // 
            // NotificationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.notificationLabel);
            this.Name = "NotificationForm";
            this.Text = "Notification";
            this.Load += new System.EventHandler(this.NotificationForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label notificationLabel;
    }
}