using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebsiteChungKhoann.Models
{
    [Table("Account")]
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        [StringLength(50)]
        public string Rolex { get; set; }
        public virtual ICollection<Cart> Cart { get; set; }
        public virtual ICollection<Comment_Product> Comment_Product { get; set; }
        public virtual ICollection<Post> Post { get; set; }
        public virtual ICollection<Comment_Post> Comment_Post { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Comment_Course> Comment_Course { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}