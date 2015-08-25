using System;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using GloryS.LinqSample.DAL;
using GloryS.LinqSample.DAL.DataEntities;

namespace GloryS.LinqSample.AllExamples
{
    public static class SelectNonCache
    {
        public static void ShowStudents(SchoolContext context)
        {
            Mapper.Register(new StudentModelMapper());
            var students = context.Students.ResolveSelectExternal<Student, StudentModel>().ToList();
        }

        public class StudentModel
        {
            public int StudentId { get; set; }
            public string StudentName { get; set; }
            public int? MinutesAfterEnrollment { get; set; }
        }

        public class StudentModelMapper : ISelectExpressionNonCache<Student, StudentModel>
        {

            /// <summary>
            /// We need to use ISelectExpressionNonCache because DateTime.Now is changed and expression will give different results, so it requires to be rebuild every time.
            /// </summary>
            public Expression<Func<Student, StudentModel>> GetSelectExpression()
            {
                DateTime now = DateTime.Now;

                return student => new StudentModel
                {
                    StudentId = student.ID,
                    StudentName = student.FirstMidName + " " + student.LastName,
                    MinutesAfterEnrollment = SqlFunctions.DateDiff("minute", student.EnrollmentDate, now)
                };
            }
        }
    }
}
