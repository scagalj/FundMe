using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FundMe.Models;
using System.Data.Entity;

namespace FundMe.DAL
{
    public class FundMeContext : DbContext
    {
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }

    }
}