using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebBanHang.DTO.Entity;
using WebBanHang.GUI.Models;
using WebBanHang.ViewModel;
using System.Linq;

namespace WebBanHang.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> uM;
        private readonly SignInManager<ApplicationUser> sM;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            uM = userManager;
            sM = signInManager;
        }

        [HttpGet] public IActionResult Register() => View();
        [HttpGet] public IActionResult Login() => View();

        [HttpPost("/api/account/register")]
        public async Task<IActionResult> RegisterApi([FromBody] RegisterVM rvm)
        {
            var user = new ApplicationUser
            {
                UserName = rvm.UserName,
                Email = rvm.Email,
                DateOfBirth = rvm.DateOfBirth,
                Address = rvm.Address
            };

            var result = await uM.CreateAsync(user, rvm.Password);
            if (result.Succeeded)
            {
                await uM.AddToRoleAsync(user, "Customer");
                await sM.SignInAsync(user, isPersistent: false);
                return Ok(new { success = true, redirectUrl = Url.Action("Index", "Product") });
            }

            return Ok(new { success = false, errors = result.Errors.Select(e => e.Description) });
        }

        [HttpPost("/api/account/login")]
        public async Task<IActionResult> LoginApi([FromBody] LoginVM login)
        {
            var user = await uM.FindByNameAsync(login.UserName);
            if (user != null)
            {
                var result = await sM.PasswordSignInAsync(user, login.Password, false, false);
                if (result.Succeeded)
                {
                    string url = await uM.IsInRoleAsync(user, "Admin")
                                 ? "/Admin/Dashboard"
                                 : "/Product/Index";

                    return Ok(new { success = true, redirectUrl = url });
                }
            }
            return Ok(new { success = false, message = "Tài khoản hoặc mật khẩu không đúng" });
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await sM.SignOutAsync();
            return RedirectToAction("Index", "Product");
        }
    }
}