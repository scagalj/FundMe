namespace FundMe.Migrations
{
    using FundMe.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FundMe.DAL.FundMeContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FundMe.DAL.FundMeContext context)
        {
            try
            {
                var categories = new List<Category>
                {
                new Category { Name = "Odjeæa" },
                new Category { Name = "Hrana" },
                new Category { Name = "Igraèke" },
                new Category { Name = "Putovanje" },
                new Category { Name= "Sport" }
                };

                categories.ForEach(c => context.Categories.AddOrUpdate(p => p.Name, c));
                context.SaveChanges();

                var campaign = new List<Campaign>
            {
                new Campaign{ Title = "One way to explore" , Description = "One way or another",CampaignsGoal=100, CategoryID=categories.Single(c => c.Name == "Hrana").ID, Date=DateTime.Now },
                new Campaign{ Title = "Travel to America" , Description = "Travel to America wiht my bag",CampaignsGoal=500, CategoryID=categories.Single(c => c.Name == "Putovanje").ID,Date=DateTime.Now },
                new Campaign{ Title = "Game to Develope" , Description = "Arcade Game Developements",CampaignsGoal=250, CategoryID=categories.Single(c => c.Name == "Igraèke").ID,Date=DateTime.Now }
            };

                campaign.ForEach(c => context.Campaigns.AddOrUpdate(p => p.Title, c));
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
    }
}
