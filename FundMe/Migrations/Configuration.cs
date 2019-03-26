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
                new Campaign{ Title = "One way to explore" , Description = "One way or another",CampaignsGoal=100, CategoryID=categories.Single(c => c.Name == "Hrana").ID, StartDate=DateTime.Now,CurrentlyRaised=0,IsActive=true,EndDate=DateTime.Now.AddDays(24),Country="Hrvatska",City="Zagreb" },
                new Campaign{ Title = "Travel to America" , Description = "Travel to America wiht my bag",CampaignsGoal=500, CategoryID=categories.Single(c => c.Name == "Putovanje").ID,StartDate=DateTime.Now,CurrentlyRaised=0,IsActive=true,EndDate=DateTime.Now.AddDays(15),Country="Hrvatska",City="Zagreb" },
                new Campaign{ Title = "Game to Develope" , Description = "Arcade Game Developements",CampaignsGoal=250, CategoryID=categories.Single(c => c.Name == "Igraèke").ID,StartDate=DateTime.Now, CurrentlyRaised=0,IsActive=true,EndDate=DateTime.Now.AddDays(50),Country="Hrvatska",City="Zagreb" }
            };

                campaign.ForEach(c => context.Campaigns.AddOrUpdate(p => p.Title, c));
                context.SaveChanges();

                var donation = new List<Donation>
                {
                    new Donation{Iznos = 10, CampaignID = campaign.Single(c => c.ID ==3).ID,DonationDate = DateTime.Now},
                    new Donation{Iznos = 50, CampaignID = campaign.Single(c => c.ID ==2).ID,DonationDate = DateTime.Now},
                    new Donation{Iznos = 30, CampaignID = campaign.Single(c => c.ID ==3).ID,DonationDate = DateTime.Now},
                    new Donation{Iznos = 25, CampaignID = campaign.Single(c => c.ID ==4).ID,DonationDate = DateTime.Now},
                    new Donation{Iznos = 15, CampaignID = campaign.Single(c => c.ID ==2).ID,DonationDate = DateTime.Now}
                };

                donation.ForEach(c => context.Donations.AddOrUpdate(p => p.ID, c));
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
