using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebsiteChungKhoann.Models
{
    [Table("Pay")]
    public class Pay
    {
        [Key]
        public int Id_Pay { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Order> Order { get; set; }
    }
}