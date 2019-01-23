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
    /// Interaction logic for CurrentTimeIndicator.xaml
    /// </summary>
    public partial class CurrentTimeIndicator : UserControl
    {
        public int Slot { get => (int)this.GetValue(SlotProperty); set => this.SetValue(SlotProperty, value); }
        public static readonly DependencyProperty SlotProperty = DependencyProperty.Register(
            "Slot", typeof(int), typeof(CurrentTimeIndicator), new PropertyMetadata(0));

        public CurrentTimeIndicator()
        {
            InitializeComponent();
        }
    }
}
