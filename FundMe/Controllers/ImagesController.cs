using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using FundMe.DAL;
using FundMe.Models;

namespace FundMe.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ImagesController : Controller
    {
        private FundMeContext db = new FundMeContext();

        // GET: Images
        public ActionResult Index()
        {
            ViewBag.Path = Constants.Constants.CampaignsThumbnailsPath;
            return View(db.Images.ToList());
        }

        // GET: Images/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image images = db.Images.Find(id);
            if (images == null)
            {
                return HttpNotFound();
            }
            ViewBag.Path = Constants.Constants.CampaignsImagePath;
            return View(images);
        }

        // GET: Images/Create
        public ActionResult Upload()
        {
            return View();
        }

        // POST: Images/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if(file != null)
            {
                if (ValidateFile(file))
                {
                    try
                    {
                        SaveFileToDisk(file);
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("FileName", "Sorry an error occurred saving the file to disk, please try again");
                    }
                }
                else
                {
                    ModelState.AddModelError("FileName", "The file must be gif, png, jpeg or jpg and less than 2MB in size");
                }
            }
            else
            {
                ModelState.AddModelError("FileName", "Please choose a file");
            }
            if (ModelState.IsValid)
            {
                db.Images.Add(new Image { FileName = file.FileName });
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Images/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image images = db.Images.Find(id);
            if (images == null)
            {
                return HttpNotFound();
            }
            return View(images);
        }

        // POST: Images/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FileName")] Image images)
        {
            if (ModelState.IsValid)
            {
                db.Entry(images).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(images);
        }

        // GET: Images/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image images = db.Images.Find(id);
            if (images == null)
            {
                return HttpNotFound();
            }
            ViewBag.Path = Constants.Constants.CampaignsImagePath;
            return View(images);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Image images = db.Images.Find(id);
            System.IO.File.Delete(Request.MapPath(Constants.Constants.CampaignsImagePath + images.FileName));
            System.IO.File.Delete(Request.MapPath(Constants.Constants.CampaignsThumbnailsPath + images.FileName));
            var camp = db.Campaigns.Where(c => c.PictureID == id);
            foreach (var c in camp)
            {
                c.PictureID = null;
            }
            db.Images.Remove(images);
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

        private bool ValidateFile(HttpPostedFileBase file)
        {
            string fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
            string[] allowedFileTypes = { ".gif", ".png", ".jpeg", ".jpg" };
            if ((file.ContentLength > 0 && file.ContentLength < 2097152) &&
            allowedFileTypes.Contains(fileExtension))
            {
                return true;
            }
            return false;
        }

        private void SaveFileToDisk(HttpPostedFileBase file)
        {
            WebImage img = new WebImage(file.InputStream);
            if (img.Width > 250)
            {
                img.Resize(250, img.Height);
            }
            img.Save(Constants.Constants.CampaignsImagePath + file.FileName);
            if (img.Width > 250)
            {
                img.Resize(180, img.Height);
            }
            img.Save(Constants.Constants.CampaignsThumbnailsPath + file.FileName);
        }

    }
}
