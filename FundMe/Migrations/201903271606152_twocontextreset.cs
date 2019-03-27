namespace FundMe.Migrations.FundMeConfigurations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class twocontextreset : DbMigration
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
                        CurrentlyRaised = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Country = c.String(),
                        City = c.String(),
                        CategoryID = c.Int(),
                        PictureID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.CategoryID)
                .ForeignKey("dbo.Images", t => t.PictureID)
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
                "dbo.Images",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.FileName, unique: true);
            
            CreateTable(
                "dbo.Donations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Iznos = c.Int(nullable: false),
                        DonationDate = c.DateTime(nullable: false),
                        CampaignID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Campaigns", t => t.CampaignID, cascadeDelete: true)
                .Index(t => t.CampaignID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Donations", "CampaignID", "dbo.Campaigns");
            DropForeignKey("dbo.Campaigns", "PictureID", "dbo.Images");
            DropForeignKey("dbo.Campaigns", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Donations", new[] { "CampaignID" });
            DropIndex("dbo.Images", new[] { "FileName" });
            DropIndex("dbo.Campaigns", new[] { "PictureID" });
            DropIndex("dbo.Campaigns", new[] { "CategoryID" });
            DropTable("dbo.Donations");
            DropTable("dbo.Images");
            DropTable("dbo.Categories");
            DropTable("dbo.Campaigns");
        }
    }
}
