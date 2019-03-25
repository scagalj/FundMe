namespace FundMe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDonation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Donations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Iznos = c.Int(nullable: false),
                        CampaignID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Campaigns", t => t.CampaignID, cascadeDelete: true)
                .Index(t => t.CampaignID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Donations", "CampaignID", "dbo.Campaigns");
            DropIndex("dbo.Donations", new[] { "CampaignID" });
            DropTable("dbo.Donations");
        }
    }
}
