using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Xml.Linq;
using WebsiteChungKhoann.Models;

namespace WebsiteChungKhoann.Controllers
{
    public class OrderController : Controller
    {
        private Mode1 db = new Mode1();
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Order()
        {
            ViewBag.AccountId = TempData["AccountId"];
            ViewBag.ProductImg = TempData["ProductImg"];
            ViewBag.ProductName = TempData["ProductName"];
            ViewBag.ProductPrice = TempData["ProductPrice"];
            ViewBag.ProductQuantity = TempData["ProductQuantity"];

            return View();
        }

        [HttpPost]
        public ActionResult Order(int quantity ,string fullname, string address, string phonenumber, int paymentmethod, string name_pr, string img, int price, int status, int total, string Concepct_pay, int id, string date)
        {
            @ViewBag.FullName = fullname;
            @ViewBag.Address = address;
            ViewBag.PhoneNumber = phonenumber;
            ViewBag.PaymentMethod = paymentmethod;
            ViewBag.NamePr = name_pr;
            ViewBag.Img = img;
            ViewBag.Price = price;
            ViewBag.Status = status;
            ViewBag.Total = total;
            ViewBag.ConcepctPay = Concepct_pay;
            ViewBag.Date = date;
            ViewBag.Id = id;


            Order order = new Order
                    {
                        Quantity = quantity,
                        Create_Date = date, // ngày hiện tại
                        Name = fullname,
                        Address = address,
                        Phone = phonenumber,
                        Id_Pay = paymentmethod,
                        Name_Pro = name_pr,
                        Id_Account = id,
                        Price = price,
                        Id_Status = status,
                        Total = total,
                        Concepct_pay = Concepct_pay
             
                    };
                    db.Orders_pr.Add(order);
                    db.SaveChanges();
               
                int orderId = order.Id_Order;
                int tt = order.Id_Status;
                var m = order.Concepct_pay;

            Detail_Order detail = new Detail_Order
                 {
                     Id_Order = orderId,
                     Price = price,
                     Name_Pro = name_pr,
                     Address = address,
                     Total = total
                };

            // Thêm đối tượng Detail vào dbcontext
            db.Details.Add(detail);
            
            // Lưu thay đổi vào cơ sở dữ liệu
            db.SaveChanges();
            // xử lý dữ liệu ở đây, ví dụ lưu vào cơ sở dữ liệu hoặc gửi email thông báo đăng ký thành công, v.v.
            // đặt dữ liệu vào viewbag để truyền vào view
            if (paymentmethod == 1)
                {
                    // thêm đối tượng order vào dbcontext và lưu thay đổi vào cơ sở dữ liệu
                    return RedirectToAction("PayOnline", new { orderId = orderId, tt = tt, m = m });

                }
                else
                {
                  
                    return RedirectToAction("Status", new { orderId = orderId, tt = tt, m = m });
                }

            }
            

        public ActionResult Error()
        {
            return View();
        }
        public ActionResult Status()
        {
            return View();
        }
        public ActionResult PayOnline(int orderId)
        {
            var orderStatus = (from o in db.Orders_pr
                               where o.Id_Order == orderId
                               select o.Id_Status).FirstOrDefault();

            // Lấy thuộc tính concept_pay từ bảng Order
            var orderConceptPay = (from o in db.Orders_pr
                                   where o.Id_Order == orderId
                                   select o.Concepct_pay).FirstOrDefault();



            ViewBag.Status = orderStatus;
            ViewBag.ConceptPay = orderConceptPay;

            return View();
            //var order = db.Orders_pr.FirstOrDefault(o => o.Id_Order == orderId);

            //if (order != null)
            //{
            //    var data = new
            //    {
            //        status = order.Id_Status,
            //        conceptPay = order.Concepct_pay
            //    };

            //    return Json(data, JsonRequestBehavior.AllowGet);
            //}

            //// Xử lý trường hợp không tìm thấy đơn hàng
            //return HttpNotFound();
        }
            public ActionResult Details() 
           {
            return View();
          }
       
    }
}