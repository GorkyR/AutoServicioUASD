using Android.Content;
using Android.Support.Constraints;
using Android.Views;
using Android.Widget;
using UASD;

namespace Client.Droid
{
    class ReportPeriod : ConstraintLayout
    {
        public ReportPeriod(Context context, AcademicPeriod academicPeriod) : base(context)
        {
            Inflate(context, Resource.Layout.report_period, this);
            SetPadding(0, 0, 0, Resources.GetDimensionPixelOffset(Resource.Dimension.period_gutters));

            var textPeriodName = FindViewById<TextView>(Resource.Id.text_period_name);
            var imageWaiting   = FindViewById<ImageView>(Resource.Id.image_period_waiting);
            var imageDone      = FindViewById<ImageView>(Resource.Id.image_period_done);
            var layoutGrades   = FindViewById<LinearLayout>(Resource.Id.layout_report_list);

            textPeriodName.Text = academicPeriod.Title;
            if (academicPeriod.IsActive)
                imageWaiting.Visibility = ViewStates.Visible;
            else
                imageDone.Visibility    = ViewStates.Visible;
            foreach(CourseGrade courseGrade in academicPeriod.Courses)
                layoutGrades.AddView( new ReportItem(context, courseGrade) );
        }
    }
}