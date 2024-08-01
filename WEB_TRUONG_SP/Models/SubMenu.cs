using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.UI.WebControls;
using System;

namespace WEB_TRUONG_SP.Models
{
    public class SubMenu 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        public string Name { get; set; }

        public DateTime CreateDate { get; set; }

        public int MenuID { get; set; }

        public bool IsDelete { get; set; }

        [ForeignKey("MenuID")]
        public virtual MenuModel Menu { get; set; }
        public SubMenu()
        {
            CreateDate = DateTime.Now;
            IsDelete = false;
        }
    }
}