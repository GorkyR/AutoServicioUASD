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

namespace Client.WPF.Controls
{
    /// <summary>
    /// Interaction logic for OpenCourseItem.xaml
    /// </summary>
    public partial class OpenCourseItem : UserControl
    {
        public Action AddAction { get; set; }

        public OpenCourseItem()
        {
            InitializeComponent();
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            AddAction();
        }
    }

    public class ScheduleDayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var scheduleCollection = (List<CourseClass>)value;
            return string.Concat(
                scheduleCollection.Select(si =>
                    UASD.Utilities.Convert.Days.First(kvp =>
                        kvp.Value == si.DayOfWeek
                    ).Key
                )
            );
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
