namespace FundMe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDateToDonation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Donations", "DonationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Donations", "DonationDate");
        }
    }
}
