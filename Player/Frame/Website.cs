using Shell32;
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
    class Website : BaseFrame
    {
        WebBrowser Layer;
        int Duration;

        public Website(Window screen, FileInfo file, int load, int time) : base(screen, file, time)
        {
            Layer = Screen.FindName("website") as WebBrowser;
            Time = load * 1000;
            Duration = time * 1000;
        }

        public override void clearLayer()
        {
            Layer.Visibility = Visibility.Hidden;
            Progress.Fill = new SolidColorBrush(Color.FromRgb(104, 255, 0));
            WasInit = false;
        }

        protected override void drawLayer()
        {
            Layer.Visibility = Visibility.Visible;
            Progress.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            Layer.Navigate(readUrl());
        }

        public override void endWebsite()
        {
            Progress.Fill = new SolidColorBrush(Color.FromRgb(104, 255, 0));
            HideScriptErrors(Layer, true);
            Time = scrollDown();
            End = Environment.TickCount + Time;
        }

        private int scrollDown()
        {
            string page = "var page = Math.max( document.body.scrollHeight, document.body.offsetHeight, document.documentElement.clientHeight, document.documentElement.scrollHeight, document.documentElement.offsetHeight ); ";
            string browser = "var browser = window.innerHeight || document.body.clientHeight; ";
            Layer.InvokeScript("eval", new object[] { page });
            Layer.InvokeScript("eval", new object[] { browser });
            int pageHeight = int.Parse(Layer.InvokeScript("eval", new object[] { "page" }).ToString());
            int browserHeight = int.Parse(Layer.InvokeScript("eval", new object[] { "browser" }).ToString());

            int milliseconds = (pageHeight - browserHeight) * 20;

            string js = " setInterval(function(){ window.scrollBy(0, 1);}, 20)";
            Layer.InvokeScript("eval", new object[] { js });
            return milliseconds;
        }

        private void HideScriptErrors(WebBrowser wb, bool hide)
        {
            var fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;
            var objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null)
            {
                wb.Loaded += (o, s) => HideScriptErrors(wb, hide); //In case we are to early
                return;
            }
            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { hide });
        }

        private string readUrl()
        {
            Shell shell = new Shell();
            Folder folder = shell.NameSpace(File.DirectoryName);
            FolderItem link = folder.ParseName(File.Name);
            return folder.GetDetailsOf(link, 196);
        }

    }
}
