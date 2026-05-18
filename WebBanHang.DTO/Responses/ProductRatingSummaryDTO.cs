using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBanHang.DTO.Responses
{
    public class ProductRatingSummaryDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int TotalReviews { get; set; }
        public decimal AverageRating { get; set; }
    }
}
