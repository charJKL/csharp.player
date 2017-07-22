using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace Player.Frame
{
    abstract public class BaseFrame
    {
        protected Window Screen;
        protected FileInfo File;
        protected bool WasInit;
        protected int End;
        protected int Time;

        protected Rectangle Progress;

        public BaseFrame(Window screen, int time)
        {
            Screen = screen;
            Time = time * 1000;

            Progress = screen.FindName("progress") as Rectangle;
        }

        public BaseFrame(Window screen, FileInfo file, int time)
        {
            Screen = screen;
            File = file;
            Time = time * 1000;

            Progress = screen.FindName("progress") as Rectangle;
        }

        public virtual bool isEnd()
        {
            if (!WasInit) return false;
            return Environment.TickCount > End;
        }

        public void drawFrame()
        {
            if( !WasInit)
            {
                drawLayer();
                End = Environment.TickCount + Time;
                WasInit = true;
            }
            drawProgress();
        }

        public virtual void drawProgress()
        {
            float percentage = (float)(End - Environment.TickCount) / Time;
            Progress.Width = Screen.Width * (1 - percentage);
        }

        abstract protected void drawLayer();

        abstract public void clearLayer();

        public virtual void startWebsite()
        {
        }

        public virtual void endWebsite()
        {
        }

        public virtual void videoReady()
        {
        }

        public virtual void videoEnd()
        {
        }
    }
}
