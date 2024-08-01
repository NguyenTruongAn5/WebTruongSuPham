namespace WEB_TRUONG_SP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateuser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PasswordHash", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "PasswordHash");
        }
    }
}
