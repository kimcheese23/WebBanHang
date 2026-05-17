using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebBanHang.BLL.Services;
using WebBanHang.DTO.Requests;

namespace WebBanHang.GUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderBLL;

        public OrderController(OrderService orderBLL)
        {
            _orderBLL = orderBLL;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequestDTO request)
        {
            try
            {
                request.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(request.UserId))
                    return Unauthorized(new { success = false, message = "Vui lòng đăng nhập lại!" });

                bool result = await _orderBLL.ProcessCheckoutAsync(request);

                if (result)
                    return Ok(new { success = true, message = "Đặt hàng thành công!" });

                return BadRequest(new { success = false, message = "Có lỗi xảy ra khi xử lý đơn hàng." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
