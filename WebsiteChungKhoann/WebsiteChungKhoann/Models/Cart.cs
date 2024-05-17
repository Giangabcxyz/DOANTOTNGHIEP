using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebsiteChungKhoann.Models
{
    [Table("Cart")]
    public class Cart
    {
        [Key]
        public int Id_Cart { get; set; }
        public int Id_Product {  get; set; }
        public int Quantity { get; set; }
        public int Id_Account { get; set; }
        public virtual Product Product { get; set; } 
        public Cart ()
        {

        }

        public Cart (int id_Cart, int id_Product, int quantity, int id_Order, int price, int id_Account)
        {
            Id_Cart = id_Cart;
            Id_Product = id_Product;
            Quantity = quantity;
            Id_Account = id_Account;
        }
        

    }
}