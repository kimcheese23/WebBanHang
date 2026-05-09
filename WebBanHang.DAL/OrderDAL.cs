using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanHang.DTO.Entity;
using Microsoft.EntityFrameworkCore;

namespace WebBanHang.DAL
{
    public class OrderDAL
    {
        private readonly MyDbContext _context;

        public OrderDAL(MyDbContext context)
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
    }
}
