using StudentsTechFeeEvalApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StudentsTechFeeEvalApp.Controllers
{
    [Authorize(Roles = "CommitteeChairman")]
    public class PeriodsController : Controller
    {
        protected ApplicationDbContext db;
        public PeriodsController()
        {
            this.db = new ApplicationDbContext();
        }
        // GET: Periods
        public ActionResult Index()
        {
            ViewBag.ActivePeriod = db.Periods.Where(p => p.IsActive == true).Single();
            return View();
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public ActionResult ActivatePeriod(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var currentPeriod = db.Periods.Where(p => p.IsActive == true).Single();
            var newPeriod = db.Periods.Where(r => r.Id == id).Single();
            try
            {
                currentPeriod.IsActive = false;
                newPeriod.IsActive = true;
                db.Entry(currentPeriod).State = EntityState.Modified;
                db.Entry(newPeriod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again");
            }

            ViewBag.ActivePeriod = db.Periods.Where(p => p.IsActive == true).Single();
            return View();
        }

       
    }
}
