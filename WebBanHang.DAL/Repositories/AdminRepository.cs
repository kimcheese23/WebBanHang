using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebBanHang.DAL;
using WebBanHang.DTO.Responses;

namespace WebBanHang.DAL.Repositories
{
    public class AdminRepository
    {
        private readonly MyDbContext _context;

        public AdminRepository(MyDbContext context)
        {
            _context = context;
        }

        public List<AdminOrderDTO> GetOrdersWithADO()
        {
            string? connectionString = _context.Database.GetDbConnection().ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
                return new List<AdminOrderDTO>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.sp_GetAdminOrders", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        var orderList = dt.AsEnumerable().Select(row => new AdminOrderDTO
                        {
                            OrderId = Convert.ToInt32(row["OrderId"]),
                            OrderDate = Convert.ToDateTime(row["OrderDate"]),
                            CustomerName = row["CustomerName"] != DBNull.Value ? row["CustomerName"].ToString() : "Chưa cập nhật",
                            CustomerAccount = row["CustomerAccount"] != DBNull.Value ? row["CustomerAccount"].ToString() : "Khách vãng lai",
                            TotalAmount = row["TotalAmount"] != DBNull.Value ? Convert.ToDecimal(row["TotalAmount"]) : 0m,
                            Status = row["Status"] != DBNull.Value ? Convert.ToInt32(row["Status"]) : 0
                        }).ToList();

                        return orderList;
                    }
                }
            }
        }

        public bool UpdateOrderStatus(int orderId, int newStatus)
        {
            var order = _context.Orders.Find(orderId);
            if (order == null) return false;

            int currentStatus = order.Status;

            if (currentStatus == 4 || currentStatus == 3)
            {
                throw new Exception("Đơn hàng đã hoàn thành hoặc đã hủy, không thể thay đổi trạng thái nữa!");
            }

            if (newStatus != currentStatus + 1)
            {
                string mgsError = "";
                switch (currentStatus)
                {
                    case 0: mgsError = "Đơn chưa xác nhận! Bạn chỉ có thể chuyển sang 'Chờ lấy hàng'."; break;
                    case 1: mgsError = "Đơn đang chờ lấy! Bạn chỉ có thể chuyển sang 'Đang giao hàng'."; break;
                    case 2: mgsError = "Đơn đang giao! Bạn chỉ có thể chuyển sang 'Đã giao thành công'."; break;
                }
                throw new Exception(mgsError);
            }

            order.Status = newStatus;
            _context.SaveChanges();

            return true;
        }

        public async Task<DashboardSummaryDTO> GetDashboardSummaryAsync()
        {
            var stockValueQuery = _context.Database.SqlQueryRaw<decimal>("SELECT dbo.fn_CalculateStock() AS Value");
            decimal stockValue = await stockValueQuery.FirstOrDefaultAsync();

            int totalProducts = await _context.Products.CountAsync(p => !p.IsDeleted);
            int totalOrders = await _context.Orders.CountAsync(o => o.Status == 3);

            return new DashboardSummaryDTO
            {
                TotalStockValue = stockValue,
                TotalProducts = totalProducts,
                TotalOrders = totalOrders
            };
        }

        public async Task<List<MonthlyRevenueDTO>> GetMonthlyRevenueReportAsync(int year)
        {
            var query = _context.Database.SqlQueryRaw<MonthlyRevenueDTO>(
                "SELECT Month, TotalRevenue, TotalOrders FROM dbo.fn_GetMonthlyRevenueReport({0})", year);

            return await query.ToListAsync();
        }

        public async Task<List<TopSellingProductDTO>> GetTopSellingProductsAsync(int topCount)
        {
            var topParam = new SqlParameter("@TopCount", topCount);

            var query = _context.Database.SqlQueryRaw<TopSellingProductDTO>(
                "EXEC dbo.sp_GetTopSellingProducts @TopCount", topParam);

            return await query.ToListAsync();
        }
    }
}