namespace WEB_TRUONG_SP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatenews : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "ImagePath");
        }
    }
}
