using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBanHang.DTO.Requests
{
    public class ReviewRequestDTO
    {
        public string? UserId { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public required string Content { get; set; }
    }
}