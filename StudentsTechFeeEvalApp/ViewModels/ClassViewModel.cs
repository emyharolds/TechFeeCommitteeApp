using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsTechFeeEvalApp.ViewModels
{
    public class ClassViewModel
    {
        public int Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name (length: 100)
        public int DepartmentId { get; set; } // DepartmentId
    }
}