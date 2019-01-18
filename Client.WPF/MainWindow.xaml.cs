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
            void LogoutAndTryAgain() {
                StateService.IsLoggedIn = false;
                StateService.ResetSession();
                Init();
            }

            if (!StateService.IsLoggedIn)
            {
                var loginWindow = new LoginWindow();
                if (!(loginWindow.ShowDialog() ?? false))
                    this.Close();
            }
            else
            {
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

            TopBar.Nombre = ClientService.AutoServicio.Username;
            TopBar.LogoutAction = LogoutAndTryAgain;
            Page.Content = InitPage(Pages[0]);
            NavigationPanel.SelectedIndex = 0;
        }

        private void Navigation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = NavigationPanel.SelectedIndex;
            if (Page != null) {
                Page.Content = InitPage(Pages[index]);
            }
            return;
        }

        private UserControl InitPage(Type type, params object[] parameters) {
            var page = type.GetConstructors()[0].Invoke(parameters ?? null) as UserControl;
            TopBar.ActualizarAction = (page as Pages.IPage).Refresh;
            return page;
        }
    }
}
