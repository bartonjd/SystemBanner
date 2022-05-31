using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfScreenHelper;
using System.Windows.Shapes;
using ClassBanner;



namespace ClassBanner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void CreateBannerWindowObj(Screen s,bool ShowOnBottom = false) {
            var mainWindow = new MainWindow();
            Rect r = s.WorkingArea;
            if (ShowOnBottom)
            {
                mainWindow.Top = (r.Bottom / s.ScaleFactor) - mainWindow.Height;
            }
            else
            {
                mainWindow.Top = r.Top / s.ScaleFactor;
            }
            mainWindow.Left = r.Left / s.ScaleFactor;
            mainWindow.Width = r.Width / s.ScaleFactor;

            mainWindow.Show();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            List<MainWindow> MainWindowList = new List<MainWindow>();
           bool check =  Utils.TestRegPath(@"HKEY_LOCAL_MACHINE\SOFTWARE\ClassBanner\");
            bool ShowOnBottom = true;
            foreach (var ss in Screen.AllScreens)
                {
                this.CreateBannerWindowObj(ss);
                if (ShowOnBottom) {
                    this.CreateBannerWindowObj(ss, true);
                    }
                }
        }
    }
}
