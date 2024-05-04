using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using WebsiteChungKhoann.Models;

namespace WebsiteChungKhoann.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private Mode1 db = new Mode1();

        // GET: Admin/Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Author).Include(p => p.Category);
            return View(products.ToList());
        }

        // GET: Admin/Products/Details/5
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
            return View(product);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.Id_Author = new SelectList(db.Authors, "Id_Author", "Name_Author");
            ViewBag.Id_Category = new SelectList(db.Categories, "Id_Category", "Name_Category");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Product,Name,Quantity,Price,Id_Author,Id_Category,Img,Description,Year")] Product product, HttpPostedFileBase imgFile)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem Session["Id"] có tồn tại không
                if (imgFile != null && imgFile.ContentLength > 0)
                {
                    // Lưu tệp ảnh vào thư mục trên server
                    string fileName = Path.GetFileName(imgFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    imgFile.SaveAs(path);

                    // Lưu đường dẫn của ảnh vào trường img của đối tượng post
                    product.Img = "~/Images/" + fileName;
                }
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Author = new SelectList(db.Authors, "Id_Author", "Name_Author", product.Id_Author);
            ViewBag.Id_Category = new SelectList(db.Categories, "Id_Category", "Name_Category", product.Id_Category);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.Id_Author = new SelectList(db.Authors, "Id_Author", "Name_Author", product.Id_Author);
            ViewBag.Id_Category = new SelectList(db.Categories, "Id_Category", "Name_Category", product.Id_Category);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Product,Name,Quantity,Price,Id_Author,Id_Category,Img,Description,Year")] Product product, HttpPostedFileBase imgFile)
        {
            if (ModelState.IsValid)
            {
                if (imgFile != null && imgFile.ContentLength > 0)
                {
                    // Lưu tệp ảnh vào thư mục trên server
                    string fileName = Path.GetFileName(imgFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    imgFile.SaveAs(path);

                    // Lưu đường dẫn của ảnh vào trường img của đối tượng post
                    product.Img = "~/Images/" + fileName;
                }
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Author = new SelectList(db.Authors, "Id_Author", "Name_Author", product.Id_Author);
            ViewBag.Id_Category = new SelectList(db.Categories, "Id_Category", "Name_Category", product.Id_Category);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
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
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
