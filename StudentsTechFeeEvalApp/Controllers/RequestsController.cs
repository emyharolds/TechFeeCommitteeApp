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
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StudentsTechFeeEvalApp.Controllers
{
    public class RequestsController : Controller
    {
        // GET: Requests
        protected ApplicationDbContext db;
        protected UserManager<ApplicationUser> UserManager { get; set; }
        public RequestsController()
        {
            this.db = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.db));
        }

        // GET: Requests
        [Authorize(Roles ="DepartmentChair,CommitteeChairman,CommitteeMember,Dean")]
        public ActionResult Index(DeptRequestsMessageId? message)
        {
            ViewBag.StatusMessage =
              message == DeptRequestsMessageId.RequestEditSuccess ? "Departmental Request successfully edited"
              : message == DeptRequestsMessageId.DeptChairReviewSuccess ? "Request successfully reviewed by Department Chair"
              : message == DeptRequestsMessageId.WrongPhaseAccess ? "Requested feature not available during this current phase."
              : message == DeptRequestsMessageId.Error ? "An error occurred."
              : "";
            ViewBag.CurrentPeriod = this.CheckPeriod();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var viewModel = new RequestsInfoData();
            viewModel.Requests = db.Requests.Where(i => i.DepartmentId == currentUser.DepartmentId).OrderBy(i => i.Id);
            return View(viewModel);
        }


     
  
        [Authorize(Roles = "DepartmentChair,CommitteeChairman,CommitteeMember,Dean")]
        public ActionResult Details(int? id)
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = db.Requests.Where(r => r.DepartmentId == currentUser.DepartmentId).Where(s => s.Id == id).SingleOrDefault();
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        private void PopulateAssignedClassData(Request request)
        {
            var allClasses = db.StudentClasses;
            var requestClasses = new HashSet<int>(request.StudentClasses.Select(s => s.Id));
            var viewModel = new List<AssignedClassData>();
            foreach (var aClass in allClasses)
            {
                viewModel.Add(new AssignedClassData
                {
                    ClassId = aClass.Id,
                    ClassName = aClass.Name,
                    Assigned = requestClasses.Contains(aClass.Id)
                });
            }
            ViewBag.Classes = viewModel;
        }

        // GET: Requests/Edit/5
        [Authorize(Roles = "DepartmentChair,CommitteeChairman,CommitteeMember,Dean")]
        public ActionResult Edit(int? id)
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            ViewBag.CurrentPeriod = this.CheckPeriod();
            if ((this.CheckPeriod() == 2) || (this.CheckPeriod() == 3))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var request = db.Requests.Where(r => r.DepartmentId == currentUser.DepartmentId).Where(s => s.Id == id).SingleOrDefault();
                if (request == null)
                {
                    return HttpNotFound();
                }
                PopulateAssignedClassData(request);
                return View(request);
            }
            else
            {
                return RedirectToAction("Index", new { Message = DeptRequestsMessageId.WrongPhaseAccess});
            }
            
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DepartmentChair,CommitteeChairman,CommitteeMember,Dean")]
        public ActionResult Edit(int? id, string[] selectedClasses)
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var request = db.Requests.Where(r => r.DepartmentId == currentUser.DepartmentId).Where(s => s.Id == id).SingleOrDefault();
            if (request == null)
            {
                return HttpNotFound();
            }
            try
            {
                if (TryUpdateModel(request, "", new string[] { "ItemDescription", "ItemCost", "ItemUsage", "Justification", "NoOfStudentsImpacted" }))
                {
                    request.DateOfSubmission = DateTime.Now;
                    UpdateRequestClasses(selectedClasses, request);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { Message = DeptRequestsMessageId.RequestEditSuccess});
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again");
            }
            PopulateAssignedClassData(request);
            return View(request);
        }

        private void UpdateRequestClasses(string[] selectedClasses, Request request)
        {
            if (selectedClasses == null)
            {
                request.StudentClasses = new List<StudentClass>();
                return;
            }

            var selectedClassHashSet = new HashSet<string>(selectedClasses);
            var requestClasses = new HashSet<int>(request.StudentClasses.Select(c => c.Id));
            foreach (var aClass in db.StudentClasses)
            {
                if (selectedClassHashSet.Contains(aClass.Id.ToString()))
                {
                    if (!requestClasses.Contains(aClass.Id))
                    {
                        request.StudentClasses.Add(aClass);
                    }
                }
                else
                {
                    if (requestClasses.Contains(aClass.Id))
                    {
                        request.StudentClasses.Remove(aClass);
                    }
                }
            }
        }

      
        [Authorize(Roles = "DepartmentChair")]
        public ActionResult ReviewByDepartmentChair(int? id)
        {
            ViewBag.CurrentPeriod = this.CheckPeriod();
            if (this.CheckPeriod() == 3)
            {
                ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var request = db.Requests.Where(r => r.DepartmentId == currentUser.DepartmentId).Where(r => r.Id == id).SingleOrDefault();
                if (request == null)
                {
                    return HttpNotFound();
                }
                return View(request);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ActionName("ReviewByDepartmentChair")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DepartmentChair")]
        public ActionResult ReviewedByDepartmentChair(int? id)
        {
            ViewBag.CurrentPeriod = this.CheckPeriod();
            if (this.CheckPeriod() == 3)
            {
                ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var request = db.Requests.Where(r => r.DepartmentId == currentUser.DepartmentId).Where(r => r.Id == id).SingleOrDefault();
                if (request == null)
                {
                    return HttpNotFound();
                }
                try
                {
                    if (TryUpdateModel(request, "", new string[] { "ItemDescription", "ItemCost", "ItemUsage", "Justification", "NoOfStudentsImpacted", "DepartmentChairComment", "IsApprovedByDepartmentChair", "RankByDept" }))
                    {
                        if (request.IsApprovedByDepartmentChair == true)
                        {
                            request.StatusId = 2;
                        }
                        else
                        {
                            request.StatusId = 5;
                        }
                        db.SaveChanges();
                        return RedirectToAction("Index", new { Message = DeptRequestsMessageId.DeptChairReviewSuccess});
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again");
                }
                return View(request);
            }
            else
            {
                return RedirectToAction("Index", new { Message = DeptRequestsMessageId.WrongPhaseAccess});
            }
        }

              


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private int CheckPeriod()
        {
            var checkPeriod = db.Periods.Where(c => c.IsActive == true).Single();
            var periodId = checkPeriod.Id;
            return periodId;
        }
    }

    public enum DeptRequestsMessageId
    {
        RequestEditSuccess,
        DeptChairReviewSuccess,
        WrongPhaseAccess,
        Error
    }
}