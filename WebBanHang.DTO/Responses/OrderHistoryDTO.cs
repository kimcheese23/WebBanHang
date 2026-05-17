using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBanHang.DTO.Responses
{
    public class OrderHistoryDTO
    {
        public DateTime ChangedDate { get; set; }
        public int OldStatus { get; set; }
        public int NewStatus { get; set; }

        public string OldStatusName => GetStatusName(OldStatus);
        public string NewStatusName => GetStatusName(NewStatus);

        private string GetStatusName(int status)
        {
            return status switch
            {
                0 => "Chờ xác nhận",
                1 => "Chờ lấy hàng",
                2 => "Đang giao hàng",
                3 => "Đã giao thành công",
                4 => "Đã hủy",
                _ => "Không xác định"
            };
        }
    }
}
