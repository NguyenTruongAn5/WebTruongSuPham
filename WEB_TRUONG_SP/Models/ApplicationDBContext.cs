using System;
using System.Data.Entity;

namespace WEB_TRUONG_SP.Models
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext() : base("name = DefaultConnection")
        {
            if (string.IsNullOrEmpty(this.Database.Connection.ConnectionString))
            {
                throw new InvalidOperationException("Connection string is null or empty.");
            }
        }
        public DbSet<MenuModel> Menus { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Lecturer> Lecturer { get; set; }
        public DbSet<SubMenu> SubMenu { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Logs> Logs { get; set; }
        public DbSet<Users> Users { get; set; }

        public DbSet<Banner> Banner { get; set; }
    }
}