namespace WEB_TRUONG_SP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatemodeluser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ConfirmPassword", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ConfirmPassword");
        }
    }
}
