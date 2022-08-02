﻿using System;
using System.Collections.Generic;
using System.Windows;
using WpfScreenHelper;
using Microsoft.Win32;
using System.Threading;

namespace DesktopBanner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Dictionary<String, Banner> BannerList = new Dictionary<String, Banner>();
        public BannerManager WDM = new BannerManager();

        protected override void OnStartup(StartupEventArgs e)
        {
            Boolean exists = Reg.PropertyExists(@"HKLM\SOFTWARE\DesktopBanner","BannerLabel");
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
            //Sleep to ensure displays finish adjusting after a display is added or removed
            //if this is not done the banner will often move to the wrong position by performing
            //calculations before the correct screen size can be determined
            Thread.Sleep(5000);
            WDM.Refresh();
        }
    }
}