using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Client.Droid.Models;
using static Client.Droid.ClientStateService;
using IDs = Client.Droid.Resource.Id;

namespace Client.Droid
{
    [Activity(Label = "@string/app_name", MainLauncher = false)]
    public class LoginActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);

            var loginButton = FindViewById<Button>(IDs.button_login);
            var userEdit = FindViewById<EditText>(IDs.edit_user);
            var passwordEdit = FindViewById<EditText>(IDs.edit_password);
            var progressSpinner = FindViewById<ProgressBar>(IDs.progress_login);

            userEdit.Text = StatePersistanceService.LastIDUsed;

            loginButton.Click += async (s, e) => {
                string userID = userEdit.Text;
                string userPassword = passwordEdit.Text;
                if (string.IsNullOrWhiteSpace(userID)) {
                    userEdit.RequestFocus();
                    userEdit.Error = "Introduzca su matricula";
                    return;
                }
                else if (string.IsNullOrEmpty(userPassword))
                {
                    passwordEdit.RequestFocus();
                    passwordEdit.Error = "Introduzca su NIP";
                    return;
                }

                var toastGood  = Toast.MakeText(this, "Acceso!"                   , ToastLength.Short);
                var toastBad   = Toast.MakeText(this, "Matricula o NIP incorrecto", ToastLength.Short);
                var toastError = Toast.MakeText(this, "Error al intentar accesar" , ToastLength.Long);

                try
                {
                    progressSpinner.Visibility = ViewStates.Visible;
                    await AutoServicio.LoginAsync(userEdit.Text, passwordEdit.Text);
                    progressSpinner.Visibility = ViewStates.Gone;
                }
                catch (Exception error)
                {
                    progressSpinner.Visibility = ViewStates.Gone;
                    toastError.Show();
                    return;
                }

                if (AutoServicio.IsLoggedIn) {
                    StatePersistanceService.IsLoggedIn = true;
                    StatePersistanceService.LastIDUsed = userID;
                    StatePersistanceService.CurrentSession = new SessionInformation(
                        AutoServicio.Username,
                        userID, userPassword
                    );
                    toastGood.Show();
                    SetResult(Result.Ok);
                    Finish();
                }
                else
                {
                    toastBad.Show();
                }
            };
        }
        public override void OnBackPressed()
        {
            SetResult(Result.Canceled);
            Finish();
        }
    }
}