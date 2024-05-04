using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebsiteChungKhoann.Models
{
    [Table("Status")]
    public class Status
    {
        [Key]
         public int Id_Status { get; set; }
         public string status {  get; set; }
         public virtual ICollection<Order> Order { get; set; }
    }
}