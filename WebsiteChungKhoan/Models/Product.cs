namespace WebsiteChungKhoan.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [Key]
        [StringLength(10)]
        public string Id_Product { get; set; }

        public string Name { get; set; }

        [StringLength(10)]
        public string Id_Category { get; set; }

        [StringLength(10)]
        public string Id_Author { get; set; }

        public int? Price { get; set; }

        public int? Quantity { get; set; }

        [StringLength(50)]
        public string Img { get; set; }

        public int? Id_Star { get; set; }

        public int? Year { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        public virtual Author Author { get; set; }

        public virtual Category Category { get; set; }

        public virtual Reciept Reciept { get; set; }

        public virtual Star Star { get; set; }
    }
}
