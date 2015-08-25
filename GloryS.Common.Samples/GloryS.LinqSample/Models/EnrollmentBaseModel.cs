using System;
using System.Linq.Expressions;
using GloryS.LinqSample.DAL.DataEntities;

namespace GloryS.LinqSample.Models
{
    public class EnrollmentBaseModel: ISelectExpression<Enrollment, EnrollmentBaseModel>
    {
        public int EnrollmentId { get; set; }
        public Grade? Grade { get; set; }
        Expression<Func<Enrollment, EnrollmentBaseModel>> ISelectExpression<Enrollment, EnrollmentBaseModel>.GetSelectExpression()
        {
            return enrollment => new EnrollmentBaseModel
            {
                EnrollmentId = enrollment.EnrollmentID,
                Grade = enrollment.Grade,
            };
        }

        public override string ToString()
        {
            return String.Format("Enrollment ({0}) ({1})", EnrollmentId, Grade.ToString());
        }
    }
}
