using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GloryS.LinqSample.DAL;
using GloryS.LinqSample.DAL.DataEntities;

namespace GloryS.LinqSample.AllExamples
{
    public static class FilterExpression
    {
        public static void FilterStudents(SchoolContext ctx)
        {
            var studentsFilter = new StudentFilter
            {
                ContainsInName = new[] { "a", "b"},
                EnrollmentMinDate = new DateTime(2002, 1, 1),
                MinCourses = 1,
                MaxCourses = 3
            };

            Mapper.Register(new StudentModelMapper());
            //GetFilter() is extension method for IFilterExpression<Student> interface and it combines all expression into one using &&.
            var students = ctx.Students.Where(studentsFilter.GetFilter()).ResolveSelectExternal<Student, StudentModel>().ToList();
        }

        public class StudentModel
        {
            public int StudentId { get; set; }

            public string FullName { get; set; }
        }

        public class StudentModelMapper: ISelectExpression<Student, StudentModel>
        {
            public Expression<Func<Student, StudentModel>> GetSelectExpression()
            {
                return student => new StudentModel
                {
                    StudentId = student.ID,
                    FullName = student.FirstMidName + " " + student.LastName
                };
            }
        }

        public class StudentFilter: IFilterExpression<Student>
        {
            public string[] ContainsInName { get; set; }

            public DateTime? EnrollmentMinDate { get; set; }

            public int? MinCourses { get; set; }

            public int? MaxCourses { get; set; }

            public IEnumerable<Expression<Func<Student, bool>>> GetFilterExpressions()
            {
                //FirstName or LastName should contains any of symbols from ContainsInName.
                if (ContainsInName != null)
                {
                    var containesFilters = ContainsInName.Select(c =>
                    {
                        Expression<Func<Student, bool>> filter = student => student.FirstMidName.Contains(c) || student.LastName.Contains(c);

                        return filter;
                    });

                    //Combining all compare segments with ||
                    yield return containesFilters.Combine(Expression.OrElse);

                    //It's the same expression:
                    //yield return containesFilters.Combine((f1, f2) => f1 || f2);
                }

                if (EnrollmentMinDate.HasValue)
                {
                    yield return student => student.EnrollmentDate >= EnrollmentMinDate.Value;
                }

                if (MinCourses.HasValue)
                {
                    yield return student => student.Enrollments.Count >= MinCourses.Value;
                }

                if (MaxCourses.HasValue)
                {
                    yield return student => student.Enrollments.Count <= MaxCourses.Value;
                }
            }
        }
    }
}
