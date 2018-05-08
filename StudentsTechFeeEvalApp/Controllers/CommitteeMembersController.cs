using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using StudentsTechFeeEvalApp.Models;
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
    [Authorize (Roles = "CommitteeChairman")]
    public class CommitteeMembersController : Controller
    {
        private ApplicationDbContext db;
        private ApplicationUserManager _userManager;
        public CommitteeMembersController()
        {
            db = new ApplicationDbContext();
        }

        public CommitteeMembersController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        // GET: ApplicationUsers
        public ActionResult Index(CommitteeMessageId? message)
        {
            ViewBag.StatusMessage =
               message == CommitteeMessageId.CreateMemberSuccess ? "Committee Member Account successfully created."
               : message == CommitteeMessageId.EditMemberSuccess ? "Committee Member Account successfully edited."
               : message == CommitteeMessageId.ResetMemberPasswordSuccess ? "Committee Member Account Password successfully reset."
               : message == CommitteeMessageId.Error ? "An error occurred."
               : message == CommitteeMessageId.DeleteMemberSuccess ? "Committee Member Account successfully deleted."
               : "";
            var applicationUsers = db.Users.Where(u => u.Roles.Any(r => r.RoleId.Equals("7e15a9b7-ecb6-4d7f-a7ef-f7fd47fee111"))).Include(a => a.Department);
            return View(applicationUsers.ToList());
        }


        // GET: ApplicationUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Create
        public ActionResult Create()
        {
            ViewBag.Department = new SelectList(db.Departments.ToList(), "Id", "Name");
            return View();
        }

        // POST: ApplicationUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CommitteeMemberViewModel model)
        {
            if (ModelState.IsValid)
            {
                PasswordHasher hasher = new PasswordHasher();
                var user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    Email = model.Email,
                    DepartmentId = Convert.ToInt32(model.Department),
                    UserName = model.UserName,
                    IsPasswordChanged = false,
                    InitialPassword = model.Password,
                    PasswordHash = hasher.HashPassword(model.Password)
                };

                IdentityResult result = UserManager.Create(user, model.Password);

                if (result.Succeeded)
                {
                    this.UserManager.AddToRole(user.Id, "CommitteeMember");
                    return RedirectToAction("Index", "CommitteeMembers", new {Message = CommitteeMessageId.CreateMemberSuccess });
                }
                else
                {
                    ModelState.AddModelError("", "Error! Kindly check your password.");
                    return RedirectToAction("Create");
                }
                
            }

            ViewBag.Department = new SelectList(db.Departments.ToList(), "Id", "Name");
            return View();
        }

        // GET: ApplicationUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", applicationUser.DepartmentId);
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FullName,FirstName,MiddleName,LastName,IsPasswordChanged,InitialPassword,DepartmentId,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicationUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { Message = CommitteeMessageId.EditMemberSuccess });
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentName", applicationUser.DepartmentId);
            return View(applicationUser);
        }

        public ActionResult ResetPassword(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = applicationUser.Id;            
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", applicationUser.DepartmentId);
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("ResetPassword")]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPasswordConfirmed(string id)
        {
            //if (ModelState.IsValid)
            //{
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            var code = UserManager.GeneratePasswordResetToken(id);
            var result = UserManager.ResetPassword(applicationUser.Id, code, applicationUser.InitialPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", new { Message = CommitteeMessageId.ResetMemberPasswordSuccess });
                }
            //}
            ModelState.AddModelError("", "An error occurred");
            return View(applicationUser);
        }




        // GET: ApplicationUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Index", new { Message = CommitteeMessageId.DeleteMemberSuccess });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public enum CommitteeMessageId
        {
            CreateMemberSuccess,
            EditMemberSuccess,
            ResetMemberPasswordSuccess,
            DeleteMemberSuccess,
            Error
        }

    }
}