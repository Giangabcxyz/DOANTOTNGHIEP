using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteChungKhoan.Models;

namespace WebsiteChungKhoan.Areas.Admin.Controllers
{
    public class AccountsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Admin/Accounts
        public ActionResult Index()
        {
            return View(db.Accounts.ToList());
        }

        // GET: Admin/Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Admin/Accounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,Address,Password,Rolex")] Account account)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Accounts.Add(account);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException ex)
            {
                // Xử lý lỗi DbUpdateException (lỗi khi lưu dữ liệu vào cơ sở dữ liệu)
                ViewBag.error = "Lỗi khi lưu dữ liệu vào cơ sở dữ liệu: " + ex.Message;
            }
            catch (Exception ex)
            {
                // Xử lý các loại ngoại lệ khác
                ViewBag.error = "Có lỗi xảy ra: " + ex.Message;
            }

            return View(account);


        }

        // GET: Admin/Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Admin/Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,Address,Password,Rolex")] Account account)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(account).State = EntityState.Modified;
                    db.SaveChanges();
                }
                    return RedirectToAction("Index", "Accounts", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                ViewBag.error = "Lỗi Sửa dữ liệu " + ex.Message;
               return View(account);
            }
            
        }

        // GET: Admin/Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Admin/Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
                Account account = db.Accounts.Find(id);
            try
            {
                db.Accounts.Remove(account);
                db.SaveChanges();
                return RedirectToAction("Index", "Accounts", new { area = "Admin" });
            }
            catch(Exception ex)
            {
                ViewBag.error = "Không xóa đưuọc dữ liệu " + ex.Message;
                return View(account);
            }
            
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
