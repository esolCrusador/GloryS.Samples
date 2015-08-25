using System;
using System.Collections.Generic;

namespace GloryS.LinqSample.DAL.DataEntities
{
    public class Student
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }

        public override string ToString()
        {
            return String.Format("{{ID: {0}, FirstMidName: {1}, LastName: {2}, EnrollmentDate: {3:dd.MM.yy}}}", ID, FirstMidName, LastName, EnrollmentDate);
        }
    }
}
