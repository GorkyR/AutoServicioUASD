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
        public MainWindow()
        {
            InitializeComponent();
            // Page.Content = new Pages.DashboardPage();
            Test();
        }

        async void Test()
        {

        }

        private void Navigation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Page is null)
                return;
            switch (NavigationPanel.SelectedIndex) {
                case 1:
                    Page.Content = new Pages.SchedulePage(); break;
                case 2:
                    Page.Content = new Pages.ReportPage(); break;
                case 3:
                    Page.Content = new Pages.ProjectionPage(); break;
            }
            return;
        }
    }
}
