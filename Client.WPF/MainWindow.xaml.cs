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
        public Type[] Pages =
        {
            typeof(Pages.DashboardPage),
            typeof(Pages.SchedulePage),
            typeof(Pages.ReportPage),
            typeof(Pages.ProjectionPage),
            typeof(Pages.InformationPage),
            typeof(Pages.SelectionPage)
        };

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        async void Init()
        {
            if (!StateService.IsLoggedIn)
            {
                var loginWindow = new LoginWindow();
                if (!(loginWindow.ShowDialog() ?? false))
                    this.Close();
            }
            else
            {
                void LogoutAndTryAgain() {
                    StateService.IsLoggedIn = false;
                    StateService.ResetSession();
                    Init();
                }

                try {
                    await ClientService.AutoServicio.LoginAsync(
                        StateService.CurrentSession.ID,
                        StateService.CurrentSession.NIP
                    );
                    if (!ClientService.AutoServicio.IsLoggedIn)
                    {
                        LogoutAndTryAgain();
                        return;
                    }
                    Console.WriteLine($"[i] Already logged in. ID: {StateService.CurrentSession.ID}");
                }
                catch { LogoutAndTryAgain(); return; }
            }
            Page.Content = Pages[0].New();

        }

        private void Navigation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Page != null) {
                Page.Content = Pages[NavigationPanel.SelectedIndex].New();
            }
            return;
        }


    }

    internal static class TypeExtension
    {
        public static UserControl New(this Type type, params object[] parameters)
        {
            return type.GetConstructors()[0].Invoke(parameters ?? null) as UserControl;
        }
    }
}
