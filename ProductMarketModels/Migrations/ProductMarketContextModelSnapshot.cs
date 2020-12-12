﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProductMarketModels;

namespace ProductMarketModels.Migrations
{
    [DbContext(typeof(ProductMarketContext))]
    partial class ProductMarketContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ProductMarketModels.CategoryProduct", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<byte[]>("Poster")
                        .HasColumnType("image");

                    b.HasKey("Id");

                    b.ToTable("CategoryProduct");
                });

            modelBuilder.Entity("ProductMarketModels.DiscountProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("DateEnd")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("DateStart")
                        .HasColumnType("datetime");

                    b.Property<int?>("IdProduct")
                        .HasColumnType("int");

                    b.Property<double?>("ProcentDiscount")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("IdProduct");

                    b.ToTable("DiscountProduct");
                });

            modelBuilder.Entity("ProductMarketModels.Fabricator", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Fabricator");
                });

            modelBuilder.Entity("ProductMarketModels.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Commentary")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<DateTime>("DateOrder")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("ProductMarketModels.OrderStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Commentary")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime");

                    b.Property<int?>("IdOrder")
                        .HasColumnType("int");

                    b.Property<byte?>("IdStatus")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("IdOrder");

                    b.HasIndex("IdStatus");

                    b.ToTable("OrderStatus");
                });

            modelBuilder.Entity("ProductMarketModels.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Amount")
                        .HasColumnType("int");

                    b.Property<short?>("IdFabricator")
                        .HasColumnType("smallint");

                    b.Property<short>("IdSubCategory")
                        .HasColumnType("smallint");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<byte[]>("Poster")
                        .HasColumnType("image");

                    b.Property<decimal?>("Price")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("IdFabricator");

                    b.HasIndex("IdSubCategory");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("ProductMarketModels.ProductsInOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<short?>("Count")
                        .HasColumnType("smallint");

                    b.Property<int?>("IdProduct")
                        .HasColumnType("int");

                    b.Property<int?>("OrderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdProduct");

                    b.HasIndex("OrderId");

                    b.ToTable("ProductsInOrder");
                });

            modelBuilder.Entity("ProductMarketModels.SubCategoryProduct", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<short?>("IdCategory")
                        .HasColumnType("smallint");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<byte[]>("Poster")
                        .HasColumnType("image");

                    b.HasKey("Id");

                    b.HasIndex("IdCategory");

                    b.ToTable("SubCategoryProduct");
                });

            modelBuilder.Entity("ProductMarketModels.TypeStatusOrder", b =>
                {
                    b.Property<byte>("Id")
                        .HasColumnType("tinyint");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("TypeStatusOrder");
                });

            modelBuilder.Entity("ProductMarketModels.DiscountProduct", b =>
                {
                    b.HasOne("ProductMarketModels.Product", "IdProductNavigation")
                        .WithMany("DiscountProduct")
                        .HasForeignKey("IdProduct")
                        .HasConstraintName("FK_DiscountProduct_Product")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProductMarketModels.OrderStatus", b =>
                {
                    b.HasOne("ProductMarketModels.Order", "IdOrderNavigation")
                        .WithMany("OrderStatus")
                        .HasForeignKey("IdOrder")
                        .HasConstraintName("FK_OrderStatus_Order");

                    b.HasOne("ProductMarketModels.TypeStatusOrder", "IdStatusNavigation")
                        .WithMany("OrderStatus")
                        .HasForeignKey("IdStatus")
                        .HasConstraintName("FK_OrderStatus_TypeStatusOrder")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProductMarketModels.Product", b =>
                {
                    b.HasOne("ProductMarketModels.Fabricator", "IdFabricatorNavigation")
                        .WithMany("Product")
                        .HasForeignKey("IdFabricator")
                        .HasConstraintName("FK_Product_Fabricator")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProductMarketModels.SubCategoryProduct", "IdSubCategoryNavigation")
                        .WithMany("Product")
                        .HasForeignKey("IdSubCategory")
                        .HasConstraintName("FK_Product_SubCategoryProduct")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProductMarketModels.ProductsInOrder", b =>
                {
                    b.HasOne("ProductMarketModels.Product", "IdProductNavigation")
                        .WithMany("ProductsInOrder")
                        .HasForeignKey("IdProduct")
                        .HasConstraintName("FK_ProductsInOrder_Product");

                    b.HasOne("ProductMarketModels.Order", "Order")
                        .WithMany("ProductsInOrder")
                        .HasForeignKey("OrderId")
                        .HasConstraintName("FK_ProductsInOrder_Order");
                });

            modelBuilder.Entity("ProductMarketModels.SubCategoryProduct", b =>
                {
                    b.HasOne("ProductMarketModels.CategoryProduct", "IdCategoryNavigation")
                        .WithMany("SubCategoryProduct")
                        .HasForeignKey("IdCategory")
                        .HasConstraintName("FK_SubCategoryProduct_CategoryProduct")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}