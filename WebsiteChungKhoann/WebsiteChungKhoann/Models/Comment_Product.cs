using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebsiteChungKhoann.Models
{
    [Table("Comment_Product")]
    public class Comment_Product
    {
        [Key]
        public int Id_Comment { get; set; }
        public int? Id_Product { get; set; }
        public string Comment { get; set; }
        public int? Id_Account { get; set; }
        public virtual Product Product { get; set; }
        public virtual Account Account { get; set; }
    }
}