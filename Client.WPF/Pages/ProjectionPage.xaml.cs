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
    public partial class ProjectionPage : UserControl
    {
        public IEnumerable<Course> Proyeccion { get => (IEnumerable<Course>)this.GetValue(ProyeccionProperty); set => this.SetValue(ProyeccionProperty, value); }
        public static readonly DependencyProperty ProyeccionProperty = DependencyProperty.Register(
            "Proyeccion", typeof(IEnumerable<Course>), typeof(ProjectionPage), new PropertyMetadata());

        public ProjectionPage()
        {
            InitializeComponent();
        }
    }
}
