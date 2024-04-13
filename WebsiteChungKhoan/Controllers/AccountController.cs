using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebsiteChungKhoan.Models;

namespace WebsiteChungKhoan.Controllers
{
    public class AccountController : Controller
    {
       // ChungKhoanDb Db = new ChungKhoanDb();
        // GET: Account

        Model1 db = new Model1();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register([Bind(Include = "Name,Email,Address,Password,Rolex")] Account acount)
        {

            if (ModelState.IsValid)
            {
                int maxId = db.Accounts.Max(a => (int?)a.Id) ?? 0;

                // Gán giá trị ID mới bằng ID lớn nhất tăng lên 1 đơn vị
                acount.Id = maxId + 1;

                db.Accounts.Add(acount);
                db.SaveChanges();
                return RedirectToAction("Login","Account");
            }
            else
            {
                return View("Register");
            }

            
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username , string password)
        {

             
            var y = db.Accounts.Where(e => e.Name.ToString() == username && e.Password.ToString() == password).FirstOrDefault();
            var x = db.Accounts.Where(e => e.Name.ToString() == username && e.Password.ToString() == password && e.Rolex=="Admin").FirstOrDefault();
            if (x!=null && y!=null)
            {
               
                Session["name"] = username;
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }else if(y != null && x == null)
            {
                Session["name"] = username;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.errorlogin = "sai tài khoản hoac mật khẩu";
                return View("Login");
            }
 
        }
        public ActionResult Logout()
        {
            Session["name"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}