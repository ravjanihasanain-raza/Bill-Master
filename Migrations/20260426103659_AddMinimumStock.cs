using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bill_Master.Migrations
{
    /// <inheritdoc />
    public partial class AddMinimumStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MinimumStock",
                table: "ProductMasters",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinimumStock",
                table: "ProductMasters");
        }
    }
}
