 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanHang.DTO.Entity;
using WebBanHang.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebBanHang.Controllers
{
    //public class AccountController : Controller
    //{

    //    public ActionResult Register()
    //    {
    //        return View();
    //    }
    //    [HttpPost]
    //    public ActionResult Register(RegisterVM rvm)
    //    {
    //        if (ModelState.IsValid) 
    //        { 
    //            var appDbContext = new AppDBContext();
    //            var appUserStore = new AppUserStore(appDbContext);
    //            var UserManager = new AppUserManager(appUserStore);
    //            var passwordHash = Crypto.HashPassword(rvm.Password);
    //            var user = new AppUser()
    //            {
    //                Email = rvm.Email,
    //                UserName = rvm.UserName,
    //                PasswordHash = passwordHash,
    //                City = rvm.City,
    //                Birthday = rvm.DateOfBirth,
    //                Address = rvm.Addresss
    //            };

    //            IdentityResult result = UserManager.Create(user);
    //            if (result.Succeeded)
    //            {
    //                UserManager.AddToRole(user.Id, "Customer");
    //                var authenManager = HttpContext.GetOwinContext().Authentication;
    //                var userIdentity = UserManager.CreateIdentity(user, 
    //                    DefaultAuthenticationTypes.ApplicationCookie);
    //                authenManager.SignIn(new AuthenticationProperties(), userIdentity);
    //            }
    //            return RedirectToAction("Index", "Product");
    //        }
    //        else
    //        {
    //            ModelState.AddModelError("New Error", "Invalid Data");
    //            return View();
    //        }
                
    //    }
    //    public ActionResult Login()
    //    {
    //        return View();
    //    }

    //    [HttpPost]
    //    public ActionResult Login(LoginVM login)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return View(login);
    //        }

    //        var appDbContext = new AppDBContext();
    //        var appUserStore = new AppUserStore(appDbContext);
    //        var UserManager = new AppUserManager(appUserStore);

    //        var user = UserManager.Find(login.UserName, login.Password);

    //        if (user != null)
    //        {
    //            var authenManager = HttpContext.GetOwinContext().Authentication;
    //            var userIdentity = UserManager.CreateIdentity(user,
    //                DefaultAuthenticationTypes.ApplicationCookie);
    //            authenManager.SignIn(new AuthenticationProperties(), userIdentity);

    //            if (UserManager.IsInRole(user.Id, "Admin"))
    //            {
    //                return RedirectToAction("Dashboard", "Admin");
    //            }
    //            else
    //            {
    //                return RedirectToAction("Index", "Product");
    //            }
    //        }
    //        else
    //        {
    //            ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng");
    //            return View(login);
    //        }
    //    }


    //    [Authorize]
    //    public ActionResult Logout()
    //    {
    //        var authenManager = HttpContext.GetOwinContext().Authentication;
    //        authenManager.SignOut();
    //        return RedirectToAction("Index", "Product");
    //    }
    //}
}