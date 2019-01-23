using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using UASD;

namespace Client.WPF.Controls
{
    /// <summary>
    /// Interaction logic for Schedule.xaml
    /// </summary>
    public partial class Schedule : UserControl
    {
        public bool ShowCurrentTime {
            get => (bool)this.GetValue(ShowCurrentTypeProperty);
            set => this.SetValue(ShowCurrentTypeProperty, value);
        }
        public static readonly DependencyProperty ShowCurrentTypeProperty = DependencyProperty.Register(
            "ShowCurrentTime", typeof(bool), typeof(Schedule), new PropertyMetadata(false,
                (obj, args) => {
                    var schedule = obj as Schedule;
                    if ((bool)args.NewValue)
                        schedule.IndicateCurrentTime();
                    else
                        schedule.ClearTimeIndicator();
                }
            )
        );

        private List<ScheduleItem> Items = new List<ScheduleItem>();
        private List<ScheduleItem> ShadowItems = new List<ScheduleItem>();
        private DispatcherTimer IndicatorTimer = null;
        private CurrentTimeIndicator Indicator = null;

        public Schedule()
        {
            InitializeComponent();
        }

        public void LoadCourses(IEnumerable<Course> courses, bool isShadow = false)
        {
            if (isShadow)
                ClearShadow();
            else
                ClearRegular();
            foreach (var course in courses)
                AddCourse(course, isShadow);
        }

        public void AddCourse(Course course, bool isShadow = false) {
            foreach (var courseInstance in course.ScheduleInfo)
            {
                var item = new ScheduleItem
                {
                    Titulo = course.Title,
                    Codigo = course.Code,
                    Lugar = UASD.Utilities.Convert.Place(courseInstance.Place),
                    IsShadow = isShadow,
                    ToolTip = $"{course.Title}\n{course.Code}\nNRC: {course.NRC}\n{course.Credits} creditos\nLugar: {courseInstance.Place}\nProf.: {course.Professor}"
                };
                AddItem(item,
                    courseInstance.DayOfWeek,
                    courseInstance.StartTime,
                    courseInstance.Duration,
                    isShadow
                );
            }
        }

        public void AddItem(ScheduleItem item, DayOfWeek day, TimeSpan time, int duration, bool isShadow = false) {
            Grid.SetColumn (item, (int)day);
            Grid.SetRow    (item, time.Hours - 6);
            Grid.SetRowSpan(item, duration);
            if (isShadow)
                ShadowItems.Add(item);
            else
                Items.Add(item);
            GSchedule.Children.Add(item);
        }

        public void ClearRegular() {
            foreach (var item in Items)
                GSchedule.Children.Remove(item);
            Items.Clear();
        }
        public void ClearShadow() {
            foreach (var item in ShadowItems)
                GSchedule.Children.Remove(item);
            ShadowItems.Clear();
        }
        public void ClearTimeIndicator()
        {
            IndicatorTimer?.Stop();
            GSchedule.Children.Remove(Indicator);
            IndicatorTimer = null;
            Indicator = null;
        }
        public void Clear()
        {
            ClearRegular();
            ClearShadow();
            ClearTimeIndicator();
        }

        public void IndicateCurrentTime()
        {
            ClearTimeIndicator();

            int slotFromTime  (DateTime time) => time.Minute / 5;
            int columnFromTime(DateTime time) => (int)time.DayOfWeek;
            int rowFromTime   (DateTime time) => time.Hour - 6;

            void setupindicator(CurrentTimeIndicator cTIndicator) {
                var now = DateTime.Now;

                cTIndicator.Visibility = (now.DayOfWeek == DayOfWeek.Sunday || now.Hour < 7 || now.Hour > 21)
                    ? Visibility.Collapsed
                    : Visibility.Visible;

                cTIndicator.Slot = slotFromTime(now);
                Grid.SetColumn(cTIndicator, columnFromTime(now));
                Grid.SetRow   (cTIndicator, rowFromTime   (now));
            }

            var indicator = new CurrentTimeIndicator();
            Panel.SetZIndex(indicator, 1);
            setupindicator(indicator);

            IndicatorTimer = new DispatcherTimer(
                interval: TimeSpan.FromSeconds(5),
                priority: DispatcherPriority.Background,
                callback: (s, args)  => setupindicator(indicator),
                dispatcher: this.Dispatcher
            );
            IndicatorTimer.Start();

            Indicator = indicator;
            GSchedule.Children.Add(indicator);
        }
    }
}
