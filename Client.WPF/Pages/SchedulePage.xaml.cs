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
    public partial class SchedulePage : UserControl, IPage
    {
        public ObservableCollection<Course> Materias {
            get => (ObservableCollection<Course>)this.GetValue(MateriasProperty);
            set {
                this.SetValue(MateriasProperty, value);
                Schedule.LoadCourses(value);
                value.CollectionChanged += (s, args) => Schedule.LoadCourses(value);
            }
        }
        public static readonly DependencyProperty MateriasProperty = DependencyProperty.Register(
            "Materias", typeof(ObservableCollection<Course>), typeof(SchedulePage), new PropertyMetadata(
                new ObservableCollection<Course>(),
                (d, args) =>
                {
                    var courses = (IEnumerable<Course>)args.NewValue;
                    var schedulePage = (SchedulePage)d;
                    schedulePage.Schedule.LoadCourses(courses);
                }
            )
        );

        public ObservableCollection<Course> MateriasTemporales
        {
            get => (ObservableCollection<Course>)this.GetValue(MateriasTemporalesProperty);
            set {
                this.SetValue(MateriasTemporalesProperty, value);
                Schedule.LoadCourses(value, true);
                value.CollectionChanged += (s, args) => Schedule.LoadCourses(value, true);
            }
        }
        public static readonly DependencyProperty MateriasTemporalesProperty = DependencyProperty.Register(
            "MateriasTemporales", typeof(ObservableCollection<Course>), typeof(SchedulePage), new PropertyMetadata(
                new ObservableCollection<Course>(),
                (d, args) => {
                    var temporaryCourses = (IEnumerable<Course>)args.NewValue;
                    var schedulePage = (SchedulePage)d;
                    schedulePage.Schedule.LoadCourses(temporaryCourses, true);
                }
            )
        );

        public SchedulePage()
        {
            InitializeComponent();
            Materias.CollectionChanged += (s, args) => Schedule.LoadCourses(Materias);
            MateriasTemporales.CollectionChanged += (s, args) => Schedule.LoadCourses(MateriasTemporales, true);
        }

        private async void DidLoad(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.AppStarting;
            Materias = new ObservableCollection<Course>(await ClientService.ScheduleAsync());
            Cursor = Cursors.Arrow;
        }

        public async void Refresh()
        {
            Cursor = Cursors.Wait;
            await ClientService.FetchScheduleAsync();
            this.DidLoad(null, null);
        }
    }
}
