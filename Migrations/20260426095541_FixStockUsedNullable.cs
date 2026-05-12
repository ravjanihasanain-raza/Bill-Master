using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bill_Master.Migrations
{
    /// <inheritdoc />
    public partial class FixStockUsedNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockUseds_Outwards_OutwardMasterId",
                table: "StockUseds");

            migrationBuilder.AlterColumn<int>(
                name: "OutwardMasterId",
                table: "StockUseds",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_StockUseds_Outwards_OutwardMasterId",
                table: "StockUseds",
                column: "OutwardMasterId",
                principalTable: "Outwards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockUseds_Outwards_OutwardMasterId",
                table: "StockUseds");

            migrationBuilder.AlterColumn<int>(
                name: "OutwardMasterId",
                table: "StockUseds",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StockUseds_Outwards_OutwardMasterId",
                table: "StockUseds",
                column: "OutwardMasterId",
                principalTable: "Outwards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
