using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Windows.Shapes;
using WpfScreenHelper;
using ClassBanner;

namespace ClassBanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    //
    public partial class MainWindow : Window
    {
        private DispatcherTimer showTimer;
        private const int BANNER_HEIGHT = 23;
        private int REGULAR_OPACITY = 100;
        private Dictionary<string, string> ClassificationColors;
        private Dictionary<string, string> ClassificationLabels;


        public MainWindow()
        {
            InitializeComponent();

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
            Utils.GetHostName();


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
                Visibility = Visibility.Visible;
                var anim = new DoubleAnimation(REGULAR_OPACITY, (Duration)TimeSpan.FromSeconds(.25), FillBehavior.Stop);
                anim.Completed += (s, _) =>
                {
                    showTimer.Stop();
                    this.BeginAnimation(Window.HeightProperty, null);
                };
                this.BeginAnimation(UIElement.OpacityProperty, anim);
                var animShrink = new DoubleAnimation(BANNER_HEIGHT, (Duration)TimeSpan.FromSeconds(.25), FillBehavior.Stop);
                this.BeginAnimation(Window.HeightProperty, animShrink);
            }            
        }
        private void Window_MouseEnter(object sender, EventArgs e)
        {
            //stop expanding
            var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(.25), FillBehavior.Stop);
            anim.Completed += (s, _) =>
            {
                this.Hide();
            };
            this.BeginAnimation(UIElement.OpacityProperty, anim);
            var animShrink = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(.25), FillBehavior.Stop);

            this.BeginAnimation(Window.HeightProperty, animShrink);

            showTimer.Start();
        }
        
        private void Window_Activated(object sender, EventArgs e)
        {

            
        }
    }
}
