using System;
using System.Collections.Generic;
using System.Windows;
using WpfScreenHelper;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using Microsoft.Win32;
using System.Collections;

namespace ClassBanner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Dictionary<String, MainWindow> MainWindowList = new Dictionary<String, MainWindow>();
        private Rect GetScaledScreen(Screen s)
        {
            Rect r = s.WorkingArea;
            Rect scaledScreen = new Rect(
                    (r.Left / s.ScaleFactor),
                    (r.Top / s.ScaleFactor),
                    (r.Width / s.ScaleFactor),
                    (r.Height / s.ScaleFactor)
            );
            return scaledScreen;
        }
        private void CreateBannerWindowObj(Screen s, bool ShowOnBottom = false)
        {
            var mainWindow = new MainWindow(ShowOnBottom);
            Rect r = s.WorkingArea;
            Rect scaledScreen = GetScaledScreen(s);
            String bannerPosition;

            if (ShowOnBottom)
            {
                mainWindow.Top = (scaledScreen.Bottom) - mainWindow.Height;
                bannerPosition = "bot";
            }
            else
            {
                mainWindow.Top = scaledScreen.Top;
                bannerPosition = "top";
            }
            mainWindow.Left = scaledScreen.Left;
            mainWindow.Width = scaledScreen.Width;
            mainWindow.Bounds = new Rect(mainWindow.Left, mainWindow.Top, mainWindow.Width, mainWindow.Height);
            String displayId = scaledScreen.ToString() + s.DeviceName;
            mainWindow.DisplayIdentifier = displayId;
            MainWindowList.Add(displayId + "@" + bannerPosition, mainWindow);
            mainWindow.Show();
        }
        protected override void OnStartup(StartupEventArgs e)
        {

            base.OnStartup(e);
            SystemEvents.DisplaySettingsChanged += new EventHandler(SystemEvents_DisplaySettingsChanged);

            bool ShowOnBottom = false;
            //string ShowBottomBanner = Utils.GetRegValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\ClassBanner\", "ShowBottomBanner");
            string ShowBottomBanner = "1";
            if (ShowBottomBanner == "1")
            {
                ShowOnBottom = true;
            }
            else
            {
                ShowOnBottom = false;
            }
            foreach (var ss in Screen.AllScreens)
            {
                this.CreateBannerWindowObj(ss);
                if (ShowOnBottom)
                {
                    this.CreateBannerWindowObj(ss, true);
                }
            }
        }
        void App_Exit(object sender, ExitEventArgs e)
        {

        }
        protected override void OnExit(ExitEventArgs e)
        {
            Microsoft.Win32.SystemEvents.DisplaySettingsChanged -= new EventHandler(SystemEvents_DisplaySettingsChanged);
            base.OnExit(e);

        }


        //closing
        //

        private void SystemEvents_DisplaySettingsChanged(Object sender,EventArgs e)
        {
            List<String> screenBounds = new List<String>();
            foreach (var sb in Screen.AllScreens) 
            {
                String boundString = GetScaledScreen(sb).ToString() + sb.DeviceName;
                screenBounds.Add(boundString);
            }
            //Screen.AllScreens[0];
            foreach (var mw in MainWindowList) {
               
                string[] displayInfo = mw.Key.Split('@');

                bool screenExists = screenBounds.Contains(displayInfo[0]);
                if (screenExists) {
                    mw.Value.Close();
                }
                string bob = "l";
            }
    }
}
}