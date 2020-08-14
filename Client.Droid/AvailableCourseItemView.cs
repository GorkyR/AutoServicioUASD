using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Constraints;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using UASD;

namespace Client.Droid
{
    public class AvailableCourseItemView : ConstraintLayout
    {
        public AvailableCourseItemView(Context context, OpenCourse course, Action onAdd) : base(context)
        {
            Inflate(context, Resource.Layout.view_item_available_course, this);

            FindViewById<TextView>(Resource.Id.text_nrc).Text = course.NRC;
            FindViewById<TextView>(Resource.Id.text_section).Text = course.Section;
            FindViewById<TextView>(Resource.Id.text_vacancy).Text = $"{course.Vacancy}";

            FindViewById<ImageButton>(Resource.Id.button_add).Click += (s, e) => { onAdd(); };

            var layoutSchedule = FindViewById<LinearLayout>(Resource.Id.layout_class_schedule);

            foreach (var scheduledClass in course.Schedule)
                layoutSchedule.AddView(new AvailableCourseItemClassView(context, scheduledClass));
        }
    }
}