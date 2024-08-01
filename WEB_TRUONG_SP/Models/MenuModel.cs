using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_TRUONG_SP.Models
{
    public class MenuModel 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        public string Name { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsDelete { get; set; }
        public MenuModel()
        {
            CreateDate = DateTime.Now;
            IsDelete = false;
        }
    }
}