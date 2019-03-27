namespace FundMe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_user_fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.AspNetUsers", "DateOfBirth", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "Adress_AdressLine", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "Adress_Town", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "Adress_Country", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "Adress_PostCode", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Adress_PostCode");
            DropColumn("dbo.AspNetUsers", "Adress_Country");
            DropColumn("dbo.AspNetUsers", "Adress_Town");
            DropColumn("dbo.AspNetUsers", "Adress_AdressLine");
            DropColumn("dbo.AspNetUsers", "DateOfBirth");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
        }
    }
}
