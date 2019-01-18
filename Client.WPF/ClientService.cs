using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using UASD;

namespace Client.WPF {
    public static class ClientService
    {
        public static UASD.Client AutoServicio { get; } = new UASD.Client();

        static CourseCollection schedule;
        static AcademicReport report;
        static CourseCollection projection;
        static CareerInformation information;

        public static void ResetClientInformation()
        {
            schedule    = null;
            report      = null;
            projection  = null;
            information = null;
        }

        public static async Task<CourseCollection> ScheduleAsync()
        {
            if (schedule != null)
                return schedule;
            schedule = StateService.CurrentSession.Schedule;
            if (schedule != null)
                return schedule;
            await CacheScheduleAsync();
            return schedule;
        }
        public static async Task<CourseCollection> FetchScheduleAsync()
        {
            await CacheScheduleAsync();
            return schedule;
        }

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
            await CacheReportAsync();
            return report;
        }
        public static async Task<AcademicReport> FetchReportAsync()
        {
            await CacheReportAsync();
            return report;
        }

        public static async Task<CourseCollection> ProjectionAsync()
        {
            if (!(projection is null))
                return projection;
            projection = StateService.CurrentSession.Projection;
            if (!(projection is null))
                return projection;
            await CacheProjectionAsync();
            return projection;
        }
        public static async Task<CourseCollection> FetchProjectionAsync()
        {
            await CacheProjectionAsync();
            return projection;
        }

        public static async Task<CareerInformation> CareerInformationAsync()
        {
            if (!(information is null))
                return information;
            information = StateService.CurrentSession.Information;
            if (!(information is null))
                return information;
            await CacheCareerInformationAsync();
            return information;
        }
        public static async Task<CareerInformation> FetchCareerInformationAsync()
        {
            await CacheCareerInformationAsync();
            return information;
        }

        public static async Task<List<CourseCollection>> AvailableCoursesAsync()
        {
            List<CourseCollection> availableCourses = null;
            try {
                availableCourses = await AutoServicio?.FetchAvailableCoursesAsync();
            }
            catch(NotLoggedInException) {
                if (StateService.IsLoggedIn)
                {
                    var matricula = StateService.CurrentSession.ID;
                    var nip = StateService.CurrentSession.NIP;

                    if (await AutoServicio.LoginAsync(matricula, nip))
                        return await AvailableCoursesAsync();
                }
                StateService.IsLoggedIn = false;
                throw new NotLoggedInException();
            }
            return availableCourses;
        }

        private static async Task CacheScheduleAsync()
        {
            try { schedule = await AutoServicio?.FetchScheduleDetailAsync(); }
            catch (NotLoggedInException) { await ReLoginThen(CacheScheduleAsync); return;  }
            var updatedSession = StateService.CurrentSession;
            updatedSession.Schedule = schedule;
            StateService.CurrentSession = updatedSession;
        }
        private static async Task CacheReportAsync()
        {
            try { report = await AutoServicio?.FetchAcademicReportAsync(); }
            catch (NotLoggedInException) { await ReLoginThen(CacheReportAsync); return; }
            report.Periods.Reverse();
            var updatedSession = StateService.CurrentSession;
            var repModel = new List<Models.AcademicPeriodModel>();
            foreach (var period in report.Periods)
                repModel.Add(
                    new Models.AcademicPeriodModel()
                    {
                        Title = period.Title,
                        Courses = period.Courses.ToList()
                    }
                );
            updatedSession.Report = repModel;
            StateService.CurrentSession = updatedSession;
        }
        private static async Task CacheProjectionAsync()
        {
            try { projection = await AutoServicio?.FetchCourseProjectionAsync(); }
            catch(NotLoggedInException) { await ReLoginThen(CacheProjectionAsync); return; }
            var updatedSession = StateService.CurrentSession;
            updatedSession.Projection = projection;
            StateService.CurrentSession = updatedSession;
        }
        private static async Task CacheCareerInformationAsync()
        {
            try { information = await AutoServicio?.FetchCareerInformationAsync(); }
            catch(NotLoggedInException) { await ReLoginThen(CacheCareerInformationAsync); return; }
            var updatedSession = StateService.CurrentSession;
            updatedSession.Information = information;
            StateService.CurrentSession = updatedSession;
        }

        public static async Task ReLoginThen(Func<Task> callback)
        {
            if (StateService.IsLoggedIn)
            {
                var matricula = StateService.CurrentSession.ID;
                var nip = StateService.CurrentSession.NIP;

                if (await AutoServicio.LoginAsync(matricula, nip)) {
                    await callback();
                    return;
                }
            }
            StateService.IsLoggedIn = false;
            throw new NotLoggedInException();
        }
    }
}