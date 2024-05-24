
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using PagedList;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using WebsiteChungKhoann.Models;

namespace WebsiteChungKhoann.Areas.Admin.Controllers
{
    public class CoursesController : Controller
    {
        private Mode1 db = new Mode1();

        // GET: Admin/Courses
        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            // Số trang hiện tại, nếu không có thì mặc định là 1
            int pageNumber = (page ?? 1);

            // Lấy danh sách các khóa học từ cơ sở dữ liệu, và kèm theo thông tin về tài khoản (nếu có)
            var courses = db.Courses.Include(c => c.Account);

            // Sắp xếp các khóa học theo một tiêu chí nào đó (nếu cần)
            // Ví dụ: courses = courses.OrderBy(c => c.TenCot);
            courses = courses.OrderBy(c => c.Id_Course);
            // Phân trang cho danh sách các khóa học
            var pagedCourses = courses.ToPagedList(pageNumber, pageSize);

            return View(pagedCourses);
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
            ViewBag.Id_Account = new SelectList(db.Accounts, "Id", "Name");
            return View();
        }

        // POST: Admin/Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Course,Name,Link_Id,Description,Create_Date")] Course course)
        {
            if (ModelState.IsValid)
            {
                // Lấy ngày hiện tại và cập nhật vào trường Create_Date của khóa học
                course.Create_Date = DateTime.Now;

                // Lấy ID của video từ đường dẫn YouTube
                string videoId = GetYouTubeVideoId(course.Link_Id);

                // Kiểm tra xem videoId có giá trị và hợp lệ không
                if (!string.IsNullOrEmpty(videoId))
                {
                    course.Link_Id = videoId;

                    // Lấy Id của người dùng từ Session (nếu tồn tại)
                    if (Session["Id"] != null && int.TryParse(Session["Id"].ToString(), out int userId))
                    {
                        course.Id_Account = userId;

                        // Lưu khóa học vào cơ sở dữ liệu
                        db.Courses.Add(course);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // Xử lý trường hợp không tìm thấy Id của người dùng trong Session
                        // hoặc không thể chuyển đổi sang kiểu int
                        ModelState.AddModelError("", "Không tìm thấy người dùng hoặc thông tin người dùng không hợp lệ.");
                    }
                }
                else
                {
                    // Xử lý trường hợp không thể lấy được ID của video từ đường dẫn YouTube
                    ModelState.AddModelError("Link_Id", "Không thể lấy ID của video từ đường dẫn YouTube.");
                }
            }

            // Nếu ModelState không hợp lệ, trả về View với dữ liệu đã nhập và thông báo lỗi
            ViewBag.Id_Account = new SelectList(db.Accounts, "Id", "Name", course.Id_Account);
            return View(course);
        }

        // GET: Admin/Courses/Edit/5
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
            ViewBag.Id_Account = new SelectList(db.Accounts, "Id", "Name", course.Id_Account);
            return View(course);
        }


        // POST: Admin/Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include = "Id_Course,Name,Link_Id,Description,Create_Date")] Course updatedCourse)
        {
            if (ModelState.IsValid)
            {
                var existingCourse = db.Courses.Find(id);

                if (existingCourse == null)
                {
                    return HttpNotFound();
                }

                string videoId = GetYouTubeVideoId(updatedCourse.Link_Id);

                if (!string.IsNullOrEmpty(videoId))
                {
                    updatedCourse.Link_Id = videoId;

                    if (Session["Id"] != null && int.TryParse(Session["Id"].ToString(), out int userId))
                    {
                        existingCourse.Name = updatedCourse.Name;
                        existingCourse.Link_Id = updatedCourse.Link_Id;
                        existingCourse.Description = updatedCourse.Description;
                        existingCourse.Create_Date = DateTime.Now;
                        existingCourse.Id_Account = userId;

                        try
                        {
                            db.Entry(existingCourse).State = EntityState.Modified;
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            ModelState.AddModelError("", "Không thể lưu thay đổi. Thực thể đã bị cập nhật bởi người khác.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Không tìm thấy người dùng hoặc thông tin người dùng không hợp lệ.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Link_Id", "Không thể lấy ID của video từ đường dẫn YouTube.");
                }
            }

            ViewBag.Id_Account = new SelectList(db.Accounts, "Id", "Name", updatedCourse.Id_Account);
            return View(updatedCourse);
        }

        public ActionResult Xoa(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
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
        public string GetYouTubeVideoId(string url)
        {
            string videoId = string.Empty;
            try
            {
                Uri videoUri = new Uri(url);
                string host = videoUri.Host.ToLower();

                if (host.Contains("youtube.com"))
                {
                    string query = videoUri.Query;
                    if (!string.IsNullOrEmpty(query))
                    {
                        videoId = System.Web.HttpUtility.ParseQueryString(query)["v"];
                    }
                    else
                    {
                        string[] segments = videoUri.Segments;
                        if (segments.Length >= 2)
                        {
                            videoId = segments[segments.Length - 1].Trim('/');
                        }
                    }
                }
                else if (host.Contains("youtu.be"))
                {
                    string[] segments = videoUri.Segments;
                    if (segments.Length >= 2)
                    {
                        videoId = segments[1].Trim('/');
                    }
                }

                // Kiểm tra xem videoId có phải là ID hợp lệ hay không
                if (!IsYouTubeVideoIdValid(videoId))
                {
                    videoId = string.Empty;
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu cần
            }
            return videoId;
        }

        private bool IsYouTubeVideoIdValid(string videoId)
        {
            // Kiểm tra xem videoId có đúng định dạng của một ID video YouTube hay không
            // Định dạng ID video YouTube là chuỗi có độ dài 11 ký tự
            return !string.IsNullOrEmpty(videoId) && videoId.Length == 11;
        }
    }
}
