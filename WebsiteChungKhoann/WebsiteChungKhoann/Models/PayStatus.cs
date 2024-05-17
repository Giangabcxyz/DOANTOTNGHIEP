using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebsiteChungKhoann.Models
{
    [Table("PayStatus")]
    public class PayStatus
    {
        [Key]
        public int Id_PayStatus { get; set; }

        public string Pay_Status { get; set; }

        public virtual ICollection<Order> Order { get; set; }
    }
}