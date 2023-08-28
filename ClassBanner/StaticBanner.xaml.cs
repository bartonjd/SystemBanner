using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using WpfScreenHelper;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media;
using static DesktopBanner.NativeMethods;
using static System.Net.Mime.MediaTypeNames;


namespace DesktopBanner
{


    public partial class StaticBanner : Banner
    {

        new private DisplayMode DisplayMode =  DesktopBanner.DisplayMode.Static;
        //String used to identify AppBars registered in Windows by this application, this will alos be needed in order to remove the registration
        private const string APPBAR_UNIQUE_IDENTIFIER = "DesktopBanner_AppBarMessage_ECDFB5206FC2";
        private bool IsAppBarRegistered;
        private bool IsAppBarPositioned = false;
        private bool IsInAppBarResize;

        static StaticBanner()
        {
            //ShowInTaskbarProperty.OverrideMetadata(typeof(MainWindow), new FrameworkPropertyMetadata(false));
            /*MinHeightProperty.OverrideMetadata(typeof(MainWindow), new FrameworkPropertyMetadata(20d, MinMaxHeightWidth_Changed));
            MinWidthProperty.OverrideMetadata(typeof(MainWindow), new FrameworkPropertyMetadata(20d, MinMaxHeightWidth_Changed));
            MaxHeightProperty.OverrideMetadata(typeof(MainWindow), new FrameworkPropertyMetadata(MinMaxHeightWidth_Changed));
            MaxWidthProperty.OverrideMetadata(typeof(MainWindow), new FrameworkPropertyMetadata(MinMaxHeightWidth_Changed));
            */
        }
        public StaticBanner(bool ShowOnBottom) : base(ShowOnBottom)
        {

            bool checkRegPath = Reg.KeyExists(@"HKEY_LOCAL_MACHINE\SOFTWARE\DesktopBanner\");
            if (checkRegPath)
            {


                if (ShowOnBottom)
                {
                    DockMode = AppBarDockMode.Bottom;
                }
                else
                {
                    DockMode = AppBarDockMode.Top;
                }
            }
            bool checkUserRegPath = Reg.KeyExists(@"HKEY_CURRENT_USER\SOFTWARE\DesktopBanner\");
            if (!checkUserRegPath)
            {
                //Ensure user registration key exists
                Reg.NewKey(@"HKEY_CURRENT_USER\SOFTWARE\", "DesktopBanner");


            }
        }

            public void Unregister() {
            if (IsAppBarRegistered)
            {
                var abd = GetAppBarData();
                SHAppBarMessage(ABM.REMOVE, ref abd);
                IsAppBarRegistered = false;
                IsAppBarPositioned = false;
            }
        }

        


        private void Window_Show()
        {
            //Visibility = Visibility.Visible;
        }

        public AppBarDockMode DockMode
        {
            get { return (AppBarDockMode)GetValue(DockModeProperty); }
            set { SetValue(DockModeProperty, value); }
        }
        
        public static readonly DependencyProperty DockModeProperty =
            DependencyProperty.Register("DockMode", typeof(AppBarDockMode), typeof(StaticBanner),
                new FrameworkPropertyMetadata(AppBarDockMode.Top, DockLocation_Changed));
        
        public MonitorInfo Monitor
        {
            get { return (MonitorInfo)GetValue(MonitorProperty); }
            set { SetValue(MonitorProperty, value); }
        }

        public static readonly DependencyProperty MonitorProperty =
            DependencyProperty.Register("Monitor", typeof(MonitorInfo), typeof(StaticBanner),
                new FrameworkPropertyMetadata(null, DockLocation_Changed));

        public int DockedWidthOrHeight
        {
            get { return (int)GetValue(DockedWidthOrHeightProperty); }
            set { SetValue(DockedWidthOrHeightProperty, value); }
        }

        public static readonly DependencyProperty DockedWidthOrHeightProperty =
            DependencyProperty.Register("DockedWidthOrHeight", typeof(int), typeof(StaticBanner),
                new FrameworkPropertyMetadata(BANNER_HEIGHT, DockLocation_Changed, DockedWidthOrHeight_Coerce));

        private static object DockedWidthOrHeight_Coerce(DependencyObject d, object baseValue)
        {
            var @this = (StaticBanner)d;
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

                AppBarDockMode.Top    => BoundIntToDouble(newValue, @this.MinHeight, @this.MaxHeight),
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
            var @this = (StaticBanner)d;

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
            SaveAppbarRestoreData();
            
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (e.Cancel)
            {
                return;
            }

            if (IsAppBarRegistered)
            {
                var abd = GetAppBarData();
                SHAppBarMessage(ABM.REMOVE, ref abd);
                IsAppBarRegistered = false;
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
            if (!IsAppBarPositioned)
            {
                var abd = GetAppBarData();
                abd.rc = (RECT)Display.Bounds;

                SHAppBarMessage(ABM.QUERYPOS, ref abd);

                var dockedWidthOrHeightInDesktopPixels = WpfDimensionToDesktop(DockedWidthOrHeight);
                if ((abd.rc.Height > 300) || (abd.rc.Height == 0))
                {
                    switch (DockMode)
                    {
                        case AppBarDockMode.Top:
                            abd.rc.bottom = abd.rc.top + dockedWidthOrHeightInDesktopPixels;
                            break;
                        case AppBarDockMode.Bottom:
                            abd.rc.top = (int)(Display.WorkingArea.Bottom - dockedWidthOrHeightInDesktopPixels);
                            break;
                        case AppBarDockMode.Left:
                            abd.rc.right = abd.rc.left + dockedWidthOrHeightInDesktopPixels;
                            break;
                        case AppBarDockMode.Right:
                            abd.rc.left = abd.rc.right - dockedWidthOrHeightInDesktopPixels;
                            break;
                        default: throw new NotSupportedException();
                    }
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

                IsAppBarPositioned = true;
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
                    _AppBarMessageId = RegisterWindowMessage(APPBAR_UNIQUE_IDENTIFIER);
                }

                return _AppBarMessageId;
            }
        }



/*        private static string _ABDWindowId = "";
        public static string ABDWindowId
        {
            get
            {
                if (_ABDWindowId == "")
                {
                    _ABDWindowId = AppBarMessageId;
                }

                return _AppBarMessageId;
            }
        }

*/














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

        public void SaveAppbarRestoreData() {

            var appBarData = GetAppBarData();
            DateTimeOffset currentDateTimeOffset = DateTimeOffset.UtcNow;
            long unixEpochTime = currentDateTimeOffset.ToUnixTimeMilliseconds();

            

            Reg.SetStringProperty("HKEY_CURRENT_USER\\SOFTWARE\\DesktopBanner\\ABRegistrations", AppBarMessageId.ToString() + "_" + unixEpochTime, appBarData.hWnd.ToString());

            /*
             * 
            // Main Application (During AppBar Registration)
            IntPtr appBarWindowHandle = ...; // Your AppBar window handle
            string uniqueIdentifier = "YourAppIdentifier"; // Use a unique identifier

            // Store the AppBar window handle in the Windows Registry
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\DesktopBanner\\ABRegistrations"))
            {
                if (key != null)
                {
                    Reg.S key.SetValue(uniqueIdentifier, appBarWindowHandle.ToString());
                }
            }*/
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