using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductMarketModels.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "CategoryProduct",
            //    columns: table => new
            //    {
            //        Id = table.Column<short>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(maxLength: 50, nullable: true),
            //        Poster = table.Column<byte[]>(type: "image", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_CategoryProduct", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Fabricator",
            //    columns: table => new
            //    {
            //        Id = table.Column<short>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Fabricator", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Order",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        UserId = table.Column<string>(nullable: true),
            //        Commentary = table.Column<string>(maxLength: 500, nullable: true),
            //        Address = table.Column<string>(maxLength: 250, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Order", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TypeStatusOrder",
            //    columns: table => new
            //    {
            //        Id = table.Column<byte>(nullable: false),
            //        Name = table.Column<string>(maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TypeStatusOrder", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "SubCategoryProduct",
            //    columns: table => new
            //    {
            //        Id = table.Column<short>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        IdCategory = table.Column<short>(nullable: true),
            //        Name = table.Column<string>(maxLength: 50, nullable: true),
            //        Poster = table.Column<byte[]>(type: "image", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_SubCategoryProduct", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_SubCategoryProduct_CategoryProduct",
            //            column: x => x.IdCategory,
            //            principalTable: "CategoryProduct",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "OrderStatus",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        IdOrder = table.Column<int>(nullable: true),
            //        IdStatus = table.Column<byte>(nullable: true),
            //        Date = table.Column<DateTime>(type: "datetime", nullable: true),
            //        Commentary = table.Column<string>(maxLength: 500, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_OrderStatus", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_OrderStatus_Order",
            //            column: x => x.IdOrder,
            //            principalTable: "Order",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_OrderStatus_TypeStatusOrder",
            //            column: x => x.IdStatus,
            //            principalTable: "TypeStatusOrder",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Product",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(maxLength: 50, nullable: true),
            //        Price = table.Column<decimal>(type: "money", nullable: true),
            //        Amount = table.Column<int>(nullable: true),
            //        IdSubCategory = table.Column<short>(nullable: false),
            //        IdFabricator = table.Column<short>(nullable: true),
            //        Poster = table.Column<byte[]>(type: "image", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Product", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Product_Fabricator",
            //            column: x => x.IdFabricator,
            //            principalTable: "Fabricator",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Product_SubCategoryProduct",
            //            column: x => x.IdSubCategory,
            //            principalTable: "SubCategoryProduct",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "DiscountProduct",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        IdProduct = table.Column<int>(nullable: true),
            //        DateStart = table.Column<DateTime>(type: "datetime", nullable: true),
            //        DateEnd = table.Column<DateTime>(type: "datetime", nullable: true),
            //        ProcentDiscount = table.Column<double>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_DiscountProduct", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_DiscountProduct_Product",
            //            column: x => x.IdProduct,
            //            principalTable: "Product",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ProductsInOrder",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        OrderId = table.Column<int>(nullable: true),
            //        IdProduct = table.Column<int>(nullable: true),
            //        Count = table.Column<short>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ProductsInOrder", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_ProductsInOrder_Product",
            //            column: x => x.IdProduct,
            //            principalTable: "Product",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_ProductsInOrder_Order",
            //            column: x => x.OrderId,
            //            principalTable: "Order",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_DiscountProduct_IdProduct",
            //    table: "DiscountProduct",
            //    column: "IdProduct");

            //migrationBuilder.CreateIndex(
            //    name: "IX_OrderStatus_IdOrder",
            //    table: "OrderStatus",
            //    column: "IdOrder");

            //migrationBuilder.CreateIndex(
            //    name: "IX_OrderStatus_IdStatus",
            //    table: "OrderStatus",
            //    column: "IdStatus");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Product_IdFabricator",
            //    table: "Product",
            //    column: "IdFabricator");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Product_IdSubCategory",
            //    table: "Product",
            //    column: "IdSubCategory");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProductsInOrder_IdProduct",
            //    table: "ProductsInOrder",
            //    column: "IdProduct");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProductsInOrder_OrderId",
            //    table: "ProductsInOrder",
            //    column: "OrderId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_SubCategoryProduct_IdCategory",
            //    table: "SubCategoryProduct",
            //    column: "IdCategory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "DiscountProduct");

            //migrationBuilder.DropTable(
            //    name: "OrderStatus");

            //migrationBuilder.DropTable(
            //    name: "ProductsInOrder");

            //migrationBuilder.DropTable(
            //    name: "TypeStatusOrder");

            //migrationBuilder.DropTable(
            //    name: "Product");

            //migrationBuilder.DropTable(
            //    name: "Order");

            //migrationBuilder.DropTable(
            //    name: "Fabricator");

            //migrationBuilder.DropTable(
            //    name: "SubCategoryProduct");

            //migrationBuilder.DropTable(
            //    name: "CategoryProduct");
        }
    }
}
