using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsTechFeeEvalApp.Models.Model_Classes
{
    public class Status
    {
        public int Id { get; set; } // Id (Primary key)
        public string StatusName { get; set; } // StatusName (length: 50)

        // Reverse navigation

        /// <summary>
        /// Child Requests where [Request].[StatusId] point to this entity (FK_Request_Status)
        /// </summary>
        public virtual ICollection<Request> Requests { get; set; } // Request.FK_Request_Status

        public Status()
        {
            Requests = new HashSet<Request>();
        }
    }
}