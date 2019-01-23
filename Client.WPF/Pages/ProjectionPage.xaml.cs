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

namespace Client.WPF.Pages
{
    /// <summary>
    /// Interaction logic for ProjectionPage.xaml
    /// </summary>
    public partial class ProjectionPage : UserControl, IPage
    {
        public IEnumerable<Course> Proyeccion { get => (IEnumerable<Course>)this.GetValue(ProyeccionProperty); set => this.SetValue(ProyeccionProperty, value); }
        public static readonly DependencyProperty ProyeccionProperty = DependencyProperty.Register(
            "Proyeccion", typeof(IEnumerable<Course>), typeof(ProjectionPage), new PropertyMetadata());

        public ProjectionPage()
        {
            InitializeComponent();
        }

        private async void DidLoad(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.AppStarting;
            try { Proyeccion = await ClientService.ProjectionAsync(); }
            catch (NoProyectionAvailableException) {
                SPUnavailable.Visibility = Visibility.Visible;
            }
            finally { Cursor = Cursors.Arrow; }
        }

        public async void Refresh()
        {
            Cursor = Cursors.Wait;
            try {
                await ClientService.FetchProjectionAsync();
                this.DidLoad(null, null);
            }
            catch {
                SPUnavailable.Visibility = Visibility.Visible;
                ICProjection.Visibility = Visibility.Collapsed;
                Cursor = Cursors.Arrow;
            }
        }
    }
}
