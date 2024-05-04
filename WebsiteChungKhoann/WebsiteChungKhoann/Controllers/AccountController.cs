using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteChungKhoann.Models;

namespace WebsiteChungKhoann.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        
        private Mode1 db = new Mode1();
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
                db.Accounts.Add(acount);
                db.SaveChanges();
                return RedirectToAction("Login", "Account");
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
        public ActionResult Login(string username, string password)
        {


            var y = db.Accounts.Where(e => e.Name.ToString() == username && e.Password.ToString() == password).FirstOrDefault();
            var x = db.Accounts.Where(e => e.Name.ToString() == username && e.Password.ToString() == password && e.Rolex == "Admin").FirstOrDefault();
            var account = db.Accounts.FirstOrDefault(e => e.Name == username && e.Password == password && e.Rolex == "Admin");
            var user = db.Accounts.FirstOrDefault(e => e.Name == username && e.Password == password);
            if (account != null)
            {
                int accountId = account.Id;
                // Sử dụng accountId ở đây
                ViewBag.accountId = accountId;
                // Tạo cookie và lưu Id_Account vào đó
                HttpCookie cookie = new HttpCookie("AccountId");
                cookie.Value = accountId.ToString();
                Response.Cookies.Add(cookie);
                Session["Id"] = account.Id;
                Debug.WriteLine("Cookie Value: " + cookie.Value);
            }
            if (user != null)
            {
                Session["Id"] = user.Id;
            }
            if (x != null)
            {
                Session["name"] = username;


                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
            else if (y != null)
            {
                int accountId = y.Id;

                // Tạo cookie và lưu Id_Account vào đó
                HttpCookie cookie = new HttpCookie("AccountId");
                cookie.Value = accountId.ToString();
                Response.Cookies.Add(cookie);

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
            Session["Id"] = null;
            Session["giohang"] = null;
            return RedirectToAction("Index", "Home");
        }


    }
}