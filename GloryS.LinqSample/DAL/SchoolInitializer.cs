using System;
using System.Collections.Generic;
using System.Globalization;
using GloryS.LinqSample.DAL.DataEntities;

namespace GloryS.LinqSample.DAL
{
    public class SchoolInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SchoolContext>
    {
        protected override void Seed(SchoolContext context)
        {
            var students = new List<Student>
            {
            new Student{FirstMidName="Carson",LastName="Alexander",EnrollmentDate=DateTime.Parse("2005-09-01")},
            new Student{FirstMidName="Meredith",LastName="Alonso",EnrollmentDate=DateTime.Parse("2002-09-01")},
            new Student{FirstMidName="Arturo",LastName="Anand",EnrollmentDate=DateTime.Parse("2003-09-01")},
            new Student{FirstMidName="Gytis",LastName="Barzdukas",EnrollmentDate=DateTime.Parse("2002-09-01")},
            new Student{FirstMidName="Yan",LastName="Li",EnrollmentDate=DateTime.Parse("2002-09-01")},
            new Student{FirstMidName="Peggy",LastName="Justice",EnrollmentDate=DateTime.Parse("2001-09-01")},
            new Student{FirstMidName="Laura",LastName="Norman",EnrollmentDate=DateTime.Parse("2003-09-01")},
            new Student{FirstMidName="Nino",LastName="Olivetto",EnrollmentDate=DateTime.Parse("2005-09-01")}
            };

            students.ForEach(s => context.Students.Add(s));
            context.SaveChanges();
            var courses = new List<Course>
            {
            new Course{CourseID=1050,Title="Chemistry",Credits=3,},
            new Course{CourseID=4022,Title="Microeconomics",Credits=3,},
            new Course{CourseID=4041,Title="Macroeconomics",Credits=3,},
            new Course{CourseID=1045,Title="Calculus",Credits=4,},
            new Course{CourseID=3141,Title="Trigonometry",Credits=4,},
            new Course{CourseID=2021,Title="Composition",Credits=3,},
            new Course{CourseID=2042,Title="Literature",Credits=4,}
            };

            courses.ForEach(s => context.Courses.Add(s));
            context.SaveChanges();

            var coursesRes = new List<CourseRes>
            {
                new CourseRes {CourseID = 1050, Culture = Culture.Default, Title = "Chemistry"},
                new CourseRes {CourseID = 1050, Culture = Culture.RU, Title = "Химия"},
                new CourseRes {CourseID = 1050, Culture = Culture.DE, Title = "Chemie"},
                new CourseRes {CourseID = 1050, Culture = Culture.ES, Title = "Química"},

                new CourseRes {CourseID = 4022, Culture = Culture.Default, Title = "Microeconomics"},
                new CourseRes {CourseID = 4022, Culture = Culture.RU, Title = "Микроэкономика"},
                new CourseRes {CourseID = 4022, Culture = Culture.DE, Title = "Mikroökonomie"},
                new CourseRes {CourseID = 4022, Culture = Culture.ES, Title = "Microeconomía"},

                new CourseRes {CourseID = 4041, Culture = Culture.Default, Title = "Macroeconomics"},
                new CourseRes {CourseID = 4041, Culture = Culture.RU, Title = "Макроэкономика"},
                new CourseRes {CourseID = 4041, Culture = Culture.DE, Title = "Makroökonomie"},
                new CourseRes {CourseID = 4041, Culture = Culture.ES, Title = "Macroeconomía"},

                new CourseRes {CourseID = 1045, Culture = Culture.Default, Title = "Calculus"},
                new CourseRes {CourseID = 1045, Culture = Culture.RU, Title = "Арифметика"},
                new CourseRes {CourseID = 1045, Culture = Culture.DE, Title = "Calculus"},
                new CourseRes {CourseID = 1045, Culture = Culture.ES, Title = "Cálculo"},

                new CourseRes {CourseID = 3141, Culture = Culture.Default, Title = "Trigonometry"},
                new CourseRes {CourseID = 3141, Culture = Culture.RU, Title = "Тригонометрия"},
                new CourseRes {CourseID = 3141, Culture = Culture.DE, Title = "Trigonometrie"},
                new CourseRes {CourseID = 3141, Culture = Culture.ES, Title = "Trigonometría"},

                new CourseRes {CourseID = 2021, Culture = Culture.Default, Title = "Composition"},
                new CourseRes {CourseID = 2021, Culture = Culture.RU, Title = "Оформление работ"},
                new CourseRes {CourseID = 2021, Culture = Culture.DE, Title = "Zusammensetzung"},
                new CourseRes {CourseID = 2021, Culture = Culture.ES, Title = "Composición"},

                new CourseRes {CourseID = 2042, Culture = Culture.Default, Title = "Literature"},
                new CourseRes {CourseID = 2042, Culture = Culture.RU, Title = "Литература"},
                new CourseRes {CourseID = 2042, Culture = Culture.DE, Title = "Fachliteratur"},
                new CourseRes {CourseID = 2042, Culture = Culture.ES, Title = "Bibliografía"}
            };

            coursesRes.ForEach(s=>context.CourseRes.Add(s));
            context.SaveChanges();

            var enrollments = new List<Enrollment>
            {
            new Enrollment{StudentID=1,CourseID=1050,Grade=Grade.A},
            new Enrollment{StudentID=1,CourseID=4022,Grade=Grade.C},
            new Enrollment{StudentID=1,CourseID=4041,Grade=Grade.B},
            new Enrollment{StudentID=2,CourseID=1045,Grade=Grade.B},
            new Enrollment{StudentID=2,CourseID=3141,Grade=Grade.F},
            new Enrollment{StudentID=2,CourseID=2021,Grade=Grade.F},
            new Enrollment{StudentID=3,CourseID=1050},
            new Enrollment{StudentID=4,CourseID=1050,},
            new Enrollment{StudentID=4,CourseID=4022,Grade=Grade.F},
            new Enrollment{StudentID=5,CourseID=4041,Grade=Grade.C},
            new Enrollment{StudentID=6,CourseID=1045},
            new Enrollment{StudentID=7,CourseID=3141,Grade=Grade.A},
            };
            enrollments.ForEach(s => context.Enrollments.Add(s));
            context.SaveChanges();
        }
    }
}
