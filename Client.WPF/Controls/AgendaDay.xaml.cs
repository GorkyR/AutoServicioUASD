using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UASD;
using static UASD.Utilities.Convert;

namespace Client.WPF.Controls
{
    /// <summary>
    /// Interaction logic for AgendaSchedule.xaml
    /// </summary>
    public partial class AgendaDay : UserControl
    {
        public DayOfWeek DayOfWeek {
            get => (DayOfWeek)this.GetValue(DayOfWeekProperty);
            set => this.SetValue(DayOfWeekProperty, value);
        }
        public static readonly DependencyProperty DayOfWeekProperty = DependencyProperty.Register(
            "DayOfWeek", typeof(DayOfWeek), typeof(AgendaDay), new PropertyMetadata());

        public IEnumerable<CourseInstance> Items {
            get => (IEnumerable<CourseInstance>)this.GetValue(ItemProperty);
            set => this.SetValue(ItemProperty, value);
        }
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(
            "Items", typeof(IEnumerable<CourseInstance>), typeof(AgendaDay), new PropertyMetadata(null,
                (obj, args) => {
                    var agendaDay = obj as AgendaDay;
                    agendaDay.TBEmpty.Visibility = Visibility.Collapsed;
                    agendaDay.ICClasses.Visibility = Visibility.Collapsed;

                    var newValueIsEmpty = (args.NewValue as IEnumerable<CourseInstance>).Count() == 0;
                    if (newValueIsEmpty)
                        agendaDay.TBEmpty.Visibility = Visibility.Visible;
                    else
                        agendaDay.ICClasses.Visibility = Visibility.Visible;
                }
            )
        );

        public AgendaDay()
        {
            InitializeComponent();
        }
    }

    public class DayOfWeekConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var day = (DayOfWeek)value;
            return UASD.Utilities.Convert.Day(day);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var courseClass = value as CourseClass;
            var start = courseClass.StartTime;
            var end = start + TimeSpan.FromHours(courseClass.Duration);
            return $"{Time(start, shorten: true)} - {Time(end, shorten: true)}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class PlaceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var place = (string)value;
            return UASD.Utilities.Convert.Place(place);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ClassInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var agendaItem = (CourseInstance)value;
            return UASD.CourseClass.GetInfoString(agendaItem.Course, agendaItem.Class);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class CurrentClassConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var now = DateTime.Now;
            var today = now.DayOfWeek;
            var time = now.TimeOfDay;

            var courseClass = value as CourseClass;
            if (courseClass.DayOfWeek == today) {
                if (courseClass.StartTime < time && courseClass.EndTime > time)
                    return "#05F";
            }
            return "#555";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
