using System;
using System.IO;
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
using Android.Text.Method;
using System.Linq;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Util;

namespace Client.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        public const string DatabaseName = "ClientState.bin";
        public static string PersonalFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        public static string DatabasePath = Path.Combine(PersonalFolder, DatabaseName);

        public LinearLayout MainContent;
        public DrawerLayout Drawer;

        private NavigationView Navigation;

	    private Android.Support.V7.Widget.Toolbar AppBar;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            bool goodState = await InitState();
            if (!goodState)
                return;

            SetContentView(Resource.Layout.activity_main);
            MainContent = FindViewById<LinearLayout>(Resource.Id.main_content);

            AppBar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(AppBar);

            Drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, Drawer, AppBar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            Drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            Navigation = FindViewById<NavigationView>(Resource.Id.nav_view);
            Navigation.SetNavigationItemSelectedListener(this);
            Navigation.SetCheckedItem(Resource.Id.nav_dashboard);
            OnNavigationItemSelected(Navigation.Menu.FindItem(Resource.Id.nav_dashboard));

            var textUsername = Navigation.GetHeaderView(0).FindViewById<TextView>(Resource.Id.text_username);
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

            if (!IsLoggedIn)
            {
                StartActivityForResult(new Intent(this, typeof(LoginActivity)), 1);
                return false;
            }
            else
            {
                try
                {
                    ClientStateService.dataSource = ClientStateService.fakeUsers.GetValueOrDefault(CurrentSession.ID);
                    if (ClientStateService.dataSource == ClientStateService.DataSource.Production)
                    {
                        await ClientStateService.AutoServicio.LoginAsync(
                            CurrentSession.ID,
                            CurrentSession.NIP
                        );
                    }
                    if (ClientStateService.dataSource == ClientStateService.DataSource.Production
                        && !ClientStateService.AutoServicio.IsLoggedIn)
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
            else if (id == Resource.Id.action_refresh)
            {
                // TODO: refresh data
                var nav = Navigation.CheckedItem.ItemId;
                if (nav == Resource.Id.nav_dashboard)
                {
                    ClientStateService.ForgetSchedule();
                    ClientStateService.ForgetReport();
                    ClientStateService.ForgetCareerInformation();
                }
                else if (nav == Resource.Id.nav_agenda)
                {
                    ClientStateService.ForgetSchedule();
                }
                else if (nav == Resource.Id.nav_reports)
                {
                    ClientStateService.ForgetReport();
                    ClientStateService.ForgetCareerInformation();
                }
                else if (nav == Resource.Id.nav_projection)
                {
                    ClientStateService.ForgetProjection();
                }
                else if (nav == Resource.Id.nav_selection)
                {
                    ClientStateService.ForgetSelectionCalendar();
                }
                OnNavigationItemSelected(Navigation.CheckedItem);
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
            else if (id == Resource.Id.nav_selection)
            {
                SetupSelection();
            }

            return true;
        }

        async void SetupDashboard()
        {
            AppBar.SetTitle(Resource.String.app_name);
            MainContent.RemoveAllViews();

            CourseCollection  schedule    = null;
            AcademicReport    report      = null;
            CareerInformation information = null;
            var firstName = CurrentSession.Username.Split().First();

            try
            {
                schedule = await ClientStateService.ScheduleAsync();
            }
            catch (NotLoggedInException) { LogoutAndTryAgain(); }
            catch (NoDataReceivedException) { }
            catch { }

            try
            {
                report = await ClientStateService.ReportAsync();
                information = await ClientStateService.CareerInformationAsync();
            }
            catch (NotLoggedInException) { LogoutAndTryAgain(); }
            catch { }

            MainContent.AddView(new DashboardView(this, firstName, information, schedule, report));
        }

        async void SetupAgenda()
        {
            AppBar.SetTitle(Resource.String.menu_schedule);
            MainContent.RemoveAllViews();
            try
            {
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
                    var virtualTitle  = new TextView(this)     { Text = "Virtuales" };
                    virtualTitle.SetTextAppearance(Resource.Style.TextAppearance_AppCompat_Large);
                    virtualTitle.Typeface = Android.Graphics.Typeface.DefaultBold;
                    virtualTitle.SetTextColor(Resources.GetColor(Resource.Color.material_grey_600));
                    virtualTitle.SetPadding(0, 0, 0, 16);
                    virtualLayout.AddView(virtualTitle);

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
            catch (NotLoggedInException) { LogoutAndTryAgain(); }
            catch (NoDataReceivedException)
            {
                var textUnavailable = new TextView(this) {
                    LayoutParameters = new FrameLayout.LayoutParams(
                        DP(256),
                        FrameLayout.LayoutParams.WrapContent
                    )
                    { Gravity = GravityFlags.Center },
                    Gravity = GravityFlags.Center
                };
                textUnavailable.SetText(Resource.String.unavailable_schedule);
                textUnavailable.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.no_disponible_sized, 0, 0);
                textUnavailable.CompoundDrawablePadding = DP(24);
                textUnavailable.SetTextAppearance(Resource.Style.TextAppearance_AppCompat_Large);
                textUnavailable.SetTextColor(Resources.GetColor(Resource.Color.material_grey_600));
                var frameUnavailable = new FrameLayout(this);
                frameUnavailable.AddView(textUnavailable);
                MainContent.AddView(frameUnavailable);
            }
        }

        async void SetupReports()
        {
            AppBar.SetTitle(Resource.String.menu_reports);
            MainContent.RemoveAllViews();
            try
            {
                var report      = await ClientStateService.ReportAsync();
                var information = await ClientStateService.CareerInformationAsync();

                // Carreer information
                {
                    var informationView = new InformationView(this, information, report.GlobalIndex);
                    informationView.SetPadding(0, 0, 0, Resources.GetDimensionPixelOffset(Resource.Dimension.dashboard_gutters));
                    MainContent.AddView(informationView);
                }
                // Grades
                {
                    foreach (AcademicPeriod academicPeriod in report.Periods)
                    {
                        var card = new CardView(this) { CardElevation = 16 };
                        var cardLayoutParams = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.MatchParent, FrameLayout.LayoutParams.MatchParent);
                        cardLayoutParams.SetMargins(16, 16, 16, 16);
                        card.LayoutParameters = cardLayoutParams;
                        card.SetContentPadding(16, 16, 16, 16);
                        card.Radius = DP(6);

                        card.AddView(new ReportPeriodView(this, academicPeriod));

                        MainContent.AddView(card);
                    }
                }
            }
            catch (NotLoggedInException) { LogoutAndTryAgain(); }
        }

        async void SetupProjection()
        {
            AppBar.SetTitle(Resource.String.menu_projection);
            MainContent.RemoveAllViews();
            try
            {
                var projection = await ClientStateService.ProjectionAsync();
                MainContent.AddView(new ProjectionView(this, projection));
            }
            catch (NotLoggedInException) { LogoutAndTryAgain(); }
            catch (NoProyectionAvailableException)
            {
                MainContent.AddView(new ProjectionView(this));
            }
        }

        async void SetupSelection()
        {
            AppBar.SetTitle(Resource.String.menu_selection);
            MainContent.RemoveAllViews();

            List<DateTimeRange>    selectionCalendar = null;
            List<CourseCollection> availableCourses  = null;

            try
            {
                selectionCalendar = await ClientStateService.SelectionCalendarAsync();
            }
            catch (NotLoggedInException) { LogoutAndTryAgain(); }
            catch { }

            try
            {
                availableCourses = await ClientStateService.AvailableCoursesAsync();
            }
            catch (NotLoggedInException) { LogoutAndTryAgain(); }
            catch (Exception e) { }

            MainContent.AddView(new SelectionView(this, availableCourses, selectionCalendar, () => {
                //Navigation.SetCheckedItem(Resource.Id.nav_dashboard);
                //OnNavigationItemSelected(Navigation.Menu.FindItem(Resource.Id.nav_dashboard));
            }));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void LogoutAndTryAgain()
        {
            Toast.MakeText(this, Resource.String.not_logged_in_message, ToastLength.Long).Show();
            ClientStateService.ResetClientInformation();
            Recreate();
        }

        private int DP(float value) => (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, value, Resources.DisplayMetrics);
    }

    public static class Utilities
    {
        public static int DP(this View view, float value) => (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, value, view.Resources.DisplayMetrics);
    }
}