using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
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
            var user = db.Accounts.FirstOrDefault(e => e.Name == username && e.Password == password);
          
            if (user != null)
            {
                Session["Id"] = user.Id;
                Session["name"] = user.Name;
                Session["password"] = user.Password;
                Session["role"] = user.Rolex;
                ViewBag.id = user.Id;
                ViewBag.rolex = user.Rolex;
                
                //if (user.Rolex == "Admin")
                //{

                //    return RedirectToAction("Index", "Admin", new { area = "Admin" });
                //}
                if (Session["post"]!=null)
                {
                    Session["post"] = null;
                    return RedirectToAction("Index", "Post");
                }
                if (Session["Course"] !=null)
                {
                        
                        return RedirectToAction("Detail", "Course", new { id = int.Parse(Session["Course"].ToString()) });
                   
                }
                if (Session["Pro"] != null)
                {

                    return RedirectToAction("Detail", "Product", new { id = int.Parse(Session["Pro"].ToString()) });

                }
                if (Session["Cart"] != null)
                {

                    return RedirectToAction("Index", "Cart");

                }
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
            Session["post"] = null;
            Session["Course"] = null;
            Session["Pro"] = null;
            return RedirectToAction("Index", "Home");
        }


    }
}