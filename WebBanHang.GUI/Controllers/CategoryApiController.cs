using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBanHang.DAL;
using WebBanHang.DTO.Entity;

namespace WebBanHang.GUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoryApiController: ControllerBase
    {
        private readonly MyDbContext db;

        public CategoryApiController(MyDbContext db)
        {
            this.db = db;
        }
        [HttpGet]
        public IActionResult GetAll() => Ok(db.Categories.ToList());

        [HttpGet("search")]
        public IActionResult Search(string name)
        {
            var result = db.Categories
                .Where(c => c.Name.Contains(name))
                .ToList();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create(Category cat)
        {
            db.Categories.Add(cat);
            db.SaveChanges();
            return Ok(cat);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Category cat)
        {
            var existing = db.Categories.Find(id);
            if (existing == null) return NotFound();
            existing.Name = cat.Name;
            db.SaveChanges();
            return Ok(existing);
        }

        [HttpGet("categories-stat")]
        public IActionResult GetCategoriesStat()
        {
            var stats = db.Categories.Select(c => new
            {
                Id = c.Id,
                Name = c.Name,
                ProductCount = db.Products.Count(p => p.CategoryId == c.Id)
            }).ToList();

            return Ok(stats);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = db.Categories.Find(id);
            if (category == null) return NotFound();
            var products = db.Products.Where(p => p.CategoryId == id).ToList();

            foreach (var p in products)
            {
                p.CategoryId = 3; 
            }

            db.Categories.Remove(category);
            db.SaveChanges();

            return NoContent();
        }
    }
}
