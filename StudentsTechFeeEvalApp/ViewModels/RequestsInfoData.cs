using StudentsTechFeeEvalApp.Models.Model_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsTechFeeEvalApp.ViewModels
{
    public class RequestsInfoData
    {
        public IEnumerable<StudentClass> Classes { get; set; }
        public IEnumerable<CommitteeMemberReview> CommitteeMemberReviews { get; set; }
        public IEnumerable<Request> Requests { get; set; }
        public IEnumerable<Period> Periods { get; set; }
        public IEnumerable<RequestsViewModel> ViewModels { get; set; }
    }
}