using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using WpfScreenHelper;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Interop;


namespace DesktopBanner
{
    /// <summary>
    /// Interaction logic for Banner.xaml
    /// </summary>

    using System.Windows.Media;
    using static NativeMethods;

    public partial class Banner : Window
    {
        public bool ShowOnBottom { get; set; }
        public String? BannerPosition { get; set; }
        private DispatcherTimer showTimer;
        private int? BannerType = 2;
        private const Int32 BANNER_HEIGHT = 23;
        private Double REGULAR_OPACITY = 1.0;
        private Dictionary<string, string> BannerColors;
        private Dictionary<string, string> BannerLabels;
        private String HostName = "";
        private String CurrentUser = "";
        public String? BannerLabel = "";

        public String BannerColor = "#008000";
        public String TextColor = "#000000";
        public Screen? Display;
        public String LeftDisplay = "";
        public String RightDisplay = "";
        public Rect ScaledScreen;
        public String? DisplayDevice;
        public String? DisplayIdentifier;
        public Rect Bounds;
        private bool IsAppBarRegistered;
        private bool IsInAppBarResize;


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


            if (ShowOnBottom && BannerType == 2)
            {
                DockMode = AppBarDockMode.Bottom;
            }
            else {
                DockMode = AppBarDockMode.Top;
            }
        
            BannerColors = new()
            {
                {"UNCATEGORIZED", "#008000"}
            };

            BannerLabels = new()
            {
                {"UNCATEGORIZED", "Uncategorized"}
            };

            bool checkRegPath = Reg.KeyExists(@"HKEY_LOCAL_MACHINE\SOFTWARE\DesktopBanner\");
            if (checkRegPath)
            {
                String? LeftDisplayFormat = Reg.GetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\DesktopBanner\", "LeftDisplay");
                String? RightDisplayFormat = Reg.GetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\DesktopBanner\", "RightDisplay");
                BannerLabel = Reg.GetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\DesktopBanner\", "BannerLabel");
                //BannerType 1 is Rollup banner (hides on mouseover, BannerType 2 is static banner 
                if (Reg.PropertyExists(@"HKEY_LOCAL_MACHINE\SOFTWARE\DesktopBanner\", "BannerType"))
                {
                    BannerType = (int?)Reg.GetInt(@"HKEY_LOCAL_MACHINE\SOFTWARE\DesktopBanner\", "BannerType");
                }
                Double BannerOpacityLvl = Reg.GetDouble(@"HKEY_LOCAL_MACHINE\SOFTWARE\DesktopBanner\", "BannerOpacity");
                if (BannerOpacityLvl == -1)
                {
                    BannerOpacityLvl = REGULAR_OPACITY;
                }
                REGULAR_OPACITY = (BannerOpacityLvl) / 100.0;
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
                        LeftDisplay = LeftDisplayFormat ?? "";
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
                        RightDisplay = RightDisplayFormat ?? "";
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
            if (BannerType == 1)
            {
                if (Visibility == Visibility.Hidden)
                {
                    Point mousePos = GetCursorPosition();
                    if (!ShowOnBottom)
                    {
                        if (mousePos.Y <= Bounds.Bottom)
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (mousePos.Y >= Bounds.Top)
                        {
                            return;
                        }
                    }
                    Visibility = Visibility.Visible;
                    var anim = new DoubleAnimation(REGULAR_OPACITY, (Duration)TimeSpan.FromSeconds(.5), FillBehavior.Stop);
                    anim.Completed += (s, _) =>
                    {
                        showTimer.Stop();
                        BeginAnimation(Window.HeightProperty, null);
                    };
                    BeginAnimation(UIElement.OpacityProperty, anim);
                }
            }
        }
        private void Window_MouseEnter(object sender, EventArgs e)
        {
            if (BannerType == 1)
            {
                //stop expanding
                var anim = new DoubleAnimation(REGULAR_OPACITY, (Duration)TimeSpan.FromSeconds(.5), FillBehavior.Stop);
                anim.Completed += (s, _) =>
                {
                    Hide();
                };
                BeginAnimation(UIElement.OpacityProperty, anim);
                showTimer.Start();
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {


        }

        public AppBarDockMode DockMode
        {
            get { return (AppBarDockMode)GetValue(DockModeProperty); }
            set { SetValue(DockModeProperty, value); }
        }
        
        public static readonly DependencyProperty DockModeProperty =
            DependencyProperty.Register("DockMode", typeof(AppBarDockMode), typeof(Banner),
                new FrameworkPropertyMetadata(AppBarDockMode.Top, DockLocation_Changed));
        
        public MonitorInfo Monitor
        {
            get { return (MonitorInfo)GetValue(MonitorProperty); }
            set { SetValue(MonitorProperty, value); }
        }

        public static readonly DependencyProperty MonitorProperty =
            DependencyProperty.Register("Monitor", typeof(MonitorInfo), typeof(Banner),
                new FrameworkPropertyMetadata(null, DockLocation_Changed));

        public int DockedWidthOrHeight
        {
            get { return (int)GetValue(DockedWidthOrHeightProperty); }
            set { SetValue(DockedWidthOrHeightProperty, value); }
        }

        public static readonly DependencyProperty DockedWidthOrHeightProperty =
            DependencyProperty.Register("DockedWidthOrHeight", typeof(int), typeof(Banner),
                new FrameworkPropertyMetadata(23, DockLocation_Changed, DockedWidthOrHeight_Coerce));

        private static object DockedWidthOrHeight_Coerce(DependencyObject d, object baseValue)
        {
            var @this = (Banner)d;
            var newValue = (int)baseValue;

/*            switch (@this.DockMode)
            {
                case AppBarDockMode.Left:
                case AppBarDockMode.Right:
                    return BoundIntToDouble(newValue, @this.MinWidth, @this.MaxWidth);

                case AppBarDockMode.Top:
                case AppBarDockMode.Bottom:
                    return BoundIntToDouble(newValue, @this.MinHeight, @this.MaxHeight);

                default: throw new NotSupportedException();
            }*/

            return @this.DockMode switch { 

                AppBarDockMode.Left  => BoundIntToDouble(newValue, @this.MinWidth, @this.MaxWidth),
                AppBarDockMode.Right => BoundIntToDouble(newValue, @this.MinWidth, @this.MaxWidth),

                AppBarDockMode.Top => BoundIntToDouble(newValue, @this.MinHeight, @this.MaxHeight),
                AppBarDockMode.Bottom => BoundIntToDouble(newValue, @this.MinHeight, @this.MaxHeight),

                _                     => throw new NotSupportedException()
            };
        }
    
    private static int BoundIntToDouble(int value, double min, double max)
        {
            if (min > value)
            {
                return (int)Math.Ceiling(min);
            }
            if (max < value)
            {
                return (int)Math.Floor(max);
            }

            return value;
        }

        private static void MinMaxHeightWidth_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(DockedWidthOrHeightProperty);
        }

        private static void DockLocation_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = (Banner)d;

            if (@this.IsAppBarRegistered)
            {
                @this.OnDockLocationChanged();
            }
        }

        protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
        {
            base.OnDpiChanged(oldDpi, newDpi);

            OnDockLocationChanged();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            if (BannerType == 2)
            {
                base.OnSourceInitialized(e);

                // add the hook, setup the appbar
                var source = (HwndSource)PresentationSource.FromVisual(this);

                if (!ShowInTaskbar)
                {
                    var exstyle = (ulong)GetWindowLongPtr(source.Handle, GWL_EXSTYLE);
                    exstyle |= (ulong)((uint)WS_EX_TOOLWINDOW);
                    SetWindowLongPtr(source.Handle, GWL_EXSTYLE, unchecked((IntPtr)exstyle));
                }

                source.AddHook(WndProc);

                var abd = GetAppBarData();
                SHAppBarMessage(ABM.NEW, ref abd);

                // set our initial location
                this.IsAppBarRegistered = true;
                OnDockLocationChanged();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (e.Cancel)
            {
                return;
            }
            if (BannerType == 2)
            {
                if (IsAppBarRegistered)
                {
                    var abd = GetAppBarData();
                    SHAppBarMessage(ABM.REMOVE, ref abd);
                    IsAppBarRegistered = false;
                }
            }
        }

        private int WpfDimensionToDesktop(double dim)
        {
            var dpi = VisualTreeHelper.GetDpi(this);

            return (int)Math.Ceiling(dim * dpi.PixelsPerDip);
        }

        private double DesktopDimensionToWpf(double dim)
        {
            var dpi = VisualTreeHelper.GetDpi(this);

            return dim / dpi.PixelsPerDip;
        }

        private void OnDockLocationChanged()
        {
            if (IsInAppBarResize)
            {
                return;
            }
            
            var abd = GetAppBarData();
            abd.rc = (RECT)Display.Bounds;

            SHAppBarMessage(ABM.QUERYPOS, ref abd);

            var dockedWidthOrHeightInDesktopPixels = WpfDimensionToDesktop(DockedWidthOrHeight);
            switch (DockMode)
            {
                case AppBarDockMode.Top:
                    abd.rc.bottom = abd.rc.top + dockedWidthOrHeightInDesktopPixels;
                    break;
                case AppBarDockMode.Bottom:
                    abd.rc.top = abd.rc.bottom - dockedWidthOrHeightInDesktopPixels;
                    break;
                case AppBarDockMode.Left:
                    abd.rc.right = abd.rc.left + dockedWidthOrHeightInDesktopPixels;
                    break;
                case AppBarDockMode.Right:
                    abd.rc.left = abd.rc.right - dockedWidthOrHeightInDesktopPixels;
                    break;
                default: throw new NotSupportedException();
            }

            SHAppBarMessage(ABM.SETPOS, ref abd);
            IsInAppBarResize = true;
            try
            {
                WindowBounds = (Rect)abd.rc;
            }
            finally
            {
                IsInAppBarResize = false;
            }
            
        }

        private APPBARDATA GetAppBarData()
        {
            return new APPBARDATA()
            {
                cbSize = Marshal.SizeOf(typeof(APPBARDATA)),
                hWnd = new WindowInteropHelper(this).Handle,
                uCallbackMessage = AppBarMessageId,
                uEdge = (int)DockMode
            };
        }

        private static int _AppBarMessageId;
        public static int AppBarMessageId
        {
            get
            {
                if (_AppBarMessageId == 0)
                {
                    _AppBarMessageId = RegisterWindowMessage("AppBarMessage_EEDFB5206FC3");
                }

                return _AppBarMessageId;
            }
        }

        public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_WINDOWPOSCHANGING && !IsInAppBarResize)
            {
                var wp = Marshal.PtrToStructure<WINDOWPOS>(lParam);
                wp.flags |= SWP_NOMOVE | SWP_NOSIZE;
                Marshal.StructureToPtr(wp, lParam, false);
            }
            else if (msg == WM_ACTIVATE)
            {
                var abd = GetAppBarData();
                uint hresult = SHAppBarMessage(ABM.ACTIVATE, ref abd);
                var bob = 0;
            }
            else if (msg == WM_WINDOWPOSCHANGED)
            {
                var abd = GetAppBarData();
                uint hresult = SHAppBarMessage(ABM.WINDOWPOSCHANGED, ref abd);
            }
            else if (msg == AppBarMessageId)
            {
                switch ((ABN)(int)wParam)
                {
                    case ABN.POSCHANGED:
                        OnDockLocationChanged();
                        handled = true;
                        break;
                }
            }

            return IntPtr.Zero;
        }

        private Rect WindowBounds
        {
            set
            {
                this.Left = DesktopDimensionToWpf(value.Left);
                this.Top = DesktopDimensionToWpf(value.Top);
                this.Width = DesktopDimensionToWpf(value.Width);
                this.Height = DesktopDimensionToWpf(value.Height);
            }
        }
    }
    public enum AppBarDockMode
    {
        Left = 0,
        Top,
        Right,
        Bottom
    }
}