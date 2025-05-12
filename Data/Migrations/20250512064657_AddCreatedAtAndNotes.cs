using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesWebApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtAndNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Expense",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Expense",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Expense");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Expense");
        }
    }
}
