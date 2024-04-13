namespace WebsiteChungKhoan.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Comment")]
    public partial class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_Comment { get; set; }

        public int? Id_Account { get; set; }

        public int? Id_Course { get; set; }

        public int? Id_Post { get; set; }

        [Column("Comment", TypeName = "text")]
        public string Comment1 { get; set; }

        public virtual Course Course { get; set; }

        public virtual Post Post { get; set; }
    }
}
