using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteChungKhoann.Models;

namespace WebsiteChungKhoann.Areas.Admin.Controllers
{
    public class PostsController : Controller
    {
        private Mode1 db = new Mode1();

        // GET: Admin/Posts
        public ActionResult Index(int? page)
        {
            // Số lượng bài viết trên mỗi trang
            int pageSize = 5;
            // Số trang hiện tại, nếu không có thì mặc định là 1
            int pageNumber = (page ?? 1);

            // Truy vấn dữ liệu từ DbSet
            var postsQuery = db.Posts;

            // Sắp xếp các bài viết theo một tiêu chí nào đó (ví dụ: Id hoặc Ngày đăng)
            var sortedPostsQuery = postsQuery.OrderByDescending(p => p.Id_Post);

            // Phân trang cho danh sách các bài viết
            var pagedPosts = sortedPostsQuery.ToPagedList(pageNumber, pageSize);

            return View(pagedPosts);
        }


        // GET: Admin/Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Admin/Posts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Post,Name,Description,Img")] Post post, HttpPostedFileBase imgFile)
        {
            if (ModelState.IsValid)
            {
                // Lấy ngày hiện tại và cập nhật vào trường Date
                post.Date = DateTime.Now;
                // Kiểm tra xem Session["Id"] có tồn tại không
                if (Session["Id"] != null)
                {
                    // Ép kiểu Session["Id"] về kiểu int (nếu cần thiết)
                    int id;
                    if (int.TryParse(Session["Id"].ToString(), out id))
                    {
                        // Gán Id_Account bằng Id của người dùng hiện tại
                        post.Id_Account = id;
                    }
                    else
                    {
                        // Xử lý trường hợp không thể chuyển đổi được Session["Id"] sang kiểu int
                    }
                }
                else
                {
                    // Xử lý trường hợp Session["Id"] không tồn tại
                }


                if (imgFile != null && imgFile.ContentLength > 0)
                {
                    // Lưu tệp ảnh vào thư mục trên server
                    string fileName = Path.GetFileName(imgFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    imgFile.SaveAs(path);

                    // Lưu đường dẫn của ảnh vào trường img của đối tượng post
                    post.Img = "~/Images/" + fileName;
                }

                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }
        public ActionResult Xoa(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin/Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Admin/Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Post,Name,Description,Img,Date,Id_Account")] Post post, HttpPostedFileBase imgFile)
        {
            if (ModelState.IsValid)
            {
                post.Date = DateTime.Now;
                if (imgFile != null && imgFile.ContentLength > 0)
                {

                    // Lưu tệp ảnh mới lên máy chủ
                    string fileName = Path.GetFileName(imgFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    imgFile.SaveAs(path);

                    // Cập nhật đường dẫn của ảnh trong đối tượng Course
                    post.Img = "~/Images/" + fileName;
                }

                // Kiểm tra xem bản ghi có tồn tại trong cơ sở dữ liệu không
                var existingPost = db.Posts.Find(post.Id_Post);
                if (existingPost == null)
                {
                    return HttpNotFound(); // Hoặc thực hiện một hành động phù hợp với ứng dụng của bạn
                }

                // Cập nhật thuộc tính của bản ghi hiện tại
                db.Entry(existingPost).CurrentValues.SetValues(post);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Admin/Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Admin/Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
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
