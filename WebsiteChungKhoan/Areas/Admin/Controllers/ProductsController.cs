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
using WebsiteChungKhoan.Models;

namespace WebsiteChungKhoan.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Admin/Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Author).Include(p => p.Category).Include(p => p.Reciept).Include(p => p.Star);
            return View(products.ToList());
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(string id)
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
            ViewBag.Id_Author = new SelectList(db.Authors, "Id_Author", "Name");
            ViewBag.Id_Category = new SelectList(db.Categories, "Id_Category", "Name");
            ViewBag.Year = new SelectList(db.Reciepts, "Id_Reciept", "Status");
            ViewBag.Id_Star = new SelectList(db.Stars, "Id_Star", "Id_Star");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Product,Name,Id_Category,Id_Author,Price,Quantity,Id_Star,Year,Description")] Product product, HttpPostedFileBase imgFile)
        {
            if (ModelState.IsValid)
            {
                if (imgFile != null && imgFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(imgFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    imgFile.SaveAs(path);
                    product.Img = "~/Images/" + fileName;
                }

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Author = new SelectList(db.Authors, "Id_Author", "Name", product.Id_Author);
            ViewBag.Id_Category = new SelectList(db.Categories, "Id_Category", "Name", product.Id_Category);
            ViewBag.Id_Star = new SelectList(db.Stars, "Id_Star", "Id_Star", product.Id_Star);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(string id)
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
            ViewBag.Id_Author = new SelectList(db.Authors, "Id_Author", "Name", product.Id_Author);
            ViewBag.Id_Category = new SelectList(db.Categories, "Id_Category", "Name", product.Id_Category);
            ViewBag.Year = new SelectList(db.Reciepts, "Id_Reciept", "Status", product.Year);
            ViewBag.Id_Star = new SelectList(db.Stars, "Id_Star", "Id_Star", product.Id_Star);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Product,Name,Id_Category,Id_Author,Price,Quantity,Img,Id_Star,Year,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Author = new SelectList(db.Authors, "Id_Author", "Name", product.Id_Author);
            ViewBag.Id_Category = new SelectList(db.Categories, "Id_Category", "Name", product.Id_Category);
            ViewBag.Year = new SelectList(db.Reciepts, "Id_Reciept", "Status", product.Year);
            ViewBag.Id_Star = new SelectList(db.Stars, "Id_Star", "Id_Star", product.Id_Star);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(string id)
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
        public ActionResult DeleteConfirmed(string id)
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
