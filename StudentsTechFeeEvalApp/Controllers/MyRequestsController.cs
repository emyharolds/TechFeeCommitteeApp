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
    [Authorize(Roles = "Faculty,DepartmentChair,CommitteeChairman,CommitteeMember,Dean")]
    public class MyRequestsController : Controller
    {
        // GET: Requests
        protected ApplicationDbContext db;
        protected UserManager<ApplicationUser> UserManager { get; set; }
        public MyRequestsController()
        {
            this.db = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.db));
        }

        // GET: MyRequests
        public ActionResult Index(RequestMessageId? message)
        {
            ViewBag.StatusMessage =
              message == RequestMessageId.CreateRequestSuccess ? "Request successfully created."
              : message == RequestMessageId.EditRequestSuccess ? "Request successfully edited."
              : message == RequestMessageId.DeleteRequestSuccess ? "Request successfully deleted."
              : message == RequestMessageId.WrongPhaseAccess ? "Requested feature not available during this current phase."
              : message == RequestMessageId.Error ? "An error occurred."
              : "";
            ViewBag.CurrentPeriod = this.CheckPeriod();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var viewModel = new RequestsInfoData();
            viewModel.Requests = db.Requests.Where(r => r.UserId == currentUser.Id).OrderBy(i => i.Id);
            return View(viewModel);
        }

        // GET: Requests/Details/5
        public ActionResult Details(int? id)
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = db.Requests.Where(r => r.UserId == currentUser.Id).Where(s => s.Id == id).SingleOrDefault();

            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // GET: Requests/Create
        public ActionResult Create()
        {
            if (this.CheckPeriod() == 2)
            {
                var request = new Request();
                request.StudentClasses = new List<StudentClass>();
                PopulateAssignedClassData(request);
                return View();
            }
            else
            {
                return RedirectToAction("Index", new { Message = RequestMessageId.WrongPhaseAccess });
            }
            
        }

        // POST: Requests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Request request, string[] selectedClasses)
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            if (selectedClasses != null)
            {
                request.StudentClasses = new List<StudentClass>();
                foreach (var aClass in selectedClasses)
                {
                    var classToAdd = db.StudentClasses.Find(int.Parse(aClass));
                    request.StudentClasses.Add(classToAdd);
                }
            }
            if (ModelState.IsValid)
            {
                request.DateOfSubmission = DateTime.Now;
                request.SessionId = 1;
                request.StatusId = 1;
                request.UserId = currentUser.Id;
                request.DepartmentId = currentUser.DepartmentId;
                db.Requests.Add(request);
                db.SaveChanges();
                return RedirectToAction("Index", new { Message = RequestMessageId.CreateRequestSuccess});
            }
            PopulateAssignedClassData(request);
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
        public ActionResult Edit(int? id)
        {
            ViewBag.CurrentPeriod = this.CheckPeriod();
            if (this.CheckPeriod() == 2)
            {
                ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var request = db.Requests.Where(r => r.UserId == currentUser.Id).Where(s => s.Id == id).SingleOrDefault();
                if (request == null)
                {
                    return HttpNotFound();
                }
                PopulateAssignedClassData(request);
                return View(request);
            }
            else
            {
                return RedirectToAction("Index", new { Message = RequestMessageId.WrongPhaseAccess});
            }
           
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedClasses)
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var request = db.Requests.Where(r => r.UserId == currentUser.Id).Where(s => s.Id == id).SingleOrDefault();
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
                    return RedirectToAction("Index", new { Message = RequestMessageId.EditRequestSuccess});
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

        // GET: Requests/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.CurrentPeriod = this.CheckPeriod();
            if (this.CheckPeriod() == 2)
            {
                ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var request = db.Requests.Where(r => r.UserId == currentUser.Id).Where(s => s.Id == id).SingleOrDefault();
                if (request == null)
                {
                    return HttpNotFound();
                }
                return View(request);
            }
            else
            {
                return RedirectToAction("Index", new { Message = RequestMessageId.WrongPhaseAccess});
            }
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var request = db.Requests.Where(r => r.UserId == currentUser.Id).Where(s => s.Id == id).SingleOrDefault();
            db.Requests.Remove(request);
            db.SaveChanges();
            return RedirectToAction("Index", new { Message = RequestMessageId.DeleteRequestSuccess});
        }
            

        private int CheckPeriod()
        {
            var checkPeriod = db.Periods.Where(c => c.IsActive == true).Single();
            var periodId = checkPeriod.Id;
            return periodId;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public enum RequestMessageId
        {
            CreateRequestSuccess,
            EditRequestSuccess,
            DeleteRequestSuccess,
            WrongPhaseAccess,
            Error
        }
    }
}