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

namespace ClassBanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    //
    public partial class MainWindow : Window
    {
        private bool FadedOut = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;


          /*  if (this.FadedOut) {
                //this.Show();
                var anim = new DoubleAnimation(100, (Duration)TimeSpan.FromSeconds(3), FillBehavior.Stop);
                anim.Completed += (s, _) => this.Window_Show();
                this.BeginAnimation(UIElement.OpacityProperty, anim);
            }*/
        }
        private void Window_Hide() {
            this.FadedOut = true;
            this.Hide();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += (s, _) => this.Show(); ;
            timer.Start();
        }
        private void Window_Show()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += (s,_) => this.Show(); ;
            timer.Start();
            this.FadedOut = false;
            
        }
        private void Window_MouseEnter(object sender, EventArgs e)
        {

            var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(1), FillBehavior.Stop);
            anim.Completed += (s, _) => this.Window_Hide();
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }



    }
}
