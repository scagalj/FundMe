using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FundMe.Models;
using System.Data.Entity;

namespace FundMe.DAL
{
    public class FundMeContex : DbContext
    {
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Picture> Pictures { get; set; }

    }
}