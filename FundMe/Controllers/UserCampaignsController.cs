using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FundMe.DAL;
using FundMe.Models;
using Microsoft.AspNet.Identity;

namespace FundMe.Controllers
{
    [Authorize(Roles = "Users")]
    public class UserCampaignsController : Controller
    {
        private FundMeContext db = new FundMeContext();

        // GET: Campaigns/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
            ViewBag.PictureID = new SelectList(db.Images, "ID", "FileName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Description,CampaignsGoal,Country,City,EndDate,CategoryID")] Campaign campaign, HttpPostedFileBase file)
        {
            ImagesController images = new ImagesController();
            if (file == null)
            {
                ModelState.AddModelError("PictureID", "Please choose a file");
            }
            else
            {
                images.Upload(file);
                Image image = db.Images.Single(i => i.FileName == file.FileName);
                if (image != null)
                {
                    campaign.Picture = image;
                    campaign.PictureID = image.ID;
                }
            }
            var user = User.Identity;
            campaign.UserID = user.GetUserId();
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

            if (User.Identity.GetUserId() != campaign.UserID)
            {
                if (!User.IsInRole("Admin"))
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            campaign.CurrentlyRaised = db.Donations.Where(d => d.CampaignID == id).Select(c => c.Iznos).DefaultIfEmpty(0).Sum();
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
            if (User.Identity.GetUserId() != campaign.UserID)
            {
                if (!User.IsInRole("Admin"))
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
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
            if (User.Identity.GetUserId() != campaign.UserID)
            {
                if (!User.IsInRole("Admin"))
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(campaign);
        }

        // POST: Campaigns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Campaign campaign = db.Campaigns.Find(id);
            if (User.Identity.GetUserId() != campaign.UserID)
            {
                if (!User.IsInRole("Admin"))
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
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

        private Image DeleteImage(Image image)
        {
            System.IO.File.Delete(Request.MapPath(Constants.Constants.CampaignsImagePath + image.FileName));
            System.IO.File.Delete(Request.MapPath(Constants.Constants.CampaignsThumbnailsPath + image.FileName));
            return image;
        }

    }
}