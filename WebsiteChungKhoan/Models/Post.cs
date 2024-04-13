namespace WebsiteChungKhoan.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Post")]
    public partial class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_Post { get; set; }

        public string Name { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        [StringLength(50)]
        public string Img { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        public int? Id_Account { get; set; }

        public virtual Comment Comment { get; set; }
    }
}
