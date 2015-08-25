using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using GloryS.LinqSample.DAL.DataEntities;

namespace GloryS.LinqSample.Models
{
    public class CourseWithOldStudentsModel : CourseModel, ISelectExpression<Course, CourseWithOldStudentsModel>
    {
        public IEnumerable<StudentModel> Students { get; set; }
        public Expression<Func<Course, CourseWithOldStudentsModel>> GetSelectExpression()
        {
            Expression<Func<Course, CourseWithOldStudentsModel>> select = course => new CourseWithOldStudentsModel();

            select = select.InheritInit(Mapper.GetExternalExpression<Course, CourseModel>());
            select = select.AddMemberInit(
                course => course.Enrollments.Select(e => e.Student).Where(s => SqlFunctions.DateDiff("year", s.EnrollmentDate, DateTime.Now) > 12),
                course => course.Students,
                Mapper.GetExternalExpression<Student, StudentModel>()
                );

            return select;
        }

        public override string ToString()
        {
            return base.ToString() + " Students:\r\n" + String.Join("\r\n", Students.Select(s => s.ToString()));
        }
    }
}
