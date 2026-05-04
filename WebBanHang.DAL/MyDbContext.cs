using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebBanHang.DTO.Entity;

namespace WebBanHang.DAL
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Thiết bị điện tử" },
                new Category { Id = 2, Name = "Laptop" },
                new Category { Id = 3, Name = "Phụ kiện" },
                new Category { Id = 4, Name = "Tablet" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "iPhone 14", Price = 25000000, Quantity = 10, Image = "iphone14.jpg", CategoryId = 1 },
                new Product { Id = 2, Name = "Laptop Dell", Price = 20000000, Quantity = 10, Image = "laptop.jpg", CategoryId = 2 },
                new Product { Id = 3, Name = "Tai nghe AirPods Pro", Price = 6000000, Quantity = 15, Image = "airpods.jpg", CategoryId = 3 },
                new Product { Id = 4, Name = "iPad Pro", Price = 25000000, Quantity = 6, Image = "ipad.jpg", CategoryId = 4 }
            );
        }

    }
}
