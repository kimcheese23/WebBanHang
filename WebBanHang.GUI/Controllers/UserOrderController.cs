using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebBanHang.BLL.Services;
using WebBanHang.DAL;

namespace WebBanHang.GUI.Controllers
{
    [Authorize]
    public class UserOrderController : Controller
    {
        private readonly OrderService _orderService;
        private readonly MyDbContext db; 

        public UserOrderController(OrderService orderService, MyDbContext context)
        {
            _orderService = orderService;
            db = context;
        }

        public async Task<IActionResult> History()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderService.GetUserOrderHistoryAsync(userId);
            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _orderService.CancelOrderAsync(orderId, userId);

            if (success)
                TempData["Message"] = "Hủy đơn hàng thành công!";
            else
                TempData["Error"] = "Không thể hủy đơn hàng này. Vui lòng kiểm tra lại trạng thái!";

            return RedirectToAction("History");
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = await db.Orders.FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
            {
                return NotFound("Không tìm thấy đơn hàng hoặc bạn không có quyền xem đơn hàng này!");
            }

            var orderDetails = await db.OrderDetails
                .Include(od => od.Product)
                .Where(od => od.OrderId == id)
                .ToListAsync();

            ViewBag.OrderStatus = order.Status;
            ViewBag.OrderId = order.Id;
            ViewBag.OrderDate = order.OrderDate;

            return View(orderDetails);
        }
    }
}