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
    /// Interaction logic for ReportWidget.xaml
    /// </summary>
    public partial class ReportWidget : UserControl
    {
        public AcademicPeriod Periodo {
            get => (AcademicPeriod)this.GetValue(PeriodoProperty);
            set => this.SetValue(PeriodoProperty, value);
        }
        public static readonly DependencyProperty PeriodoProperty = DependencyProperty.Register(
            "Periodo", typeof(AcademicPeriod), typeof(ReportWidget), new PropertyMetadata());

        public ReportWidget()
        {
            InitializeComponent();
        }

        private async void DidLoad(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.AppStarting;
            var reporte = await ClientService.ReportAsync();
            Periodo = reporte.Periods.First();
            Cursor = Cursors.Arrow;
        }
    }
}
