using System;
using System.Collections;
using System.Windows;
using System.Collections.Generic;
using WpfScreenHelper;
using Microsoft.Win32;
using ClassBanner;

namespace ClassBanner
{
    public class WindowManager
    {
        public Dictionary<String, MainWindow> MainWindowList = new Dictionary<String, MainWindow>();
        
        public Dictionary<String, Display> Displays = new Dictionary<String, Display>();
        public void Init()
        {

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
            //Refresh();
        }
        public void Add(String DisplayId, MainWindow mw)
        {
            Display? DisplayInfo = null;
            if (Displays.ContainsKey(DisplayId))
            {
                DisplayInfo = Displays[DisplayId];
            }
            else {
                DisplayInfo = new Display(DisplayId);
                Displays.Add(DisplayId,DisplayInfo);
            }
            if (mw.BannerPosition == "top")
            {
                DisplayInfo.TopBanner = mw;
            }
            if (mw.BannerPosition == "bottom")
            {
                DisplayInfo.BottomBanner = mw;
            }
            

        }
        public void Delete(String DisplayId)
        {
            if (Displays.ContainsKey(DisplayId))
            {
                Displays.Remove(DisplayId);
            }
        }

        public void Refresh()
        {
            String fred = "g";
            foreach (var s in Screen.AllScreens) 
            {
                foreach (var d in Displays)
                {
                    String displayId = d.Key;
                    
                    String ScreenId = GenerateUniqueId(s);
                    if (displayId == ScreenId) 
                    {
                        fred = "a";
                    }
                    
                }
            }

        }
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
        public String GenerateUniqueId(Screen s) 
        {
            Rect scaledScreen = GetScaledScreen(s);
            String UniqueId = scaledScreen.ToString() + "_" + s.DeviceName;
            return UniqueId;
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
            }
            else
            {
                mainWindow.Top = scaledScreen.Top;
            }
            mainWindow.Left = scaledScreen.Left;
            mainWindow.Width = scaledScreen.Width;
            mainWindow.Bounds = new Rect(mainWindow.Left, mainWindow.Top, mainWindow.Width, mainWindow.Height);
            mainWindow.ScaledScreen = scaledScreen;
            mainWindow.DisplayDevice = s.DeviceName;
            String displayId = scaledScreen.ToString() + "_" + s.DeviceName;
            mainWindow.DisplayIdentifier = displayId;
            this.Add(displayId, mainWindow);
            mainWindow.Show();
        }

    }
    public class Display
    {
        public String DeviceId = "";
        public MainWindow? TopBanner;
        public MainWindow? BottomBanner;
        public Display(String Id,MainWindow? Top=null, MainWindow? Bottom = null) 
        {
            DeviceId = Id;
            TopBanner = Top;
            BottomBanner = Bottom;
        }
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
    }
}