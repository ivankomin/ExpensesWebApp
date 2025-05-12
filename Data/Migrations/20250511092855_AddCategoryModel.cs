using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesWebApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpenseCategory",
                table: "Expense");

            migrationBuilder.AddColumn<int>(
                name: "ExpenseCategoryId",
                table: "Expense",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expense_ExpenseCategoryId",
                table: "Expense",
                column: "ExpenseCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_Category_ExpenseCategoryId",
                table: "Expense",
                column: "ExpenseCategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_Category_ExpenseCategoryId",
                table: "Expense");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Expense_ExpenseCategoryId",
                table: "Expense");

            migrationBuilder.DropColumn(
                name: "ExpenseCategoryId",
                table: "Expense");

            migrationBuilder.AddColumn<string>(
                name: "ExpenseCategory",
                table: "Expense",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
