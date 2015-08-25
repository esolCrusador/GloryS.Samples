using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using GloryS.LinqSample.DAL;
using GloryS.LinqSample.DAL.DataEntities;

namespace GloryS.LinqSample.AllExamples
{
    public static class InitInheritanceExample
    {
        public static void ShowCourses(SchoolContext context)
        {
            //Initialization of course base models.
            var basceCourses = context.Courses.ResolveSelect<Course, CourseBaseModel>().ToList();

            //Initialization of course with inherited course model.
            var courses = context.Courses.ResolveSelect<Course, CourseModel>().ToList();

            //Initialization of cross entities inherited model.
            var localizedCourses = context.CourseRes.ResolveSelect<CourseRes, LocalizedCourseModel>().ToList();
        }

        public class CourseBaseModel : ISelectExpression<Course, CourseBaseModel>
        {
            public int CourseId { get; set; }
            public string Name { get; set; }
            Expression<Func<Course, CourseBaseModel>> ISelectExpression<Course, CourseBaseModel>.GetSelectExpression()
            {
                return course => new CourseBaseModel
                {
                    CourseId = course.CourseID,
                    Name = course.Title + "!"
                };
            }
        }

        public class CourseModel : CourseBaseModel, ISelectExpression<Course, CourseModel>
        {
            public int EnrollmentsCount { get; set; }

            Expression<Func<Course, CourseModel>> ISelectExpression<Course, CourseModel>.GetSelectExpression()
            {
                Expression<Func<Course, CourseModel>> select = course => new CourseModel
                {
                    Name = course.Title,
                    EnrollmentsCount = course.Enrollments.Count
                };

                select = select.InheritInit(Mapper.GetExpression<Course, CourseBaseModel>());

                return select;
            }
        }

        public class LocalizedCourseModel : CourseModel, ISelectExpression<CourseRes, LocalizedCourseModel>
        {
            public Culture Culture { get; set; }

            Expression<Func<CourseRes, LocalizedCourseModel>> ISelectExpression<CourseRes, LocalizedCourseModel>.GetSelectExpression()
            {
                Expression<Func<CourseRes, LocalizedCourseModel>> select = course => new LocalizedCourseModel
                {
                    Name = course.Title,
                    Culture = course.Culture
                };

                select = select.InheritInit(course => course.Course, Mapper.GetExpression<Course, CourseModel>());

                return select;
            }
        }
    }
}
