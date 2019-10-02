using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using UASD;

namespace Client.Droid
{
    class Projection : FrameLayout
    {
        public Projection(Context context, CourseCollection projection) : base(context)
        {
            Inflate(context, Resource.Layout.projection, this);

            var layoutUnavailabe = FindViewById<LinearLayout>(Resource.Id.layout_unavailable);
            var layoutProjection = FindViewById<LinearLayout>(Resource.Id.layout_projection_list);

            if (projection.Count == 0)
                layoutUnavailabe.Visibility = ViewStates.Visible;
            else 
                foreach (Course course in projection)
                    layoutProjection.AddView( new ProjectionItem(context, course) );
        }
    }
}