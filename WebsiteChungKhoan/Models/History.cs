namespace WebsiteChungKhoan.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("History")]
    public partial class History
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_History { get; set; }

        public int? Id_Accout { get; set; }

        public int? Id_Reciept { get; set; }

        public virtual Account Account { get; set; }

        public virtual Reciept Reciept { get; set; }
    }
}
