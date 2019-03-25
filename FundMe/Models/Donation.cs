using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FundMe.Models;

namespace FundMe.Models
{
    public class Donation
    {
        public int ID { get; set; }
        public int Iznos { get; set; }
        public DateTime DonationDate { get; set; }
        public int CampaignID { get; set; }


        public virtual Campaign Campaigns { get; set; }
    }
}