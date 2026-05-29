using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bill_Master.Migrations
{
    /// <inheritdoc />
    public partial class AddPONumberAndPODate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PODate",
                table: "InvoiceMasters",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PONumber",
                table: "InvoiceMasters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockUseds_InvoiceMasterId",
                table: "StockUseds",
                column: "InvoiceMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockUseds_InvoiceMasters_InvoiceMasterId",
                table: "StockUseds",
                column: "InvoiceMasterId",
                principalTable: "InvoiceMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockUseds_InvoiceMasters_InvoiceMasterId",
                table: "StockUseds");

            migrationBuilder.DropIndex(
                name: "IX_StockUseds_InvoiceMasterId",
                table: "StockUseds");

            migrationBuilder.DropColumn(
                name: "PODate",
                table: "InvoiceMasters");

            migrationBuilder.DropColumn(
                name: "PONumber",
                table: "InvoiceMasters");
        }
    }
}
