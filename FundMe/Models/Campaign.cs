using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FundMe.Models
{
    public class Campaign
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CampaignsGoal { get; set; }
        public DateTime Date { get; set; }

        public int? CategoryID { get; set; }
        public int PictureID { get; set; }

        public virtual Category Category { get; set; }
        public virtual Picture Picture { get; set; }
    }
}