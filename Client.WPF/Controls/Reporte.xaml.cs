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
    /// Interaction logic for Reporte.xaml
    /// </summary>
    public partial class Reporte : UserControl
    {
        public string Periodo { get => (string)GetValue(PeriodoProperty); set => SetValue(PeriodoProperty, value); }
        public static readonly DependencyProperty PeriodoProperty = DependencyProperty.Register(
            "Periodo", typeof(string), typeof(Reporte), new PropertyMetadata("[Reporte notas]"));


        public bool Activo { get => (bool)GetValue(ActivoProperty); set => SetValue(ActivoProperty, value); }
        public static readonly DependencyProperty ActivoProperty = DependencyProperty.Register(
            "Activo", typeof(bool), typeof(Reporte), new PropertyMetadata(
                true,
                (DependencyObject d, DependencyPropertyChangedEventArgs a) =>
                {
                    var value = (bool)a.NewValue;
                    var reporte = (Reporte)d;
                    reporte.SIEstado.Estado = value ? 0 : 1;
                }
            ));


        public IEnumerable<CourseGrade> Notas { get => (IEnumerable<CourseGrade>)GetValue(NotasProperty); set => SetValue(NotasProperty, value); }
        public static readonly DependencyProperty NotasProperty = DependencyProperty.Register(
            "Notas", typeof(IEnumerable<CourseGrade>), typeof(Reporte), new PropertyMetadata());


        public Reporte()
        {
            InitializeComponent();
        }
    }
}
