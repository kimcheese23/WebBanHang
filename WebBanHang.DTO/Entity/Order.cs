using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanHang.DTO.Entity;

namespace WebBanHang.DTO.Entity
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}