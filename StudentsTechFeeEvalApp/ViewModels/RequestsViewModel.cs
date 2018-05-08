using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using StudentsTechFeeEvalApp.Models;
using StudentsTechFeeEvalApp.Models.Model_Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentsTechFeeEvalApp.ViewModels
{
    public class RequestsViewModel
    {
        public RequestsViewModel()
        {
            Classes = new Collection<AssignedClassData>();
        }

        public int RequestId { get; set; }

        [Required]
        [Display(Name = "Description of Item: ")]
        public string ItemDescription { get; set; }

        [Required]
        [Display(Name = "How will the Item be used: ")]
        public string ItemUsage { get; set; }

        [Required]
        [Display(Name = "Cost of Item (with Shipping): ")]
        public decimal ItemCost { get; set; }


        [Required]
        [Display(Name = "Justification for Request: ")]
        public string Justification { get; set; }

        [Required]
        [Display(Name = "Number of Students Impacted: ")]
        public int NoOfStudentsImpacted { get; set; }


        [Display(Name = "Select Classes Supported: ")]
        public virtual ICollection<AssignedClassData> Classes { get; set; }

        //public virtual ICollection<CommitteeVotesData> CommitteeMemberReviews { get; set; }

        public int StatusId { get; set; }
        public bool IsApprovedByDept { get; set; }
        public int SessionId { get; set; }
        public int DepartmentId { get; set; }
        public DateTime DateOfSubmission { get; set; }
        public string UserId { get; set; }
        public string DepartmentChairComment { get; set; }
        public string RankByDept { get; set; }
        public string CommitteeChairReview { get; set; }
        public bool IsApprovedByDean { get; set; }
    }

    public static class RequestHelpers
    {
        public static RequestsViewModel ToViewModel(this Request request)
        {
            var requestViewModel = new RequestsViewModel
            {
                RequestId = request.Id,
                ItemDescription = request.ItemDescription,
                ItemCost = request.ItemCost,
                ItemUsage = request.ItemUsage,
                Justification = request.Justification,
                NoOfStudentsImpacted = request.NoOfStudentsImpacted,
                UserId = request.UserId,
                DepartmentId = request.DepartmentId,
                DateOfSubmission = request.DateOfSubmission,
                SessionId = request.SessionId,
                StatusId = request.StatusId
            };

            foreach (var studentClass in requestViewModel.Classes)
            {
                requestViewModel.Classes.Add(new AssignedClassData
                {
                    ClassId = studentClass.ClassId,
                    ClassName = studentClass.ClassName,
                    Assigned = true
                });
            }

            return requestViewModel;
        }

        public static RequestsViewModel ToViewModel(this Request request, ICollection<StudentClass> allClasses)
        {
            var requestViewModel = new RequestsViewModel
            {
                RequestId = request.Id,
                ItemDescription = request.ItemDescription,
                ItemCost = request.ItemCost,
                ItemUsage = request.ItemUsage,
                Justification = request.Justification,
                NoOfStudentsImpacted = request.NoOfStudentsImpacted,
                UserId = request.UserId,
                DepartmentId = request.DepartmentId,
                DateOfSubmission = request.DateOfSubmission,
                SessionId = request.SessionId,
                StatusId = request.StatusId
            };

            // Collection for full list of courses with user's already assigned courses included
            ICollection<AssignedClassData> allAssignedClasses = new List<AssignedClassData>();
            foreach (var aClass in allClasses)
            {
                // Create new AssignedCourseData for each course and set Assigned = true if user already has course
                var assignedClass = new AssignedClassData
                {
                    ClassId = aClass.Id,
                    ClassName = aClass.Name,
                    Assigned = request.StudentClasses.FirstOrDefault(s => s.Id == aClass.Id) != null
                };

                allAssignedClasses.Add(assignedClass);
            }

            requestViewModel.Classes = allAssignedClasses;
            return requestViewModel;
        }

        public static Request ToDomainModel(this RequestsViewModel requestViewModel)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            // int sessionId = (db.Sessions.Where(s => s.IsActive == true).Select(s => s.SessionId)).Single();

            var request = new Request
            {
                ItemDescription = requestViewModel.ItemDescription,
                ItemUsage = requestViewModel.ItemUsage,
                ItemCost = requestViewModel.ItemCost,
                Justification = requestViewModel.Justification,
                NoOfStudentsImpacted = requestViewModel.NoOfStudentsImpacted,
                UserId = currentUser.Id,
                DepartmentId = currentUser.DepartmentId,
                DateOfSubmission = DateTime.Now,
                SessionId = 1,
                StatusId = 1
            };

            return request;
        }
    }
}