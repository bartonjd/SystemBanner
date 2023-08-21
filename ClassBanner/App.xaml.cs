using System;
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
        //Configurable
        // Base registry path, should end with trailing \
        public const string REGISTRYROOT = @"HKLM\SOFTWARE\DesktopBanner\";

        public Dictionary<string, Banner> BannerList = new();
        //Instantiate a new BannerManager, this creates and destroys banners, keeps track of open banners and adjusts how many are displayed when a monitor/display is added or removed
        public BannerManager WDM = new();

        protected override void OnStartup(StartupEventArgs e)
        {
            DisplayMode displayMode;
            //DisplayMode 1 is Rollup banner (hides on mouseover, DisplayMode 2 is static banner 
            if (Reg.PropertyExists(@"HKEY_LOCAL_MACHINE\SOFTWARE\DesktopBanner\", "DisplayMode"))
            {
                //If DisplayMode is null set to Overlay style
                displayMode = (DisplayMode)((int?)Reg.GetInt(@"HKLM\SOFTWARE\DesktopBanner\", "DisplayMode") ?? 0);
            }
            else {
                displayMode = DisplayMode.Overlay;
            }            

            //If at least the CenterDisplay value is set then proceed
            bool exists = Reg.PropertyExists(@"HKLM\SOFTWARE\DesktopBanner","CenterDisplay");
            base.OnStartup(e);
            //Start listening for Display events, e.g. monitor plugged in or removed, dpi etc
            SystemEvents.DisplaySettingsChanged += new EventHandler(SystemEvents_DisplaySettingsChanged);

            WDM.Init(displayMode);
        }


        protected override void OnExit(ExitEventArgs e)
        {
            //Clean up DisplaySettingsChanged listener on exit
            Microsoft.Win32.SystemEvents.DisplaySettingsChanged -= new EventHandler(SystemEvents_DisplaySettingsChanged);
            base.OnExit(e);

        }


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