namespace FundMe.Migrations.FundMeConfigurations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedCampaigns : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Campaigns", "Description", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Campaigns", "Description", c => c.String(nullable: false, maxLength: 200));
        }
    }
}
