using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using WebBanHang.BLL.Services;

namespace WebBanHang.GUI.Controllers
{
    [Authorize]
    public class UserOrderController : Controller
    {
        private readonly OrderService _orderService;

        public UserOrderController(OrderService orderService)
        {
            _orderService = orderService;
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
    }
}