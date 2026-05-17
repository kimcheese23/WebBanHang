using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBanHang.DTO.Responses
{
    public class DashboardSummaryDTO
    {
        public decimal TotalStockValue { get; set; }
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
    }
}