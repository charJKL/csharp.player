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
    class Unknown : BaseFrame
    {
        protected FrameworkElement Layer;
        protected TextBlock Title;
        protected TextBlock Text;

        public Unknown(Window screen, FileInfo file, int time) : base(screen, file, time)
        {
            Layer = Screen.FindName("unknown") as FrameworkElement;
            Title = Screen.FindName("unknownTitle") as TextBlock;
            Text = Screen.FindName("unknownText") as TextBlock;
        }

        public override void clearLayer()
        {
            Layer.Visibility = Visibility.Hidden;
            WasInit = false;
        }

        protected override void drawLayer()
        {
            Layer.Visibility = Visibility.Visible;
            Title.Text = "Unknown extension";
            Text.Text = File.FullName;
        }

    }
}
