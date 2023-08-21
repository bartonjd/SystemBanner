using System;
using System.Windows;
using WpfScreenHelper;
using DesktopBanner;

namespace DesktopBanner
{
    public class BannerScreenConfig
	{
        public Screen DisplayScreen;
        public Rect WorkingArea;
        public Rect ScaledScreen;
        public Rect Bounds;

        public BannerScreenConfig(Screen displayScreen) 
        {
            DisplayScreen = displayScreen;
            ScaledScreen = Display.GetScaledScreen(DisplayScreen);
            WorkingArea = displayScreen.WorkingArea;
        }


        public String GenerateUniqueId()
        {
            Rect scaledScreen = Display.GetScaledScreen(DisplayScreen);
            String UniqueId = ScaledScreen.ToString() + "_" + DisplayScreen.DeviceName;
            return UniqueId;
        }
    }
}
