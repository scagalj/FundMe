namespace FundMe.Migrations.FundMeConfigurations
{
    using FundMe.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;

    internal sealed class FundMeConfiguration : DbMigrationsConfiguration<FundMe.DAL.FundMeContext>
    {
        public FundMeConfiguration()
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
