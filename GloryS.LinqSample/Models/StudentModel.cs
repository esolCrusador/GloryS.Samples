using System;
using System.Linq.Expressions;
using GloryS.LinqSample.DAL.DataEntities;

namespace GloryS.LinqSample.Models
{
    public class StudentModel:ISelectExpression<Student, StudentModel>
    {
        public int StudentId { get; set; }
        public string FirstMidName { get; set; }
        public string LastName { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public Expression<Func<Student, StudentModel>> GetSelectExpression()
        {
            return student => new StudentModel
            {
                StudentId = student.ID,
                FirstMidName = student.FirstMidName,
                LastName = student.LastName
            };
        }

        public override string ToString()
        {
            return String.Format("Student ({0}) {1} {2} was enrolled {3:dd.mm.yy}", StudentId, FirstMidName, LastName, EnrollmentDate);
        }
    }
}
