namespace ExpensesWebApp.Helpers;
using System.IO;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

public static class UserActionLogger
{
    private static readonly string LogFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "Log.txt");
    public static void Log(HttpContext context, string action, int? expenseId = null, string? userIdOverride = null)
    {
        var userId = userIdOverride
                     ?? context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? "Unknown";
        var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var entry = expenseId.HasValue
            ? $"Time: {time} - User: {userId} - Action: {action} - Expense: {expenseId}\n"
            : $"Time: {time} - User: {userId} - Action: {action}\n";
        File.AppendAllText(LogFilePath, entry);
    }
}