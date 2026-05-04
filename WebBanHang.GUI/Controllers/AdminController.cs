using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanHang.DAL;

namespace WebBanHang.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        //public ActionResult Dashboard()
        //{
        //    ViewBag.ProductCount = db.products.Count();
        //    ViewBag.OrderCount = db.orders.Count();
        //    return View();
        //}


        //public ActionResult Products()
        //{
        //    var products = db.products.ToList();
        //    return View(products);
        //}

        //public ActionResult Orders()
        //{
        //    var orders = db.orders.Include("OrderDetails").ToList();
        //    return View(orders);
        //}
    }

}