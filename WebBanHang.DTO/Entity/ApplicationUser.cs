using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanHang.DTO.Entity;

namespace WebBanHang.DTO.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public String? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
    }
}