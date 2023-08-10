using System;
using System.Windows;
using System.Collections.Generic;
using WpfScreenHelper;

namespace DesktopBanner
{
    public class BannerManager
    {
        public Dictionary<String, Banner> BannerList = new ();
        public bool ShowOnBottom = true;
        public Dictionary<String, Display> Displays = new ();
        public void Init()
        {

            bool ShowOnBottom = false;
            //string ShowBottomBanner = Utils.GetRegValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\DesktopBanner\", "ShowBottomBanner");
            //////
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
                this.CreateBanner(ss);
                if (ShowOnBottom)
                {
                    this.CreateBanner(ss, true);
                }
            }
            //Refresh();
        }
        public void Add(String DisplayId, Banner mw)
        {
            Display? DisplayInfo;
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
        private void DisposeDisplay(Display d)
        {
            if (d.TopBanner != null)
            {
                d.TopBanner.Unregister();
                d.TopBanner.Close();
            }
            if (d.BottomBanner != null)
            {
                d.BottomBanner.Unregister();
                d.BottomBanner.Close();
            }
            Displays.Remove(d.DeviceId);
        }
        private static Rect GetScaledScreen(Screen s)
        {
            Rect r = s.WorkingArea;
            Rect scaledScreen = new (
                (r.Left / s.ScaleFactor),
                (r.Top / s.ScaleFactor),
                (r.Width / s.ScaleFactor),
                (r.Height / s.ScaleFactor)
            );
            return scaledScreen;
        }
        public static String GenerateUniqueId(Screen s) 
        {
            Rect scaledScreen = GetScaledScreen(s);
            String UniqueId = scaledScreen.ToString() + "_" + s.DeviceName;
            return UniqueId;
        }
        private void CreateBanner(Screen s, bool ShowOnBottom = false)
        {
            var banner = new Banner(ShowOnBottom);
            Rect r = s.WorkingArea;
            Rect scaledScreen = Display.GetScaledScreen(s);

            if (ShowOnBottom)
            {
                banner.Top = (scaledScreen.Bottom) - banner.Height;
            }
            else
            {
                banner.Top = scaledScreen.Top;
            }
            banner.Left = scaledScreen.Left;
            banner.Width = scaledScreen.Width;
            banner.Bounds = new Rect(banner.Left, banner.Top, banner.Width, banner.Height);
            banner.ScaledScreen = scaledScreen;
            banner.DisplayDevice = s.DeviceName;
            banner.Display = s;
            var displayId = scaledScreen.ToString() + "_" + s.DeviceName;
            banner.DisplayIdentifier = displayId;
            Add(displayId, banner);
            banner.Show();
            banner.Topmost = true;
        }
        public void Refresh()
        {
            Dictionary<String, Screen?> AllScreensList = new();
            foreach (var s in Screen.AllScreens)
            {
                String ScreenId = GenerateUniqueId(s);
                AllScreensList.Add(ScreenId, s);
                if (!Displays.ContainsKey(ScreenId))
                {
                    CreateBanner(s, false);
                    if (ShowOnBottom)
                    {
                        CreateBanner(s, true);
                    }
                }
            }

/*            int? bannerType;
            if (Reg.PropertyExists(@"HKEY_LOCAL_MACHINE\SOFTWARE\DesktopBanner\", "BannerType"))
            {
                bannerType = (int?)Reg.GetInt(@"HKEY_LOCAL_MACHINE\SOFTWARE\DesktopBanner\", "BannerType");
            }
            else {
                bannerType = 1;
            }*/

            foreach (var d in Displays)
            {
                String displayId = d.Key;
                if (!AllScreensList.ContainsKey(d.Key))
                {

                    DisposeDisplay(d.Value);
                }
            }

        }

    }
}