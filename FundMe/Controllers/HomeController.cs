using FundMe.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FundMe.Controllers
{
    public class HomeController : Controller
    {
        private FundMeContext db = new FundMeContext();

        public ActionResult Index()
        {
            var top5 = (from c in db.Campaigns
                        where c.IsActive == true
                        orderby c.CurrentlyRaised descending
                        select c).Take(3);

            ViewBag.Path = Constants.Constants.CampaignsThumbnailsPath;
            return View(top5.ToList());
        }


    }
}