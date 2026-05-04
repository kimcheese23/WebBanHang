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
        public void Add(Product product) { db.Products.Add(product); db.SaveChanges(); }
        public void Update(Product product) { db.Products.Update(product); db.SaveChanges(); }
        public void Delete(int id)
        {
            var product = GetById(id);
            if (product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();
            }
        }
    }

}
