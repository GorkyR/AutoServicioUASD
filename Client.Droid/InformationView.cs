using Android.Content;
using Android.Support.Constraints;
using Android.Widget;
using UASD;

namespace Client.Droid
{
    class InformationView : ConstraintLayout
    {
        public InformationView(Context context, CareerInformation information, float globalIndex) : base(context)
        {
            Inflate(context, Resource.Layout.view_information, this);

            var textCarreer        = FindViewById<TextView>(Resource.Id.text_carreer);
            var textIndex          = FindViewById<TextView>(Resource.Id.text_index);
            var textCredits        = FindViewById<TextView>(Resource.Id.text_credits);
            var textCourses        = FindViewById<TextView>(Resource.Id.text_courses);
            var textCreditsPercent = FindViewById<TextView>(Resource.Id.text_credits_percent);
            var textCoursesPercent = FindViewById<TextView>(Resource.Id.text_courses_percent);
            var progressCredits    = FindViewById<ProgressBar>(Resource.Id.progress_credits);
            var progressCourses    = FindViewById<ProgressBar>(Resource.Id.progress_courses);

            textCarreer.Text = information.Name;

            textIndex.Text = $"{globalIndex:N2}";

            textCredits.Text        = $"{information.Credits}/{information.RequiredCredits}";
            textCreditsPercent.Text = $"({information.CreditsPercentage:N2}%)";
            progressCredits.Max      = information.RequiredCredits;
            progressCredits.Progress = information.Credits;

            textCourses.Text        = $"{information.Courses}/{information.RequiredCourses}";
            textCoursesPercent.Text = $"({information.CoursesPercentage:N2}%)";
            progressCourses.Max      = information.RequiredCourses;
            progressCourses.Progress = information.Courses;
        }
    }
}