using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebsiteChungKhoann.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int Id_Product { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public int? Id_Author { get; set; }

        public int? Id_Category { get; set; }

        public string Img { get; set; }

        public string Description { get; set; }

        public int Count { get; set; }

        public int? Year { get; set; }
        public virtual Category Category { get; set; }
        public virtual Author Author { get; set; }

        public virtual ICollection<Comment_Product> Comment_Product { get; set; }

        public virtual ICollection<Cart> Cart { get; set; } 

        public virtual ICollection<Order> Order { get; set; }
    }
}