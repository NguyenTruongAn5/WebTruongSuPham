using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_TRUONG_SP.Models
{
    public class News 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Tóm tắt không được để trống")]
        public string Summary { get; set; }

        [Required(ErrorMessage = "Nội dung không được để trống")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Ngày đăng không được để trống")]
        public DateTime PublishDate { get; set; }

        public DateTime CreateDate { get; set; }

        [Required(ErrorMessage = "Đanh mục không được để trống")]
        public string Category { get; set; }

        public int IDMenu { get; set; }

        public int IDSubMenu { get; set; }

        public string ImagePath { get; set; }

        public bool IsDelete { get; set; }
        public News()
        {
            CreateDate = DateTime.Now;
            IsDelete = false;
        }
    }
}