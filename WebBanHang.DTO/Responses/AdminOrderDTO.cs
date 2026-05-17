using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBanHang.DTO.Responses
{
    public class AdminOrderDTO
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerAccount { get; set; }

        public decimal TotalAmount { get; set; }
        public int Status { get; set; }
    }
}
