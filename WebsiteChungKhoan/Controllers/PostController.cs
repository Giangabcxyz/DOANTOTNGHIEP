using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteChungKhoan.Models;

namespace WebsiteChungKhoan.Controllers
{
    public class PostController : Controller
    {
        private Model1 db = new Model1();
        // GET: Post
        public ActionResult Index()
        {
            var list =  db.Posts.ToList();
            return View(list);
        }
    }
}