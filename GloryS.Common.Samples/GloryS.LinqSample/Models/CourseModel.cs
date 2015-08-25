using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GloryS.LinqSample.DAL.DataEntities;

namespace GloryS.LinqSample.Models
{
    public class CourseModel:ISelectExpression<Course, CourseModel>
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }

        Expression<Func<Course, CourseModel>> ISelectExpression<Course, CourseModel>.GetSelectExpression()
        {
            return course => new CourseModel
            {
                CourseId = course.CourseID,
                Title = course.Title,
                Credits = course.Credits
            };
        }

        public override string ToString()
        {
            return String.Format("Course ({0}) {1}, has {2} credits.", CourseId, Title, Credits);
        }
    }
}
