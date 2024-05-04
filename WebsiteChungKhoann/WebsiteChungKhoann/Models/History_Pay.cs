using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebsiteChungKhoann.Models
{
    [Table("History_Pay")]
    public class History_Pay
    {
        [Key]
        public int Id_history { get; set; }
        public int Id_Order { get; set; }
        public DateTime date { get; set; }
        public int total { get; set; }
        public virtual ICollection<Order> order { get; set;}
    }
}