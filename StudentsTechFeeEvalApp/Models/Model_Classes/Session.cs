using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsTechFeeEvalApp.Models.Model_Classes
{
    public class Session
    {
        public int Id { get; set; } // Id (Primary key)
        public string Semester { get; set; } // Semester (length: 50)
        public DateTime Year { get; set; } // Year

        // Reverse navigation

        /// <summary>
        /// Child Requests where [Request].[SessionId] point to this entity (FK_Request_Session)
        /// </summary>
        public virtual ICollection<Request> Requests { get; set; } // Request.FK_Request_Session

        public Session()
        {
            Requests = new HashSet<Request>();
        }
    }
}