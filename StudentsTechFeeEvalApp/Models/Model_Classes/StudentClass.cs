using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsTechFeeEvalApp.Models.Model_Classes
{
    public class StudentClass
    {
        public int Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name (length: 100)
        //public int DepartmentId { get; set; } // DepartmentId

        // Reverse navigation

        /// <summary>
        /// Child StudentClassRequests where [StudentClassRequest].[StudentClassId] point to this entity (FK_StudentClassRequest_StudentClass)
        /// </summary>
        public virtual ICollection<Request> Requests { get; set; } // StudentClassRequest.FK_StudentClassRequest_StudentClass

        // Foreign keys

        /// <summary>
        /// Parent Department pointed by [StudentClass].([DepartmentId]) (FK_StudentClass_Department)
        /// </summary>
        //public virtual Department Department { get; set; } // FK_StudentClass_Department

        public StudentClass()
        {
            Requests = new HashSet<Request>();
        }
    }
}