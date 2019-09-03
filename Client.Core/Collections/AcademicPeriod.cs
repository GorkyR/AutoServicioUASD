using System.Linq;
using System.Collections.Generic;

namespace UASD
{
    public class AcademicPeriod
    {
        public string Title { get; set; }
        public float Index
        {
            get
            {
                float indice = 0, creditos = 0;
                foreach (CourseGrade m in this.Courses.Where(m => (m as CourseGrade).State == 0))
                {
                        indice += m.Grade * m.Credits;
                        creditos += m.Credits;
                }
                return indice / creditos;
            }
        }
        public int Credits
        {
            get
            {
                int creds = 0;
                foreach (CourseGrade m in this.Courses.Where(m => (m as CourseGrade).State == 0))
                        creds += m.Credits;
                return creds;
            }
        }
        public bool IsActive {
            get =>
                Courses.Any(c => ((CourseGrade)c).State == CourseGrade.CourseState.NotPublished);
        }
        public bool IsSummerCourse { get; set; }
        public CourseCollection Courses { get; set; }

        public AcademicPeriod() { this.Courses = new CourseCollection(); }
        public AcademicPeriod(string desc):this() { this.Title = desc; }

        public override string ToString() => $"{Title} ({Credits} cred.) - [{Index}]";
    }
}
