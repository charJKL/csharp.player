using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Player.Frame
{
    using System.Windows.Media.Imaging;
    using sysImage = System.Windows.Controls.Image;

    class Image : BaseFrame
    {
        sysImage Layer;

        public Image(Window screen, FileInfo file, int time) : base(screen, file, time)
        {
            Layer = Screen.FindName("image") as sysImage;
        }

        public override void clearLayer()
        {
            Layer.Visibility = Visibility.Hidden;
            WasInit = false;
        }

        protected override void drawLayer()
        {
            Layer.Visibility = Visibility.Visible;
            Layer.Source = new BitmapImage(new Uri(File.FullName));
        }
    }
}
