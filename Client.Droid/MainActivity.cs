using System;
using System.IO;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using static Client.Droid.StatePersistanceService;

namespace Client.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        public const string DatabaseName = "ClientState.bin";
        public static string PersonalFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        public static string DatabasePath = Path.Combine(PersonalFolder, DatabaseName);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            InitState();

            SetContentView(Resource.Layout.activity_main);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            var textUsername = navigationView.GetHeaderView(0).FindViewById<TextView>(Resource.Id.text_username);
            textUsername.Text = CurrentSession.Username;
        }

        private async void InitState()
        {
            using (var db = new LiteDB.LiteDatabase(DatabasePath))
            {
                var globals = db.GetCollection<Models.Global>();
                globals.EnsureIndex("key");

                void setupGlobal(string key, object value)
                {
                    if (!globals.Exists(g => g.Key == key))
                        globals.Insert(new Models.Global(key, value));
                }

                setupGlobal(nameof(IsLoggedIn), false);
                setupGlobal(nameof(LastIDUsed), "");
                setupGlobal(nameof(CurrentSession), null);
            }

            void LogoutAndTryAgain()
            {
                ClientStateService.ResetClientInformation();
                Recreate();
            }

            if (!IsLoggedIn)
            {
                StartActivityForResult(new Intent(this, typeof(LoginActivity)), 2705);
                return;
            }
            else
            {
                try
                {
                    await ClientStateService.AutoServicio.LoginAsync(
                        StatePersistanceService.CurrentSession.ID,
                        StatePersistanceService.CurrentSession.NIP
                    );
                    if (!ClientStateService.AutoServicio.IsLoggedIn)
                    {
                        LogoutAndTryAgain();
                        return;
                    }
                }
                catch { LogoutAndTryAgain(); return; }
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
                Recreate();
            else
                Finish();
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if(drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_about)
            {
                return true;
            }
            else if (id == Resource.Id.action_logout)
            {
                ClientStateService.ResetClientInformation();
                Toast.MakeText(this, "Sesión terminada", ToastLength.Short).Show();
                Recreate();
            }

            return base.OnOptionsItemSelected(item);
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_dashboard)
            {
                // Fragments here?
                Toast.MakeText(this, "Dashboard!", ToastLength.Short).Show();
            }
            else if (id == Resource.Id.nav_schedule)
            {
                Toast.MakeText(this, "Horario!", ToastLength.Short).Show();
            }
            else if (id == Resource.Id.nav_reports)
            {
                Toast.MakeText(this, "Reportes!", ToastLength.Short).Show();
            }
            else if (id == Resource.Id.nav_projection)
            {
                Toast.MakeText(this, "Proyección!", ToastLength.Short).Show();
            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

