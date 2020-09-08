using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using UASD;
using UASD.Utilities;

namespace Client.Droid
{
    public class DashboardView : ScrollView
    {
        public DashboardView(
            Context context,
            string name,
            CareerInformation career,
            CourseCollection schedule,
            AcademicReport report) : base(context)
        {
            Inflate(context, Resource.Layout.view_dashboard, this);

            { // Greeting/information card
                var textGreeting = FindViewById<TextView>(Resource.Id.text_greeting);
                textGreeting.Text = string.Format(Resources.GetText(Resource.String.dashboard_greeting), name, career.Name);

                FindViewById<TextView>(Resource.Id.text_courses).Text = career.Courses.ToString();
                FindViewById<TextView>(Resource.Id.text_required_courses).Text = career.RequiredCourses.ToString();
                FindViewById<TextView>(Resource.Id.text_credits).Text = career.Credits.ToString();
                FindViewById<TextView>(Resource.Id.text_required_credits).Text = career.RequiredCredits.ToString();
                FindViewById<TextView>(Resource.Id.text_index).Text = report.GlobalIndex.ToString("N2");
            }

            if (schedule != null) { // Upcoming classes card
                var now = DateTime.Now;
                var today = now.DayOfWeek;
                var timeOfDay = now.TimeOfDay;

                if (today == DayOfWeek.Sunday)
                {
                    today = DayOfWeek.Monday;
                    timeOfDay = new TimeSpan();

                    FindViewById<TextView>(Resource.Id.text_caption_next_classes)
                        .SetText(Resource.String.dashboard_next_courses_tomorrow);
                }

                var todaysCourses = schedule.FilterByDay(today);
                var upcomingClasses = todaysCourses.SkipWhile(courseInstance =>
                    Math.Ceiling(courseInstance.Class.EndTime.TotalHours) <= timeOfDay.Hours
                );

                var layoutNextCourses = FindViewById<LinearLayout>(Resource.Id.layout_next_courses);

                if (upcomingClasses.Count() > 0)
                {
                    now = DateTime.Now;
                    today = now.DayOfWeek;
                    foreach (CourseClassInstance classInstance in upcomingClasses)
                    {
                        var instance = classInstance.Class;
                        bool ongoing = (today == instance.DayOfWeek) && (instance.StartTime.TotalHours <= now.TimeOfDay.TotalHours);

                        layoutNextCourses.AddView(new AgendaItemView(context, classInstance, ongoing));
                    }
                }
                else
                    layoutNextCourses.AddView(new AgendaDayView(context, string.Empty, new CourseClassInstance[] { }));
            } else {
                var layoutNextCourses = FindViewById<LinearLayout>(Resource.Id.layout_next_courses);
                layoutNextCourses.AddView(new AgendaDayView(context, string.Empty, new CourseClassInstance[] { }));
            }

            { // Active period card
                FindViewById<LinearLayout>(Resource.Id.layout_current_period).AddView(
                    new ReportPeriodView(context, report.Periods.First())
                );
            }
        }
    }
}