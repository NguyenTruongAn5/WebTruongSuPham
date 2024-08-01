namespace WEB_TRUONG_SP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateLecturer : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Lecturers", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Lecturers", "ImagePath", c => c.String(nullable: false));
        }
    }
}
