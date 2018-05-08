using System.ComponentModel.DataAnnotations.Schema;

namespace StudentsTechFeeEvalApp.Models.Model_Classes
{
    //public enum Vote
    //{
    //    [DescriptionAttribute("Yes")]
    //    Yes,
    //    [DescriptionAttribute("No")]
    //    No,
    //    [DescriptionAttribute("Yes, with reservation")]
    //    YesWithReservation,
    //    [DescriptionAttribute("Need more information")]
    //    NeedMoreInformation
    //}
    public class CommitteeMemberReview
    {
        public int Id { get; set; } // Id (Primary key)
        public string Vote { get; set; } // Vote (length: 50)
        public string Comment { get; set; } // Comment (length: 500)

        public bool IsReviewed { get; set; }

        [Index("IX_FirstAndSecond", 1, IsUnique = true)]
        public string UserId { get; set; } // User_Id (length: 50)

        [Index("IX_FirstAndSecond", 2, IsUnique = true)]
        public int RequestId { get; set; } // RequestId

        // Foreign keys

        /// <summary>
        /// Parent ApplicationUser pointed by [CommitteeMemberReview].([UserId]) (FK_CommitteeMemberReview_Request)
        /// </summary>
        public virtual ApplicationUser ApplicationUser { get; set; } // FK_CommitteeMemberReview_Request

        /// <summary>
        /// Parent Request pointed by [CommitteeMemberReview].([RequestId]) (FK_CommitteeMemberReview_User)
        /// </summary>
        public virtual Request Request { get; set; } // FK_CommitteeMemberReview_User
    }

}