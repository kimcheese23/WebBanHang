using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBanHang.BLL.Services;

namespace WebBanHang.GUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminOrderController : ControllerBase
    {
        private readonly AdminService _adminService;

        public AdminOrderController(AdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("orders")]
        public IActionResult GetOrders()
        {
            var data = _adminService.GetOrdersWithADO();
            return Ok(data);
        }

        [HttpPut("orders/{id}/status")]
        public IActionResult UpdateStatus(int id, [FromBody] int newStatus)
        {
            _adminService.UpdateOrderStatus(id, newStatus);
            return Ok(new { message = "Cập nhật trạng thái thành công!" });
        }
    }
}