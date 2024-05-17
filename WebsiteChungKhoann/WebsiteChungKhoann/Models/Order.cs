using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebsiteChungKhoann.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int Id_Order { get; set; }
        public DateTime Create_Date { get; set; }
        public int Id_Account { get; set; }
        public int Id_Product { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string Name_Pro { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int Id_Pay {  get; set; }
        public int Id_Status { get; set; }
        public int Id_PayStatus{ get; set; }
        public int Total { get; set; }
        public string code { get; set; }
        public virtual Status Status { get; set; }
        public virtual Pay Pay { get; set; }
        public virtual ICollection<Finance> Finance { get; set; }
        public virtual PayStatus PayStatus { get; set; }
        public virtual Account Account { get; set; }
        public virtual Product Product { get; set; }

        public Order()
        {
            // Constructor mặc định không có tham số
        }

        // Constructor với tham số, bạn có thể thêm các tham số tương ứng với các thuộc tính của lớp
        public Order(DateTime createDate, int accountId, int productId, int price, int quantity, string namePro, string name, string address, string phone, int idPay, int idStatus, int idPayStatus, int total, string conceptPay)
        {
            Create_Date = createDate;
            Id_Account = accountId;
            Id_Product = productId;
            Price = price;
            Quantity = quantity;
            Name_Pro = namePro;
            Name = name;
            Address = address;
            Phone = phone;
            Id_Pay = idPay;
            Id_Status = idStatus;
            Id_PayStatus = idPayStatus;
            Total = total;
            code = conceptPay;
        }
        //public virtual Detail_Order Detail { get; set; }
        //public virtual History_Pay History_Pay { get; set; }



    }
   
}