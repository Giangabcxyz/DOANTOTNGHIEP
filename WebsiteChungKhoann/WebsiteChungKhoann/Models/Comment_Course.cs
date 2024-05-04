using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebsiteChungKhoann.Models
{
    [Table("Comment_Course")]
    public class Comment_Course
    {
        [Key]
        public int Id_Comment { get; set; }

        public int? Id_Course { get; set; }

        public int? Id_Account { get; set; }

        public string Comment { get; set; }

        public virtual Account Account { get; set; }
        public virtual Course Course { get; set; }

    }
}