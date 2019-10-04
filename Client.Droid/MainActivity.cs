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
using UASD;
using UASD.Utilities;
using Convert = UASD.Utilities.Convert;
using System.Threading.Tasks;
using Android.Support.V7.View.Menu;
using Android.Text.Method;
using System.Linq;

namespace Client.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        public const string DatabaseName = "ClientState.bin";
        public static string PersonalFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        public static string DatabasePath = Path.Combine(PersonalFolder, DatabaseName);

        public LinearLayout MainContent;
        public DrawerLayout Drawer;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            bool goodState = await InitState();
            if (!goodState)
                return;

            SetContentView(Resource.Layout.activity_main);
            MainContent = FindViewById<LinearLayout>(Resource.Id.main_content);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            Drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, Drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            Drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            navigationView.SetCheckedItem(Resource.Id.nav_dashboard);
            OnNavigationItemSelected(navigationView.Menu.FindItem(Resource.Id.nav_dashboard));

            var textUsername = navigationView.GetHeaderView(0).FindViewById<TextView>(Resource.Id.text_username);
            textUsername.Text = CurrentSession.Username;
        }

        private async Task<bool> InitState()
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
                return false;
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
                        return false;
                    }
                    return true;
                }
                catch { LogoutAndTryAgain(); return false; }
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
            //if (menu is MenuBuilder)
            //    (menu as MenuBuilder).SetOptionalIconsVisible(true);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_about)
            {
                var aboutDialog = new Dialog(this);
                aboutDialog.SetContentView(Resource.Layout.modal_about);
                var textAuthor = aboutDialog.FindViewById<TextView>(Resource.Id.text_author);
                var textVersion = aboutDialog.FindViewById<TextView>(Resource.Id.text_version);

                textAuthor.MovementMethod = LinkMovementMethod.Instance;
                string versionName = PackageManager.GetPackageInfo(PackageName, 0).VersionName;
                textVersion.Text = $"v{versionName}";

                aboutDialog.Show();
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

            Drawer.CloseDrawer(GravityCompat.Start);

            if (id == Resource.Id.nav_dashboard)
            {
                SetupDashboard();
            }
            else if (id == Resource.Id.nav_agenda)
            {
                SetupAgenda();
            }
            else if (id == Resource.Id.nav_reports)
            {
                SetupReports();
            }
            else if (id == Resource.Id.nav_projection)
            {
                SetupProjection();
            }

            return true;
        }

        async void SetupDashboard()
        {
            MainContent.RemoveAllViews();
            var information = await ClientStateService.CareerInformationAsync();
            var schedule = await ClientStateService.ScheduleAsync();
            var report = await ClientStateService.ReportAsync();

            // Today's agenda
            {
                var now = DateTime.Now;
                var today = now.DayOfWeek;
                var timeOfDay = now.TimeOfDay;
                string title = "Hoy";

                if (today == DayOfWeek.Sunday)
                {
                    today = DayOfWeek.Monday;
                    timeOfDay = new TimeSpan();
                    title = "Mañana";
                }

                var todaysCourses = schedule.FilterByDay(today);
                var upcomingClasses = todaysCourses.SkipWhile(courseInstance =>
                    Math.Ceiling(courseInstance.Class.EndTime.TotalHours) <= timeOfDay.Hours
                );
                var agendaDayView = new AgendaDayView(this, title, upcomingClasses);
                agendaDayView.SetPadding(0, 0, 0, Resources.GetDimensionPixelOffset(Resource.Dimension.dashboard_gutters));
                MainContent.AddView(agendaDayView);
            }
            // Carreer information
            {
                var informationView = new InformationView(this, information, report.GlobalIndex);
                informationView.SetPadding(0, 0, 0, Resources.GetDimensionPixelOffset(Resource.Dimension.dashboard_gutters));
                MainContent.AddView(informationView);
            }
            // Current period's report
            {
                var reportPeriodView = new ReportPeriodView(this, report.Periods.First());
                var activePeriodLayout = new LinearLayout(this) { Orientation = Orientation.Vertical };
                var titleText = new TextView(activePeriodLayout.Context) { Text = "Período Activo" } ;
                titleText.SetTextAppearance(Resource.Style.TextAppearance_AppCompat_Large);
                titleText.SetTextColor(Resources.GetColor(Resource.Color.material_grey_600));
                titleText.Typeface = Android.Graphics.Typeface.DefaultBold;
                activePeriodLayout.AddView( titleText );
                activePeriodLayout.AddView( reportPeriodView );
                MainContent.AddView(activePeriodLayout);
            }
        }

        async void SetupAgenda()
        {
            MainContent.RemoveAllViews();
            var schedule = await ClientStateService.ScheduleAsync();

            var agendaLayout = new LinearLayout(this) { Orientation = Orientation.Vertical };
            agendaLayout.SetDividerDrawable(Resources.GetDrawable(Resource.Drawable.divider));
            agendaLayout.ShowDividers = ShowDividers.Middle;

            var inPersonLayout = new LinearLayout(this) { Orientation = Orientation.Vertical };
            foreach (DayOfWeek day in Convert.Days.Values)
            {
                var coursesInDay = schedule.FilterByDay(day);
                var title = Convert.Day(day);
                inPersonLayout.AddView(new AgendaDayView(this, title, coursesInDay));
            }
            agendaLayout.AddView(inPersonLayout);

            var virtualCourses = schedule.Where(c => c.Schedule.Count == 0);
            if (virtualCourses.Count() > 0)
            {
                var virtualLayout = new LinearLayout(this) { Orientation = Orientation.Vertical };
                foreach (var course in virtualCourses)
                {
                    var itemLayout = new FrameLayout(this);
                    var item = LayoutInflater.Inflate(Resource.Layout.view_class_card, itemLayout, true);

                    item.FindViewById<TextView>(Resource.Id.text_title).Text = course.Title;
                    item.FindViewById<TextView>(Resource.Id.text_info).Text = $"{course.Code} - {course.Section}";
                    item.FindViewById<TextView>(Resource.Id.text_location).Visibility = ViewStates.Gone;

                    item.Click += (s, e) =>
                    {
                        var classModal = new Dialog(this);
                        classModal.SetContentView(Resource.Layout.modal_class_information);

                        classModal.FindViewById<TextView>(Resource.Id.text_title).Text = course.Title;
                        classModal.FindViewById<TextView>(Resource.Id.text_info).Text = $"{course.Code} - {course.Section}";
                        classModal.FindViewById<TextView>(Resource.Id.text_professor).Text = course.Professor;
                        classModal.FindViewById<TextView>(Resource.Id.text_credits).Text = $"{course.Credits} crédito{(course.Credits == 1 ? "" : "s")}";
                        classModal.FindViewById<TextView>(Resource.Id.text_id).Text = $"{course.NRC}";

                        classModal.FindViewById<TableRow>(Resource.Id.row_location).Visibility = ViewStates.Gone;
                        classModal.FindViewById<TableRow>(Resource.Id.row_hour).Visibility = ViewStates.Gone;

                        classModal.Show();
                    };

                    virtualLayout.AddView(item);
                }
                agendaLayout.AddView(virtualLayout);
            }

            MainContent.AddView(agendaLayout);
        }

        async void SetupReports()
        {
            MainContent.RemoveAllViews();
            var report = await ClientStateService.ReportAsync();
            foreach (AcademicPeriod academicPeriod in report.Periods)
                MainContent.AddView( new ReportPeriodView(this, academicPeriod) );
        }

        async void SetupProjection()
        {
            MainContent.RemoveAllViews();
            var projection = await ClientStateService.ProjectionAsync();
            MainContent.AddView( new ProjectionView(this, projection) );
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

