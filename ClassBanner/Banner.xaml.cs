using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using WpfScreenHelper;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Interop;


/// <summary>
/// Interaction logic for Banner.xaml
/// </summary>

using System.Windows.Media;
using static DesktopBanner.NativeMethods;


namespace DesktopBanner
{
    //using System.Windows.Navigation;

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
        private DispatcherTimer showTimer;
        public DisplayMode DisplayMode = DesktopBanner.DisplayMode.Overlay;
        protected const int BANNER_HEIGHT = 23;
        // Base registry path, should end with trailing \
        private const string REG_ROOT = @"HKLM\SOFTWARE\DesktopBanner\";
        protected double RegularOpacity = 1.0;
        private Dictionary<string, string> BannerColors;
        private Dictionary<string, string> BannerLabels;
        private BannerScreenConfig? ScreenConfig;

        public string BannerColor = "#008000";
        public string TextColor = "#000000";
        public Screen? Display;
        private string? LeftDisplay = "";
        public string? CenterDisplay = "";
        private string? RightDisplay = "";
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

        static Banner()
        {
            //ShowInTaskbarProperty.OverrideMetadata(typeof(MainWindow), new FrameworkPropertyMetadata(false));
            /*MinHeightProperty.OverrideMetadata(typeof(MainWindow), new FrameworkPropertyMetadata(20d, MinMaxHeightWidth_Changed));
            MinWidthProperty.OverrideMetadata(typeof(MainWindow), new FrameworkPropertyMetadata(20d, MinMaxHeightWidth_Changed));
            MaxHeightProperty.OverrideMetadata(typeof(MainWindow), new FrameworkPropertyMetadata(MinMaxHeightWidth_Changed));
            MaxWidthProperty.OverrideMetadata(typeof(MainWindow), new FrameworkPropertyMetadata(MinMaxHeightWidth_Changed));
            */
        }
        public Banner(bool ShowOnBottom = false)
        {
            InitializeComponent();
            DataContext = this;
            this.ShowOnBottom = ShowOnBottom;
            BannerPosition = (ShowOnBottom) ? "bottom" : "top";
            showTimer = new DispatcherTimer();
            showTimer.Interval = TimeSpan.FromSeconds(3);
            showTimer.Tick += (s, _) => Window_Show();


            BannerColors = new()
            {
                {"UNCATEGORIZED", "#008000"}
            };

            BannerLabels = new()
            {
                {"UNCATEGORIZED", "Uncategorized"}
            };

            bool checkRegPath = Reg.KeyExists(REG_ROOT);
            if (checkRegPath)
            {

                double OpacityLvl = Reg.GetDouble(REG_ROOT, "Opacity");
                if (OpacityLvl == -1)
                {
                    OpacityLvl = RegularOpacity;
                }
                //Set the defauilt opacity state, this is the value that the banner returns to after any animations or effects which may change the opacity
                RegularOpacity = (OpacityLvl) / 100.0;

                //Set the banners opacity
                Opacity = RegularOpacity;

                PrepareBannerText();
            }


        }
        private Dictionary<string, string> GetTokenMap()
        {
            return new()
            {
                { "@USER", Utils.GetCurrentUser()},
                { "@HOST", Utils.GetHostName()}

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
            LeftDisplay = Reg.GetString(REG_ROOT, "LeftDisplay");
            RightDisplay = Reg.GetString(REG_ROOT, "RightDisplay");
            CenterDisplay = Reg.GetString(REG_ROOT, "CenterDisplay");

            //Get the dictionary containing value which will be used for any token substitutions in the banner labels, e.g. @USER would become the current users username
            Dictionary<string, string> tokenMap = GetTokenMap();
            lblLeftDisplay.Content = PerformTokenSubstitution(LeftDisplay, tokenMap);
            lblCenterDisplay.Content = PerformTokenSubstitution(CenterDisplay, tokenMap);
            lblRightDisplay.Content = PerformTokenSubstitution(RightDisplay, tokenMap);
        }

        private void Window_Activated(object sender, EventArgs e)
        {
        }
        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }

        private void Window_MouseEnter(object sender, EventArgs e)
        {
        }
        private void Window_Show()
        {
           
        }

        //Helper function to set the positional bounds using display device values 
        public void SetBannerBounds(BannerScreenConfig config)
        {
            ScreenConfig = config;
            Top = ShowOnBottom ? (ScreenConfig.WorkingArea.Bottom) - Height : ScreenConfig.ScaledScreen.Top;
            Left = ScreenConfig.ScaledScreen.Left;
            Width = ScreenConfig.ScaledScreen.Width;
            Bounds = new Rect(Left, Top, Width, Height);
        }
    }
}