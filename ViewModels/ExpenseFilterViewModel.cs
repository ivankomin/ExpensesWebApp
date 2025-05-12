using ExpensesWebApp.Models;

namespace ExpensesWebApp.ViewModels;

public class ExpenseFilterViewModel
{
    public List<Expense> Expenses { get; set; }
    public string? SelectedCategory { get; set; }
    public string? SelectedOrder { get; set; }
}