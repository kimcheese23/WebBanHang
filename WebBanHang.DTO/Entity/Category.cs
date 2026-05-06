using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanHang.DTO.Entity
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public virtual ICollection<Product> Product { get; set; } = new List<Product>();
    }
}