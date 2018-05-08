using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsTechFeeEvalApp.Models.Model_Classes
{
    public class Department
    {
        public int Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name (length: 50)

        // Reverse navigation

        /// <summary>
        /// Child ApplicationUsers where [ApplicationUser].[DepartmentId] point to this entity (FK_User_Department)
        /// </summary>
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; } // ApplicationUser.FK_User_Department
        /// <summary>
        /// Child Requests where [Request].[DepartmentId] point to this entity (FK_Request_Department)
        /// </summary>
        public virtual ICollection<Request> Requests { get; set; } // Request.FK_Request_Department
        /// <summary>
        /// Child StudentClasses where [StudentClass].[DepartmentId] point to this entity (FK_StudentClass_Department)
        /// </summary>
        //public virtual ICollection<StudentClass> StudentClasses { get; set; } // StudentClass.FK_StudentClass_Department

        public Department()
        {
            ApplicationUsers = new HashSet<ApplicationUser>();
            Requests = new HashSet<Request>();
            //StudentClasses = new HashSet<StudentClass>();
        }
    }
}