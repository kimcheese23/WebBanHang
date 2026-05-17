using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanHang.DTO.Entity;
using WebBanHang.DTO.Responses;
using Microsoft.EntityFrameworkCore;

namespace WebBanHang.DAL.Repositories
{
    public class OrderRepository
    {
        private readonly MyDbContext _context;

        public OrderRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateOrderWithTransactionAsync(Order order, List<OrderDetail> details)
        {
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    foreach (var item in details)
                    {
                        item.OrderId = order.Id;
                        _context.OrderDetails.Add(item);
                    }

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Quá trình đặt hàng thất bại: " + ex.Message);
                }
            }
        }

        public async Task<List<UserOrderHistoryDTO>> GetUserOrderHistoryAsync(string userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new UserOrderHistoryDTO
                {
                    OrderId = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    TotalAmount = o.OrderDetails.Sum(od => od.Quantity * od.UnitPrice),
                    FirstProductName = o.OrderDetails.FirstOrDefault() != null ? o.OrderDetails.FirstOrDefault().Product.Name : "Đơn hàng"
                })
                .ToListAsync();

            return orders;
        }

        public async Task<bool> CancelOrderAsync(int orderId, string userId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order != null && (order.Status == 0 || order.Status == 1))
            {
                order.Status = 4;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
