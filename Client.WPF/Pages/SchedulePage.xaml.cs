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
        public ObservableCollection<Course> Materias {
            get => (ObservableCollection<Course>)this.GetValue(MateriasProperty);
            set {
                this.SetValue(MateriasProperty, value);
                Refresh(value);
                value.CollectionChanged += (s, args) => Refresh(value);
            }
        }
        public static readonly DependencyProperty MateriasProperty = DependencyProperty.Register(
            "Materias", typeof(ObservableCollection<Course>), typeof(SchedulePage), new PropertyMetadata(
                new ObservableCollection<Course>(),
                (d, args) =>
                {
                    var courses = (IEnumerable<Course>)args.NewValue;
                    var schedulePage = (SchedulePage)d;
                    schedulePage.Refresh(courses);
                }
            )
        );

        public ObservableCollection<Course> MateriasTemporales
        {
            get => (ObservableCollection<Course>)this.GetValue(MateriasTemporalesProperty);
            set {
                this.SetValue(MateriasTemporalesProperty, value);
                Refresh(value, true);
                value.CollectionChanged += (s, args) => Refresh(value, true);
            }
        }
        public static readonly DependencyProperty MateriasTemporalesProperty = DependencyProperty.Register(
            "MateriasTemporales", typeof(ObservableCollection<Course>), typeof(SchedulePage), new PropertyMetadata(
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
                        IsShadow = isShadow,
                        ToolTip = $"{course.Title}\n{course.Code}\nNRC: {course.NRC}\n{course.Credits} creditos\nLugar: {courseInstance.Place}\nProf.: {course.Professor}"
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
            Materias.CollectionChanged += (s, args) => Refresh(Materias);
            MateriasTemporales.CollectionChanged += (s, args) => Refresh(MateriasTemporales, true);
        }

        private async void DidLoad(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.AppStarting;
            Materias = new ObservableCollection<Course>(await ClientService.ScheduleAsync());
            Cursor = Cursors.Arrow;
        }
    }
}
