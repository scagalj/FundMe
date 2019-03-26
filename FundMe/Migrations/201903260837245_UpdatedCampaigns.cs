namespace FundMe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedCampaigns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Campaigns", "EndDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Campaigns", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Campaigns", "Country", c => c.String());
            AddColumn("dbo.Campaigns", "City", c => c.String());
            DropColumn("dbo.Campaigns", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Campaigns", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Campaigns", "City");
            DropColumn("dbo.Campaigns", "Country");
            DropColumn("dbo.Campaigns", "IsActive");
            DropColumn("dbo.Campaigns", "EndDate");
            DropColumn("dbo.Campaigns", "StartDate");
        }
    }
}
