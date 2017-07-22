using Player.Frame;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Player
{
    /// <summary>
    /// Interaction logic for Screen.xaml
    /// </summary>
    public partial class Screen : Window
    {
        private bool IsFullScreenMode;
        private BaseFrame Frame;

        public Screen()
        {
            InitializeComponent();
            IsFullScreenMode = false;
        }

        public void setFrame(BaseFrame frame)
        {
            Frame = frame;
        }

        public void reset()
        {
            unknown.Visibility = Visibility.Hidden;
            video.Visibility = Visibility.Hidden;
            website.Visibility = Visibility.Hidden;
            progress.Fill = new SolidColorBrush(Color.FromRgb(104, 255, 0));
        }

        public void changeFullScreenMode()
        {
            if(IsFullScreenMode)
            {
                this.WindowState = WindowState.Normal;
                help.Visibility = Visibility.Visible;
                IsFullScreenMode = false;
                return;
            }
            this.WindowState = WindowState.Maximized;
            help.Visibility = Visibility.Hidden;
            IsFullScreenMode = true;
        }

        private void Move(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void KeyboradAction(object sender, KeyEventArgs e)
        {
            if ( e.Key != Key.F)
            {
                return;
            }
            changeFullScreenMode();
        }

        private void DoubleClick(object sender, MouseButtonEventArgs e)
        {
            changeFullScreenMode();
        }

        private void startLoading(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if( Frame is Website )
            {
                Frame.startWebsite();
            }
            progress.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
        }

        private void endLoading(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if( Frame is Website )
            {
                Frame.endWebsite();
            }
            progress.Fill = new SolidColorBrush(Color.FromRgb(104, 255, 0));
        }

        private void videoReady(object sender, RoutedEventArgs e)
        {
            if( Frame is Video )
            {
                Frame.videoReady();
            }
        }

        private void videoEnd(object sender, RoutedEventArgs e)
        {
            if( Frame is Video )
            {
                Frame.videoEnd();
            }
        }
    }
}
