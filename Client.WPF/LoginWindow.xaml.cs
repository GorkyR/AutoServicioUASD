using Client.WPF.Models;
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
using System.Windows.Shapes;

namespace Client.WPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            if (!string.IsNullOrWhiteSpace(StateService.LastIDUsed))
                Console.WriteLine($"[i] Last used ID: {StateService.LastIDUsed}");
            TBMatricula.Text = StateService.LastIDUsed;
        }

        private void ShowError(string message)
        {
            TBError.Text = message;
            TBError.Visibility = Visibility.Visible;
        }
        private void ClearError()
        {
            TBError.Visibility = Visibility.Hidden;
        }
        private void ShowProgressBar(bool show = true)
        {
            PBar.Visibility = show? Visibility.Visible : Visibility.Hidden;
        }

        private async void Login(object sender, RoutedEventArgs e)
        {
            ClearError();
            var matricula = TBMatricula.Text;
            var nip = PBNIP.Password;

            if (String.IsNullOrWhiteSpace(matricula)) {
                ShowError("Ingrese su matrícula");
                TBMatricula.Focus();
                return;
            } else if (String.IsNullOrWhiteSpace(nip)) {
                ShowError("Ingrese su NIP");
                PBNIP.Focus();
                return;
            }

            try {
                ShowProgressBar();
                await ClientService.AutoServicio.LoginAsync(matricula, nip);
                ShowProgressBar(false);
            }
            catch { ShowError("No hay conexión a Internet"); return; }

            if (ClientService.AutoServicio.IsLoggedIn) {
                StateService.IsLoggedIn = true;
                StateService.LastIDUsed = matricula;
                StateService.CurrentSession = new SessionInformation(
                    ClientService.AutoServicio.Username,
                    matricula, nip
                );
                this.DialogResult = true;
                this.Close();
            } else {
                ShowError("Matrícula o NIP incorrectos.");
            }
        }
    }
}
