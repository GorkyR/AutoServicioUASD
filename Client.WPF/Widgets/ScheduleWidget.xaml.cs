using Client.WPF.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UASD;

namespace Client.WPF.Widgets
{
    /// <summary>
    /// Interaction logic for ScheduleWidget.xaml
    /// </summary>
    public partial class ScheduleWidget : UserControl
    {
        public ScheduleWidget()
        {
            InitializeComponent();
        }

        private async void DidLoad(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.AppStarting;
            var courses = await ClientStateService.ScheduleAsync();

            var now = DateTime.Now;
            var today = now.DayOfWeek;
            var timeOfDay = now.TimeOfDay;

            if (today == DayOfWeek.Sunday) {
                today = DayOfWeek.Monday;
                timeOfDay = new TimeSpan();
            }

            var todaysCourses = courses.FilterByDay(today);
            var upcomingClasses = todaysCourses.SkipWhile(courseInstance =>
                Math.Ceiling(courseInstance.Class.EndTime.TotalHours) <= timeOfDay.Hours
            );

            var agendaDays = new AgendaDay[6];
            agendaDays[0] = new AgendaDay {
                DayOfWeek = today,
                Items = upcomingClasses
            };
            for (int i = 1; i < 6 && (int)today + i < 7; i++) {
                DayOfWeek day = (DayOfWeek)((int)today + i);

                agendaDays[i] = new AgendaDay {
                    DayOfWeek = day,
                    Items = courses.FilterByDay(day)
                };
            }

            ICAgenda.ItemsSource = agendaDays;
            Cursor = Cursors.Arrow;
        }
    }

    internal static class CourseCollectionExtensions
    {
        
        public static List<CourseInstance> FilterByDay(this UASD.CourseCollection courseCollection, DayOfWeek dayOfWeek)
        {
            var thatDaysCourses =
                (from course in courseCollection
                 select (from instance in course.Schedule
                         where instance.DayOfWeek == dayOfWeek
                         select new CourseInstance { Course = course, Class = instance }))
                .Aggregate((a, b) => a.Concat(b)).ToList();
            thatDaysCourses.Sort((a, b) =>
                (a.Class.StartTime - b.Class.StartTime).Hours
            );
            return thatDaysCourses;
        }
    }
}
