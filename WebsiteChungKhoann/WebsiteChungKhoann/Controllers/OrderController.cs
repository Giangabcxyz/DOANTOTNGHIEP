using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Web.UI.WebControls;
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
        public ActionResult CheckOut()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult VnPay_Return()
        {
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                string orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                       
                        //Thanh toan thanh cong
                        ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                    }
                    else
                    {
                        Order order = db.Orders_pr.Find(int.Parse(Session["OrderId"].ToString()));
                        order.Id_PayStatus = 2;
                        db.SaveChanges();

                        return RedirectToAction("Error", "Order");

                        ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;

                    }

                    ViewBag.ThanhToanThanhCong = "Số tiền thanh toán (VND):" + vnp_Amount.ToString();

                }

            }
            return View();
        }


        public ActionResult Status()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Order(FormCollection form)
        {
            
            int TypePayMent = int.Parse(form["paymentmethod"]);

            Order order = new Order();
            order.Create_Date = DateTime.Now;
            order.Id_Product = int.Parse(form["id_pr"]); 
            order.Id_Account = int.Parse(form["id"]);
            order.Price = int.Parse(form["price"]); 
            order.Quantity = int.Parse(form["quantity"]);
            order.Name_Pro = form["name_pr"];
            order.Name = form["fullname"];
            order.Address = form["address"];
            order.Phone = form["phonenumber"];
            order.Id_Pay= int.Parse(form["paymentmethod"]);
            order.Id_Status= int.Parse(form["status"]);
            order.Id_PayStatus = int.Parse(form["status"]);
            order.Total= int.Parse(form["total"]);
            Random rd = new Random();
            order.code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
            db.Orders_pr.Add(order);

            Product product = new Product();
            product = db.Products.Find(int.Parse(form["id_pr"]));
            if(product != null)
            {
                int quantity;
                if (int.TryParse(form["quantity"], out quantity))
                {
                    if(product.Count <=0)
                    {
                        product.Count = 0;
                    }    
                    product.Count -= quantity;
                    db.SaveChanges();
                }
                else
                {
                    // Xử lý khi không thể chuyển đổi 'quantity' sang kiểu số nguyên
                }
            }    

            db.SaveChanges();
            //carts.Clear();
            string url = UrlPayment(order.code);
            
            if (TypePayMent == 1)
            {
                Session["OrderId"]= order.Id_Order;
                return Redirect(url);
            }
            else
            {
                order.Id_PayStatus = 2;
                db.SaveChanges();
                return RedirectToAction("Status", "Order");
            }


        }



        public ActionResult Order(int id)
        {
            var cart = db.Carts_pr.Find(id);
            if (cart != null)
            {
                ViewBag.IdCart = cart.Id_Cart;
                ViewBag.ProductId = cart.Id_Product;
                ViewBag.ProductName = cart.Product.Name;
                ViewBag.ProductImg = cart.Product.Img;
                ViewBag.ProductPrice = cart.Product.Price;
                ViewBag.Total = (int)(cart.Quantity * cart.Product.Price);
                ViewBag.ProductQuantity = cart.Quantity;
                ViewBag.AccountId = cart.Id_Account;
                ViewBag.count = cart.Product.Count;
                db.Carts_pr.Remove(cart);
                db.SaveChanges();

            }

            return View();
        }
        public string UrlPayment(string ordercode)
        {
            //Get Config Info
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

            //Get payment input
            //ListCart carts = Session["Cart"] as ListCart;
            //double amount = carts.Total_Money();
            //var price = (long)amount * 100;

            //Save order to db
            Order item = db.Orders_pr.FirstOrDefault(s => s.code == ordercode);
            var price = (long)item.Total * 100;
            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (price).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            //if (TypePaymentVN ==1)
            //{
            //    vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            //}
            //else if (TypePaymentVN == 2)
            //{
            //    vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            //}
            //else if (TypePaymentVN == 3)
            //{
            //    vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            //}

            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang Fashion");
            vnpay.AddRequestData("vnp_OrderType", "VNPAY"); //default value: other
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", ordercode.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

            return paymentUrl;
        }


        public ActionResult User_Order()
        {
                int id = int.Parse(Session["Id"].ToString());
            //List<Order> orders = new List<Order>();
            //orders = db.Orders_pr.Where(e=>e.Id_Account == id && e.Id_Status == 3).ToList();
            //List<Order> Pendding = new List<Order>();
            //  Pendding = db.Orders_pr.Where(e => e.Id_Account == id && e.Id_Status == 1).ToList();
            //List<Order>danger = new List<Order>();
            //  danger = db.Orders_pr.Where(e=> e.Id_Order == id && e.Id_Status== 4).ToList();

            // List<Order> Wanrring = new List<Order>();
            // Wanrring = db.Orders_pr.Where(e => e.Id_Order == id && e.Id_Status == 2).ToList();
            List<Order> orders = db.Orders_pr.Where(e => e.Id_Account == id && e.Id_Status == 3).ToList();
            List<Order> Pendding = db.Orders_pr.Where(e => e.Id_Account == id && e.Id_Status == 1).ToList();
            List<Order> danger = db.Orders_pr.Where(e => e.Id_Account == id && e.Id_Status == 4).ToList();
            List<Order> Wanrring = db.Orders_pr.Where(e => e.Id_Account == id && e.Id_Status == 2).ToList();

            Order k = new Order();
            if(orders != null)
            {
               ViewBag.status = "Có";
            }
            else
            {
                ViewBag.status = "không";
            }
            var Account = db.Accounts.Find(id);
            ViewBag.OrderList = orders;
            ViewBag.pending = Pendding;
            ViewBag.danger = danger;
            ViewBag.Wanrring = Wanrring;
            ViewBag.Name = Account.Name;
            ViewBag.Address = Account.Address;
            ViewBag.Email = Account.Email;

              return View();
        }
    }
}

