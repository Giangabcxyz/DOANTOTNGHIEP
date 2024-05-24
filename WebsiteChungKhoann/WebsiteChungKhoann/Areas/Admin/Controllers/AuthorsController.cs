using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteChungKhoann.Models;

namespace WebsiteChungKhoann.Areas.Admin.Controllers
{
    public class AuthorsController : Controller
    {
        private Mode1 db = new Mode1();

        // GET: Admin/Authors
        public ActionResult Index(string sortOrder, int? page)
        {
            // Define a default sort order if none is specified
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.AddressSortParm = sortOrder == "Address" ? "address_desc" : "Address";
            ViewBag.AgeSortParm = sortOrder == "Age" ? "age_desc" : "Age";

            // Query all authors
            var authors = from a in db.Authors
                          select a;

            // Apply sorting based on the sortOrder parameter
            switch (sortOrder)
            {
                case "name_desc":
                    authors = authors.OrderByDescending(a => a.Name_Author);
                    break;
                case "Address":
                    authors = authors.OrderBy(a => a.Address_Author);
                    break;
                case "address_desc":
                    authors = authors.OrderByDescending(a => a.Address_Author);
                    break;
                case "Age":
                    authors = authors.OrderBy(a => a.Age_Author);
                    break;
                case "age_desc":
                    authors = authors.OrderByDescending(a => a.Age_Author);
                    break;
                default:
                    authors = authors.OrderBy(a => a.Name_Author);
                    break;
            }

            // Set page size (number of items per page)
            int pageSize = 7;
            int pageNumber = (page ?? 1);

            // Return the paginated list to the view
            return View(authors.ToPagedList(pageNumber, pageSize));
        }

        // Other actions
    

    // GET: Admin/Authors/Details/5
    public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // GET: Admin/Authors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Author,Name_Author,Address_Author,Age_Author")] Author author)
        {
            if (ModelState.IsValid)
            {
                db.Authors.Add(author);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(author);
        }

        // GET: Admin/Authors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }
        public ActionResult Xoa(int id)
        {
            Author author = db.Authors.Find(id);
            db.Authors.Remove(author);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // POST: Admin/Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Author,Name_Author,Address_Author,Age_Author")] Author author)
        {
            if (ModelState.IsValid)
            {
                db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(author);
        }

        // GET: Admin/Authors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Admin/Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Author author = db.Authors.Find(id);
            db.Authors.Remove(author);
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
