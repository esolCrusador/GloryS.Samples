using System;
using System.Linq.Expressions;
using GloryS.LinqSample.DAL.DataEntities;

namespace GloryS.LinqSample.Models
{
    public class CourseEnrollmentModel : EnrollmentBaseModel, ISelectExpression<Enrollment, CourseEnrollmentModel>
    {
        public StudentModel Student { get; set; }

        Expression<Func<Enrollment, CourseEnrollmentModel>> ISelectExpression<Enrollment, CourseEnrollmentModel>.GetSelectExpression()
        {
            Expression<Func<Enrollment, CourseEnrollmentModel>> select = enrollment => new CourseEnrollmentModel
            {

            };

            select = select.InheritInit(Mapper.GetExternalExpression<Enrollment, EnrollmentBaseModel>());
            select = select.AddMemberInit(enrollment => enrollment.Student, enrollment => enrollment.Student, Mapper.GetExternalExpression<Student, StudentModel>());

            return select;
        }

        public override string ToString()
        {
            return base.ToString() + " for student>>>\r\n" + Student.ToString();
        }
    }
}
