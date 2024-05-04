using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebsiteChungKhoann.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int Id_Order { get; set; }
        public string Create_Date { get; set; }
        public int Id_Account { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string Name_Pro { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int Id_Pay {  get; set; }
        public int Id_Status { get; set; }
        public int Total { get; set; }
        
        public string Concepct_pay { get; set; }
        public virtual Status Status { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }
        public virtual Pay Pay { get; set; }
        //public virtual Detail_Order Detail { get; set; }
        //public virtual History_Pay History_Pay { get; set; }

    }
}