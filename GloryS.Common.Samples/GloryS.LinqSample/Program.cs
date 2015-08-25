using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GloryS.LinqSample.AllExamples;
using GloryS.LinqSample.DAL;
using GloryS.LinqSample.DAL.DataEntities;
using GloryS.LinqSample.Models;

namespace GloryS.LinqSample
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.GetFullPath("..\\..\\App_Data"));

            using (SchoolContext ctx = new SchoolContext())
            {
                HowWeDid(ctx);

                HowWeDo(ctx);

                SimpleSelect.ShowStudents(ctx);
                SelectNonCache.ShowStudents(ctx);
                InitInheritanceExample.ShowCourses(ctx);
                MemberInitExample.ShowStudents(ctx);
                CultureResolveExample.ShowCourses(ctx);
                FilterExpression.FilterStudents(ctx);
            }

            Console.ReadKey();
        }

        private static void HowWeDid(SchoolContext ctx)
        {
            var students = ctx.Students.Select(s => new StudentModel
            {
                StudentId = s.ID,
                FirstMidName = s.FirstMidName,
                LastName = s.LastName,
                EnrollmentDate = s.EnrollmentDate
            })
            .ToList();

            Console.WriteLine(GetResultsString("Students", students));

            //Reuse
            var studentsInOtherController = ctx.Students.Where(s => SqlFunctions.DateDiff("year", s.EnrollmentDate, DateTime.Now) < 12)
                .Select(s => new StudentModel
                {
                    StudentId = s.ID,
                    FirstMidName = s.FirstMidName,
                    LastName = s.LastName,
                    EnrollmentDate = s.EnrollmentDate
                })
                .ToList();
            Console.WriteLine(GetResultsString("Other Students", studentsInOtherController));

            Expression<Func<Enrollment, EnrollmentBaseModel>> enrollmentSelect = enrollment => new EnrollmentBaseModel
            {
                EnrollmentId = enrollment.EnrollmentID,
                Grade = enrollment.Grade
            };

            var enrollments1 = ctx.Enrollments.Select(enrollmentSelect).ToList();
            Console.WriteLine(GetResultsString("Enrollment1", enrollments1));

            var enrollments2 = ctx.Enrollments.Where(e => e.Grade != null).Select(enrollmentSelect).ToList();
            Console.WriteLine(GetResultsString("Enrollment2", enrollments2));

            var courses = ctx.Courses.Select(course => new CourseFullModel
            {
                CourseId = course.CourseID,
                Title = course.Title,
                Credits = course.Credits,
                Enrollments = course.Enrollments.Select(
                    enrollment => new CourseEnrollmentModel
                    {
                        EnrollmentId = enrollment.EnrollmentID,
                        Grade = enrollment.Grade,
                        Student = new StudentModel
                        {
                            FirstMidName = enrollment.Student.FirstMidName,
                            LastName = enrollment.Student.LastName,
                            EnrollmentDate = enrollment.Student.EnrollmentDate,
                            StudentId = enrollment.Student.ID
                        }
                    })
            });
            Console.WriteLine(GetResultsString("Courses", courses));
        }
        private static void HowWeDo(SchoolContext ctx)
        {
            var students = ctx.Students.ResolveSelect<Student, StudentModel>().ToList();
            Console.WriteLine(GetResultsString("Students", students));

            var studentsInOtherController = ctx.Students.Where(s => SqlFunctions.DateDiff("year", s.EnrollmentDate, DateTime.Now) < 12)
                .ResolveSelectExternal<Student, StudentModel>();
            Console.WriteLine(GetResultsString("Other Students", studentsInOtherController));

            var enrollments1 = ctx.Enrollments.ResolveSelect<Enrollment, EnrollmentBaseModel>().ToList();
            Console.WriteLine(GetResultsString("Enrollment1", enrollments1));

            var enrollments2 = ctx.Enrollments.Where(e => e.Grade != null).ResolveSelectExternal<Enrollment, EnrollmentBaseModel>().ToList();
            Console.WriteLine(GetResultsString("Enrollment2", enrollments2));

            var courses = ctx.Courses.ResolveSelectExternal<Course, CourseFullModel>().ToList();
            Console.WriteLine(GetResultsString("Courses", courses));

            var courseWithStudents = ctx.Courses.ResolveSelectExternal<Course, CourseWithOldStudentsModel>().ToList();
            Console.WriteLine(GetResultsString("Courses with students", courseWithStudents));
        }
        static string GetResultsString(string name, IEnumerable<object> results)
        {
            return String.Format(
                "{0} results:\r\n{1}",
                name,
                String.Join("\r\n", results.Select(r => r.ToString()))
                );
        }
    }
}
