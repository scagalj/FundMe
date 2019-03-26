using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FundMe.DAL;
using FundMe.Models;
using Microsoft.Ajax.Utilities;

namespace FundMe.Controllers
{
    public class CampaignsController : Controller
    {
        private FundMeContext db = new FundMeContext();

        // GET: Campaigns
        public ActionResult Index(string category, string search)
        {
            var campaigns = db.Campaigns.Include(c => c.Category).Include(c => c.Picture);
            
            if (!String.IsNullOrEmpty(search))
            {
                campaigns = campaigns.Where(c => c.Title.Contains(search) || c.Description.Contains(search) || c.Category.Name.Contains(search));
                ViewBag.Search = search;
            }

            var categories = campaigns.OrderBy(c => c.Category.Name).Select(c => c.Category.Name).Distinct();

            if (!String.IsNullOrEmpty(category))
            {
                campaigns = campaigns.Where(c => c.Category.Name == category);
            }
            ViewBag.Category = new SelectList(categories);
            ViewBag.Path = Constants.Constants.CampaignsThumbnailsPath;
            return View(campaigns.ToList());
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
            campaign.CurrentlyRaised = LoadDonation(campaign.ID);
            ViewBag.Path = Constants.Constants.CampaignsImagePath;

            //Popis svih donacija
            var donations = db.Donations.Where(d => d.CampaignID == id).OrderByDescending(d => d.DonationDate).ToList();
            ViewBag.Donations = donations;
            return View(campaign);
        }

        // GET: Campaigns/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
            ViewBag.PictureID = new SelectList(db.Images, "ID", "FileName");
            return View();
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

        // POST: Campaigns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Description,CampaignsGoal,Country,City,EndDate,CategoryID")] Campaign campaign, HttpPostedFileBase file)
        {
            ImagesController images = new ImagesController();
            if (file == null)
            {
                ModelState.AddModelError("PictureID", "Please choose a file");
            }else
            {
                images.Upload(file);
                Image image = db.Images.Single(i => i.FileName == file.FileName);
                if (image != null)
                {
                    campaign.Picture = image;
                    campaign.PictureID = image.ID;
                }
            }
            campaign.CurrentlyRaised = 0;
            campaign.IsActive = true;
            campaign.StartDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Campaigns.Add(campaign);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", campaign.CategoryID);
            return View(campaign);
        }

        // GET: Campaigns/Edit/5
        public ActionResult Edit(int? id)
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
            campaign.CurrentlyRaised = LoadDonation(campaign.ID);
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", campaign.CategoryID);
            return View(campaign);
        }

        // POST: Campaigns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Description,CampaignsGoal,Country,City,StartDate,EndDate,IsActive,CategoryID,PictureID")] Campaign campaign)
        {
            if (ModelState.IsValid)
            {
                db.Entry(campaign).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", campaign.CategoryID);
            ViewBag.PictureID = new SelectList(db.Images, "ID", "FileName", campaign.PictureID);
            return View(campaign);
        }

        // GET: Campaigns/Delete/5
        public ActionResult Delete(int? id)
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
            campaign.CurrentlyRaised = LoadDonation(campaign.ID);
            return View(campaign);
        }

        // POST: Campaigns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Campaign campaign = db.Campaigns.Find(id);
            Image image = db.Images.Find(campaign.PictureID);
            if (image != null)
                db.Images.Remove(DeleteImage(image));
            db.Campaigns.Remove(campaign);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private int LoadDonation(int id)
        {
            return db.Donations.Where(d => d.CampaignID == id).Select(c => c.Iznos).DefaultIfEmpty(0).Sum();
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

        private Image DeleteImage(Image image)
        {
            System.IO.File.Delete(Request.MapPath(Constants.Constants.CampaignsImagePath + image.FileName));
            System.IO.File.Delete(Request.MapPath(Constants.Constants.CampaignsThumbnailsPath + image.FileName));
            return image;
        }

    }
}
