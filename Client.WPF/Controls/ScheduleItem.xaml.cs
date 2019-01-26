﻿using System;
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

namespace Client.WPF.Controls
{
    /// <summary>
    /// Interaction logic for ScheduleItem.xaml
    /// </summary>
    public partial class ScheduleItem : UserControl
    {
        public string Titulo   {
            get => (string)this.GetValue(TituloProperty);
            set => this.SetValue(TituloProperty, value);
        }
        public static readonly DependencyProperty TituloProperty = DependencyProperty.Register(
            "Titulo", typeof(string), typeof(ScheduleItem), new PropertyMetadata("[Schedule Item]"));

        public string Codigo
        {
            get => (string)this.GetValue(CodigoProperty);
            set => this.SetValue(CodigoProperty, value);
        }
        public static readonly DependencyProperty CodigoProperty = DependencyProperty.Register(
            "Codigo", typeof(string), typeof(ScheduleItem), new PropertyMetadata());

        public string Lugar
        {
            get => (string)this.GetValue(LugarProperty);
            set => this.SetValue(LugarProperty, value);
        }
        public static readonly DependencyProperty LugarProperty = DependencyProperty.Register(
            "Lugar", typeof(string), typeof(ScheduleItem), new PropertyMetadata());

        public bool   IsShadow
        {
            get => (bool)this.GetValue(IsShadowProperty);
            set => this.SetValue(IsShadowProperty, value);
        }
        public static readonly DependencyProperty IsShadowProperty = DependencyProperty.Register(
            "IsShadow", typeof(bool), typeof(ScheduleItem), new PropertyMetadata());

        public ScheduleItem()
        {
            InitializeComponent();
        }
    }

    public class ShadowBackgroundConverter : IValueConverter
    {
        public object Convert(object v, Type targetType, object parameter, CultureInfo culture)
        {
            bool value = (bool)v;
            if (!value)
                return Brushes.LightGreen;
            else
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AA555555"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ShadowForegroundConverter : IValueConverter
    {
        public object Convert(object v, Type targetType, object parameter, CultureInfo culture)
        {
            bool value = (bool)v;
            if (!value)
                return Brushes.Black;
            else
                return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
