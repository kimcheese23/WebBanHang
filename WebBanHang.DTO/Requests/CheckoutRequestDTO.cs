using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBanHang.DTO.Requests
{
    public class CheckoutRequestDTO
    {
        public required string UserId { get; set; }
        public required List<CartItemDTO> Items { get; set; }
    }

    public class CartItemDTO
    {
        public int Id { get; set; } // Chính là ProductId
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
