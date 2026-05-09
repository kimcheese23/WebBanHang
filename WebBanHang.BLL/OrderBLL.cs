using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanHang.DAL;
using WebBanHang.DTO.Entity;
using WebBanHang.DTO.Requests;

namespace WebBanHang.BLL
{
    public class OrderBLL
    {
        private readonly OrderDAL _orderDAL;

        public OrderBLL(OrderDAL orderDAL)
        {
            _orderDAL = orderDAL;
        }

        public async Task<bool> ProcessCheckoutAsync(CheckoutRequestDTO request)
        {
            if (request == null || request.Items == null || request.Items.Count == 0)
                throw new ArgumentException("Giỏ hàng trống.");

            var order = new Order
            {
                OrderDate = DateTime.Now,
                UserId = request.UserId
            };

            var orderDetails = new List<OrderDetail>();
            foreach (var item in request.Items)
            {
                orderDetails.Add(new OrderDetail
                {
                    ProductId = item.Id,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                });
            }

            return await _orderDAL.CreateOrderWithTransactionAsync(order, orderDetails);
        }
    }
}
