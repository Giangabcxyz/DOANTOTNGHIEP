using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebsiteChungKhoann.Models
{
    [Table("Finance")]
    public class Finance
    {
        [Key]
        public int Id_Fa { get; set; }
        public int Id_Order { get; set; }
        public DateTime date { get; set; }
        public int Id_pr { get; set; }
        public int Total {  get; set; }
        public virtual Order Order { get; set; }


    }
}