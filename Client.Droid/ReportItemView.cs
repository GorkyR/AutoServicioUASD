﻿using Android.Content;
using Android.Support.Constraints;
using Android.Views;
using Android.Widget;
using UASD;

namespace Client.Droid
{
    class ReportItemView : ConstraintLayout
    {
        public ReportItemView(Context context, GradedCourse courseGrade) : base(context)
        {
            Inflate(context, Resource.Layout.view_item_report, this);
            SetPadding(0, 0, 0, Resources.GetDimensionPixelOffset(Resource.Dimension.grade_gutter));
            var textTitle    = FindViewById<TextView>(Resource.Id.text_course_title);
            var textCode     = FindViewById<TextView>(Resource.Id.text_course_code);
            var textCredits  = FindViewById<TextView>(Resource.Id.text_course_credits);
            var imageAbsent  = FindViewById<ImageView>(Resource.Id.image_grade_absent);
            var imageWaiting = FindViewById<ImageView>(Resource.Id.image_grade_waiting);
            var textGrade    = FindViewById<TextView>(Resource.Id.text_course_grade);

            textTitle.Text   = courseGrade.Title;
            textCode.Text    = courseGrade.Code;
            textCredits.Text = $"{courseGrade.Credits} crédito{(courseGrade.Credits == 1? "" : "s")}";
            switch(courseGrade.State)
            {
                case GradedCourse.CourseState.NotPublished:
                    imageWaiting.Visibility = ViewStates.Visible;
                    break;
                case GradedCourse.CourseState.Absent:
                    imageAbsent.Visibility = ViewStates.Visible;
                    break;
                case GradedCourse.CourseState.Published:
                    textGrade.Text       = $"{courseGrade.Grade}";
                    textGrade.Visibility = ViewStates.Visible;
                    break;
            }
        }
    }
}