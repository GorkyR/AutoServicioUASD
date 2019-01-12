using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace UASD.MaterialWPF {
    public static class ClientService
    {
        public static Client AutoServicio { get; } = new Client();

        static CourseCollection schedule;
        public static async Task<CourseCollection> ScheduleAsync()
        {
            if (schedule != null)
                return schedule;
            schedule = StateService.CurrentSession.Schedule;
            if (schedule != null)
                return schedule;
            await FetchSchedule();
            return schedule;
        }
        public static async Task<CourseCollection> ForceFetchSchedule()
        {
            await FetchSchedule();
            return schedule;
        }
        private static async Task FetchSchedule()
        {
            schedule = await AutoServicio?.FetchScheduleDetailAsync();
            var updatedSession = StateService.CurrentSession;
            updatedSession.Schedule = schedule;
            StateService.CurrentSession = updatedSession;
        }

        static AcademicReport report;
        public static async Task<AcademicReport> ReportAsync()
        {
            if (!(report is null))
                return report;
            var p = StateService.CurrentSession.Report;
            if (!(p is null)) {
                var rep = new AcademicReport();
                foreach (var period in p) {
                    var courseCollection = new CourseCollection();
                    foreach (var course in period.Courses)
                        courseCollection.Add(course);
                    rep.Periods.Add(
                        new AcademicPeriod() {
                            Title = period.Title,
                            Courses = courseCollection
                        }
                    );
                }
                report = rep;
                return report;
            }
            await FetchReport();
            return report;
        }
        public static async Task<AcademicReport> ForceFetchReport()
        {
            await FetchReport();
            return report;
        }
        private static async Task FetchReport()
        {
            report = await AutoServicio?.FetchAcademicReportAsync();
            var updatedSession = StateService.CurrentSession;
            var repModel = new List<Models.AcademicPeriodModel>();
            foreach (var period in report.Periods)
                repModel.Add(
                    new Models.AcademicPeriodModel() {
                        Title = period.Title,
                        Courses = period.Courses.ToList()
                    }
                );
            updatedSession.Report = repModel;
            StateService.CurrentSession = updatedSession;
        }

        static CourseCollection projection;
        public static async Task<CourseCollection> ProjectionAsync()
        {
            if (!(projection is null))
                return projection;
            projection = StateService.CurrentSession.Projection;
            if (!(projection is null))
                return projection;
            await FetchProjection();
            return projection;
        }
        public static async Task<CourseCollection> ForceFetchProjection()
        {
            await FetchProjection();
            return projection;
        }
        private static async Task FetchProjection()
        {
            projection = await AutoServicio?.FetchCourseProjectionAsync();
            var updatedSession = StateService.CurrentSession;
            updatedSession.Projection = projection;
            StateService.CurrentSession = updatedSession;
        }

        static CareerInformation information;
        public static async Task<CareerInformation> CareerInformationAsync()
        {
            if (!(information is null))
                return information;
            information = StateService.CurrentSession.Information;
            if (!(information is null))
                return information;
            await FetchCareerInformation();
            return information;
        }
        public static async Task<CareerInformation> ForceFetchCareerInformation()
        {
            await FetchCareerInformation();
            return information;
        }
        private static async Task FetchCareerInformation()
        {
            information = await AutoServicio?.FetchCareerInformationAsync();
            var updatedSession = StateService.CurrentSession;
            updatedSession.Information = information;
            StateService.CurrentSession = updatedSession;
        }

        public static async Task<List<CourseCollection>> AvailableCoursesAsync()
        {
            return await AutoServicio?.FetchAvailableCoursesAsync();
        }
        
        public static void ResetClientInformation()
        {
            schedule    = null;
            report      = null;
            projection  = null;
            information = null;
        }
    }
}