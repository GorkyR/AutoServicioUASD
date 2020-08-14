using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Constraints;
using Android.Util;
using Android.Views;
using Android.Widget;
using UASD;

namespace Client.Droid
{
    public class SelectedCourseItemView : ConstraintLayout
    {
        public SelectedCourseItemView(Context context, OpenCourse course, Action onRemove) : base(context)
        {
            Inflate(context, Resource.Layout.view_item_selected_course, this);

            FindViewById<TextView>(Resource.Id.text_title).Text   = course.Title;
            FindViewById<TextView>(Resource.Id.text_nrc).Text     = course.NRC;
            FindViewById<TextView>(Resource.Id.text_section).Text = course.Section;

            FindViewById<ImageButton>(Resource.Id.button_remove).Click += (s, e) => { onRemove(); };

            var layoutSchedule = FindViewById<LinearLayout>(Resource.Id.layout_class_schedule);

            foreach (var scheduledClass in course.Schedule)
                layoutSchedule.AddView(new AvailableCourseItemClassView(context, scheduledClass));
        }
    }
}