using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebsiteChungKhoann.Models
{
    [Table("Detail_Order")]
    public class Detail_Order
    {
        [Key]

        public int Id_detail { get; set; }

        public int Id_Order {  get; set; }

        public int Price { get; set; }
        public string Name_Pro { get; set; }
        public string Address { get; set; }
        public int Total { get; set; }
        public virtual ICollection<Order> Order { get; set;}
    }
}