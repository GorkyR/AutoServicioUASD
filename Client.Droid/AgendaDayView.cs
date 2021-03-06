﻿using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Widget;
using UASD;

namespace Client.Droid
{
    public class AgendaDayView : LinearLayout
    {
        public AgendaDayView(Context context, string title, IEnumerable<CourseClassInstance> classes) : base(context)
        {
            Orientation = Orientation.Vertical;
            Inflate(context, Resource.Layout.view_item_agenda_day, this);
            SetPadding(0, 0, 0, Resources.GetDimensionPixelOffset(Resource.Dimension.agenda_gutters));

            var textDay         = FindViewById<TextView>(Resource.Id.text_day);
            var textNothing     = FindViewById<TextView>(Resource.Id.text_nothing);
            var layoutClassList = FindViewById<LinearLayout>(Resource.Id.layout_class_list);

            if (string.IsNullOrEmpty(title))
                textDay.Visibility = Android.Views.ViewStates.Gone;
            else
                textDay.Text = title;
            if (classes == null || classes.Count() == 0)
            {
                textNothing.Visibility = Android.Views.ViewStates.Visible;
                if (string.IsNullOrEmpty(title))
                    textNothing.Text = "Día libre.";
            }
            else
            {
                foreach (CourseClassInstance classInstance in classes)
                {
                    layoutClassList.AddView(new AgendaItemView(context, classInstance));
                }
            }
        }
    }
}