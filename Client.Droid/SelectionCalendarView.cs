using System;
using System.Collections.Generic;
using Android.Content;
using Android.Widget;
using UASD;
using static UASD.Utilities.Convert;

namespace Client.Droid
{
    public class SelectionCalendarView : LinearLayout
    {
        public SelectionCalendarView(Context context, List<DateTimeRange> selectionCalendar) : base(context)
        {
            Inflate(context, Resource.Layout.view_selection_calendar, this);

            string dateTimeToString(DateTime date)
            {
                string[] months = new[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

                return $"{months[date.Month].Substring(0, 3)} {date.Day:00}, {date.Year}, {Time(date.TimeOfDay, shorten: true)}";
            }

            var layoutStartDate = FindViewById<LinearLayout>(Resource.Id.layout_start_date);
            var layoutEndDate   = FindViewById<LinearLayout>(Resource.Id.layout_end_date);

            foreach (var range in selectionCalendar)
            {
                layoutStartDate.AddView(new TextView(context) { Text = dateTimeToString(range.StartDate) });
                layoutEndDate.AddView(new TextView(context)   { Text = $" → {dateTimeToString(range.EndDate)}" });
            }
        }
    }
}