using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Personal_Expense_Tracker
{
    public partial class NotificationForm : Form
    {
        public NotificationForm(string message)
        {
            InitializeComponent();

            // Set fixed form properties for notification style
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;

            // Configure label for multiline text and word wrap
            notificationLabel.AutoSize = false;
            notificationLabel.MaximumSize = new Size(300, 0); // Max width, unlimited height
            notificationLabel.TextAlign = ContentAlignment.MiddleLeft;
            notificationLabel.Font = new Font("Segoe UI", 10, FontStyle.Regular);

            // Set the multiline message
            notificationLabel.Text = message;

            // Measure text size with word wrap to adjust label height properly
            Size textSize = TextRenderer.MeasureText(notificationLabel.Text, notificationLabel.Font,
                                                     new Size(notificationLabel.MaximumSize.Width, 0),
                                                     TextFormatFlags.WordBreak);

            notificationLabel.Size = new Size(notificationLabel.MaximumSize.Width, textSize.Height + 10);

            // Adjust the form size to fit the label plus padding
            this.ClientSize = new Size(notificationLabel.Width + 20, notificationLabel.Height + 20);

            StartAutoClose();
        }

        private async void StartAutoClose()
        {
            await Task.Delay(3000); // Show notification for 3 seconds
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => this.Close()));
            }
            else
            {
                this.Close();
            }
        }

        private void NotificationForm_Load(object sender, EventArgs e)
        {
            // No longer needed, StartPosition set in constructor
        }

        private void notificationLabel_Click(object sender, EventArgs e)
        {
            // Optional: close notification on click
            this.Close();
        }

        private void notificationLabel_Click_1(object sender, EventArgs e)
        {

        }
    }
}
