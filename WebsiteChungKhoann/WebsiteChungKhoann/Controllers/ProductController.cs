using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteChungKhoann.Models;

namespace WebsiteChungKhoann.Controllers
{
    public class ProductController : Controller
    {
        private Mode1 db = new Mode1();
        // GET: Product
        public ActionResult Index()
        {
            var list =  db.Products.ToList();
            Session["hanghoa"] = list;
            return View(list);
        }
        public ActionResult Details(int? id)
        {
            if (Session["Id"] == null)
            {
                Session["Pro"] = id;
                ViewBag.id = true;
            }
            else
            {
                Session["Pro"] = null;
                ViewBag.id = false;
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var productt = db.Products.Find(id);

            var categoryName = (from p in db.Products
                                join c in db.Categories on p.Id_Category equals c.Id_Category
                                where p.Id_Category == productt.Id_Category
                                select c.Name_Category).FirstOrDefault();

            // Lấy danh sách sản phẩm có trong danh mục categoryName nếu có, nếu không trả về danh sách sản phẩm loại bỏ sản phẩm hiện tại
            // Truy vấn các sản phẩm từ cơ sở dữ liệu
            var relatedProducts = (from p in db.Products
                                   join c in db.Categories on p.Id_Category equals c.Id_Category
                                   where (c.Name_Category == categoryName && p.Id_Product != id)
                                          || (c.Name_Category != categoryName && p.Id_Product != id)
                                   select new { p, c }).ToList();

            // Tạo danh sách sản phẩm để gán vào ViewBag
            List<Product> productsToShow = new List<Product>();

            foreach (var item in relatedProducts)
            {
                if (item.c.Name_Category == categoryName)
                {
                    // Nếu sản phẩm thuộc vào danh mục categoryName, thêm vào danh sách sản phẩm
                    productsToShow.Add(item.p);
                }
                else if(item.c.Name_Category != categoryName)
                {
                    // Nếu sản phẩm không thuộc vào danh mục categoryName, không thêm vào danh sách sản phẩm
                    // Điều này tương đương với không làm gì cả
                    //productsToShow.Add(item.p);
                }
            }
            var list = db.Products.ToList();
            ViewBag.id = id;
            // Lấy danh sách các comment từ csdl dựa trên id của video
            var comments = (from comment in db.Comment_Product
                            join account in db.Accounts on comment.Id_Account equals account.Id// Điều kiện để lấy các bình luận của bài đăng hiện tại
                            select new CommentViewModel
                            {
                                Comment = comment.Comment,
                                AccountName = account.Name, // Lấy tên từ bảng Account
                                Id_Product = (int)comment.Id_Product                           // Các thuộc tính khác của bình luận nếu cần
                            }).ToList();
            ViewBag.Comment = comments;
            // Gán danh sách sản phẩm vào ViewBag để sử dụng trong view
            ViewBag.RelatedProducts = productsToShow;
            ViewBag.count = productsToShow.Count();


            // Thêm danh sách sản phẩm vào ViewBag hoặc Model để sử dụng trong view

            return View(product);
        }
        public ActionResult Category()
        {
            var categories = db.Categories.ToList(); // Chú ý đổi từ db.Products sang db.Categories
            return PartialView("_Category", categories);
        }

        [HttpPost]
        public ActionResult Create(string Comment, int? Id_Account, string Name, int Id_Product)
        {

            if (Id_Account == null)
            {
                // Redirect hoặc hiển thị thông báo lỗi khi Id_Account là null
                return RedirectToAction("Login", "Account"); // Chuyển hướng đến trang đăng nhập
            }

            Comment_Product newComment = new Comment_Product
            {
                Comment = Comment,
                Id_Account = Id_Account,
                Id_Product = Id_Product
            };

            // Thêm mới đối tượng Comment vào cơ sở dữ liệu
            db.Comment_Product.Add(newComment);
            db.SaveChanges();

            // Trả về Action Index trong Controller khác có tên là HomeController
            return RedirectToAction("Details", "Product", new { id = Id_Product });




        }
        public ActionResult Cart(Product product)
        {
            return View(product);
        }
      
            // Các action khác ở đây

            public ActionResult _Category(IEnumerable<WebsiteChungKhoann.Models.Product> model)
            {
                // Xử lý logic ở đây
                return PartialView("_Category", model);
            }
        public class CommentViewModel
        {
            public string Comment { get; set; }
            public string AccountName { get; set; }
            public int Id_Product { get; set; }
        }


    }
}