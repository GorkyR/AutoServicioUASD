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

namespace Client.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public UserControl[] Pages =
        {
            new Pages.DashboardPage(),
            new Pages.SchedulePage(),
            new Pages.ReportPage(),
            new Pages.ProjectionPage(),
            new Pages.InformationPage(),
            new Pages.SelectionPage()
        };

        public MainWindow()
        {
            InitializeComponent();
            Page.Content = Pages[0];
            Test();
        }

        async void Test()
        {

        }

        private void Navigation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Page != null)
                Page.Content = Pages[NavigationPanel.SelectedIndex];
            return;
        }
    }
}
