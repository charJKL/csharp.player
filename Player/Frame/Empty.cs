using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Player.Frame
{
    class Empty : BaseFrame
    {
        protected FrameworkElement Layer;
        protected TextBlock Title;
        protected TextBlock Text;

        public Empty(Window screen, int time) : base(screen, time)
        {
            Layer = Screen.FindName("unknown") as FrameworkElement;
            Title = Screen.FindName("unknownTitle") as TextBlock;
            Text = Screen.FindName("unknownText") as TextBlock;
        }

        public override bool isEnd()
        {
            return false;
        }

        public override void clearLayer()
        {
            Layer.Visibility = Visibility.Hidden;
            WasInit = false;
        }

        protected override void drawLayer()
        {
            Layer.Visibility = Visibility.Visible;
            Title.Text = "Empty playlist.";
            Text.Text = "Playlist is empty. Add files and click refresh button.";
        }

        public override void drawProgress()
        {
            Progress.Width = Screen.Width;
        }

    }
}
