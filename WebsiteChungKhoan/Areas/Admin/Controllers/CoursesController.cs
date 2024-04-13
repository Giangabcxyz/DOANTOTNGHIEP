using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using WebsiteChungKhoan.Models;

namespace WebsiteChungKhoan.Areas.Admin.Controllers
{
    public class CoursesController : Controller
    {
        private Model1 db = new Model1();

        // GET: Admin/Courses
        public ActionResult Index()
        {
            var courses = db.Courses.Include(c => c.Comment);
            return View(courses.ToList());
        }

        // GET: Admin/Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Admin/Courses/Create
        public ActionResult Create()
        {
            ViewBag.Id_Course = new SelectList(db.Comments, "Id_Comment", "Comment1");
            return View();
        }

        // POST: Admin/Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Course,Name,Link_Id,Description,Id_Account")] Course course)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(course.Link_Id) && IsYouTubeUrl(course.Link_Id))
                {
                    // Lấy chỉ ID của video từ URL YouTube
                    string youtubeVideoId = GetYoutubeVideoId(course.Link_Id);
                    // Gán ID video vào trường Link_Id
                    course.Link_Id = youtubeVideoId;
                }

                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Course = new SelectList(db.Comments, "Id_Comment", "Comment1", course.Id_Course);
            return View(course);
        }

        private bool IsYouTubeUrl(string url)
        {
            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                return uri.Host.ToLower().Contains("youtube.com") || uri.Host.ToLower().Contains("youtu.be");
            }
            return false;
        }

        private string GetYoutubeVideoId(string url)
        {
            try
            {
                Uri uri = new Uri(url);
                string host = uri.Host.ToLower();

                if (host.Contains("youtu.be"))
                {
                    // If the URL is of the format https://youtu.be/RowlWdRlfqk
                    return uri.Segments[1];
                }
                else if (host.Contains("youtube.com"))
                {
                    // If the URL is of the format https://www.youtube.com/watch?v=RowlWdRlfqk
                    string queryString = uri.Query;
                    string[] queryParams = queryString.Split('&');
                    foreach (string param in queryParams)
                    {
                        if (param.StartsWith("?v="))
                        {
                            return param.Substring(3);
                        }
                    }
                }
            }
            catch (UriFormatException)
            {
                // URL is not in correct format
                // Handle accordingly
            }

            return null;
        }

        // GET: Admin/Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Course = new SelectList(db.Comments, "Id_Comment", "Comment1", course.Id_Course);
            return View(course);
        }

        // POST: Admin/Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Course,Name,Link_Id,Description,Id_Account")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Course = new SelectList(db.Comments, "Id_Comment", "Comment1", course.Id_Course);
            return View(course);
        }

        // GET: Admin/Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Admin/Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
