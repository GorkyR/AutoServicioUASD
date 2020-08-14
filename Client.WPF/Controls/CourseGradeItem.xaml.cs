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
using static UASD.GradedCourse;

namespace Client.WPF.Controls
{
    /// <summary>
    /// Interaction logic for CourseGradeItem.xaml
    /// </summary>
    public partial class CourseGradeItem : UserControl
    {
        public CourseGradeItem()
        {
            InitializeComponent();
        }
    }

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
    public class GradeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int v = (int)value;
            return v != 0 ? $"{v}" : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
