using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Text.Json;
using WpfScreenHelper;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace ClassBanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    //
    public partial class MainWindow : Window
    {
        private bool isBottom;
        public String BannerPosition;
        private DispatcherTimer showTimer;
        private const Int32 BANNER_HEIGHT = 23;
        private Double REGULAR_OPACITY = 100;
        private Dictionary<string, string> ClassificationColors;
        private Dictionary<string, string> ClassificationLabels;
        private String HostName = "";
        private String CurrentUser = "";
        private String BannerLabel = "";
        private String BannerColor = "#008000";
        private String LeftDisplay = "";
        private String RightDisplay = "";
        public Rect ScaledScreen;
        public String DisplayDevice;
        public String DisplayIdentifier;
        internal Rect Bounds;

        public MainWindow(bool isBottom=false)
        {
            InitializeComponent();
            this.DataContext = this;
            BannerPosition = (isBottom) ? "bottom" : "top";
            showTimer = new DispatcherTimer();
            showTimer.Interval = TimeSpan.FromSeconds(3);
            showTimer.Tick += (s, _) => this.Window_Show();
            ClassificationColors = new()
            {
                {"UNCATEGORIZED", "#008000"}
            };

            ClassificationLabels = new()
            {
                {"UNCATEGORIZED", "Uncategorized"}
            };

            bool checkRegPath = Utils.TestRegPath(@"HKEY_LOCAL_MACHINE\SOFTWARE\ClassBanner\");
            if (checkRegPath)
            {
                string LeftDisplayFormat = Utils.GetRegValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\ClassBanner\", "LeftDisplay");
                string RightDisplayFormat = Utils.GetRegValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\ClassBanner\", "RightDisplay");
                BannerLabel = Utils.GetRegValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\ClassBanner\", "BannerLabel");
                //Function still doesn't work with bools
                //bool HideOnBannerMouseOver = bool.Parse(Utils.GetRegValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\ClassBanner\", "HideOnMouseOver"));
                Int32 BannerOpacityLvl = Int32.Parse(Utils.GetRegValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\ClassBanner\", "BannerOpacity"));
                REGULAR_OPACITY = BannerOpacityLvl / 100.0;
                Opacity = REGULAR_OPACITY;
                HostName = Utils.GetHostName();
                CurrentUser = Utils.GetCurrentUser();

                switch (LeftDisplayFormat)
                {
                    case "@HOST":
                        LeftDisplay = HostName;
                        break;
                    case "@USER":
                        LeftDisplay = CurrentUser;
                        break;
                    default:
                        break;
                }
                switch (RightDisplayFormat)
                {
                    case "@HOST":
                        RightDisplay = HostName;
                        break;
                    case "@USER":
                        RightDisplay = CurrentUser;
                        break;
                    default:
                        break;
                }
                lblLeftDisplay.Content = LeftDisplay;
                lblRightDisplay.Content = RightDisplay;
                lblBannerLabel.Content = BannerLabel;


            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;

        }

        private void Window_Show()
        {

            if (Visibility == Visibility.Hidden)
            {
                Point mousePos = GetCursorPosition();
                if (!isBottom) {
                    if (mousePos.Y <= this.Bounds.Bottom)
                    {
                        return;
                    }
                } else {
                    if (mousePos.Y >= this.Bounds.Top)
                    {
                        return;
                    }
                }
                Visibility = Visibility.Visible;
                var anim = new DoubleAnimation(REGULAR_OPACITY,(Duration)TimeSpan.FromSeconds(.5), FillBehavior.Stop);
                anim.Completed += (s, _) =>
                {
                    showTimer.Stop();
                    this.BeginAnimation(Window.HeightProperty, null);
                };
                this.BeginAnimation(UIElement.OpacityProperty, anim);
            }
        }
        private void Window_MouseEnter(object sender, EventArgs e)
        {
            //stop expanding
            var anim = new DoubleAnimation(REGULAR_OPACITY, (Duration)TimeSpan.FromSeconds(.5), FillBehavior.Stop);
            anim.Completed += (s, _) =>
            {
                this.Hide();
            };
            this.BeginAnimation(UIElement.OpacityProperty, anim);
            showTimer.Start();
        }

        private void Window_Activated(object sender, EventArgs e)
        {


        }
    }
}
