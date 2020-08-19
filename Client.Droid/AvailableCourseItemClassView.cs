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
using static UASD.Utilities.Convert;

namespace Client.Droid
{
    public class AvailableCourseItemClassView : LinearLayout
    {
        public AvailableCourseItemClassView(Context context, CourseClass courseClass) : base(context)
        {
            Inflate(context, Resource.Layout.view_item_available_course_class, this);

            FindViewById<TextView>(Resource.Id.text_day).Text        = Day(courseClass.DayOfWeek);
//            FindViewById<TextView>(Resource.Id.text_hour_range).Text = $"{Time(courseClass.StartTime,true)} - {Time(courseClass.EndTime,true)} ({courseClass.Duration}h)";
            FindViewById<TextView>(Resource.Id.text_hour_range).Text = $"{Time(courseClass.StartTime,true)} - {Time(courseClass.EndTime,true)}";
            FindViewById<TextView>(Resource.Id.text_place).Text      = Place(courseClass.Place);
        }
    }
}