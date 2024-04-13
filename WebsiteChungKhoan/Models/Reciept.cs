namespace WebsiteChungKhoan.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Reciept")]
    public partial class Reciept
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reciept()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_Reciept { get; set; }

        public int? Id_Account { get; set; }

        public int? Id_Product { get; set; }

        public int? Quantity { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public int? Total { get; set; }

        public virtual History History { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
