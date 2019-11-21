using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICBackend.Models
{
    public class StudentAdd
    {
        public int? StudentID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public Nullable<System.DateTime> EnrollmentDate { get; set; }
        public string MiddleName { get; set; }
        //public string RowVersion { get; set; }

    }
}