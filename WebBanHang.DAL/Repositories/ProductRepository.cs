using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.DAL;
using WebBanHang.DTO.Entity;
using WebBanHang.DTO.Responses;

namespace WebBanHang.DAL.Repositories
{
    public class ProductRepository
    {
        private readonly MyDbContext db;

        public ProductRepository(MyDbContext context)
        {
            db = context;
        }

        public IEnumerable<Product> GetAll() => db.Products.Where(p => p.IsDeleted == false).ToList();

        public Product? GetById(int id) => db.Products.FirstOrDefault(p => p.Id == id && p.IsDeleted == false);

        public async Task<List<ProductDetailDTO>> GetAllProductDetailsAsync()
        {
            return await db.Products
                .Include(p => p.Category)
                .Where(p => p.IsDeleted == false)
                .Select(p => new ProductDetailDTO
                {
                    Id = p.Id,
                    ProductName = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Image = p.Image,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.Name : "Chưa phân loại"
                }).ToListAsync();
        }

        public async Task<int> Add(Product p)
        {
            var result = await db.Database
                .SqlQueryRaw<int>("EXEC sp_AddProduct {0}, {1}, {2}, {3}, {4}",
                    p.Name, p.Price, p.Quantity, p.CategoryId, p.Image ?? (object)DBNull.Value)
                .ToListAsync();

            int newId = result.FirstOrDefault();
            p.Id = newId;
            return newId;
        }

        public List<Product> Search(string keyword)
        {
            return db.Products
                .Where(p => p.Name.Contains(keyword) && p.IsDeleted == false)
                .ToList();
        }

        public async Task Update(Product p)
        {
            await db.Database.ExecuteSqlRawAsync("EXEC sp_UpdateProduct {0}, {1}, {2}, {3}, {4}, {5}",
            p.Id, p.Name, p.Price, p.Quantity, p.CategoryId, p.Image ?? (object)DBNull.Value);
            db.ChangeTracker.Clear();
        }

        public async Task Delete(int id)
        {
            var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product != null)
            {
                product.IsDeleted = true;
                await db.SaveChangesAsync();
            }
        }
    }
}