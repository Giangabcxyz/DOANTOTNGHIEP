using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebsiteChungKhoann.Models
{
    [Table("Post")]
    public partial class Post
    {
        [Key]
        public int Id_Post { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Img { get; set; }

        public DateTime? Date { get; set; }

        public int? Id_Account { get; set; }

        public virtual Account Account { get; set; }

        public virtual ICollection<Comment_Post> Comment_Post { get; set; }
    }
    
}