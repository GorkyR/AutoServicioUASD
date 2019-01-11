using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static UASD.CourseGrade;

namespace Client.WPF.Controls
{
    public class GradeStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = (CourseState)value;
            switch (state)
            {
                case CourseState.Published:
                    return 0;
                case CourseState.NotPublished:
                    return 1;
                case CourseState.Absent:
                    return 2;
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
