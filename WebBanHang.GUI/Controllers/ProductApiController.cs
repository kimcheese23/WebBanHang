using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBanHang.BLL;
using WebBanHang.BLL.Services;
using WebBanHang.DTO.Entity;

namespace WebBanHang.GUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductApiController : ControllerBase
    {
        private readonly ProductService s;

        public ProductApiController(ProductService service)
        {
            s = service;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(s.GetAllProducts());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = s.GetProduct(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetProductDetails()
        {
            var data = await s.GetProductDetailsAsync(); 
            return Ok(data);
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string name)
        {
            var results = s.SearchProducts(name);
            return Ok(results); 
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Product product, IFormFile? ImageFile)
        {
            product.Image = await HandleFileUpload(ImageFile) ?? "default.jpg";
            await s.CreateProduct(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromForm] Product product, IFormFile? ImageFile)
        {
            if (id != product.Id) return BadRequest("ID không khớp");

            var existing = s.GetProduct(id);
            if (existing == null || existing.Id == 0) return NotFound();

            if (ImageFile != null)
                product.Image = await HandleFileUpload(ImageFile);
            else
                product.Image = existing.Image;

            await s.UpdateProduct(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = s.GetProduct(id);
            if (existing == null || existing.Id == 0) return NotFound();

            await s.DeleteProduct(id);
            return Ok(new { message = "Đã xóa thành công" });
        }

        private async Task<string?> HandleFileUpload(IFormFile? file)
        {
            if (file == null) return null;
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/images", fileName);

            var dir = Path.GetDirectoryName(path);
            if (dir != null && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }
    }
}
