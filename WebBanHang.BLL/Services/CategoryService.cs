using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanHang.DAL.Repositories;
using WebBanHang.DTO.Entity;

namespace WebBanHang.BLL.Services
{
    public class CategoryService
    {
        private readonly CategoryRepository _categoryRepo;

        public CategoryService(string connectionString)
        {
            _categoryRepo = new CategoryRepository(connectionString);
        }

        public List<Category> GetCategoriesForDropdown()
        {
            return _categoryRepo.GetAllCategoriesADO();
        }
    }
}
