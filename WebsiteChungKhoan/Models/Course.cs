namespace WebsiteChungKhoan.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Course")]
    public partial class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_Course { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Link_Id { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public int? Id_Account { get; set; }

        public virtual Comment Comment { get; set; }
    }
}
