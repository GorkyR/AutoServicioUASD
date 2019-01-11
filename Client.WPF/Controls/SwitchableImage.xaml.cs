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
    public partial class SwitchableImage : UserControl
    {
        public ImageSource FirstSource { get; set; }
        public ImageSource SecondSource { get; set; }
        public ImageSource ThirdSource { get; set; }

        private Image[] images;

        public int Estado { get => (int)this.GetValue(EstadoProperty); set => this.SetValue(EstadoProperty, value); }

        public static readonly DependencyProperty EstadoProperty = DependencyProperty.Register(
            "Estado", typeof(int), typeof(SwitchableImage), new PropertyMetadata(
                (d, args) => {
                    var switchableImage = (SwitchableImage)d;
                    foreach (var image in switchableImage.images)
                        image.Visibility = Visibility.Collapsed;
                    switchableImage.images[(int)args.NewValue].Visibility = Visibility.Visible;
                })
            );

        public SwitchableImage()
        {
            InitializeComponent();
            images = new[] { I1, I2, I3 };
            foreach (var image in images) {
                image.Visibility = Visibility.Collapsed;
            }
            I1.Visibility = Visibility.Visible;
        }
    }
}
