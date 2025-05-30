﻿using System;
using System.Windows.Forms;

namespace Personal_Expense_Tracker
{
    public partial class SubscriptionTracker : Form
    {
        public SubscriptionTracker()
        {
            InitializeComponent();
        }

        private void SubscriptionTracker_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].Name = "Subscription";
            dataGridView1.Columns[1].Name = "Amount";

            UpdateTotalAmount();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!double.TryParse(textBox2.Text, out double amount))
                {
                    throw new Exception("Amount must be a valid number (decimal/double).");
                }

                Subscribe subscribe = new Subscribe(
                    textBox1.Text,
                    amount
                );

                dataGridView1.Rows.Add(
                    subscribe.SubscribeName,
                    subscribe.Amount.ToString("F2")
                );

                ClearFields();
                UpdateTotalAmount();

                string message = $"Subscription Added:\nName: {subscribe.SubscribeName}\nAmount: ₱{subscribe.Amount:F2}";
                NotificationForm nf = new NotificationForm(message);
                nf.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    if (!double.TryParse(textBox2.Text, out double amount))
                    {
                        throw new Exception("Amount must be a valid number.");
                    }

                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                    selectedRow.Cells[0].Value = textBox1.Text;
                    selectedRow.Cells[1].Value = amount.ToString("F2");

                    ClearFields();
                    UpdateTotalAmount();

                    string message = $"Subscription Updated:\nName: {textBox1.Text}\nAmount: ₱{amount:F2}";
                    NotificationForm nf = new NotificationForm(message);
                    nf.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to update.");
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string name = selectedRow.Cells[0].Value?.ToString() ?? "Unknown";
                string amount = selectedRow.Cells[1].Value?.ToString() ?? "0.00";

                dataGridView1.Rows.RemoveAt(selectedRow.Index);
                ClearFields();
                UpdateTotalAmount();

                string message = $"Subscription Deleted:\nName: {name}\nAmount: ₱{amount}";
                NotificationForm nf = new NotificationForm(message);
                nf.Show();
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ClearFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            dataGridView1.ClearSelection();
        }

        private void UpdateTotalAmount()
        {
            double total = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[1].Value != null && double.TryParse(row.Cells[1].Value.ToString(), out double amount))
                {
                    total += amount;
                }
            }

            TotalAmount.Text = "₱" + total.ToString("F2");
        }
    }

    public class Subscribe
    {
        public string SubscribeName { get; set; }
        public double Amount { get; set; }

        public Subscribe(string subName, double amount)
        {
            SubscribeName = subName;
            Amount = amount;
        }
    }
}
