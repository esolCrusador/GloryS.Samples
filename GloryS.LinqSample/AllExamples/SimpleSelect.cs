using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GloryS.LinqSample.DAL;
using GloryS.LinqSample.DAL.DataEntities;

namespace GloryS.LinqSample.AllExamples
{
    public static class SimpleSelect
    {
        public static void ShowStudents(SchoolContext context)
        {
            var students = context.Students.ResolveSelect<Student, StudentModel>().ToList();

            Mapper.Register(new CourseModelMapper());
            var courses = context.Courses.ResolveSelectExternal<Course, CourseModel>().ToList();
        }

        public class StudentModel:ISelectExpression<Student, StudentModel>
        {
            public int StudentId { get; set; }

            public string StudentName { get; set; }
            public Expression<Func<Student, StudentModel>> GetSelectExpression()
            {
                return student => new StudentModel
                {
                    StudentId = student.ID,
                    StudentName = student.FirstMidName + " " + student.LastName
                };
            }
        }

        public class CourseModel
        {
            public int CourseId { get; set; }

            public string Title { get; set; }
        }

        public class CourseModelMapper:ISelectExpression<Course, CourseModel>
        {
            public Expression<Func<Course, CourseModel>> GetSelectExpression()
            {
                return course=>new CourseModel
                {
                    CourseId = course.CourseID,
                    Title = course.Title
                };
            }
        }
    }
}
