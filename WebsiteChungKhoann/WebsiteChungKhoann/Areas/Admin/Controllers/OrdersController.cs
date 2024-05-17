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
    public class OrdersController : Controller
    {
        private Mode1 db = new Mode1();

        // GET: Admin/Orders
        public ActionResult Index()
        {
            var orders_pr = db.Orders_pr.Include(o => o.Account).Include(o => o.Pay).Include(o => o.PayStatus).Include(o => o.Product).Include(o => o.Status);
            return View(orders_pr.ToList());
        }

        // GET: Admin/Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders_pr.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Admin/Orders/Create
        public ActionResult Create()
        {
            ViewBag.Id_Account = new SelectList(db.Accounts, "Id", "Name");
            ViewBag.Id_Pay = new SelectList(db.Pays, "Id_Pay", "Name");
            ViewBag.Id_PayStatus = new SelectList(db.PayStatus, "Id_PayStatus", "Pay_Status");
            ViewBag.Id_Product = new SelectList(db.Products, "Id_Product", "Name");
            ViewBag.Id_Status = new SelectList(db.Status, "Id_Status", "status");
            return View();
        }

        // POST: Admin/Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Order,Create_Date,Id_Account,Id_Product,Price,Quantity,Name_Pro,Name,Address,Phone,Id_Pay,Id_Status,Id_PayStatus,Total,code")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders_pr.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Account = new SelectList(db.Accounts, "Id", "Name", order.Id_Account);
            ViewBag.Id_Pay = new SelectList(db.Pays, "Id_Pay", "Name", order.Id_Pay);
            ViewBag.Id_PayStatus = new SelectList(db.PayStatus, "Id_PayStatus", "Pay_Status", order.Id_PayStatus);
            ViewBag.Id_Product = new SelectList(db.Products, "Id_Product", "Name", order.Id_Product);
            ViewBag.Id_Status = new SelectList(db.Status, "Id_Status", "status", order.Id_Status);
            return View(order);
        }

        // GET: Admin/Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders_pr.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Account = new SelectList(db.Accounts, "Id", "Name", order.Id_Account);
            ViewBag.Id_Pay = new SelectList(db.Pays, "Id_Pay", "Name", order.Id_Pay);
            ViewBag.Id_PayStatus = new SelectList(db.PayStatus, "Id_PayStatus", "Pay_Status", order.Id_PayStatus);
            ViewBag.Id_Product = new SelectList(db.Products, "Id_Product", "Name", order.Id_Product);
            ViewBag.Id_Status = new SelectList(db.Status, "Id_Status", "status", order.Id_Status);
            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Order,Create_Date,Id_Account,Id_Product,Price,Quantity,Name_Pro,Name,Address,Phone,Id_Pay,Id_Status,Id_PayStatus,Total,code")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Account = new SelectList(db.Accounts, "Id", "Name", order.Id_Account);
            ViewBag.Id_Pay = new SelectList(db.Pays, "Id_Pay", "Name", order.Id_Pay);
            ViewBag.Id_PayStatus = new SelectList(db.PayStatus, "Id_PayStatus", "Pay_Status", order.Id_PayStatus);
            ViewBag.Id_Product = new SelectList(db.Products, "Id_Product", "Name", order.Id_Product);
            ViewBag.Id_Status = new SelectList(db.Status, "Id_Status", "status", order.Id_Status);
            return View(order);
        }

        // GET: Admin/Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders_pr.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders_pr.Find(id);
            db.Orders_pr.Remove(order);
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
