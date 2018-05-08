using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentsTechFeeEvalApp.Models.Model_Classes
{
    public class Request
    {
        public int Id { get; set; } // Id (Primary key)

        [Display(Name = "Description of Item: ")]
        public string ItemDescription { get; set; } // ItemDescription (length: 50)

        [Display(Name = "Cost of Item (with Shipping): ")]
        [Range(0, double.MaxValue, ErrorMessage = "Only positive amounts are allowed")]
        public decimal ItemCost { get; set; } // ItemCost

        [Display(Name = "Usage of Item: ")]
        public string ItemUsage { get; set; } // ItemUsage (length: 500)

        [Display(Name = "Justification for Item: ")]
        [DataType(DataType.MultilineText)]
        public string Justification { get; set; } // Justification (length: 500)

        [Display(Name = "Number of Students Impacted: ")]
        [Range(1, Int16.MaxValue, ErrorMessage = "Only positive integers are allowed")]
        public int NoOfStudentsImpacted { get; set; } // NoOfStudentsImpacted
        public DateTime DateOfSubmission { get; set; } // DateOfSubmission
        public string UserId { get; set; } // UserId (length: 50)
        public int DepartmentId { get; set; } // DepartmentId
        public int SessionId { get; set; } // SessionId
        public int StatusId { get; set; } // StatusId
        public bool? IsApprovedByDepartmentChair { get; set; } // IsApprovedByDepartmentChair
        public string DepartmentChairComment { get; set; } // DepartmentChairComment (length: 500)
        public string RankByDept { get; set; } // RankByDept (length: 50)
        public string CommitteeChairReview { get; set; } // CommitteeChairReview (length: 50)
        public string CommitteeChairComment { get; set; } // CommitteeChairComment (length: 500)
        public bool? IsApprovedByDean { get; set; } // IsApprovedByDean

        // Reverse navigation

        /// <summary>
        /// Child CommitteeMemberReviews where [CommitteeMemberReview].[RequestId] point to this entity (FK_CommitteeMemberReview_User)
        /// </summary>
        public virtual ICollection<CommitteeMemberReview> CommitteeMemberReviews { get; set; } // CommitteeMemberReview.FK_CommitteeMemberReview_User
        /// <summary>
        /// Child StudentClassRequests where [StudentClassRequest].[RequestId] point to this entity (FK_StudentClassRequest_Request)
        /// </summary>
        public virtual ICollection<StudentClass> StudentClasses { get; set; } // StudentClassRequest.FK_StudentClassRequest_Request

        // Foreign keys

        /// <summary>
        /// Parent ApplicationUser pointed by [Request].([UserId]) (FK_Request_User)
        /// </summary>
        public virtual ApplicationUser ApplicationUser { get; set; } // FK_Request_User

        /// <summary>
        /// Parent Department pointed by [Request].([DepartmentId]) (FK_Request_Department)
        /// </summary>
        public virtual Department Department { get; set; } // FK_Request_Department

        /// <summary>
        /// Parent Session pointed by [Request].([SessionId]) (FK_Request_Session)
        /// </summary>
        public virtual Session Session { get; set; } // FK_Request_Session

        /// <summary>
        /// Parent Status pointed by [Request].([StatusId]) (FK_Request_Status)
        /// </summary>
        public virtual Status Status { get; set; } // FK_Request_Status

        public Request()
        {
            CommitteeMemberReviews = new HashSet<CommitteeMemberReview>();
            StudentClasses = new HashSet<StudentClass>();
        }
    }

}