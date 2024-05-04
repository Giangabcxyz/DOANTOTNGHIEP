using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteChungKhoann.Models;
using static WebsiteChungKhoann.Controllers.PostController;

namespace WebsiteChungKhoann.Controllers
{
    public class CourseController : Controller
    {
        private Mode1 db = new Mode1();
        // GET: Course
        public ActionResult Index()
        {
            var list = db.Courses.ToList();
            var accounts = db.Accounts.ToDictionary(a => a.Id, a => a.Name);
            ViewBag.Accounts = accounts;
            var comments = (from comment in db.Comments_Course
                            join account in db.Accounts on comment.Id_Account equals account.Id// Điều kiện để lấy các bình luận của bài đăng hiện tại
                            select new CommentViewModel
                            {
                                Comment = comment.Comment,
                                AccountName = account.Name, // Lấy tên từ bảng Account
                                Id_Course = (int)comment.Id_Course                           // Các thuộc tính khác của bình luận nếu cần
                            }).ToList();
            ViewBag.Comment = comments;
            // Gán danh sách bình luận và
            return View(list);
        }
        [HttpPost]
        public ActionResult Create(string Comment, int Id_Account, string Name, int Id_Course)
        {
            if (Id_Account == 0)
            {
                // Redirect hoặc hiển thị thông báo lỗi khi Id_Account là null
                return RedirectToAction("Login", "Account"); // Chuyển hướng đến trang đăng nhập
            }
           
            Comment_Course newComment = new Comment_Course
            {
                Comment = Comment,
                Id_Account = Id_Account,
                Id_Course = Id_Course
            };

            // Thêm mới đối tượng Comment vào cơ sở dữ liệu
            db.Comments_Course.Add(newComment);
            db.SaveChanges();

            // Trả về Action Index trong Controller khác có tên là HomeController
            return RedirectToAction("Detail", "Course", new { id = Id_Course });
            



        }

        public ActionResult Detail(int id)
        {
            var list =  db.Courses.ToList();
            ViewBag.id = id;
            // Lấy danh sách các comment từ csdl dựa trên id của video
            var comments = (from comment in db.Comments_Course
                            join account in db.Accounts on comment.Id_Account equals account.Id// Điều kiện để lấy các bình luận của bài đăng hiện tại
                            select new CommentViewModel
                            {
                                Comment = comment.Comment,
                                AccountName = account.Name, // Lấy tên từ bảng Account
                                Id_Course = (int)comment.Id_Course                           // Các thuộc tính khác của bình luận nếu cần
                            }).ToList();
            ViewBag.Comment = comments;
            return View(list);
        }
        public class CommentViewModel
        {
            public string Comment { get; set; }
            public string AccountName { get; set; }
            public int Id_Course { get; set; }
        }
    }
}