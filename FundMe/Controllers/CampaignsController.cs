using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FundMe.DAL;
using FundMe.Models;
using PagedList;

namespace FundMe.Controllers
{
    public class CampaignsController : Controller
    {
        private FundMeContext db = new FundMeContext();

        // GET: Campaigns
        public ActionResult Index(string category, string search, int? page)
        {
            var campaigns = db.Campaigns.Include(c => c.Category).Include(c => c.Picture);
            
            if (!String.IsNullOrEmpty(search))
            {
                campaigns = campaigns.Where(c => c.Title.Contains(search) || c.Description.Contains(search) || c.Category.Name.Contains(search));
                ViewBag.Search = search;
                page = 1;
            }

            //var categories = campaigns.OrderBy(c => c.Category.Name).Select(c => c.Category.Name).Distinct();
            var categories = db.Categories.Select(c => c.Name);
            if (!String.IsNullOrEmpty(category))
            {
                campaigns = campaigns.Where(c => c.Category.Name == category);
            }
            ViewBag.Category = new SelectList(categories);
            ViewBag.Path = Constants.Constants.CampaignsThumbnailsPath;
            campaigns = campaigns.OrderByDescending(c => c.StartDate);
            int PageNumber = (page ?? 1);
            return View(campaigns.ToPagedList(PageNumber,Constants.Constants.pageSize));
        }

        // GET: Campaigns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Campaign campaign = db.Campaigns.Find(id);
            if (campaign == null)
            {
                return HttpNotFound();
            }
            campaign.CurrentlyRaised = db.Donations.Where(d => d.CampaignID == id).Select(c => c.Iznos).DefaultIfEmpty(0).Sum();
            ViewBag.Path = Constants.Constants.CampaignsImagePath;

            //Popis svih donacija
            var donations = db.Donations.Where(d => d.CampaignID == id).OrderByDescending(d => d.DonationDate).ToList();
            ViewBag.Donations = donations;
            return View(campaign);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Donate(int donate, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var camp = db.Campaigns.Find(id);
            if (camp == null)
            {
                return HttpNotFound();
            }
            Donation donation = new Donation()
            {
                Iznos = donate,
                CampaignID = camp.ID,
                DonationDate = DateTime.Now
            };

            SaveDonation(camp, donate);
            if (ModelState.IsValid)
            {
                db.Donations.Add(donation);
                db.SaveChanges();
            }
            return Redirect("Details/" + id);
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        

        private void SaveDonation(Campaign campaign, int donation)
        {
            campaign.CurrentlyRaised += donation;
            if (ModelState.IsValid)
            {
                db.Entry(campaign).State = EntityState.Modified;
                db.SaveChanges();
               
            }

        }

        public class MyDateAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)// Return a boolean value: true == IsValid, false != IsValid
            {
                DateTime d = Convert.ToDateTime(value);
                return d >= DateTime.Now; //Dates Greater than or equal to today are valid (true)

            }
        }
        /*
        private Image DeleteImage(Image image)
        {
            System.IO.File.Delete(Request.MapPath(Constants.Constants.CampaignsImagePath + image.FileName));
            System.IO.File.Delete(Request.MapPath(Constants.Constants.CampaignsThumbnailsPath + image.FileName));
            return image;
        }*/

    }
}
