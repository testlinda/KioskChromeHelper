using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace KioskChromeHelper
{
    /// <summary>
    /// Interaction logic for CloseWindows.xaml
    /// </summary>
    public partial class CloseDialog : Window
    {
        public CloseDialog()
        {
            InitializeComponent();
            Loaded += CloseDialog_Loaded;
            ContentRendered += CloseDialog_ContentRendered;
        }

        public string Url { get; set; } = string.Empty;

        public string Parameter { get; set; } = string.Empty;

        private void CloseDialog_ContentRendered(object sender, EventArgs e)
        {
            launchChrome();
        }

        private void CloseDialog_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopScreenHeight = SystemParameters.PrimaryScreenHeight;
            var desktopScreenWidth = SystemParameters.PrimaryScreenWidth;
            this.Width = image.Source.Width;
            this.Height = image.Source.Height;                      
            this.Left = desktopScreenWidth/2;
            this.Top = 10;
        }

        private void launchChrome()
        {
            string path = Url;
            string chrome_path = getChromePath();
            string chrome_para = Parameter;
            Process.Start(chrome_path, chrome_para + ' ' + path);
        }

        private string getChromePath()
        {
            var path = Microsoft.Win32.Registry.GetValue(@"HKEY_CLASSES_ROOT\ChromeHTML\shell\open\command", null, null) as string;
            if (path != null)
            {
                var split = path.Split('\"');
                path = split.Length >= 2 ? split[1] : null;
            }
            return path;
        }

        private void closeChrome()
        {
            ExitApp.ExitByName("chrome");
        }

        private void Rectangle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Close browser
            closeChrome();
            this.Close();
        }
    }
}
