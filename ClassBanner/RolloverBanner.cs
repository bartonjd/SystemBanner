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

namespace DesktopBanner
{
    using System.Reflection;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for Banner.xaml
    /// </summary>



    public partial class RolloverBanner : Banner
    {
        new private bool ShowOnBottom { get; set; }
        private DispatcherTimer showTimer;
        new public DisplayMode DisplayMode = DisplayMode.Rollover;


        static RolloverBanner()
        {
            //ShowInTaskbarProperty.OverrideMetadata(typeof(MainWindow), new FrameworkPropertyMetadata(false));

        }
        public RolloverBanner(bool ShowOnBottom) : base(ShowOnBottom)
        {
            InitializeComponent();
            DataContext = this;
            this.ShowOnBottom = ShowOnBottom;
            showTimer = new DispatcherTimer();
            showTimer.Interval = TimeSpan.FromSeconds(3);
            showTimer.Tick += (s, _) => Window_Show();

        }


        private void Window_Show()
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
                var anim = new DoubleAnimation(RegularOpacity, (Duration)TimeSpan.FromSeconds(.5), FillBehavior.Stop);
                anim.Completed += (s, _) =>
                {
                    showTimer.Stop();
                    BeginAnimation(Window.HeightProperty, null);
                };
                BeginAnimation(UIElement.OpacityProperty, anim);
            }
        }
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e); // Call the base event handler
            //stop expanding
            var anim = new DoubleAnimation(RegularOpacity, (Duration)TimeSpan.FromSeconds(.5), FillBehavior.Stop);
            anim.Completed += (s, _) =>
            {
                Hide();
            };
            BeginAnimation(UIElement.OpacityProperty, anim);
            showTimer.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (e.Cancel)
            {
                return;
            }
        }
    }
}