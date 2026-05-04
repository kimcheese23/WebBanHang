using System.Collections.Generic;
using WebBanHang.DAL.Repositories;
using WebBanHang.DTO.Entity;

namespace WebBanHang.BLL.Services
{
    public class ProductService
    {
        private readonly ProductRepository repo;

        public ProductService(ProductRepository repository)
        {
            repo = repository;
        }

        public IEnumerable<Product> GetAllProducts() => repo.GetAll();
        public Product GetProduct(int id) => repo.GetById(id) ?? new Product();
        public void CreateProduct(Product product) => repo.Add(product);
        public void UpdateProduct(Product product) => repo.Update(product);
        public void DeleteProduct(int id) => repo.Delete(id);
    }
}
