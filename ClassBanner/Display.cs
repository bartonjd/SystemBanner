using System;
using System.Windows;
using WpfScreenHelper;
using ClassBanner;

namespace ClassBanner
{
    public class Display
    {
        public String DeviceId = "";
        public MainWindow? TopBanner = null;
        public MainWindow? BottomBanner = null;
        public Display(String Id,MainWindow? Top=null, MainWindow? Bottom = null) 
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