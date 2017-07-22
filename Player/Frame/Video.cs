using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Player.Frame
{
    class Video : BaseFrame
    {
        MediaElement Layer;
        bool Load;

        public Video(Window screen, FileInfo file, int time) : base(screen, file, time)
        {
            Layer = Screen.FindName("video") as MediaElement;
            Load = false;
            Time = time * 1000;
        }

        public override void clearLayer()
        {
            Layer.Visibility = Visibility.Hidden;
            Layer.Source = null;
            WasInit = false;
        }

        protected override void drawLayer()
        {
            if ( !Load )
            {
                Progress.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
            Layer.Visibility = Visibility.Visible;
            Layer.Source = new Uri(File.FullName);
            Layer.Play();
        }

        public override void videoReady()
        {
            Time = (int) Layer.NaturalDuration.TimeSpan.TotalMilliseconds;
            End = Environment.TickCount + Time;
            Progress.Fill = new SolidColorBrush(Color.FromRgb(104, 255, 0));
            Load = true;
        }

        private MediaState GetMediaState(MediaElement myMedia)
        {
            FieldInfo hlp = typeof(MediaElement).GetField("_helper", BindingFlags.NonPublic | BindingFlags.Instance);
            object helperObject = hlp.GetValue(myMedia);
            FieldInfo stateField = helperObject.GetType().GetField("_currentState", BindingFlags.NonPublic | BindingFlags.Instance);
            MediaState state = (MediaState)stateField.GetValue(helperObject);
            return state;
        }
    }
}
