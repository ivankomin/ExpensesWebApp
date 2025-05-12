using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpensesWebApp.Helpers;

public static class CategoryHelper
{
    public static List<SelectListItem> GetCategoryOptions()
    {
        return new List<SelectListItem>
        {
            new SelectListItem { Value = "Food", Text = "Food" },
            new SelectListItem { Value = "Healthcare", Text = "Healthcare" },
            new SelectListItem { Value = "Personal", Text = "Personal Spending" },
            new SelectListItem { Value = "Entertainment", Text = "Entertainment" },
            new SelectListItem { Value = "Utilities", Text = "Utilities" },
            new SelectListItem { Value = "Other", Text = "Other" }
        };
    }
}