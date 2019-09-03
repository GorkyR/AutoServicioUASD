namespace UASD
{
    public class OpenCourse : Course
    {
        public int Capacity { get; set; }
        public int Vacancy { get; set; }

        public override string ToString() =>
            $"{Title} - {Code}{(Section is null? "" : $" - {Section}")} - [{Vacancy}]";
    }
}
