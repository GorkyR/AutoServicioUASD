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

namespace Client.WPF.Controls
{
    /// <summary>
    /// Interaction logic for Schedule.xaml
    /// </summary>
    public partial class Schedule : UserControl
    {
        private List<ScheduleItem> Items = new List<ScheduleItem>();
        private List<ScheduleItem> ShadowItems = new List<ScheduleItem>();

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
                    courseInstance.Weekday,
                    courseInstance.StartTime,
                    courseInstance.Duration,
                    isShadow
                );
            }
        }

        public void AddItem(ScheduleItem item, ScheduleInfo.Day day, TimeSpan time, int duration, bool isShadow = false) {
            Grid.SetColumn (item, (int)day + 1);
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

        public void Clear()
        {
            ClearRegular();
            ClearShadow();
        }
    }
}
