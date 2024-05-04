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

            // Gán danh sách sản phẩm vào ViewBag để sử dụng trong view
            ViewBag.RelatedProducts = productsToShow;
            ViewBag.count = productsToShow.Count();


            // Thêm danh sách sản phẩm vào ViewBag hoặc Model để sử dụng trong view

            return View(product);
        }
        
        public ActionResult Cart(Product product)
        {
            return View(product);
        }
    }
}