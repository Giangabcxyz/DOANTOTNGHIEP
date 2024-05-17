using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteChungKhoann.Models;

namespace WebsiteChungKhoann.Areas.Admin.Controllers
{
    public class ChartController : Controller
    {
        // GET: Admin/Chart

        private Mode1 db = new Mode1();

        //public ActionResult Index()
        //{
        //    List<Order> orders = db.Orders_pr.Where(e => e.Id_Status == 3).ToList();
        //    List<Finance> finances = new List<Finance>();

        //    foreach (Order order in orders)
        //    {
        //        // Kiểm tra xem Id_Order đã tồn tại trong bảng Finance chưa
        //        bool exists = db.Finance.Any(f => f.Id_Order == order.Id_Order);

        //        // Nếu không tồn tại, thêm vào danh sách finances
        //        if (!exists)
        //        {
        //            Finance finance = new Finance
        //            {
        //                Id_Order = order.Id_Order,
        //                Total = order.Total,
        //                date = order.Create_Date
        //            };

        //            finances.Add(finance);
        //        }
        //    }

        //    // Thêm danh sách finances vào bảng Finance
        //    db.Finance.AddRange(finances);
        //    db.SaveChanges();

        //    var list = db.Finance.ToList();
        //    return PartialView(list);
        //}
        public ActionResult Index()
        {
            List<Order> orders = db.Orders_pr.Where(e => e.Id_Status == 3).ToList();
            List<Finance> finances = new List<Finance>();

            foreach (Order order in orders)
            {
                bool exists = db.Finance.Any(f => f.Id_Order == order.Id_Order);

                if (!exists)
                {
                    var pr = db.Products.Find(order.Id_Product);
                    int idCate = int.Parse(pr.Id_Category.ToString());
                    Finance finance = new Finance
                    {
                        Id_Order = order.Id_Order,
                        Total = order.Total,
                        date = order.Create_Date,
                        Id_pr = idCate // Giả định rằng Order có thuộc tính Id_ProductType
                    };

                    finances.Add(finance);
                }
            }

            db.Finance.AddRange(finances);
            db.SaveChanges();
            ViewBag.OrderAll = db.Orders_pr.Count();
            ViewBag.Success = db.Finance.Count();
            ViewBag.Fails = db.Orders_pr.Where(e => e.Id_Status != 3).Count();
            int total = db.Finance.Sum(o => (int?)o.Total) ?? 0;
            ViewBag.Total = total;
           
            var list = db.Finance
                        .Select(f => new
                        {
                            f.date,
                            f.Id_pr,
                            f.Total
                        })
                        .ToList();

            var jsonData = JsonConvert.SerializeObject(list);
            ViewBag.ChartData = jsonData;

            return View();
        }

    }
}