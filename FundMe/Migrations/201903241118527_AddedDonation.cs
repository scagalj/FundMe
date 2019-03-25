namespace FundMe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDonation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Donations", "Campaign_ID", "dbo.Campaigns");
            DropForeignKey("dbo.Donations", "Campaigns_ID", "dbo.Campaigns");
            DropForeignKey("dbo.Donations", "Campaign_ID1", "dbo.Campaigns");
            DropIndex("dbo.Donations", new[] { "Campaign_ID" });
            DropIndex("dbo.Donations", new[] { "Campaigns_ID" });
            DropIndex("dbo.Donations", new[] { "Campaign_ID1" });
            DropTable("dbo.Donations");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Donations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Iznos = c.Int(nullable: false),
                        Campaign_ID = c.Int(),
                        Campaigns_ID = c.Int(),
                        Campaign_ID1 = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.Donations", "Campaign_ID1");
            CreateIndex("dbo.Donations", "Campaigns_ID");
            CreateIndex("dbo.Donations", "Campaign_ID");
            AddForeignKey("dbo.Donations", "Campaign_ID1", "dbo.Campaigns", "ID");
            AddForeignKey("dbo.Donations", "Campaigns_ID", "dbo.Campaigns", "ID");
            AddForeignKey("dbo.Donations", "Campaign_ID", "dbo.Campaigns", "ID");
        }
    }
}
