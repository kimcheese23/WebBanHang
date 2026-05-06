using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebBanHang.DAL;
using WebBanHang.DTO.Entity;

namespace WebBanHang.DAL.Repositories
{
    public class ProductRepository
    {
        private readonly MyDbContext db;

        public ProductRepository(MyDbContext context)
        {
            db = context;
        }

        public IEnumerable<Product> GetAll() => db.Products.ToList();
        public Product? GetById(int id) => db.Products.FirstOrDefault(p => p.Id == id);

        public async Task<List<ProductDetail>> GetAllProductDetailsAsync()
        {
            // Gọi View để lấy danh sách sản phẩm kèm tên Category
            return await db.ProductDetails
                           .FromSqlRaw("SELECT * FROM View_ProductDetails")
                           .ToListAsync();
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
                .Where(p => p.Name.Contains(keyword))
                .ToList();
        }

        public async Task Update(Product p)
        {
            await db.Database.ExecuteSqlRawAsync("EXEC sp_UpdateProduct {0}, {1}, {2}, {3}, {4}, {5}",
            p.Id, p.Name, p.Price, p.Quantity, p.CategoryId, p.Image ?? (object)DBNull.Value);
            db.ChangeTracker.Clear(); // Xóa cache để đảm bảo dữ liệu mới nhất được lấy khi truy vấn lại
        }

        public async Task Delete(int id)
        {
            await db.Database.ExecuteSqlRawAsync("EXEC sp_DeleteProduct {0}", id);
            db.ChangeTracker.Clear(); // Xóa cache để đảm bảo dữ liệu mới nhất được lấy khi truy vấn lại
        }        
    }
}
