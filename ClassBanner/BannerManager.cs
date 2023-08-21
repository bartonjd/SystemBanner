using System;
using System.Windows;
using System.Collections.Generic;
using WpfScreenHelper;

namespace DesktopBanner
{
    public class BannerManager
    {
        public Dictionary<string, Banner> BannerList = new ();
        public bool ShowOnBottom;
        public Dictionary<string, Display> Displays = new ();
        private DisplayMode DefaultDisplayMode;
        public void Init(DisplayMode defaultDisplayMode = DisplayMode.Overlay)
        {

            bool ShowOnBottom = false;
            DefaultDisplayMode = defaultDisplayMode;
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
        public void Add(string DisplayId, Banner mw)
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
        public void Delete(string DisplayId)
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
                UnregisterAppBar(d.TopBanner);
                d.TopBanner.Close();
            }
            if (d.BottomBanner != null)
            {
                UnregisterAppBar(d.BottomBanner);
                d.BottomBanner.Close();
            }
            Displays.Remove(d.DeviceId);
        }

        private void UnregisterAppBar(Banner b) {
            if ((b.DisplayMode == DisplayMode.Static) && (b is StaticBanner)) {
                StaticBanner sb = (StaticBanner)b;
                sb.Unregister();
            }
        }
        public static string GenerateUniqueId(Screen s) 
        {
            Rect scaledScreen = Display.GetScaledScreen(s);
            string UniqueId = scaledScreen.ToString() + "_" + s.DeviceName;
            return UniqueId;
        }

        private void CreateBanner(Screen s, bool ShowOnBottom = false)
        {
            Banner banner;
            //Based on the DisplayMode set in the registry, instantiate the correct banner type
            switch (DefaultDisplayMode) {
                case DisplayMode.Overlay:
                    banner = new Banner(ShowOnBottom);
                    break;
                case DisplayMode.Rollover:
                    banner = new RolloverBanner(ShowOnBottom);
                    break;
                case DisplayMode.Static:
                    banner = new StaticBanner(ShowOnBottom);
                    break;
                default:
                    banner = new Banner(ShowOnBottom);
                    break;
            };


            BannerScreenConfig config = new(s);
            banner.SetBannerBounds(config);
            banner.Display = s;

            //Add the banner to the manager
            //TODO: Get rid of the nullability warning
            Add(banner.DisplayIdentifier, banner);
            banner.Show();
            banner.Topmost = true;
        }
        public void Refresh()
        {
            Dictionary<string, Screen?> AllScreensList = new();
            foreach (var s in Screen.AllScreens)
            {
                string ScreenId = GenerateUniqueId(s);
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

            foreach (var d in Displays)
            {
                if (!AllScreensList.ContainsKey(d.Key))
                {
                    DisposeDisplay(d.Value);
                }
            }
        }
    }
}