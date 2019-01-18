using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace Client.WPF
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        private static FileVersionInfo FileVersionInfo { get => FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location); }
        public static string Version { get => FileVersionInfo.ProductVersion; }
        public static string Description { get => FileVersionInfo.FileDescription;  }
        public static string Copyright { get => FileVersionInfo.LegalCopyright;  }

        public AboutWindow()
        {
            InitializeComponent();
        }
    }
}
