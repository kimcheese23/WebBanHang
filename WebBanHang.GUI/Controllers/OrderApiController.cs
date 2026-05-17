using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebBanHang.BLL.Services;
using WebBanHang.DTO.Requests;

namespace WebBanHang.GUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderApiController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderApiController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequestDTO request)
        {
            if (string.IsNullOrEmpty(request.UserId))
            {
                request.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            }

            if (string.IsNullOrEmpty(request.UserId))
                return BadRequest(new { success = false, message = "Bạn cần đăng nhập để đặt hàng." });

            bool isSaved = await _orderService.ProcessCheckoutAsync(request);
            if (isSaved)
            {
                return Ok(new { success = true, message = "Đặt hàng thành công!" });
            }
            return BadRequest(new { success = false, message = "Đặt hàng thất bại (Có thể do hết hàng tồn kho)." });
        }
    }
}