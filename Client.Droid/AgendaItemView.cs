using Android.App;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Widget;
using System.Linq;
using UASD;
using Convert = UASD.Utilities.Convert;

namespace Client.Droid
{
    public class AgendaItemView : LinearLayout
    {
        public AgendaItemView(Context context, CourseClassInstance courseInstance): base(context)
        {
            Orientation = Orientation.Vertical;
            Inflate(context, Resource.Layout.view_item_agenda, this);

            Course course = courseInstance.Course;
            CourseClass courseClass = courseInstance.Class;

            var textHour     = FindViewById<TextView>(Resource.Id.text_hour);
            var textTitle    = FindViewById<TextView>(Resource.Id.text_title);
            var textInfo     = FindViewById<TextView>(Resource.Id.text_info);
            var textLocation = FindViewById<TextView>(Resource.Id.text_location);
            var cardClass    = FindViewById<CardView>(Resource.Id.card_class);

            textHour.Text     = Convert.Time(courseClass.StartTime);
            textTitle.Text    = course.Title;
            textInfo.Text     = $"{course.Code} - {course.Section}";
            textLocation.Text = Convert.Place(courseClass.Place);

            cardClass.Click += (s, e) =>
            {
                var classModal = new Dialog(context);
                classModal.SetContentView(Resource.Layout.modal_class_information);

                var modalTextTitle    = classModal.FindViewById<TextView>(Resource.Id.text_title);
                var modalTextInfo     = classModal.FindViewById<TextView>(Resource.Id.text_info);
                var modalTextLocation = classModal.FindViewById<TextView>(Resource.Id.text_location);
                var textHours         = classModal.FindViewById<TextView>(Resource.Id.text_hour);
                var textProfessor     = classModal.FindViewById<TextView>(Resource.Id.text_professor);
                var textCredits       = classModal.FindViewById<TextView>(Resource.Id.text_credits);
                var textNRC           = classModal.FindViewById<TextView>(Resource.Id.text_id);

                modalTextTitle.Text = course.Title;
                modalTextInfo.Text = $"{course.Code} - {course.Section}";

                {
                    var splitLocation = courseClass.Place.Split();
                    string locationBuilding = string.Join(" ", splitLocation.SkipLast(1));
                    string locationSmall = Convert.Place(courseClass.Place).Split()[0];
                    string locationRoom = splitLocation.Last();
                    modalTextLocation.Text = $"{locationBuilding} ({locationSmall}) {locationRoom}";
                }

                {
                    string start = Convert.Time(courseClass.StartTime);
                    string end = Convert.Time(courseClass.EndTime);
                    int duration = courseClass.Duration;
                    textHours.Text = $"{start} – {end} ({duration} hora{(duration == 1 ? "" : "s")})";
                }

                textProfessor.Text = course.Professor;
                textCredits.Text = $"{course.Credits} crédito{(course.Credits == 1? "" : "s")}";
                textNRC.Text = $"{course.NRC}";

                classModal.Show();
            };
        }
    }
}