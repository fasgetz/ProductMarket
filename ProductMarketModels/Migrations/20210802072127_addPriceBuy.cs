using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductMarketModels.Migrations
{
    public partial class addPriceBuy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "priceBuy",
                table: "ProductsInOrder",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "priceBuy",
                table: "ProductsInOrder");
        }
    }
}
