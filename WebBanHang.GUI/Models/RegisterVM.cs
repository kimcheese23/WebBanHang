using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.ViewModel
{
    public class RegisterVM
    {
        [Required(ErrorMessage ="Can Not Be Blank")]
        public String UserName { get; set; }
        [Required]
        public String Password { get; set; }
        [Required]
        [Compare("Password",ErrorMessage ="Do not match Password")]
        public String ConfirmPassword { get; set; }
        [Required]
        [EmailAddress(ErrorMessage ="")]
        public String Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public String Addresss {  get; set; }
        public String City { get; set; }
        
    }
}