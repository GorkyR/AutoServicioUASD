using Android.Content;
using Android.Support.Constraints;
using Android.Widget;

using UASD;

namespace Client.Droid
{
    class ProjectionItem : ConstraintLayout
    {
        public ProjectionItem(Context context, Course course) : base(context)
        {
            Inflate(context, Resource.Layout.projection_item, this);
            SetPadding(0, 0, 0, Resources.GetDimensionPixelOffset(Resource.Dimension.projection_item_margin));

            var textTitle   = FindViewById<TextView>(Resource.Id.text_course_title);
            var textCode    = FindViewById<TextView>(Resource.Id.text_course_code);
            var textCredits = FindViewById<TextView>(Resource.Id.text_course_credits);

            textTitle.Text   = course.Title;
            textCode.Text    = course.Code;
            textCredits.Text = $"{course.Credits} crédito{(course.Credits == 1 ? "" : "s")}";
        }
    }
}