using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebBanHang.BLL.Services;
using WebBanHang.DAL;

namespace WebBanHang.GUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly CategoryService _categoryService;
        private readonly ProductService _productService;
        private readonly MyDbContext _context;
        private readonly ReviewService _reviewService;

        public ProductController(
            CategoryService categoryService,
            ProductService productService,
            MyDbContext context,
            ReviewService reviewService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _context = context;
            _reviewService = reviewService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.CategoryList = _categoryService.GetCategoriesForDropdown();
            var products = _productService.GetAllProducts().ToList();

            var ratings = await _productService.GetProductRatingsAsync();
            ViewBag.ProductRatings = ratings.ToDictionary(r => r.ProductId);

            return View(products);
        }


        public async Task<IActionResult> Details(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            var ratings = await _productService.GetProductRatingsAsync();
            ViewBag.RatingInfo = ratings.FirstOrDefault(r => r.ProductId == id);

            var reviews = _context.Reviews
                .Where(r => r.ProductId == id)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new {
                    r.Rating,
                    r.Content,
                    r.CreatedAt,
                    UserEmail = _context.Users.Where(u => u.Id == r.UserId).Select(u => u.Email).FirstOrDefault()
                }).ToList();

            ViewBag.Reviews = reviews;

            bool canReview = false;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userId))
            {
                canReview = await _reviewService.CheckReviewEligibilityAsync(userId, id);
                ViewBag.MyReview = await _reviewService.GetExistingReviewAsync(userId, id);
            }

            ViewBag.CanReview = canReview;

            return View(product);
        }

    }
}