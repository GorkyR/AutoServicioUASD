using System.Collections.Generic;

namespace UASD
{
    using Utilities;
    public class Course
    {
        public enum CourseType { Theory, Laboratory }

        protected string title;
        public string            Title     { get => title; set => title = Convert.SimpleEncodeFix(value); }
        public string            Code      { get; set; }
        public string            Section   { get; set; }
        public string            NRC       { get; set; }
        public string            Professor { get; set; }
        public CourseType        Type      { get; set; } = CourseType.Theory;
        public int               Credits   { get; set; }
        public List<CourseClass> Schedule  { get; set; } = new List<CourseClass>();

        public bool CollidesWith(Course b)
        {
            foreach (CourseClass classA in this.Schedule)
                foreach (CourseClass classB in b.Schedule)
                    if (classA.CollidesWith(classB))
                        return true;
            return false;
        }

        public override string ToString() => $"{Title} - {Code}{(Section != null ? $" - {Section}" : "")}";
    }
}
