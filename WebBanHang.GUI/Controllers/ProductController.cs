using Microsoft.AspNetCore.Mvc;

namespace WebBanHang.GUI.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View(); 
        }
    }
}
