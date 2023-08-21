using System;
using System.Windows;
using WpfScreenHelper;
using DesktopBanner;

namespace DesktopBanner
{
    public class ScreenConfig()
	{
        public Display DisplayScreen;
        public Rect WorkingArea;
        public Rect ScaledScreen;
        public Rect Bounds;

        public ScreenConfig(Screen displayScreen) {
            DisplayScreen = displayScreen;
            ScaledScreen = Display.GetScaledScreen(DisplayScreen);
            WorkingArea = ScaledScreen.WorkingArea;
        }
        
        

        if (ShowOnBottom)
        {
            banner.Top = (ScaledScreen.Bottom) - banner.Height;
        }
        else
        {
            banner.Top = ScaledScreen.Top;
        }
        banner.Left = ScaledScreen.Left;
        banner.Width = ScaledScreen.Width;
        banner.Bounds = new Rect(banner.Left, banner.Top, banner.Width, banner.Height);
        banner.ScaledScreen = ScaledScreen;


    }
        public String GenerateUniqueId(Screen s)
        {
            Rect scaledScreen = Display.GetScaledScreen(s);
            String UniqueId = ScaledScreen.ToString() + "_" + s.DeviceName;
            return UniqueId;
}
}
