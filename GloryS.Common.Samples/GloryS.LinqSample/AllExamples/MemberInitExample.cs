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
    public static class MemberInitExample
    {
        public static void ShowStudents(SchoolContext context)
        {
            // Reuse member init of single member.
            var enrollments = context.Enrollments.ResolveSelect<Enrollment, EnrollmentModel>().ToList();

            // Reurse member init of IEnumerable of members.
            var studentsWithCourses = context.Students.ResolveSelect<Student, StudentWithCoursesModel>().ToList();
        }

        public class EnrollmentModel: ISelectExpression<Enrollment, EnrollmentModel>
        {
            public int EnrollmentId { get; set; }
            public Grade? Grade { get; set; }

            public StudentBaseModel Student { get; set; }

            public CourseBaseModel Course { get; set; }

            public Expression<Func<Enrollment, EnrollmentModel>> GetSelectExpression()
            {
                Expression<Func<Enrollment, EnrollmentModel>> select = enrollment =>
                    new EnrollmentModel
                    {
                        EnrollmentId = enrollment.EnrollmentID,
                        Grade = enrollment.Grade
                    };

                //select = select.AddMemberInit(entollment => entollment.Student, enrollmentModel => enrollmentModel.Student, Mapper.GetExternalExpression<Student, StudentBaseModel>());
                // If StudentBaseModel implements ISelectExpression<Student, StudentBaseModel> than it can be resolved without explicit getting of select expression.
                select = select.ResolveMemberInit(entollment => entollment.Student, enrollmentModel => enrollmentModel.Student);

                //select = select.AddMemberInit(enrollment => enrollment.Course, enrollmentModel => enrollmentModel.Course, Mapper.GetExternalExpression<Course, CourseBaseModel>());
                select = select.ResolveMemberInit(enrollment => enrollment.Course, enrollmentModel => enrollmentModel.Course);

                return select;
            }
        }

        public class StudentBaseModel: ISelectExpression<Student, StudentBaseModel>
        {
            public int StudentId { get; set; }

            public string FullName { get; set; }
            
            Expression<Func<Student, StudentBaseModel>> ISelectExpression<Student, StudentBaseModel>.GetSelectExpression()
            {
                return student => new StudentBaseModel
                {
                    StudentId = student.ID,
                    FullName = student.FirstMidName + " " + student.LastName
                };
            }
        }

        public class CourseBaseModel: ISelectExpression<Course, CourseBaseModel>
        {
            public int CourseId { get; set; }

            public string CourseName { get; set; }

            public int Credits { get; set; }
            public Expression<Func<Course, CourseBaseModel>> GetSelectExpression()
            {
                return course=>new CourseBaseModel
                {
                    CourseId = course.CourseID,
                    CourseName = course.Title,
                    Credits = course.Credits
                };
            }
        }

        public class StudentWithCoursesModel:StudentBaseModel, ISelectExpression<Student, StudentWithCoursesModel>
        {
            public IEnumerable<CourseBaseModel> Courses { get; set; }

            public Grade? MaxGade { get; set; }

            Expression<Func<Student, StudentWithCoursesModel>> ISelectExpression<Student, StudentWithCoursesModel>.GetSelectExpression()
            {
                Expression<Func<Student, StudentWithCoursesModel>> select = student => 
                    new StudentWithCoursesModel
                    {
                        MaxGade = student.Enrollments.Max(e=>e.Grade)
                    };

                select = select.InheritInit(Mapper.GetExpression<Student, StudentBaseModel>());
                select = select.AddMemberInit(student => student.Enrollments.Select(enrollment => enrollment.Course), courseModel => courseModel.Courses, Mapper.GetExpression<Course, CourseBaseModel>());

                return select;
            }
        }
    }
}
