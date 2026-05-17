using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using WebBanHang.DTO.Entity;

namespace WebBanHang.DAL
{
    public class MyDbContext : IdentityDbContext<ApplicationUser>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("ApplicationUser");
            builder.Entity<IdentityRole>().ToTable("Role");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRole");

            builder.Entity<OrderDetail>()
                    .ToTable(tb => tb.HasTrigger("trg_UpdateProductQuantity"));

            builder.Entity<Order>()
                .ToTable(tb => tb.HasTrigger("trg_LogOrderStatusChange"));


            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Thiết bị điện tử" },
                new Category { Id = 2, Name = "Laptop" },
                new Category { Id = 3, Name = "Phụ kiện" },
                new Category { Id = 4, Name = "Tablet" }
            );

            builder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "iPhone 14", Price = 25000000, Quantity = 10, Image = "iphone14.jpg", CategoryId = 1, IsDeleted = false },
                new Product { Id = 2, Name = "Laptop Dell", Price = 20000000, Quantity = 10, Image = "laptop.jpg", CategoryId = 2, IsDeleted = false },
                new Product { Id = 3, Name = "Tai nghe AirPods Pro", Price = 6000000, Quantity = 15, Image = "airpods.jpg", CategoryId = 3, IsDeleted = false },
                new Product { Id = 4, Name = "iPad Pro", Price = 25000000, Quantity = 6, Image = "ipad.jpg", CategoryId = 4, IsDeleted = false }
            );
        }
    }
}