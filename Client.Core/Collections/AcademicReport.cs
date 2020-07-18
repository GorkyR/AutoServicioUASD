using System.Linq;
using System.Collections.Generic;

namespace UASD
{
    public class AcademicReport
    {
        public float GlobalIndex
        {
            get
            {
                var contables = new Dictionary<string, KeyValuePair<int, int>>();
                foreach (AcademicPeriod periodo in this.Periods.Where(p => !p.IsActive && p.Credits > 0))
                    foreach (GradedCourse course in periodo.Courses.Where(m => (m as GradedCourse).State == 0))
                        if (!contables.ContainsKey(course.Code))
                            contables.Add(course.Code, new KeyValuePair<int, int>(course.Grade, course.Credits));

                float index = 0, credits = 0;
                foreach (KeyValuePair<int, int> nums in contables.Values)
                {
                    index += nums.Key * nums.Value;
                    credits += nums.Value;
                }
                return index / credits;
            }
        }
        public int Credits
        {
            get
            {
                int creds = 0;
                foreach (AcademicPeriod p in Periods.Where(p => p.Credits > 0))
                    creds += p.Credits;
                return creds;
            }
        }
        public float CoursesPerPeriod
        {
            get
            {
                IEnumerable<AcademicPeriod> primaryPeriods =
                    (from period in Periods
                     where !period.IsSummerCourse
                     select period);

                float coursesPerPeriod = 0;
                foreach (AcademicPeriod period in primaryPeriods)
                    coursesPerPeriod += period.Courses.Count;
                return coursesPerPeriod / primaryPeriods.Count();
            }
        }
        public List<AcademicPeriod> Periods { get; set; } = new List<AcademicPeriod>();

        public float GetPeriodInfluence(AcademicPeriod s)
        {
            float credits = 0;
            foreach (AcademicPeriod periodo in this.Periods.Take(this.Periods.IndexOf(s) + 1))
                credits += periodo.Credits;
            return s.Credits / credits;
        }
        public float GetGlobalIndexUpTo(AcademicPeriod s)
        {
            Dictionary<string, KeyValuePair<int, int>> materiasContadas = new Dictionary<string, KeyValuePair<int, int>>();
            foreach (AcademicPeriod periodo in this.Periods.Take(Periods.IndexOf(s) + 1).Where(p => !p.IsActive && p.Credits > 0))
                foreach (GradedCourse course in periodo.Courses.Where(m => (m as GradedCourse).State == 0))
                    if (!materiasContadas.ContainsKey(course.Code))
                        materiasContadas.Add(course.Code, new KeyValuePair<int, int>(course.Grade, course.Credits));

            float index = 0, credits = 0;
            foreach (KeyValuePair<int, int> nums in materiasContadas.Values)
            {
                index   += nums.Key * nums.Value;
                credits += nums.Value;
            }
            return index / credits;
        }
    }
}
