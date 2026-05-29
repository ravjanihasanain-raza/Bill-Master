using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bill_Master.Migrations
{
    /// <inheritdoc />
    public partial class AddExpensePaymentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "ExpenseMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PaidBy",
                table: "ExpenseMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidDate",
                table: "ExpenseMaster",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "ExpenseMaster");

            migrationBuilder.DropColumn(
                name: "PaidBy",
                table: "ExpenseMaster");

            migrationBuilder.DropColumn(
                name: "PaidDate",
                table: "ExpenseMaster");
        }
    }
}
