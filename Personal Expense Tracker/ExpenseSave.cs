using System;
// Tolentino Jose - ExpenseRepository.cs save data here
public class ExpenseSave
{
    public DateTime Date { get; set; }
    public string PaymentMethod { get; set; }   
    public string Category { get; set; }
    public string Amount { get; set; }
    public string Note { get; set; }

    public ExpenseSave(DateTime date, string paymentMethod, string category, string amount, string note)
    {
        Date = date;
        PaymentMethod = paymentMethod;
        Category = category;
        Amount = amount;
        Note = note;
    }
}
