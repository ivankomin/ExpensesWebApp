using ExpensesWebApp.Helpers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ExpensesWebApp.Models;

namespace ExpensesWebApp.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

public DbSet<Expense> Expense { get; set; } = default!;
public DbSet<Category> Categories { get; set; }
protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);
    builder.Entity<Category>().HasData(
        CategoryHelper.GetCategoryOptions()
            .Select((item, index) => new Category
            {
                Id = index + 1,
                Name = item.Value
            })
            .ToArray()
    );
}
}