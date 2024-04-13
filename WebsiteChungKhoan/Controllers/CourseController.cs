using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteChungKhoan.Models;

namespace WebsiteChungKhoan.Controllers
{
    public class CourseController : Controller
    {
        private Model1 db = new Model1();
        // GET: Courses
        public ActionResult Index()
        {
            var list = db.Courses.ToList();
            return View(list);
        }
    }
}