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
using System.Collections.ObjectModel;
using UASD;

namespace Client.WPF.Pages
{
    /// <summary>
    /// Interaction logic for SchedulePage.xaml
    /// </summary>
    public partial class SchedulePage : UserControl
    {
        public ObservableCollection<Course> Courses { get => (ObservableCollection<Course>)this.GetValue(CoursesProperty); set => this.SetValue(CoursesProperty, value); }
        public static readonly DependencyProperty CoursesProperty = DependencyProperty.Register(
            "Courses", typeof(ObservableCollection<Course>), typeof(SchedulePage), new PropertyMetadata(
                new ObservableCollection<Course>(),
                (d, args) => {
                    var courses = (IEnumerable<Course>)args.NewValue;
                    var schedulePage = (SchedulePage)d;
                    schedulePage.Refresh(courses);
                }
            )
        );

        public ObservableCollection<Course> TemporaryCourses
        {
            get => (ObservableCollection<Course>)this.GetValue(TemporaryCoursesProperty);
            set => this.SetValue(TemporaryCoursesProperty, value);
        }
        public static readonly DependencyProperty TemporaryCoursesProperty = DependencyProperty.Register(
            "TemporaryCourses", typeof(ObservableCollection<Course>), typeof(SchedulePage), new PropertyMetadata(
                new ObservableCollection<Course>(),
                (d, args) => {
                    var temporaryCourses = (IEnumerable<Course>)args.NewValue;
                    var schedulePage = (SchedulePage)d;
                    schedulePage.Refresh(temporaryCourses, true);
                }
            )
        );

        private void Refresh(IEnumerable<Course> courses, bool isShadow = false)
        {
            if (isShadow)
                Schedule.ClearShadow();
            else 
                Schedule.ClearRegular();
            foreach (var course in courses)
            {
                foreach (var courseInstance in course.ScheduleInfo)
                {
                    var item = new Controls.ScheduleItem
                    {
                        Titulo = course.Title,
                        Codigo = course.Code,
                        Lugar = UASD.Utilities.Convert.Place(courseInstance.Place),
                        IsShadow = isShadow
                    };
                    Schedule.AddItem(item,
                        courseInstance.Weekday,
                        courseInstance.StartTime,
                        courseInstance.Duration,
                        isShadow
                    );
                }
            }
        }

        public SchedulePage()
        {
            InitializeComponent();
            Courses.CollectionChanged          += (s, args) => Refresh(Courses);
            TemporaryCourses.CollectionChanged += (s, args) => Refresh(TemporaryCourses, true);
        }
    }
}
