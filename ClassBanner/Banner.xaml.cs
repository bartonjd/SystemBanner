﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using WpfScreenHelper;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Interop;


/// <summary>
/// Base class for banner
/// Interaction logic for Banner.xaml
/// </summary>

using System.Windows.Media;
using static DesktopBanner.NativeMethods;


namespace DesktopBanner
{

    public enum DisplayMode
    {
        Overlay = 0,
        Rollover = 1,
        Static = 2
    }

    public partial class Banner : Window
    {
        public bool ShowOnBottom { get; set; }
        public string? BannerPosition { get; set; }

        public DisplayMode DisplayMode = DesktopBanner.DisplayMode.Overlay;
        protected const int BANNER_HEIGHT = 23;
        private const string REGISTRYROOT = App.REGISTRYROOT;
        //The default opacity of the window before and after animations, defaults to 100% (1.0) if not set by the user
        protected double RegularOpacity = 1.0;
        private Dictionary<string, string> BannerColors;
        private Dictionary<string, string> BannerLabels;
        private BannerScreenConfig? ScreenConfig;

        public string BannerColor = "#008000";
        public string TextColor = "#000000";
        //Object representing the screen on which the banner is to be displayed on 
        public Screen? Display;
        private string? LeftDisplayText = "";
        public string? CenterDisplayText = "";
        private string? RightDisplayText = "";
        public Rect ScaledScreen;
        public string? DisplayDevice;
        public string? _displayIdentifier;
        public string? DisplayIdentifier
        {
            get
            {
                if ( _displayIdentifier is null)
                {
                    _displayIdentifier = ScreenConfig?.GenerateUniqueId();
                }
                return _displayIdentifier;
            }
            set
            {
                _displayIdentifier = value;
            }
        }
        private Rect _bounds;
        public Rect Bounds {
            get {
                return _bounds;
            }
            set {
                _bounds = value;
            }
        }

/*        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register(
              "Background",
              typeof(Brush),
              typeof(Banner),
              new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 128, 0))));

        public Brush BackgroundColor
        {
            get { return (Brush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }
    */

    public Banner(bool ShowOnBottom = false)
        {
            InitializeComponent();
            //Bind WPF Banner to class values 
            DataContext = this;
            this.ShowOnBottom = ShowOnBottom;
            BannerPosition = (ShowOnBottom) ? "bottom" : "top";


            BannerColors = new()
            {
                {"UNCATEGORIZED", "#008000"}
            };

            BannerLabels = new()
            {
                {"UNCATEGORIZED", "Uncategorized"}
            };
            UpdateSettings();
        }

        public void UpdateSettings() {
            bool checkRegPath = Reg.KeyExists(REGISTRYROOT);
            if (checkRegPath)
            {
                double OpacityLvl = Reg.GetDouble(REGISTRYROOT, "Opacity");
                if (OpacityLvl == -1)
                {
                    OpacityLvl = RegularOpacity;
                }
                //Set the defauilt opacity state, this is the value that the banner returns to after any animations or effects which may change the opacity
                RegularOpacity = (OpacityLvl) / 100.0;

                //Set the banners opacity
                Opacity = RegularOpacity;

                //Substitute tokens, check for nulls and set Left, Center and Right banner display labels
                PrepareBannerText();
                FormatBanner();
            }

        }
        private Dictionary<string, string> GetTokenMap()
        {
            return new()
            {
                { "@USER", Utils.GetCurrentUser()},
                { "@HOST", Utils.GetHostName()}
                //Add date?

            };
        }
        private string? PerformTokenSubstitution(string? text, Dictionary<string, string> tokenMap)
        {
            if (null != text)
            {
                foreach (KeyValuePair<string, string> token in tokenMap)
                {
                    text = text.Replace(token.Key, token.Value);
                }
            }
            else
            {
                text = "";
            }
            return text;
        }
        private void PrepareBannerText()
        {
            LeftDisplayText = Reg.GetString(REGISTRYROOT, "LeftDisplayText");
            RightDisplayText = Reg.GetString(REGISTRYROOT, "RightDisplayText");
            CenterDisplayText = Reg.GetString(REGISTRYROOT, "CenterDisplayText");

            //Get the dictionary containing value which will be used for any token substitutions in the banner labels, e.g. @USER would become the current users username
            Dictionary<string, string> tokenMap = GetTokenMap();
            lblLeftDisplay.Content = PerformTokenSubstitution(LeftDisplayText, tokenMap);
            lblCenterDisplay.Content = PerformTokenSubstitution(CenterDisplayText, tokenMap);
            lblRightDisplay.Content = PerformTokenSubstitution(RightDisplayText, tokenMap);
        }

        private void FormatBanner() {
            string? bgColorCode = Reg.GetString(REGISTRYROOT, "BackgroundColor");
            SolidColorBrush? bgColor = Utils.GetColorBrush(bgColorCode);
            //BackgroundColor = bgColor;
            Background = bgColor;
        }

        protected void Window_Activated(object sender, EventArgs e)
        {
        }
        protected void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }

        protected void Window_MouseEnter(object sender, EventArgs e)
        {
        }
        private void Window_Show()
        {
           
        }

        //Helper function to set the positional bounds using display device values 
        public void SetBannerBounds(BannerScreenConfig config)
        {
            ScreenConfig = config;
            Top = ShowOnBottom ? (ScreenConfig.ScaledScreen.Bottom) - Height : ScreenConfig.ScaledScreen.Top;
            Left = ScreenConfig.ScaledScreen.Left;
            Width = ScreenConfig.ScaledScreen.Width;
            Bounds = new Rect(Left, Top, Width, Height);
        }
    }
}