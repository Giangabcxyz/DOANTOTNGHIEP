using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebsiteChungKhoann.Models
{
    [Table("Author")]
    public class Author
    {
        [Key]
        public int Id_Author { get; set; }

        public string Name_Author { get; set; }

        public string Address_Author { get; set; }

        public int? Age_Author { get; set; }

        public virtual ICollection<Product> Product { get; set; }

    }
}