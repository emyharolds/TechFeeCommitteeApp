using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using StudentsTechFeeEvalApp.Models;
using StudentsTechFeeEvalApp.Models.Model_Classes;
using StudentsTechFeeEvalApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StudentsTechFeeEvalApp.Controllers
{
    public class RequestsForCommitteeController : Controller
    {
        protected ApplicationDbContext db;
        protected UserManager<ApplicationUser> UserManager { get; set; }
        public RequestsForCommitteeController()
        {
            this.db = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.db));
        }


        // GET: RequestsForCommittee
        [Authorize(Roles = "CommitteeChairman,CommitteeMember,Dean")]
        public ActionResult Index(CommitteeRequestsMessageId? message)
        {
            ViewBag.StatusMessage =
              message == CommitteeRequestsMessageId.CommitteeMemberReviewSuccess ? "Request successfully reviewed by Committee Member"
              : message == CommitteeRequestsMessageId.CommitteeMemberReviewEditSuccess ? "Request review successfully edited by Committee Member"
              : message == CommitteeRequestsMessageId.CommitteeChairReviewSuccess ? "Request successfully reviewed by Committee Chairman."
              : message == CommitteeRequestsMessageId.DeanReviewSuccess ? "Request successfully reviewed by Dean"
              : message == CommitteeRequestsMessageId.WrongPhaseAction ? "Feature requested is not available during this current phase"
              : message == CommitteeRequestsMessageId.Error ? "An error occurred."
              : "";
            ViewBag.CurrentPeriod = this.CheckPeriod();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var viewModel = new RequestsInfoData();
            var reviews = db.CommitteeMemberReviews.Where(r => r.UserId == currentUser.Id); 
            var requests = db.Requests.Where(i => i.IsApprovedByDepartmentChair == true).OrderBy(i => i.Id);
            ViewBag.ChairReviewed = db.Requests.Where(i => i.IsApprovedByDepartmentChair == true).Where(d => d.CommitteeChairReview != null).OrderBy(c => c.Id);
            ViewBag.ChairPendingReview = db.Requests.Where(i => i.IsApprovedByDepartmentChair == true).Where(d => d.CommitteeChairReview == null).OrderBy(c => c.Id);
            viewModel.Requests = requests;
           
            viewModel.CommitteeMemberReviews = reviews;
            return View(viewModel);
        }



        [Authorize(Roles = "CommitteeChairman,CommitteeMember,Dean")]
        // GET: Requests/Details/5
        public ActionResult Details(int? id)
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = db.Requests.Where(i => i.IsApprovedByDepartmentChair == true).Where(s => s.Id == id).SingleOrDefault();
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        [Authorize(Roles = "CommitteeChairman")]
        public ActionResult ViewReviews(int? id)
        {
            if ((this.CheckPeriod() == 4) || (this.CheckPeriod() == 5))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var request = db.Requests.Where(i => i.IsApprovedByDepartmentChair == true).Where(s => s.Id == id).SingleOrDefault();
                if (request == null)
                {
                    return HttpNotFound();
                }
                ViewBag.RequestId = request.Id;
                ViewBag.SelectRequest = request.ItemDescription;
                var review = new RequestsInfoData();
                review.CommitteeMemberReviews = db.CommitteeMemberReviews.Where(i => i.RequestId == id).ToList();
                return View(review);
            }
            else
            {
                return RedirectToAction("Index", new {Message = CommitteeRequestsMessageId.WrongPhaseAction });
            }
        }

        [Authorize(Roles = "CommitteeChairman")]
        public ActionResult CommitteeChairReview(int? id)
        {
            if (this.CheckPeriod() == 5)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var request = db.Requests.Where(i => i.IsApprovedByDepartmentChair == true).Where(s => s.Id == id).SingleOrDefault();
                if (request == null)
                {
                    return HttpNotFound();
                }
                return View(request);
            }
            else
            {
                return RedirectToAction("Index", new {Message = CommitteeRequestsMessageId.WrongPhaseAction });
            }
        }

        [HttpPost, ActionName("CommitteeChairReview")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CommitteeChairman")]
        public ActionResult CommitteeChairReviewed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var request = db.Requests.Where(i => i.IsApprovedByDepartmentChair == true).Where(s => s.Id == id).SingleOrDefault();
            if (request == null)
            {
                return HttpNotFound();
            }

            try
            {
                if (TryUpdateModel(request, "", new string[] { "CommitteeChairComment", "CommitteeChairReview", "ItemDescription", "ItemCost", "ItemUsage", "Justification", "NoOfStudentsImpacted", "DepartmentChairComment", "IsApprovedByDepartmentChair", "RankByDept" }))
                {
                    if (request.CommitteeChairReview != null)
                    {
                        request.StatusId = 3;
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index", new { Message = CommitteeRequestsMessageId.CommitteeChairReviewSuccess });
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again");
            }
            return View(request);
        }


        [Authorize(Roles = "Dean")]
        public ActionResult DeanReview(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = db.Requests.Where(i => i.IsApprovedByDepartmentChair == true).Where(s => s.Id == id).SingleOrDefault();
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        [HttpPost, ActionName("DeanReview")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Dean")]
        public ActionResult DeanReviewed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var request = db.Requests.Where(i => i.IsApprovedByDepartmentChair == true).Where(s => s.Id == id).SingleOrDefault();
            if (request == null)
            {
                return HttpNotFound();
            }
            try
            {
                if (TryUpdateModel(request, "", new string[] { "IsApprovedByDean", "CommitteeChairComment", "CommitteeChairReview", "ItemDescription", "ItemCost", "ItemUsage", "Justification", "NoOfStudentsImpacted", "DepartmentChairComment", "IsApprovedByDepartmentChair", "RankByDept" }))
                {
                    if (request.IsApprovedByDean == true)
                    {
                        request.StatusId = 4;
                    }
                    else
                    {
                        request.StatusId = 5;
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index", new { Message = CommitteeRequestsMessageId.DeanReviewSuccess });
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again");
            }
            return View(request);
        }

        [Authorize(Roles = "CommitteeMember")]
        public ActionResult CommitteeReview(int? id)
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = db.Requests.Where(i => i.IsApprovedByDepartmentChair == true).Where(s => s.Id == id).SingleOrDefault();
            if (request == null)
            {
                return HttpNotFound();
            }
            var reviewed = db.CommitteeMemberReviews.Where(r => r.RequestId == request.Id).Where(u => u.UserId == currentUser.Id).Where(i => i.IsReviewed == true).SingleOrDefault();
            if (reviewed != null)
            {
                ViewBag.RequestReviewed = 1;
                return RedirectToAction("EditReview");
            }            

            CommitteeMemberReview review = new CommitteeMemberReview();
            ViewBag.SelectedRequest = request;
            return View(review);
        }

        [HttpPost, ActionName("CommitteeReview")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CommitteeMember")]
        public ActionResult CommitteeReviewed([Bind(Include = "UserId,RequestId,Vote,Comment")] CommitteeMemberReview review, int? requestId)
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            if (requestId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            var request = db.Requests.Where(i => i.IsApprovedByDepartmentChair == true).Where(s => s.Id == requestId).SingleOrDefault();
            if (request == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                review.UserId = currentUser.Id;
                review.IsReviewed = true;                
                try
                {
                    db.CommitteeMemberReviews.Add(review);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { Message = CommitteeRequestsMessageId.CommitteeMemberReviewSuccess});
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Unable to save changes. You can only vote once on a request");
                }
            }

            ViewBag.SelectedRequest = request;
            return View(review);
        }


        [Authorize(Roles = "CommitteeMember")]
        public ActionResult EditReview(int? id)
        {
            var request = db.CommitteeMemberReviews.Where(r => r.Id == id).Select(x => x.Request);
            ViewBag.SelectedRequest = request;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var review = db.CommitteeMemberReviews.Where(r => r.Id == id).SingleOrDefault();
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        [HttpPost, ActionName("EditReview")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CommitteeMember")]
        public ActionResult EditReviewed(int? id)
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var review = db.CommitteeMemberReviews.Where(d => d.UserId == currentUser.Id).Where(e => e.Id == id).SingleOrDefault();
            try
            {
                if (TryUpdateModel(review, "", new string[] { "UserId","RequestId","Vote","Comment","IsReviewed" }))
                {
                    db.SaveChanges();
                    return RedirectToAction("Index", new { Message = CommitteeRequestsMessageId.CommitteeMemberReviewEditSuccess});
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again");
            }
            return View(review);
        }


        private int CheckPeriod()
        {
            var checkPeriod = db.Periods.Where(c => c.IsActive == true).Single();
            var periodId = checkPeriod.Id;
            return periodId;
        }

        private bool CheckReviewStatus(int? id)
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var reviewed = db.CommitteeMemberReviews.Where(r => r.RequestId == id).Where(u => u.UserId == currentUser.Id).Where(i => i.IsReviewed == true).SingleOrDefault();
            if (reviewed != null)
            {
                return true;
            }
            return false;
        }

        public enum CommitteeRequestsMessageId
        {
            CommitteeChairReviewSuccess,
            CommitteeMemberReviewSuccess,
            CommitteeMemberReviewEditSuccess,
            WrongPhaseAction,
            DeanReviewSuccess,
            Error
        }

    }
}