using System;
using System.Windows;
using WpfScreenHelper;
using DesktopBanner;

namespace DesktopBanner
{
    public class Display
    {
        public String DeviceId = "";
        public Banner? TopBanner = null;
        public Banner? BottomBanner = null;
        public Display(String Id, Banner? Top=null, Banner? Bottom = null) 
        {
            DeviceId = Id;
            TopBanner = Top;
            BottomBanner = Bottom;
        }
        public static Rect GetScaledScreen(Screen s)
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
    }
}