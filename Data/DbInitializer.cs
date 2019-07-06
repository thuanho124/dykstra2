using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Data
{
    public class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Students.Any())
            {
                return;   // DB has been seeded
            }

            var students = new Student[]
            {
                    new Student{FirstName="Carson",LastName="Alexander",EmailAddress="caralex2@uw.edu",YearRank ="Junior", AverageGrade = 3.4f, EnrollmentDate=DateTime.Parse("2005-09-01")},
                    new Student{FirstName="Meredith",LastName="Alonso",EmailAddress="mealo13@uw.edu",YearRank ="Junior", AverageGrade = 2.9f, EnrollmentDate=DateTime.Parse("2002-09-01")},
                    new Student{FirstName="Arturo",LastName="Anand",EmailAddress="anaart9@uw.edu",YearRank ="Sophomore", AverageGrade = 3.9f, EnrollmentDate=DateTime.Parse("2003-09-01")},
                    new Student{FirstName="Gytis",LastName="Barzdukas",EmailAddress="barzd4@uw.edu",YearRank ="Freshmen", AverageGrade = 3.5f, EnrollmentDate=DateTime.Parse("2002-09-01")},
                    new Student{FirstName="Yan",LastName="Li",EmailAddress="liyan8@uw.edu",YearRank ="Senior", AverageGrade = 2.3f, EnrollmentDate=DateTime.Parse("2002-09-01")},
                    new Student{FirstName="Peggy",LastName="Justice",EmailAddress="peggy2@uw.edu",YearRank ="Junior", AverageGrade = 3.2f, EnrollmentDate=DateTime.Parse("2001-09-01")},
                    new Student{FirstName="Laura",LastName="Norman",EmailAddress="norlau10@uw.edu",YearRank ="Freshmen", AverageGrade = 3.1f, EnrollmentDate=DateTime.Parse("2003-09-01")},
                    new Student{FirstName="Nino",LastName="Olivetto",EmailAddress="olive6@uw.edu",YearRank ="Senior", AverageGrade = 4.0f, EnrollmentDate=DateTime.Parse("2005-09-01")}
            };
            foreach (Student s in students)
            {
                context.Students.Add(s);
            }
            context.SaveChanges();

            var courses = new Course[]
            {
            new Course{CourseID=1050,Title="Chemistry",Credits=3, RoomNum="BHS 409"},
            new Course{CourseID=4022,Title="Microeconomics",Credits=3, RoomNum="CP 503"},
            new Course{CourseID=4041,Title="Macroeconomics",Credits=3, RoomNum="WG 689"},
            new Course{CourseID=1045,Title="Calculus",Credits=4, RoomNum="GWP 110"},
            new Course{CourseID=3141,Title="Trigonometry",Credits=4, RoomNum="MAT 941"},
            new Course{CourseID=2021,Title="Composition",Credits=3, RoomNum="WCG 001"},
            new Course{CourseID=2042,Title="Literature",Credits=4, RoomNum="BB 276"}
            };
            foreach (Course c in courses)
            {
                context.Courses.Add(c);
            }
            context.SaveChanges();

            var enrollments = new Enrollment[]
            {
            new Enrollment{StudentID=1,CourseID=1050,Grade=Grade.A},
            new Enrollment{StudentID=1,CourseID=4022,Grade=Grade.C},
            new Enrollment{StudentID=1,CourseID=4041,Grade=Grade.B},
            new Enrollment{StudentID=2,CourseID=1045,Grade=Grade.B},
            new Enrollment{StudentID=2,CourseID=3141,Grade=Grade.F},
            new Enrollment{StudentID=2,CourseID=2021,Grade=Grade.F},
            new Enrollment{StudentID=3,CourseID=1050},
            new Enrollment{StudentID=4,CourseID=1050},
            new Enrollment{StudentID=4,CourseID=4022,Grade=Grade.F},
            new Enrollment{StudentID=5,CourseID=4041,Grade=Grade.C},
            new Enrollment{StudentID=6,CourseID=1045},
            new Enrollment{StudentID=7,CourseID=3141,Grade=Grade.A},
            };
            foreach (Enrollment e in enrollments)
            {
                context.Enrollments.Add(e);
            }
            context.SaveChanges();
        }
    }
}