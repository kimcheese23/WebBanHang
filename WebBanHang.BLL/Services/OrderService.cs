using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebBanHang.DAL.Repositories;
using WebBanHang.DTO.Entity;
using WebBanHang.DTO.Requests;
using WebBanHang.DTO.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace WebBanHang.BLL.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;

        public OrderService(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<bool> ProcessCheckoutAsync(CheckoutRequestDTO request)
        {
            if (request == null || request.Items == null || request.Items.Count == 0)
                throw new ArgumentException("Giỏ hàng trống.");

            var order = new Order
            {
                OrderDate = DateTime.Now,
                UserId = request.UserId,
                Status = 0,

                ShippingName = request.ShippingName,
                ShippingPhone = request.ShippingPhone,
                ShippingAddress = request.ShippingAddress
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

            try
            {
                return await _orderRepository.CreateOrderWithTransactionAsync(order, orderDetails);
            }
            catch (Exception ex)
            {
                Exception rootCause = ex;
                while (rootCause.InnerException != null)
                {
                    rootCause = rootCause.InnerException;
                }
                string cleanMessage = rootCause.Message.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0];

                throw new Exception(cleanMessage);
            }
        }

        public async Task<List<UserOrderHistoryDTO>> GetUserOrderHistoryAsync(string userId)
        {
            return await _orderRepository.GetUserOrderHistoryAsync(userId);
        }

        public async Task<bool> CancelOrderAsync(int orderId, string userId)
        {
            return await _orderRepository.CancelOrderAsync(orderId, userId);
        }
    }
}