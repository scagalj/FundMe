namespace FundMe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateImage : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Pictures", newName: "Images");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Images", newName: "Pictures");
        }
    }
}
