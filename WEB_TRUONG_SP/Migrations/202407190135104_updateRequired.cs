namespace WEB_TRUONG_SP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Departments", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Lecturers", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Lecturers", "Position", c => c.String(nullable: false));
            AlterColumn("dbo.Lecturers", "Degree", c => c.String(nullable: false));
            AlterColumn("dbo.Lecturers", "ImagePath", c => c.String(nullable: false));
            AlterColumn("dbo.Lecturers", "CouncilPosition", c => c.String(nullable: false));
            AlterColumn("dbo.MenuModels", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.News", "Content", c => c.String(nullable: false));
            AlterColumn("dbo.News", "Category", c => c.String(nullable: false));
            AlterColumn("dbo.SubMenus", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SubMenus", "Name", c => c.String());
            AlterColumn("dbo.News", "Category", c => c.String());
            AlterColumn("dbo.News", "Content", c => c.String());
            AlterColumn("dbo.MenuModels", "Name", c => c.String());
            AlterColumn("dbo.Lecturers", "CouncilPosition", c => c.String());
            AlterColumn("dbo.Lecturers", "ImagePath", c => c.String());
            AlterColumn("dbo.Lecturers", "Degree", c => c.String());
            AlterColumn("dbo.Lecturers", "Position", c => c.String());
            AlterColumn("dbo.Lecturers", "Name", c => c.String());
            AlterColumn("dbo.Departments", "Name", c => c.String());
        }
    }
}
