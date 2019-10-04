using Android.Content;
using Android.Views;
using Android.Widget;
using UASD;

namespace Client.Droid
{
    class ProjectionView : FrameLayout
    {
        public ProjectionView(Context context, CourseCollection projection) : base(context)
        {
            Inflate(context, Resource.Layout.view_projection, this);

            var textUnavailable = FindViewById<TextView>(Resource.Id.text_unavailable);
            var layoutProjection = FindViewById<LinearLayout>(Resource.Id.layout_projection_list);

            if (projection.Count == 0)
                textUnavailable.Visibility = ViewStates.Visible;
            else 
                foreach (Course course in projection)
                    layoutProjection.AddView( new ProjectionItemView(context, course) );
        }
    }
}