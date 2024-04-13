using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebsiteChungKhoan.Areas.Admin.Controllers;
using WebsiteChungKhoan.Models;

namespace WebsiteChungKhoan.Controllers
{
    public class BooksController : Controller
    {
        // GET: Books
        //Models.Model1 db = new Model1();
        private Model1 db = new Model1();
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.SapTheoTen = sortOrder == "name_desc" ? "" : "name_desc";
            ViewBag.SapTheoGia = sortOrder == "Gia" ? "gia_desc" : "Gia";
            var list = db.Products.Select(p => p);

            //Phân trang
            

            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(p=> p.Name.Contains(searchString));
            }    
            switch (sortOrder)
            {
                case "name_desc":
                    list = list.OrderByDescending(s => s.Name);
                    break;
                case "Gia":
                    list = list.OrderBy(s => s.Price);
                    break;
                case "gia_desc":
                    list = list.OrderByDescending(s => s.Price);
                    break;
                default:
                    list = list.OrderBy(s => s.Name);
                    break;
            }
            return View(list.ToList());
        }
        public ActionResult DisplaySuplies( int? page)
        {
            var list = db.Products.Select(p => p);
            list = list.OrderBy(p => p.Id_Product);
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Details(string id)
        {
            var product = db.Products.FirstOrDefault(p => p.Id_Product == id);

            // Kiểm tra xem sản phẩm có tồn tại không
            if (product == null)
            {
                // Nếu không tìm thấy sản phẩm, trả về trang lỗi hoặc một trang thông báo lỗi khác
                return HttpNotFound(); // hoặc trả về view 404 NotFound
            }

            // Nếu sản phẩm tồn tại, trả về view chi tiết của sản phẩm và truyền thông tin sản phẩm vào view
            return View(product);
        }
            
        

        public ActionResult Detail(string id) 
        {
            var product = db.Products.FirstOrDefault(p => p.Id_Product == id);

            // Kiểm tra xem sản phẩm có tồn tại không
            if (product == null)
            {
                // Nếu không tìm thấy sản phẩm, trả về trang lỗi hoặc một trang thông báo lỗi khác
                return HttpNotFound(); // hoặc trả về view 404 NotFound
            }

            // Nếu sản phẩm tồn tại, trả về view chi tiết của sản phẩm và truyền thông tin sản phẩm vào view
            return View(product);
        }
    }
}