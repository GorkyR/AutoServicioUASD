using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Text.Method;
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
            var textAboutClickable = FindViewById<TextView>(IDs.text_about_clickable);

            userEdit.Text = StatePersistanceService.LastIDUsed;

            textAboutClickable.Click += (s, e) =>
            {
                var aboutDialog = new Dialog(this);
                aboutDialog.SetContentView(Resource.Layout.modal_about);
                var textAuthor = aboutDialog.FindViewById<TextView>(Resource.Id.text_author);
                var textVersion = aboutDialog.FindViewById<TextView>(Resource.Id.text_version);

                textAuthor.MovementMethod = LinkMovementMethod.Instance;
                string versionName = PackageManager.GetPackageInfo(PackageName, 0).VersionName;
                textVersion.Text = $"v{versionName}";

                aboutDialog.Show();
            };
            loginButton.Click += async (s, e) => {
                string userID = userEdit.Text;
                string userPassword = passwordEdit.Text;
                if (string.IsNullOrWhiteSpace(userID)) {
                    userEdit.RequestFocus();
                    userEdit.Error = GetString(Resource.String.login_error_user);
                    return;
                }
                else if (string.IsNullOrEmpty(userPassword))
                {
                    passwordEdit.RequestFocus();
                    passwordEdit.Error = GetString(Resource.String.login_error_password);
                    return;
                }

                var toastFail      = Toast.MakeText(this, GetString(Resource.String.login_fail)     , ToastLength.Short);
                var toastException = Toast.MakeText(this, GetString(Resource.String.login_exception), ToastLength.Long);

                try
                {
                    progressSpinner.Visibility = ViewStates.Visible;
                    dataSource = fakeUsers.GetValueOrDefault(userID);
                    if (dataSource == DataSource.Production)
                        await AutoServicio.LoginAsync(userEdit.Text, passwordEdit.Text);
                    progressSpinner.Visibility = ViewStates.Gone;
                }
                catch (Exception)
                {
                    progressSpinner.Visibility = ViewStates.Gone;
                    toastException.Show();
                    return;
                }

                if (dataSource != DataSource.Production || AutoServicio.IsLoggedIn) {
                    StatePersistanceService.IsLoggedIn = true;
                    StatePersistanceService.LastIDUsed = userID;
                    StatePersistanceService.CurrentSession = new SessionInformation(
                        dataSource == DataSource.Production ? AutoServicio.Username : "Usuario de Prueba",
                        userID, userPassword
                    );
                    SetResult(Result.Ok);
                    Finish();
                }
                else
                {
                    toastFail.Show();
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