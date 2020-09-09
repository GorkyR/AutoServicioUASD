using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using UASD;
using Java.Nio.Channels;
using Android.Text;

namespace Client.Droid {
    public static class ClientStateService
    {
        public enum   DataSource { Production, Baked, Random };
        public static DataSource dataSource = DataSource.Production;
        public static Dictionary<string, DataSource> fakeUsers = new Dictionary<string, DataSource>() {
            { "testuser"  , DataSource.Baked },
            { "randomuser", DataSource.Random }
        };

        static Random random = new Random();

        public static UASD.Client AutoServicio { get; } = new UASD.Client();

        static AcademicReport      report;
        static CourseCollection    schedule;
        static CourseCollection    projection;
        static CareerInformation   information;
        static List<DateTimeRange> calendar;

        public static void ResetClientInformation()
        {
            report      = null;
            schedule    = null;
            projection  = null;
            information = null;
            calendar    = null;
            StatePersistanceService.ResetSession();
            StatePersistanceService.IsLoggedIn = false;
        }

        public static async Task<CourseCollection> ScheduleAsync()
        {
            if (schedule is null)
                schedule = StatePersistanceService.CurrentSession.Schedule;
            if (schedule is null)
                await CacheScheduleAsync();

            // If you're not registered for the current period, we still cache an empty schedule
            // so that we don't have to go to the network to ask for it every time, but here we
            // make sure to communicate the fact that the schedule was null.
            if (schedule.Count == 0)
                throw new NoDataReceivedException();

            return schedule;
        }
        public static async Task<CourseCollection> FetchScheduleAsync()
        {
            await CacheScheduleAsync();
            if (schedule.Count == 0)
                throw new NoDataReceivedException();
            return schedule;
        }

        public static async Task<AcademicReport> ReportAsync()
        {
            if (report is null)
            {
                var p = StatePersistanceService.CurrentSession.Report;
                if (p != null)
                {
                    var rep = new AcademicReport();
                    foreach (var period in p)
                    {
                        var courseCollection = new CourseCollection();
                        foreach (var course in period.Courses)
                            courseCollection.Add(course);
                        rep.Periods.Add(new AcademicPeriod() { Title = period.Title, Courses = courseCollection });
                    }
                    report = rep;
                }
            }
            if (report is null)
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
            if (projection is null)
                projection = StatePersistanceService.CurrentSession.Projection;
            if (projection is null)
                await CacheProjectionAsync();

            // If your course projection is not available, we still cache an empty course collection
            // so that we don't have to go to the network to ask for it every time, but here we make
            // sure to communicate the fact that the projection was unavailable.
            if (projection.Count == 0)
                throw new NoProyectionAvailableException();

            return projection;
        }
        public static async Task<CourseCollection> FetchProjectionAsync()
        {
            await CacheProjectionAsync();
            if (projection.Count == 0)
                throw new NoProyectionAvailableException();
            return projection;
        }

        public static async Task<CareerInformation> CareerInformationAsync()
        {
            if (information is null)
                information = StatePersistanceService.CurrentSession.Information;
            if (information is null)
                await CacheCareerInformationAsync();
            return information;
        }
        public static async Task<CareerInformation> FetchCareerInformationAsync()
        {
            await CacheCareerInformationAsync();
            return information;
        }

        public static async Task<List<DateTimeRange>> SelectionCalendarAsync()
        {
            if (calendar is null)
                calendar = StatePersistanceService.CurrentSession.Calendar?.ToList();
            if (calendar is null)
                await CacheSelectionCalendarAsync();
            return calendar;
        }
        public static async Task<List<DateTimeRange>> FetchSelectionCalendarAsync()
        {
            await CacheSelectionCalendarAsync();
            return calendar;
        }

        public static async Task<List<CourseCollection>> AvailableCoursesAsync()
        {
            switch (dataSource)
            {
                case DataSource.Production:
                    {
                        try { return await AutoServicio?.FetchAvailableCoursesAsync(); }
                        catch (NotLoggedInException)
                        {
                            var matricula = StatePersistanceService.CurrentSession.ID;
                            var nip = StatePersistanceService.CurrentSession.NIP;

                            if (await AutoServicio.LoginAsync(matricula, nip))
                                return await AvailableCoursesAsync();
                            throw new NotLoggedInException();
                        }
                    }
                case DataSource.Baked:
                    throw new NoSelectionAvailableException();
                case DataSource.Random:
                    return TestEnvironment.TestData.GenerateFakeAvailableCourses(random);
            }
            throw new NoSelectionAvailableException();
        }

        private static async Task CacheScheduleAsync()
        {
            try {
                switch(dataSource)
                {
                    case DataSource.Production:
                        schedule = await AutoServicio?.FetchScheduleDetailAsync();
                        break;
                    case DataSource.Baked:
                        schedule = TestEnvironment.TestData.fakePreloadedSchedule;
                        break;
                    case DataSource.Random:
                        schedule = TestEnvironment.TestData.GenerateFakeCourseSchedule(random);
                        break;
                }
            }
            catch (NotLoggedInException) { await ReLoginThen(CacheScheduleAsync); return;  }
            catch (NoDataReceivedException) { schedule = new CourseCollection("No Inscrito"); }
            var updatedSession = StatePersistanceService.CurrentSession;
            updatedSession.Schedule = schedule;
            StatePersistanceService.CurrentSession = updatedSession;
        }
        private static async Task CacheReportAsync()
        {
            try
            {
                switch (dataSource)
                {
                    case DataSource.Production:
                        report = await AutoServicio?.FetchAcademicReportAsync();
                        break;
                    case DataSource.Baked:
                        var rep = TestEnvironment.TestData.fakePreloadedAcademicReport;
                        report = new AcademicReport();
                        foreach (var period in rep.Periods)
                            report.Periods.Add(period);
                        break;
                    case DataSource.Random:
                        report = TestEnvironment.TestData.GenerateFakeAcademicReport(random, await ScheduleAsync());
                        break;
                }
            }
            catch (NotLoggedInException) { await ReLoginThen(CacheReportAsync); return; }
            report.Periods.Reverse();
            var updatedSession = StatePersistanceService.CurrentSession;
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
            StatePersistanceService.CurrentSession = updatedSession;
        }
        private static async Task CacheProjectionAsync()
        {
            try {
                switch(dataSource)
                {
                    case DataSource.Production:
                        projection = await AutoServicio?.FetchCourseProjectionAsync();
                        break;
                    case DataSource.Baked:
                        throw new NoProyectionAvailableException();
                    case DataSource.Random:
                        projection = TestEnvironment.TestData.GenerateFakeCourseProjection(random);
                        break;
                }
            }
            catch(NotLoggedInException) { await ReLoginThen(CacheProjectionAsync); return; }
            catch(NoProyectionAvailableException) { projection = new CourseCollection("No disponible"); }
            var updatedSession = StatePersistanceService.CurrentSession;
            updatedSession.Projection = projection;
            StatePersistanceService.CurrentSession = updatedSession;
        }
        private static async Task CacheCareerInformationAsync()
        {
            try {
                switch(dataSource)
                {
                    case DataSource.Production:
                        information = await AutoServicio?.FetchCareerInformationAsync();
                        break;
                    case DataSource.Baked:
                        information = TestEnvironment.TestData.fakePreloadedCareerInformation;
                        break;
                    case DataSource.Random:
                        information = TestEnvironment.TestData.GenerateFakeCareerInformation(random, await ReportAsync());
                        break;
                }
            }
            catch(NotLoggedInException) { await ReLoginThen(CacheCareerInformationAsync); return; }
            var updatedSession = StatePersistanceService.CurrentSession;
            updatedSession.Information = information;
            StatePersistanceService.CurrentSession = updatedSession;
        }
        private static async Task CacheSelectionCalendarAsync()
        {
            try
            {
                switch (dataSource)
                {
                    case DataSource.Production:
                        calendar = await AutoServicio?.FetchSelectionCalendarAsync();
                        break;
                    case DataSource.Baked:
                        throw new NoSelectionAvailableException();
                    case DataSource.Random:
                        calendar = TestEnvironment.TestData.GenerateFakeSelectionCalendar();
                        break;
                }
            }
            catch (NotLoggedInException) { await ReLoginThen(CacheSelectionCalendarAsync); return; }
            var updatedSession = StatePersistanceService.CurrentSession;
            updatedSession.Calendar = calendar;
            StatePersistanceService.CurrentSession = updatedSession;
        }

        public static void ForgetSchedule()
        {
            schedule = null;
            var updatedSession = StatePersistanceService.CurrentSession;
            updatedSession.Schedule = null;
            StatePersistanceService.CurrentSession = updatedSession;
        }
        public static void ForgetReport()
        {
            report = null;
            var updatedSession = StatePersistanceService.CurrentSession;
            updatedSession.Report = null;
            StatePersistanceService.CurrentSession = updatedSession;
        }
        public static void ForgetProjection()
        {
            projection = null;
            var updatedSession = StatePersistanceService.CurrentSession;
            updatedSession.Projection = null;
            StatePersistanceService.CurrentSession = updatedSession;
        }
        public static void ForgetCareerInformation()
        {
            information = null;
            var updatedSession = StatePersistanceService.CurrentSession;
            updatedSession.Information = null;
            StatePersistanceService.CurrentSession = updatedSession;
        }
        public static void ForgetSelectionCalendar()
        {
            calendar = null;
            var updatedSession = StatePersistanceService.CurrentSession;
            updatedSession.Calendar = null;
            StatePersistanceService.CurrentSession = updatedSession;
        }

        public static async Task ReLoginThen(Func<Task> callback)
        {
            if (StatePersistanceService.IsLoggedIn)
            {
                var matricula = StatePersistanceService.CurrentSession.ID;
                var nip = StatePersistanceService.CurrentSession.NIP;

                dataSource = fakeUsers.GetValueOrDefault(matricula);

                if (dataSource != DataSource.Production || await AutoServicio.LoginAsync(matricula, nip)) {
                    await callback();
                    return;
                }
            }
            StatePersistanceService.IsLoggedIn = false;
            throw new NotLoggedInException();
        }
    }
}