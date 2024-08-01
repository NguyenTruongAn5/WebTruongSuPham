using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEB_TRUONG_SP.Models
{
    public class Banner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required(ErrorMessage ="Chưa nhập vào tên banner!")]
        public string Name { get; set; }

        public string ImgPath { get; set; }

        public DateTime CreateDate { get; set; }
        public bool IsDelete { get; set; }

        public Banner()
        {
            CreateDate = DateTime.Now;
            IsDelete= false;
        }
    }
}