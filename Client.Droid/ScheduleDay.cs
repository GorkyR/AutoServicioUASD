using System;
using System.Collections.Generic;
using Android.Content;
using Android.Widget;
using UASD;
using Convert = UASD.Utilities.Convert;

namespace Client.Droid
{
    public class ScheduleDay : LinearLayout
    {
        public ScheduleDay(Context context, DayOfWeek dayOfWeek, List<CourseClassInstance> classes) : base(context)
        {
            Inflate(context, Resource.Layout.scheduled_day, this);
            SetPadding(0, 0, 0, Resources.GetDimensionPixelOffset(Resource.Dimension.agenda_gutters));

            var textDay = FindViewById<TextView>(Resource.Id.text_day);
            var textNothing = FindViewById<TextView>(Resource.Id.text_nothing);
            var layoutClassList = FindViewById<LinearLayout>(Resource.Id.layout_class_list);

            textDay.Text = Convert.Day(dayOfWeek);
            if (classes.Count == 0)
                textNothing.Visibility = Android.Views.ViewStates.Visible;
            else
            {
                foreach (CourseClassInstance classInstance in classes)
                    layoutClassList.AddView( new ScheduleItem(context, classInstance) );
            }
        }
    }
}