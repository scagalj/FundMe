namespace FundMe.Migrations.FundMeConfigurations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedCampaigns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "UserID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Campaigns", "UserID");
        }
    }
}
