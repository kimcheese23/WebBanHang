using Microsoft.AspNetCore.Mvc;
using WebBanHang.BLL.Services;


namespace WebBanHang.GUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly CategoryService _categoryService;
        private readonly ProductService _productService;

        public ProductController(CategoryService categoryService, ProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }
        public IActionResult Index()
        {
            ViewBag.CategoryList = _categoryService.GetCategoriesForDropdown();
            var products = _productService.GetAllProducts().ToList();

            return View(products);
        }
    }
}
