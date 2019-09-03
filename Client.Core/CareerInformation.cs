namespace UASD
{
    public class CareerInformation
    {
        public string Name            { get; set; }
        public int    RequiredCredits { get; set; }
        public int    Credits         { get; set; }
        public int    RequiredCourses { get; set; }
        public int    Courses         { get; set; }
        public float  CreditsPercentage => (float)Credits * 100 / (float)RequiredCredits;
        public float  CoursesPercentage => (float)Courses * 100 / (float)RequiredCourses;
    }
}
