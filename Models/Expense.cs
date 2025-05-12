using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensesWebApp.Models
{
    public class Expense
    {
        public int Id { get; set; }

        public string? UserId { get; set; }
        [Display(Name = "Category")]
        public Category? ExpenseCategory { get; set; }
        
        [Display(Name = "Category")]
        [ForeignKey("ExpenseCategoryId")]
        public int ExpenseCategoryId { get; set; }

        [Display(Name = "Sum")]
        [Range(0.01, 100000, ErrorMessage = "Amount must be between 0.01 and 100000.")]
        public decimal ExpenseSum { get; set; }

        [Display(Name = "Date")]
        public DateOnly ExpenseDate { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Notes")]
        [StringLength(500)]
        public string? Notes { get; set; }
    }
}