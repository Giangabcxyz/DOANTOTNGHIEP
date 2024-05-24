using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteChungKhoann.Models;

namespace WebsiteChungKhoann.Controllers
{
    public class ListController : Controller
    {
        private Mode1 db = new Mode1();
        // GET: List
        public ActionResult Index(int? page, int? categoryId, string priceRange)
        {
            int pageSize = 6; // Số sản phẩm trên mỗi trang
            int pageNumber = (page ?? 1);

            var productsQuery = db.Products.AsQueryable();

            if (categoryId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Id_Category == categoryId.Value);
            }

            if (!string.IsNullOrEmpty(priceRange))
            {
                var priceParts = priceRange.Split('-');
                decimal minPrice = decimal.Parse(priceParts[0]);
                decimal maxPrice = priceParts.Length > 1 && !string.IsNullOrEmpty(priceParts[1]) ? decimal.Parse(priceParts[1]) : decimal.MaxValue;

                productsQuery = productsQuery.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
            }

            var products = productsQuery
                .OrderBy(p => p.Id_Product)
                .ToPagedList(pageNumber, pageSize);

            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;

            return View(products);
        }

        public ActionResult Search(string query, int? page)
        {
            int pageSize = 6; // Số sản phẩm trên mỗi trang
            int pageNumber = (page ?? 1);

            var books = db.Products
                .Where(p => p.Name.Contains(query))
                .OrderBy(p => p.Name)
                .ToPagedList(pageNumber, pageSize);

            ViewBag.Query = query;
            var categories = db.Categories.ToList();
            ViewBag.Categories = categories; // Đảm bảo rằng danh sách danh mục vẫn được truyền đến view

            return View("Index", books); // Sử dụng lại view Index để hiển thị kết quả tìm kiếm
        }


        //public ActionResult Category()
        //{
        //    var list = db.Categories.ToList();

        //    return PartialView(list);
        //}
        //public ActionResult GetProductsByCategory(int? categoryId, int? page)
        //{
        //    int pageSize = 10; // Số sản phẩm trên mỗi trang
        //    int pageNumber = (page ?? 1);

        //    IQueryable<Product> productsQuery = db.Products;

        //    if (categoryId != null)
        //    {
        //        productsQuery = productsQuery.Where(p => p.Id_Category == categoryId);
        //    }

        //    var products = productsQuery.OrderBy(p => p.Id_Product).ToPagedList(pageNumber, pageSize);

        //    return PartialView("GetProductsByCategory", products);
        //}
        //public ActionResult GetProductsByCategory(int? categoryId, int page = 1, int pageSize = 8)
        //{
        //    var products = db.Products
        //                    .Where(p => !categoryId.HasValue || p.Id_Category == categoryId.Value)
        //                    .OrderBy(p => p.Name)
        //                    .ToPagedList(page, pageSize);
        //    return PartialView("GetProductsByCategory",products);
        //}


    }
}