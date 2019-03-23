namespace FundMe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Campaigns",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 200),
                        CampaignsGoal = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        CategoryID = c.Int(),
                        PictureID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.CategoryID)
                .ForeignKey("dbo.Pictures", t => t.PictureID, cascadeDelete: true)
                .Index(t => t.CategoryID)
                .Index(t => t.PictureID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.FileName, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Campaigns", "PictureID", "dbo.Pictures");
            DropForeignKey("dbo.Campaigns", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Pictures", new[] { "FileName" });
            DropIndex("dbo.Campaigns", new[] { "PictureID" });
            DropIndex("dbo.Campaigns", new[] { "CategoryID" });
            DropTable("dbo.Pictures");
            DropTable("dbo.Categories");
            DropTable("dbo.Campaigns");
        }
    }
}
