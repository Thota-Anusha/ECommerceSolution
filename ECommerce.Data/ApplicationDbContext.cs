using Microsoft.EntityFrameworkCore;
using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product -> Category
            modelBuilder.Entity<Product>()
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey(p => p.CategoryId);

            // Product -> Inventory
            modelBuilder.Entity<Product>()
                .HasOne<Inventory>()
                .WithOne()
                .HasForeignKey<Inventory>(i => i.ProductId);

            // User -> Cart
            modelBuilder.Entity<User>()
                .HasOne<Cart>()
                .WithOne()
                .HasForeignKey<Cart>(c => c.UserId);

            // Cart -> CartItems
            modelBuilder.Entity<Cart>()
                .HasMany<CartItem>()
                .WithOne()
                .HasForeignKey(ci => ci.CartId);

            // CartItem -> Product
            modelBuilder.Entity<CartItem>()
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(ci => ci.ProductId);

            // User -> Orders
            modelBuilder.Entity<User>()
                .HasMany<Order>()
                .WithOne()
                .HasForeignKey(o => o.UserId);

            // Order -> OrderItems
            modelBuilder.Entity<Order>()
                .HasMany<OrderItem>()
                .WithOne()
                .HasForeignKey(oi => oi.OrderId);

            // OrderItem -> Product
            modelBuilder.Entity<OrderItem>()
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);

            // Order -> Payment
            modelBuilder.Entity<Order>()
                .HasOne<Payment>()
                .WithOne()
                .HasForeignKey<Payment>(p => p.OrderId);
        }





        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Inventory> Inventories { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Payment> Payments { get; set; }
    }
}
