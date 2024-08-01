using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_TRUONG_SP.Models
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        public string Name { get; set; }

        public string Type { get; set; }  

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public bool IsDelete { get; set; }

        public Department()
        {
            CreateDate = DateTime.Now;  
            IsDelete = false;  
        }
    }
}
