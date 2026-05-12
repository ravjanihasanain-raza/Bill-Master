using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bill_Master.Migrations
{
    /// <inheritdoc />
    public partial class billmasterdb1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoicePayments_InvoiceMasters_InvoiceMasterId",
                table: "InvoicePayments");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoicePayments_StaffMasters_StaffMasterId",
                table: "InvoicePayments");

            migrationBuilder.DropForeignKey(
                name: "FK_InwardStocks_PurchaseItems_PurchaseItemId",
                table: "InwardStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_InwardStocks_StaffMasters_StaffUserId",
                table: "InwardStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasePayments_PurchaseMasters_PurchaseMasterId",
                table: "PurchasePayments");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasePayments_StaffMasters_StaffMasterId",
                table: "PurchasePayments");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoicePayments_InvoiceMasters_InvoiceMasterId",
                table: "InvoicePayments",
                column: "InvoiceMasterId",
                principalTable: "InvoiceMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoicePayments_StaffMasters_StaffMasterId",
                table: "InvoicePayments",
                column: "StaffMasterId",
                principalTable: "StaffMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InwardStocks_PurchaseItems_PurchaseItemId",
                table: "InwardStocks",
                column: "PurchaseItemId",
                principalTable: "PurchaseItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InwardStocks_StaffMasters_StaffUserId",
                table: "InwardStocks",
                column: "StaffUserId",
                principalTable: "StaffMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasePayments_PurchaseMasters_PurchaseMasterId",
                table: "PurchasePayments",
                column: "PurchaseMasterId",
                principalTable: "PurchaseMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasePayments_StaffMasters_StaffMasterId",
                table: "PurchasePayments",
                column: "StaffMasterId",
                principalTable: "StaffMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoicePayments_InvoiceMasters_InvoiceMasterId",
                table: "InvoicePayments");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoicePayments_StaffMasters_StaffMasterId",
                table: "InvoicePayments");

            migrationBuilder.DropForeignKey(
                name: "FK_InwardStocks_PurchaseItems_PurchaseItemId",
                table: "InwardStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_InwardStocks_StaffMasters_StaffUserId",
                table: "InwardStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasePayments_PurchaseMasters_PurchaseMasterId",
                table: "PurchasePayments");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasePayments_StaffMasters_StaffMasterId",
                table: "PurchasePayments");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoicePayments_InvoiceMasters_InvoiceMasterId",
                table: "InvoicePayments",
                column: "InvoiceMasterId",
                principalTable: "InvoiceMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoicePayments_StaffMasters_StaffMasterId",
                table: "InvoicePayments",
                column: "StaffMasterId",
                principalTable: "StaffMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InwardStocks_PurchaseItems_PurchaseItemId",
                table: "InwardStocks",
                column: "PurchaseItemId",
                principalTable: "PurchaseItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InwardStocks_StaffMasters_StaffUserId",
                table: "InwardStocks",
                column: "StaffUserId",
                principalTable: "StaffMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasePayments_PurchaseMasters_PurchaseMasterId",
                table: "PurchasePayments",
                column: "PurchaseMasterId",
                principalTable: "PurchaseMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasePayments_StaffMasters_StaffMasterId",
                table: "PurchasePayments",
                column: "StaffMasterId",
                principalTable: "StaffMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
