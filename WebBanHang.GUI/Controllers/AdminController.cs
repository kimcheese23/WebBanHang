using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHang.DAL;
using Microsoft.Data.SqlClient;
using System.Data;
using WebBanHang.BLL.Services;
using WebBanHang.DTO.Responses;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanHang.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AdminService _adminService;
        private readonly MyDbContext db;

        public AdminController(AdminService adminService, MyDbContext context)
        {
            _adminService = adminService;
            db = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            int currentYear = DateTime.Now.Year;

            ViewBag.UserCount = await db.Users.CountAsync();

            var summary = await _adminService.GetDashboardSummaryAsync();
            ViewBag.Summary = summary;

            var topProducts = await _adminService.GetTopSellingProductsAsync(5);
            ViewBag.TopProducts = topProducts;

            var revenueReport = await _adminService.GetMonthlyRevenueReportAsync(currentYear);
            if (revenueReport != null && revenueReport.Any())
            {
                ViewBag.ChartMonths = string.Join(",", revenueReport.Select(r => $"\"Tháng {r.Month}\""));
                ViewBag.ChartRevenue = string.Join(",", revenueReport.Select(r => r.TotalRevenue));
                ViewBag.ChartOrders = string.Join(",", revenueReport.Select(r => r.TotalOrders));
            }

            return View();
        }

        public IActionResult Products()
        {
            var categories = db.Categories.ToList();

            ViewBag.CategoryList = categories.Select(c => new
            {
                Value = c.Id,
                Text = c.Name
            }).ToList();

            return View();
        }

        public IActionResult Categories()
        {
            return View();
        }

        public IActionResult Orders()
        {
            var orderList = _adminService.GetOrdersWithADO();
            return View(orderList);
        }

        [HttpPost]
        public IActionResult ChangeOrderStatus(int orderId, int newStatus)
        {
            try
            {
                bool success = _adminService.UpdateOrderStatus(orderId, newStatus);

                if (success)
                {
                    return Ok(new { success = true, message = "Cập nhật trạng thái thành công!" });
                }
                return BadRequest(new { success = false, message = "Không tìm thấy đơn hàng." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        public IActionResult GetOrderHistory(int orderId)
        {
            try
            {
                var history = _adminService.GetOrderHistory(orderId);
                return Ok(new { success = true, data = history });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            try
            {
                var orderDetails = await db.OrderDetails
                    .Include(od => od.Product)
                    .Where(od => od.OrderId == orderId)
                    .Select(od => new {
                        productName = od.Product.Name,
                        productImage = od.Product.Image ?? "default.jpg",
                        unitPrice = od.UnitPrice,
                        quantity = od.Quantity,
                        totalPrice = od.UnitPrice * od.Quantity,
                        isDeleted = od.Product.IsDeleted
                    })
                    .ToListAsync();

                return Ok(new { success = true, data = orderDetails });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

    }
}