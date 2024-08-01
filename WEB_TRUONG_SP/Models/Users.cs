using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_TRUONG_SP.Models
{
    public class Users 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập vào họ và tên!")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập vào địa chỉ email!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập vào mật khẩu!")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Mật khẩu phải có hơn 8 kí tự, có 1 kí tự viết hoa, chứa kí tự số và 1 kí tự đặc biệt.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập vào xác nhận mật khẩu!")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Mật khẩu chưa khớp.")]
        [Display(Name = "Confirm Password")]

        public string ConfirmPassword { get; set; }

        public string PasswordHash { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsDelete { get; set; }
        public Users()
        {
            CreateDate = DateTime.Now;
            IsDelete = false;
        }
    }
}