using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProductMarketModels
{
    public partial class ProductMarketContext : DbContext
    {
        public ProductMarketContext()
        {
        }

        public ProductMarketContext(DbContextOptions<ProductMarketContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CategoryProduct> CategoryProduct { get; set; }
        public virtual DbSet<DiscountProduct> DiscountProduct { get; set; }
        public virtual DbSet<Fabricator> Fabricator { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderStatus> OrderStatus { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductsInOrder> ProductsInOrder { get; set; }
        public virtual DbSet<SubCategoryProduct> SubCategoryProduct { get; set; }
        public virtual DbSet<TypeStatusOrder> TypeStatusOrder { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-C7LBCMN;Database=ProductMarket;Trusted_Connection=True;MultipleActiveResultSets=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryProduct>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Poster).HasColumnType("image");
            });

            modelBuilder.Entity<DiscountProduct>(entity =>
            {
                entity.Property(e => e.DateEnd).HasColumnType("datetime");

                entity.Property(e => e.DateStart).HasColumnType("datetime");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.DiscountProduct)
                    .HasForeignKey(d => d.IdProduct)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_DiscountProduct_Product");
            });

            modelBuilder.Entity<Fabricator>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.Commentary).HasMaxLength(500);
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.Property(e => e.Commentary).HasMaxLength(500);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.IdOrderNavigation)
                    .WithMany(p => p.OrderStatus)
                    .HasForeignKey(d => d.IdOrder)
                    .HasConstraintName("FK_OrderStatus_Order");

                entity.HasOne(d => d.IdStatusNavigation)
                    .WithMany(p => p.OrderStatus)
                    .HasForeignKey(d => d.IdStatus)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_OrderStatus_TypeStatusOrder");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Poster).HasColumnType("image");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasOne(d => d.IdFabricatorNavigation)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.IdFabricator)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Product_Fabricator");

                entity.HasOne(d => d.IdSubCategoryNavigation)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.IdSubCategory)
                    .HasConstraintName("FK_Product_SubCategoryProduct");
            });

            modelBuilder.Entity<ProductsInOrder>(entity =>
            {
                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.ProductsInOrder)
                    .HasForeignKey(d => d.IdProduct)
                    .HasConstraintName("FK_ProductsInOrder_Product");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ProductsInOrder)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_ProductsInOrder_Order");
            });

            modelBuilder.Entity<SubCategoryProduct>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Poster).HasColumnType("image");

                entity.HasOne(d => d.IdCategoryNavigation)
                    .WithMany(p => p.SubCategoryProduct)
                    .HasForeignKey(d => d.IdCategory)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_SubCategoryProduct_CategoryProduct");
            });

            modelBuilder.Entity<TypeStatusOrder>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
