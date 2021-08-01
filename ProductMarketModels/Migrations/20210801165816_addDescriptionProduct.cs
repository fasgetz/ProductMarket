using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductMarketModels.Migrations
{
    public partial class addDescriptionProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "Product",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayerID",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "token",
                table: "Order",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "PayerID",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "token",
                table: "Order");
        }
    }
}
