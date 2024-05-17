using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteChungKhoann.Models;

namespace WebsiteChungKhoann.Controllers
{
    public class HomeController : Controller
    {
        private Mode1 db = new Mode1();
        public ActionResult Index()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.sl = db.Accounts.ToList().Count();
            return View(ViewBag.sl);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Category()
        {
            var categories = db.Categories;
            return PartialView("Category", categories);
        }

        [HttpGet]
        public ActionResult GetProductsByCategory(int categoryId)
        {
            var products = db.Products.Where(p => p.Id_Category == categoryId).ToList();
            return View(products);
        }
    }
}

//Ngân hàng: NCB
//Số thẻ: 9704198526191432198
//Tên chủ thẻ:NGUYEN VAN A
//Ngày phát hành:07 / 15
//Mật khẩu OTP:123456