using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FundMe.Models
{
    public class Campaign
    {
        public int ID { get; set; }
        [Display(Name = "Campaign Title")]
        [Required(ErrorMessage = "Campaign title can't be empty")]
        [StringLength(50,MinimumLength =5, ErrorMessage = "Campaign title must be between 5 and 50 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Campaign description can't be empty")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 200 characters")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Campaigns Goal")]
        [Required(ErrorMessage = "Champaigns Goal can't be empty")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public int CampaignsGoal { get; set; }

        [Display(Name = "Currently Raised")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public int CurrentlyRaised { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public int? CategoryID { get; set; }
        public int? PictureID { get; set; }

        public virtual Category Category { get; set; }
        public virtual Image Picture { get; set; }
    }
}