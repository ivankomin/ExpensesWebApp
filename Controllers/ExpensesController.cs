using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpensesWebApp.Data;
using ExpensesWebApp.Helpers;
using ExpensesWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ExpensesWebApp.ViewModels;

namespace ExpensesWebApp.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpensesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Expenses
        [Authorize]
        public async Task<IActionResult> Index(string? category, string? order)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = _context.Expense
                .Where(e => e.UserId == userId)
                .Include(e => e.ExpenseCategory)
                .AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(e => e.ExpenseCategory != null && e.ExpenseCategory.Name == category);
            }

            if (!string.IsNullOrEmpty(order))
            {
                query = order switch
                {
                    "newest" => query.OrderByDescending(e => e.ExpenseDate),
                    "oldest" => query.OrderBy(e => e.ExpenseDate),
                    "highest" => query.OrderByDescending(e => e.ExpenseSum),
                    "lowest" => query.OrderBy(e => e.ExpenseSum),
                    _ => query.OrderByDescending(e => e.Id)
                };
            }
            else
            {
                query = query.OrderByDescending(e => e.Id);
            }

            var model = new ExpenseFilterViewModel
            {
                Expenses = await query.ToListAsync(),
                SelectedCategory = category,
                SelectedOrder = order
            };

            return View(model);
        }

        // GET: Expenses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var expense = await _context.Expense.Include(e => e.ExpenseCategory).FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // GET: Expenses/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.CategoryOptions = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();
            return View();
        }

        // POST: Expenses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,ExpenseCategoryId,ExpenseSum,ExpenseDate,Notes")] Expense expense)
        {
            if (expense.ExpenseDate > DateOnly.FromDateTime(DateTime.Today))
            {
                ModelState.AddModelError("ExpenseDate", "Invalid date selected.");
            }
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                expense.UserId = userId;
                _context.Add(expense);
                await _context.SaveChangesAsync();
                UserActionLogger.Log(HttpContext, "Created", expense.Id);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryOptions = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();
            return View(expense);
        }

        // GET: Expenses/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var expense = await _context.Expense.Include(e => e.ExpenseCategory).FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (expense == null)
            {
                return NotFound();
            }
            ViewBag.CategoryOptions = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();
            return View(expense);
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, Expense expense)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingExpense = await _context.Expense.AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (existingExpense == null)
            {
                return NotFound();
            }
            if (expense.ExpenseDate > DateOnly.FromDateTime(DateTime.Today))
            {
                ModelState.AddModelError("ExpenseDate", "Invalid date selected.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    expense.UserId = userId;
                    _context.Update(expense);
                    await _context.SaveChangesAsync();
                    UserActionLogger.Log(HttpContext, "Edited", expense.Id);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Expense.Any(e => e.Id == id && e.UserId == userId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryOptions = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();
            return View(expense);
        }

        // GET: Expenses/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var expense = await _context.Expense.Include(e => e.ExpenseCategory).FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var expense = await _context.Expense.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
            if (expense != null)
            {
                _context.Expense.Remove(expense);
            }

            await _context.SaveChangesAsync();
            UserActionLogger.Log(HttpContext, "Deleted", expense.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
