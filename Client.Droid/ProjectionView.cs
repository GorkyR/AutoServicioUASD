using Android.Content;
using Android.Views;
using Android.Widget;
using UASD;

namespace Client.Droid
{
    class ProjectionView : FrameLayout
    {
        public ProjectionView(Context context) : base(context)
        {
            Inflate(context, Resource.Layout.view_projection, this);

            FindViewById<TextView>(Resource.Id.text_unavailable).Visibility = ViewStates.Visible; 
        }

        public ProjectionView(Context context, CourseCollection projection) : base(context)
        {
            Inflate(context, Resource.Layout.view_projection, this);

            if (projection == null || projection.Count == 0)
            {
                var textUnavailable = FindViewById<TextView>(Resource.Id.text_unavailable);
                textUnavailable.Text = "No tienes materias proyectadas.";
                textUnavailable.Visibility = ViewStates.Visible;
            }

            var layoutProjection = FindViewById<LinearLayout>(Resource.Id.layout_projection_list);
            foreach (Course course in projection)
                layoutProjection.AddView( new ProjectionItemView(context, course) );
        }
    }
}