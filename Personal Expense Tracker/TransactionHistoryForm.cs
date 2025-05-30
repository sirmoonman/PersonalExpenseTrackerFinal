using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

// Tolentino Jose - Transaction History Form 
namespace Personal_Expense_Tracker
{
    public partial class TransactionHistoryForm : Form
    {
        public TransactionHistoryForm()
        {
            InitializeComponent();
            //Tolentino - Attach event handler to category dropdown
            cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
        }

        private void TransactionHistoryForm_Load(object sender, EventArgs e)
        {
            //Tolentino - Load categories and all transactions on form load
            LoadCategories();
            LoadTransactions();
        }

        private void LoadCategories()
        {
            //Tolentino - Populate the category dropdown with options
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("All");
            cmbCategory.Items.Add("Food");
            cmbCategory.Items.Add("Transport");
            cmbCategory.Items.Add("Bills");
            cmbCategory.Items.Add("Snacks");
            cmbCategory.SelectedIndex = 0;
        }

        private void LoadTransactions(string filter = "")
        {
            //Tolentino - Create a DataTable to store and manipulate transaction data
            DataTable table = new DataTable();
            table.Columns.Add("Amount", typeof(decimal));
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Category", typeof(string));
            table.Columns.Add("PaymentMethod", typeof(string));

            //Tolentino - Load data from the shared ExpenseRepository
            foreach (var tx in ExpenseRepository.Expenses)
            {
                //Tolentino - Add each transaction as a row in the table
                table.Rows.Add(tx.Amount, tx.Date, tx.Category, tx.Account); // tx.Account is assumed to be payment method
            }

            //Tolentino - Begin filtering the data
            var filteredRows = table.AsEnumerable();

            //Tolentino - Filter transactions by selected date range
            filteredRows = filteredRows.Where(row =>
                row.Field<DateTime>("Date") >= dtpFrom.Value.Date &&
                row.Field<DateTime>("Date") <= dtpTo.Value.Date);

            //Tolentino - Filter transactions by selected category
            if (cmbCategory.SelectedItem != null && cmbCategory.SelectedItem.ToString() != "All")
            {
                string selectedCategory = cmbCategory.SelectedItem.ToString().Trim();
                filteredRows = filteredRows.Where(row =>
                    (row.Field<string>("Category")?.Trim() ?? "")
                    .Equals(selectedCategory, StringComparison.OrdinalIgnoreCase));
            }

            //Tolentino - Filter transactions by amount or amount range
            string amountText = txtAmount.Text.Trim();
            if (!string.IsNullOrEmpty(amountText))
            {
                if (amountText.Contains("-"))
                {
                    //Tolentino - Parse min-max amount range
                    var parts = amountText.Split('-');
                    if (parts.Length == 2 &&
                        decimal.TryParse(parts[0].Trim(), out decimal minAmount) &&
                        decimal.TryParse(parts[1].Trim(), out decimal maxAmount))
                    {
                        filteredRows = filteredRows.Where(row =>
                            row.Field<decimal>("Amount") >= minAmount &&
                            row.Field<decimal>("Amount") <= maxAmount);
                    }
                    else
                    {
                        //Tolentino - Show error if range is invalid
                        MessageBox.Show("Invalid amount range format. Use min-max, e.g. 100-500.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    //Tolentino - Filter for exact amount
                    if (decimal.TryParse(amountText, out decimal amount))
                    {
                        filteredRows = filteredRows.Where(row =>
                            row.Field<decimal>("Amount") == amount);
                    }
                    else
                    {
                        //Tolentino - Show error if amount is invalid
                        MessageBox.Show("Invalid amount value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }

            //Tolentino - Finalize filtered results
            DataTable filteredTable;
            if (filteredRows.Any())
            {
                filteredTable = filteredRows.CopyToDataTable(); //Tolentino - Copy results if there are any
            }
            else
            {
                filteredTable = table.Clone(); //Tolentino - Use empty table with same structure
            }

            //Tolentino - Bind filtered data to DataGridView
            dataGridView1.DataSource = filteredTable;

            //Tolentino - Update summary labels and chart based on filtered data
            UpdateSummary(filteredTable);
            UpdateChart(filteredTable);
        }

        private void UpdateSummary(DataTable table)
        {
            //Tolentino - Initialize variables for summary
            decimal total = 0;
            var summary = new StringBuilder();

            //Tolentino - Group transactions by category
            var groups = table.AsEnumerable()
                .GroupBy(row => row.Field<string>("Category"));

            //Tolentino - Calculate totals per category
            foreach (var group in groups)
            {
                decimal categoryTotal = group.Sum(row => row.Field<decimal>("Amount"));
                total += categoryTotal;
                summary.AppendLine($"{group.Key}: ₱{categoryTotal:N2}");
            }

            //Tolentino - Display total and breakdown
            lblTotal.Text = $"Total Spending: ₱{total:N2}";
            lblCategorySummary.Text = summary.Length > 0 ? summary.ToString() : "No data to summarize.";
        }

        private void UpdateChart(DataTable table)
        {
            //Tolentino - Clear existing chart data
            chartSummary.Series.Clear();
            chartSummary.ChartAreas.Clear();

            //Tolentino - Setup new chart area
            var chartArea = new ChartArea();
            chartSummary.ChartAreas.Add(chartArea);

            //Tolentino - Create a pie chart series
            var series = new Series
            {
                Name = "SpendingByCategory",
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                LabelFormat = "₱{0:N2}"
            };

            chartSummary.Series.Add(series);

            //Tolentino - Group and add data points for each category
            var groups = table.AsEnumerable()
                .GroupBy(row => row.Field<string>("Category"));

            foreach (var group in groups)
            {
                decimal totalAmount = group.Sum(row => row.Field<decimal>("Amount"));
                series.Points.AddXY(group.Key, totalAmount);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //Tolentino - Manually reload transactions when search is clicked
            LoadTransactions();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            //Tolentino - Go back to the main form
            this.Hide();
            ExpenseTracker main = new ExpenseTracker();
            main.Show();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Tolentino - Reload transactions when category changes
            LoadTransactions();
        }

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            //Tolentino - Save the transaction data as a PDF file
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                Title = "Save transaction report",
                FileName = "TransactionReport.pdf"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //Tolentino - Create and write to PDF
                    Document doc = new Document(PageSize.A4, 10, 10, 10, 10);
                    PdfWriter.GetInstance(doc, new FileStream(saveFileDialog.FileName, FileMode.Create));
                    doc.Open();

                    //Tolentino - Add title
                    Paragraph title = new Paragraph("Transaction History", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16));
                    title.Alignment = Element.ALIGN_CENTER;
                    doc.Add(title);
                    doc.Add(new Paragraph("\n"));

                    //Tolentino - Create table with column headers
                    PdfPTable table = new PdfPTable(dataGridView1.Columns.Count);
                    table.WidthPercentage = 100;

                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        table.AddCell(new Phrase(column.HeaderText, FontFactory.GetFont(FontFactory.HELVETICA, 12)));
                    }

                    //Tolentino - Add rows of data to PDF table
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                string cellText = cell.Value?.ToString() ?? "";
                                table.AddCell(new Phrase(cellText, FontFactory.GetFont(FontFactory.HELVETICA, 11)));
                            }
                        }
                    }

                    //Tolentino - Add table to document
                    doc.Add(table);
                    doc.Close();

                    //Tolentino - Notify user
                    MessageBox.Show("PDF Exported Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    //Tolentino - Handle errors in export
                    MessageBox.Show("Error exporting PDF: " + ex.Message);
                }
            }
        }

        private void TransactionHistoryForm_Load_1(object sender, EventArgs e)
        {
            //Tolentino - Unused secondary load event
        }
    }
}
