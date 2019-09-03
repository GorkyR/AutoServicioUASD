using System;

namespace UASD
{
    using Utilities;
    public class CourseClass
    {
        public TimeSpan  StartTime { get; set; }
        public TimeSpan  EndTime   { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public string    Place     { get; set; } = "";
        public DateTime  StartDate { get; set; }
        public DateTime  EndDate   { get; set; }
        public int       Duration  => (int)Math.Ceiling((EndTime - StartTime).TotalHours);

        public bool CollidesWith(CourseClass other) => 
            (DayOfWeek == other.DayOfWeek) && (StartTime < other.EndTime && EndTime > other.StartTime);

        public override string ToString() => $"{DayOfWeek} - {Place} - {Convert.Time(StartTime)} - {Convert.Time(EndTime)}";

        public static string GetInfoString(Course course, CourseClass courseClass)
        {
            return $"{course.Title}\n" +
                   $"{course.Code} - {course.Section}\n" +
                   $"Aula: {courseClass.Place}\n\n" + 

                   $"Prof.: {course.Professor}\n" +
                   $"NRC: {course.NRC}\n" +
                   $"{course.Credits} créditos";
        }
    }

    public class CourseInstance
    {
        public Course Course     { get; set; }
        public CourseClass Class { get; set; }
    }
}
