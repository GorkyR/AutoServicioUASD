using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

namespace Client.WPF.Pages
{
    /// <summary>
    /// Interaction logic for SelectionPage.xaml
    /// </summary>
    public partial class SelectionPage : UserControl, IPage
    {
        public CourseCollection MateriasSeleccionadas { get; set; } = new CourseCollection();

        public List<CourseCollection> Materias
        {
            get => (List<CourseCollection>)this.GetValue(MateriasProperty);
            set => this.SetValue(MateriasProperty, value);
        }
        public static readonly DependencyProperty MateriasProperty = DependencyProperty.Register(
            "Materias", typeof(List<CourseCollection>), typeof(SelectionPage), new PropertyMetadata());

        public SelectionPage()
        {
            InitializeComponent();
            MateriasSeleccionadas.CollectionChanged += (_) =>
            {
                var noneSelected = MateriasSeleccionadas.Count == 0;
                SPSeleccionadas.Visibility = noneSelected ? Visibility.Collapsed : Visibility.Visible;
                SPSeleccionadas.IsEnabled = !noneSelected;
                ICSeleccionadas.ItemsSource = MateriasSeleccionadas.Select(course =>
                    new Controls.SelectedSectionItem() {
                        DataContext = course,
                        RemoveAction = () => MateriasSeleccionadas.Remove(course)
                    }
                );
                Schedule.LoadCourses(MateriasSeleccionadas);
                ChangeCourseSelection(null, null);
            };
        }

        private async void DidLoad(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.AppStarting;
            try { Materias = await ClientService.AvailableCoursesAsync(); }
            catch (NoDataReceivedException) {
                GSeleccion.Visibility = Visibility.Collapsed;
                SPUnavailable.Visibility = Visibility.Visible;
            }
            finally { Cursor = Cursors.Arrow; }
        }

        private void ChangeCourseSelection(object sender, SelectionChangedEventArgs e)
        {
            var courseCollection = LVMaterias.SelectedItem as CourseCollection;
            LVSeccionesDisponibles.ItemsSource = courseCollection
                .Where(course => MateriasSeleccionadas.IsCompatibleWith(course))
                .Select(course => {
                    var sectionElement = new Controls.OpenCourseItem();
                    sectionElement.DataContext = course;
                    sectionElement.AddAction = () => MateriasSeleccionadas.Add(course);
                    return sectionElement;
                }
            );
        }

        private void ChangeSectionSelection(object sender, SelectionChangedEventArgs e)
        {
            Schedule.ClearShadow();
            var selectedItem = LVSeccionesDisponibles.SelectedItem as Controls.OpenCourseItem;
            var temporaryCourse = selectedItem?.DataContext as OpenCourse;
            if (temporaryCourse != null)
                Schedule.AddCourse(temporaryCourse, true);
        }

        private async void Registrar(object sender, RoutedEventArgs e)
        {
            try
            {
                var nrcMaterias = MateriasSeleccionadas.Select(materia => materia.NRC).ToArray();
                await ClientService.AutoServicio.RegisterCoursesAsync(nrcMaterias);
            } catch (NoSelectionAvailableException) {
                MessageBox.Show(
                    "No es posible realizar la seleccion en estos momentos",
                    "Seleccion no disponible",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            } catch (SeleccionErrorsException see) {
                MessageBox.Show(
                    $"La seleccion presentó los siguientes errores:\n{see.Message}",
                    "Errores en seleccion",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            } catch (NotLoggedInException) {
                await ClientService.ReLoginThen(() => new Task(() => this.Registrar(null, null)));
                return;
            }
        }

        public void Refresh()
        {
            Cursor = Cursors.Wait;
            this.DidLoad(null, null);
        }
    }
}
