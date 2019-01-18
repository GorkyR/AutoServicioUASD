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
    /// Interaction logic for TopBar.xaml
    /// </summary>
    public partial class TopBar : UserControl
    {
        public string Nombre {
            get => (string)this.GetValue(NombreProperty);
            set => this.SetValue(NombreProperty, value);
        }
        public static readonly DependencyProperty NombreProperty = DependencyProperty.Register(
            "Nombre", typeof(string), typeof(TopBar), new PropertyMetadata());

        public Action ActualizarAction { get; set; }
        public Action LogoutAction { get; set; }

        public TopBar()
        {
            InitializeComponent();
        }

        private void ShowAbout(object sender, RoutedEventArgs e) => new AboutWindow().ShowDialog();

        private void Refresh(object sender, RoutedEventArgs e) => ActualizarAction();

        private void Logout(object sender, RoutedEventArgs e) => LogoutAction();
    }
}
