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
    /// Interaction logic for ToggleableImage.xaml
    /// </summary>
    public partial class ToggleableImage : UserControl
    {
        public ImageSource Image { get; set; }
        public ImageSource AlternativeImage { get; set; }
        private bool _estado;
        public bool Estado { get => _estado;
            set {
                _estado = value;
                if (Estado) {
                    I1.Visibility = Visibility.Visible;
                    I2.Visibility = Visibility.Collapsed;
                } else {
                    I1.Visibility = Visibility.Collapsed;
                    I2.Visibility = Visibility.Visible;
                }
            }
        }

        public ToggleableImage()
        {
            InitializeComponent();
        }
    }
}
