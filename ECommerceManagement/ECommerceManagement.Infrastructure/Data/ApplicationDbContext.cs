using ECommerceManagement.Domain;
using ECommerceManagement.Domain.Entities;
using ECommerceManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=. ;Database=ECommerceT4;Integrated security=True;encrypt=false");
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =============================
            // Soft Delete Query Filters فقط للـ Root Entities
            // =============================
            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
            modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Order>().HasQueryFilter(o => !o.IsDeleted);
            // CartItem و OrderItem نتحكم عليهم في الاستعلام مباشرة

            // =============================
            // Relationships
            // =============================
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.CartItems)
                .WithOne(ci => ci.User)
                .HasForeignKey(ci => ci.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderItems)
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.CartItems)
                .WithOne(ci => ci.Product)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasPrecision(18, 2);

            // =============================
            // Seed Data
            // =============================

            modelBuilder.Entity<User>().HasData(
    new User
    {
        Id = 1,
        Username = "admin",
        Password = "123456",
        Role = UserRole.Admin,
        CreatedAt = new DateTime(2026, 2, 20, 12, 0, 0),
        CreatedBy = null,
        UpdatedAt = null,
        UpdatedBy = null,
        IsDeleted = false
    },
    new User
    {
        Id = 2,
        Username = "customer1",
        Password = "123456",
        Role = UserRole.Customer,
        CreatedAt = new DateTime(2026, 2, 20, 12, 0, 0),
        CreatedBy = null,
        UpdatedAt = null,
        UpdatedBy = null,
        IsDeleted = false
    }
);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Books" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Price = 1500, CategoryId = 1, ImageUrl = "laptop.png" },
                new Product { Id = 2, Name = "Smartphone", Price = 800, CategoryId = 1, ImageUrl = "smartphone.png" },
                new Product { Id = 3, Name = "C# Programming Book", Price = 50, CategoryId = 2, ImageUrl = "csharpbook.png" }
            );

            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    Id = 1,
                    UserId = 2,
                    Status = OrderStatus.Processing,
                    OrderDate = new DateTime(2026, 2, 20, 12, 0, 0),
                    CreatedAt = new DateTime(2026, 2, 20, 12, 0, 0),
                    CreatedBy = null,
                    UpdatedAt = null,
                    UpdatedBy = null,
                    IsDeleted = false
                }
            );

            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem
                {
                    Id = 1,
                    OrderId = 1,
                    ProductId = 1,
                    Quantity = 1,
                    Price = 1500,
                    CreatedAt = new DateTime(2026, 2, 20, 12, 0, 0),
                    CreatedBy = null,
                    UpdatedAt = null,
                    UpdatedBy = null,
                    IsDeleted = false
                }
            );

            modelBuilder.Entity<CartItem>().HasData(
                new CartItem
                {
                    Id = 1,
                    UserId = 2,
                    ProductId = 2,
                    Quantity = 2,
                    CreatedAt = new DateTime(2026, 2, 20, 12, 0, 0),
                    CreatedBy = null,
                    UpdatedAt = null,
                    UpdatedBy = null,
                    IsDeleted = false
                }
            );
        }

        // =============================
        // Override SaveChanges لتحديث الـ Audit Fields
        // =============================
        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                    entry.Property("IsDeleted").CurrentValue = false;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }
    }
}
