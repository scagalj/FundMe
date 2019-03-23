namespace FundMe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddData : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Campaigns", "PictureID", "dbo.Pictures");
            DropIndex("dbo.Campaigns", new[] { "PictureID" });
            AlterColumn("dbo.Campaigns", "PictureID", c => c.Int());
            CreateIndex("dbo.Campaigns", "PictureID");
            AddForeignKey("dbo.Campaigns", "PictureID", "dbo.Pictures", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Campaigns", "PictureID", "dbo.Pictures");
            DropIndex("dbo.Campaigns", new[] { "PictureID" });
            AlterColumn("dbo.Campaigns", "PictureID", c => c.Int(nullable: false));
            CreateIndex("dbo.Campaigns", "PictureID");
            AddForeignKey("dbo.Campaigns", "PictureID", "dbo.Pictures", "ID", cascadeDelete: true);
        }
    }
}
