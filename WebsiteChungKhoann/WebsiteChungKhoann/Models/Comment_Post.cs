using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebsiteChungKhoann.Models
{
    [Table("Comment_Post")]
    public class Comment_Post
    {
        [Key]
        public int Id_Comment { get; set; }

        public int? Id_Account { get; set; }

        public string Comment { get; set; }

        public int? Id_Post { get; set; }

        public virtual Post Post { get; set; }

        public virtual Account Account { get; set; }
    }
}