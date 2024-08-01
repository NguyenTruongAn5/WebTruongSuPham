namespace WEB_TRUONG_SP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SubMenus", "MenuID", "dbo.MenuModels");
            DropIndex("dbo.SubMenus", new[] { "MenuID" });
            DropPrimaryKey("dbo.MenuModels");
            AlterColumn("dbo.MenuModels", "ID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.SubMenus", "MenuID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.MenuModels", "ID");
            CreateIndex("dbo.SubMenus", "MenuID");
            AddForeignKey("dbo.SubMenus", "MenuID", "dbo.MenuModels", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubMenus", "MenuID", "dbo.MenuModels");
            DropIndex("dbo.SubMenus", new[] { "MenuID" });
            DropPrimaryKey("dbo.MenuModels");
            AlterColumn("dbo.SubMenus", "MenuID", c => c.String(maxLength: 128));
            AlterColumn("dbo.MenuModels", "ID", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.MenuModels", "ID");
            CreateIndex("dbo.SubMenus", "MenuID");
            AddForeignKey("dbo.SubMenus", "MenuID", "dbo.MenuModels", "ID");
        }
    }
}
