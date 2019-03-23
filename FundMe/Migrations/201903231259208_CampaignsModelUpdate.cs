namespace FundMe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampaignsModelUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "CurrentlyRaised", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Campaigns", "CurrentlyRaised");
        }
    }
}
