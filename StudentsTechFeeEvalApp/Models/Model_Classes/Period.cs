using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsTechFeeEvalApp.Models.Model_Classes
{
    public class Period
    {
        public int Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name (length: 50)
        public bool IsActive { get; set; } // IsActive
    }
}