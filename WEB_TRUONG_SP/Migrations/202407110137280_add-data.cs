namespace WEB_TRUONG_SP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adddata : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.MenuModels");
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Lecturers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Position = c.String(),
                        Degree = c.String(),
                        Information = c.String(),
                        ImagePath = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        Field = c.String(),
                        CouncilPosition = c.String(),
                        IsDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        LogDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Summary = c.String(nullable: false),
                        Content = c.String(),
                        PublishDate = c.DateTime(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        Category = c.String(),
                        IsDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SubMenus",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        MenuID = c.String(maxLength: 128),
                        IsDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.MenuModels", t => t.MenuID)
                .Index(t => t.MenuID);
            
            AlterColumn("dbo.MenuModels", "ID", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.MenuModels", "Name", c => c.String());
            AddPrimaryKey("dbo.MenuModels", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubMenus", "MenuID", "dbo.MenuModels");
            DropIndex("dbo.SubMenus", new[] { "MenuID" });
            DropPrimaryKey("dbo.MenuModels");
            AlterColumn("dbo.MenuModels", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.MenuModels", "ID", c => c.String(nullable: false, maxLength: 128));
            DropTable("dbo.SubMenus");
            DropTable("dbo.News");
            DropTable("dbo.Logs");
            DropTable("dbo.Lecturers");
            DropTable("dbo.Departments");
            AddPrimaryKey("dbo.MenuModels", "ID");
        }
    }
}
