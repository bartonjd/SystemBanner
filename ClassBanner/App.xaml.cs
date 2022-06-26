using System;
using System.Collections.Generic;
using System.Windows;
using WpfScreenHelper;
using Microsoft.Win32;
using System.Threading;

namespace ClassBanner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Dictionary<String, MainWindow> MainWindowList = new Dictionary<String, MainWindow>();
        public WindowManager WDM = new WindowManager();

        protected override void OnStartup(StartupEventArgs e)
        {

            base.OnStartup(e);
            SystemEvents.DisplaySettingsChanged += new EventHandler(SystemEvents_DisplaySettingsChanged);
            WDM.Init();

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

        private void SystemEvents_DisplaySettingsChanged(object? sender,EventArgs e)
        {
            Thread.Sleep(5000);
            WDM.Refresh();
            /*
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
            */
        }
    }
}