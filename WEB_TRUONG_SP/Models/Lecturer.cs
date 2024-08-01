using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_TRUONG_SP.Models
{
    public class Lecturer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Chức vụ không được để trống")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Bằng cấp không được để trống")]
        public string Degree { get; set; }

        public string Information { get; set; }

        public string ImagePath { get; set; }

        public DateTime CreateDate { get; set; }

        public string Field { get; set; }

        [Required(ErrorMessage = "Chức vụ hội đồng không được để trống nếu không có thì để không")]
        public string CouncilPosition { get; set; }

        public bool IsDelete { get; set; }

        public Lecturer()
        {
            CreateDate = DateTime.Now;
            IsDelete = false;
        }
    }
}