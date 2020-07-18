namespace UASD
{
    public class GradedCourse : Course
    {
        public enum CourseState { Published, NotPublished, Absent }

        public int Grade { get; set; }
        public CourseState State { get; set; } = CourseState.Published;

        public override string ToString()
        {
            return string.Format("{0} - {1} - {2}: [{3}]", Title, Code, Section, State == CourseState.Absent ? "AUS" : Grade.ToString());
        }
    }
}
