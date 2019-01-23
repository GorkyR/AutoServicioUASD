﻿using System;
using System.Collections.Generic;
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

namespace Client.WPF.Widgets
{
    /// <summary>
    /// Interaction logic for ScheduleWidget.xaml
    /// </summary>
    public partial class ScheduleWidget : UserControl
    {
        public string Weekday { get; set; }
        public static readonly DependencyProperty WeekdayProperty = DependencyProperty.Register(
            "Weekday", typeof(string), typeof(ScheduleWidget), new PropertyMetadata(
                UASD.Utilities.Convert.Day(DateTime.Now.DayOfWeek)
            )
        );

        public ScheduleWidget()
        {
            InitializeComponent();
        }

        private async void DidLoad(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.AppStarting;
            var now = DateTime.Now;
            var today = now.DayOfWeek;
            var courses = await ClientService.ScheduleAsync();

            var todaysCourses =
                (from course in courses
                    select (from instance in course.ScheduleInfo
                            where instance.DayOfWeek == today
                            select (Course: course, Instance: instance)))
                .Aggregate((a, b) => a.Concat(b)).ToList();
            todaysCourses.Sort((a, b) =>
                (a.Instance.StartTime - b.Instance.StartTime).Hours
            );
            var upcomingClasses = todaysCourses.SkipWhile(courseInstance =>
                courseInstance.Instance.EndTime < now.TimeOfDay
            );
            ICClasses.ItemsSource =
                from uC in upcomingClasses
                select new StackPanel {
                    Children = {
                        new TextBlock {
                            Text = $"{UASD.Utilities.Convert.Time(uC.Instance.StartTime)}",
                            FontSize = 10, FontWeight = FontWeights.Medium,
                            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#555"))
                        },
                        new Controls.ScheduleItem()
                        {
                            Titulo  = uC.Course.Title,
                            Codigo  = uC.Course.Code,
                            Lugar   = UASD.Utilities.Convert.Place(uC.Instance.Place),
                            ToolTip = $"{uC.Course.Title}\n" +
                                        $"{uC.Course.Code}\n"+
                                        $"NRC: {uC.Course.NRC}\n" +
                                        $"{uC.Course.Credits} creditos\n"+
                                        $"Lugar: {uC.Instance.Place}\n"+
                                        $"Prof.: {uC.Course.Professor}"
                        }
                    }
                };
            Cursor = Cursors.Arrow;
        }
    }
}
