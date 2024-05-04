using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebsiteChungKhoann.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int Id_Category { get; set; }

        public string Name_Category { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}