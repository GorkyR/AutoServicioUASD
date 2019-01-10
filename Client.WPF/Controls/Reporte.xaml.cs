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

namespace Client.WPF.Controls
{
    /// <summary>
    /// Interaction logic for Reporte.xaml
    /// </summary>
    public partial class Reporte : UserControl
    {
        public string Periodo   { get; set; } = "[Reporte Periodo]";
        public bool   Publicado { get => TIEstado.Estado; set => TIEstado.Estado = value; }

        public Reporte()
        {
            InitializeComponent();
        }
    }
}
