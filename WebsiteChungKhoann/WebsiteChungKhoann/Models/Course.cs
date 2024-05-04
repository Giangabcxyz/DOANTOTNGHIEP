using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace WebsiteChungKhoann.Models
{
    [Table("Course")]
    public class Course
    {
        [Key]
        public int Id_Course { get; set; }

        public string Name { get; set; }

        public string Link_Id { get; set; }

        public string Description { get; set; }

        public int? Id_Account { get; set; }

        public DateTime? Create_Date { get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<Comment_Course> Comment_Course {   get; set;  }
    }
}