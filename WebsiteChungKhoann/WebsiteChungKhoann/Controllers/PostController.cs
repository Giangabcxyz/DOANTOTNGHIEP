using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteChungKhoann.Models;

namespace WebsiteChungKhoann.Controllers
{
    public class PostController : Controller
    {
        private Mode1 db = new Mode1();
        // GET: Post
        public ActionResult Index()
        {
            var list = db.Posts.ToList();
            var accounts = db.Accounts.ToDictionary(a => a.Id, a => a.Name);
            ViewBag.Accounts = accounts;
            var comments = (from comment in db.Comments_Post
                            join account in db.Accounts on comment.Id_Account equals account.Id// Điều kiện để lấy các bình luận của bài đăng hiện tại
                            select new CommentViewModel
                            {
                                Comment = comment.Comment,
                                AccountName = account.Name, // Lấy tên từ bảng Account
                                Id_Post = (int)comment.Id_Post                           // Các thuộc tính khác của bình luận nếu cần
                            }).ToList();

            // Gán danh sách bình luận vào ViewBag
            ViewBag.Comment = comments;
            return View(list);
        }
        [HttpPost]
        public ActionResult Create(string comment, int Id_Account, string Name, int Id_Post)
        {

            Comment_Post newComment = new Comment_Post
            {
                Comment = comment,
                Id_Account = Id_Account,
                Id_Post = Id_Post
            };

            // Thêm mới đối tượng Comment vào cơ sở dữ liệu
            db.Comments_Post.Add(newComment);
            db.SaveChanges();

            // Trả về Action Index trong Controller khác có tên là HomeController
            return RedirectToAction("Index");



        }
        public class CommentViewModel
        {
            public string Comment { get; set; }
            public string AccountName { get; set; }
            public int Id_Post { get; set; }
        }
    }
}