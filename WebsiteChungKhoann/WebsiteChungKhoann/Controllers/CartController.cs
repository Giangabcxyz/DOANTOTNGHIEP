using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WebsiteChungKhoann.Models;

namespace WebsiteChungKhoann.Controllers
{
    public class CartController : Controller
    {
        private Mode1 db = new Mode1();
        // GET: Cart
        public PartialViewResult Bagcart()
        {
            int? totalItem = null; // Sử dụng kiểu dữ liệu nullable int?
            var Id_Account = (int?)Session["Id"];
            if (Id_Account != null)
            {
                // Lấy tổng số lượng sản phẩm trong giỏ hàng của người dùng hiện tại
                totalItem = db.Carts_pr.Where(c => c.Id_Account == Id_Account).Sum(c => (int?)c.Quantity) ?? 0;
            }
            else
            {
                totalItem = 0;
            }
            ViewBag.CartItemCount = totalItem;
            return PartialView("_CartItemCount");
        }

        public ActionResult Index()
        {
            var Id_Account = (int?)Session["Id"];
            if (Id_Account == null)
            {
                Session["Cart"] = 1;
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Session["Cart"] = null;
            }

            var cartItems = db.Carts_pr.Where(c => c.Id_Account == Id_Account).ToList();
            return View(cartItems);
        }

        private Cart GetCartItem(int productId)
        {
            var Id_Account = (int?)Session["Id"];
            if (Id_Account == null)
            {
                return null;
            }

            return db.Carts_pr.FirstOrDefault(item => item.Id_Product == productId && item.Id_Account == Id_Account);
        }

        private void AddOrUpdateCartItem(int productId, int quantity)
        {
            var existingItem = GetCartItem(productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                // Kiểm tra Session["Id"] trước khi sử dụng
                var Id_Account = Session["Id"] as int?;
                if (Id_Account != null)
                {
                    var newItem = new Cart
                    {
                        Id_Product = productId,
                        Quantity = quantity,
                        Id_Account = (int)Id_Account
                    };
                    db.Carts_pr.Add(newItem);
                }
                else
                {
                    // Chuyển hướng về trang đăng nhập nếu Id_Account là null
                    RedirectToAction("Login", "Account");
                    return; // Thêm return ở đây để kết thúc phương thức
                }
            }
            db.SaveChanges();
        }

       

        public int Quantity_Cart()
        {
            var list =  db.Carts_pr.ToList();
            var sum = 0;
            foreach(var item in list )
            {
                sum += item.Quantity;
            }    
            return sum;
            
        }
        public int Disc(int id)
        {
            var c = db.Carts_pr.Where(e => e.Id_Product ==  id).FirstOrDefault();
            c.Quantity++;
            db.SaveChanges();
            var sl = c.Quantity;
            return sl;
              
        }

        public int Desc(int id)
        {
            var c = db.Carts_pr.Where(e => e.Id_Product == id).FirstOrDefault();
            if(c.Quantity <=1)
            {
               c.Quantity=1;

            }
            else
            {
                c.Quantity--;
            }  
            db.SaveChanges();
            var sl = c.Quantity;
            return sl;
        }


        public ActionResult Cart(int id)
        {
            var pr = db.Products.Find(id);
            ViewBag.img = pr.Img;
            AddOrUpdateCartItem(id, 1);
            return RedirectToAction("Index");
        }

        public ActionResult Buy(int id)
        {
            AddOrUpdateCartItem(id, 1);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            // Trích xuất giá trị của Session["Id"]
            var Id_Account = (int)Session["Id"];
            // Sử dụng giá trị đã trích xuất trong truy vấn
            var cartItemToRemove = db.Carts_pr.FirstOrDefault(item => item.Id_Product == id && item.Id_Account == Id_Account);
            if (cartItemToRemove != null)
            {
                db.Carts_pr.Remove(cartItemToRemove);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult Checkout(int id)
        {
            return RedirectToAction("Order", "Order", new {id = id});
        }

      
    }
}