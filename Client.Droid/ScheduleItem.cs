using Android.Content;
using Android.Widget;
using UASD;
using Convert = UASD.Utilities.Convert;

namespace Client.Droid
{
    public class ScheduleItem : LinearLayout
    {
        public ScheduleItem(Context context, CourseClassInstance courseInstance): base(context)
        {
            Inflate(context, Resource.Layout.schedule_class_item, this);
            var textHour             = FindViewById<TextView>(Resource.Id.text_hour);
            var textClassTitle       = FindViewById<TextView>(Resource.Id.text_course_title);
            var textInstanceLocation = FindViewById<TextView>(Resource.Id.text_class_location);
            var textClassCode        = FindViewById<TextView>(Resource.Id.text_course_code);

            textHour.Text             = Convert.Time(courseInstance.Class.StartTime);
            textClassTitle.Text       = courseInstance.Course.Title;
            textInstanceLocation.Text = Convert.Place(courseInstance.Class.Place);
            textClassCode.Text        = courseInstance.Course.Code;
        }
    }
}