using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bill_Master.Migrations
{
    /// <inheritdoc />
    public partial class nullstaff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseMasters_StaffMasters_StaffMasterId",
                table: "PurchaseMasters");

            migrationBuilder.AlterColumn<int>(
                name: "StaffMasterId",
                table: "PurchaseMasters",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseMasters_StaffMasters_StaffMasterId",
                table: "PurchaseMasters",
                column: "StaffMasterId",
                principalTable: "StaffMasters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseMasters_StaffMasters_StaffMasterId",
                table: "PurchaseMasters");

            migrationBuilder.AlterColumn<int>(
                name: "StaffMasterId",
                table: "PurchaseMasters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseMasters_StaffMasters_StaffMasterId",
                table: "PurchaseMasters",
                column: "StaffMasterId",
                principalTable: "StaffMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
