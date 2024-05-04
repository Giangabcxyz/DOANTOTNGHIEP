using System;
using System.Collections.Generic;
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

        public ActionResult Index()
        {
            var Id_Account = (int?)Session["Id"];
            if (Id_Account == null)
            {
                return RedirectToAction("Login", "Account");
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



        public ActionResult Disc(int id)
        {
            AddOrUpdateCartItem(id, +1);
            return RedirectToAction("Index");
        }

        public ActionResult Desc(int id)
        {
            var Id_Account = (int)Session["Id"];
            var cartItem = db.Carts_pr.FirstOrDefault(item => item.Id_Product == id && item.Id_Account == Id_Account);
            if (cartItem != null)
            {
                // Kiểm tra xem số lượng có lớn hơn 1 không
                if (cartItem.Quantity > 1)
                {
                    // Giảm số lượng đi 1
                    cartItem.Quantity -= 1;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }


        public ActionResult Cart(int id)
        {
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
        public ActionResult Checkout(List<int> selectedItems)
        {
            List<int> selectedProductIds = new List<int>();
            foreach (var itemId in selectedItems)
            {
                // Tìm thông tin của mục được chọn từ cơ sở dữ liệu
                var cartItem = db.Carts_pr.FirstOrDefault(item => item.Id_Cart == itemId);
                if (cartItem != null)
                {
                    // Lấy thông tin sản phẩm từ cơ sở dữ liệu
                    var product = db.Products.FirstOrDefault(p => p.Id_Product == cartItem.Id_Product);
                    if (product != null)
                    {
                        selectedProductIds.Add(product.Id_Product);
                        // Gán thông tin sản phẩm vào ViewBag
                        TempData["AccountId"] = cartItem.Id_Account;
                        TempData["ProductImg"] = product.Img;
                        TempData["ProductName"] = product.Name;
                        TempData["ProductPrice"] = product.Price;
                        TempData["ProductQuantity"] = cartItem.Quantity;
                    }

                    // Xóa mục khỏi giỏ hàng sau khi đã lấy thông tin sản phẩm
                    db.Carts_pr.Remove(cartItem);
                    db.SaveChanges();
                }
            }
            TempData["SelectedProductIds"] = selectedProductIds;

            // Chuyển hướng đến trang Order hoặc trang cần thiết
            return RedirectToAction("Order", "Order");
        }



    }
}