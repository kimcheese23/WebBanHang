using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHang.DAL;
using WebBanHang.DTO.Entity;

namespace WebBanHang.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly MyDbContext db;

        public AdminController(MyDbContext context)
        {
            db = context;
        }

        public IActionResult Dashboard()
        {
            ViewBag.ProductCount = db.Products.Count();
            ViewBag.UserCount = db.Users.Count(); 
            ViewBag.OrderCount = db.Orders.Count();

            return View();
        }

        public IActionResult Products()
        {
            var categories = db.Categories.ToList();

            ViewBag.CategoryList = categories.Select(categories => new
            {
                Value = categories.Id,
                Text = categories.Name
            }).ToList();
            return View();
        }

        public IActionResult Categories()
        {
            return View();
        }
    }
}
