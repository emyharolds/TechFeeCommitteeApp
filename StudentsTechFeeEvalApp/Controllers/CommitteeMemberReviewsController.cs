using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StudentsTechFeeEvalApp.Models;
using StudentsTechFeeEvalApp.Models.Model_Classes;
using StudentsTechFeeEvalApp.ViewModels;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace StudentsTechFeeEvalApp.Controllers
{
    public class CommitteeMemberReviewsController : Controller
    {
        protected ApplicationDbContext db;
        protected UserManager<ApplicationUser> UserManager { get; set; }
        public CommitteeMemberReviewsController()
        {
            this.db = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.db));
        }

        // GET: CommitteeMemberReviews
        //[Authorize(Roles = "CommitteeChair")]
        public ActionResult Index()
        {
            var reviewViewModel = new RequestsInfoData();
            reviewViewModel.CommitteeMemberReviews = db.CommitteeMemberReviews.OrderBy(r => r.Id);
            return View(reviewViewModel);
        }

        public ActionResult Create(int? requestId)
        {
            var committeeReview = new CommitteeMemberReview();
            if (requestId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = db.Requests.Where(r => r.Id == requestId).Single();
            ViewBag.Request = request;
            return View();
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //ViewBag.SelectedRequest.Id = db.CommitteeMemberReviews.Where(r => r.RequestId == id).Single();
            CommitteeMemberReview review = db.CommitteeMemberReviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditReview(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var review = db.CommitteeMemberReviews.Where(r => r.Id == id).Single();
            try
            {
                if (TryUpdateModel(review, "", new string[] { "Vote", "Comment" }))
                {
                    if (review.Vote != null)
                    {
                        review.Request.StatusId = 3;
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again");
            }
            return View(review);
        }

    }

}