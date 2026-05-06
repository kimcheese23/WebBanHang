using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebBanHang.DAL.Repositories;
using WebBanHang.DTO.Entity;

namespace WebBanHang.BLL.Services
{
    public class ProductService
    {
        private readonly ProductRepository repo;

        public ProductService(ProductRepository repository)
        {
            repo = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IEnumerable<Product> GetAllProducts() => repo.GetAll();

        public Product? GetProduct(int id) => repo.GetById(id);

        public List<Product> SearchProducts(string? name)
        {
            string keyword = name?.Trim() ?? string.Empty;
            return repo.Search(keyword);
        }

        public async Task<IEnumerable<ProductDetail>> GetProductDetailsAsync()
        {
            return await repo.GetAllProductDetailsAsync();
        }

        public async Task CreateProduct(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            await repo.Add(product);
        }

        public async Task UpdateProduct(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (product.Price < 0) throw new ArgumentException("Giá sản phẩm không hợp lệ", nameof(product));
            await repo.Update(product);
        }

        public async Task DeleteProduct(int id) => await repo.Delete(id);
    }
}
