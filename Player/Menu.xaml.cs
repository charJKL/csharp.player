using Player.Frame;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml;

namespace Player
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    /// 
    public struct Settings
    {
        public int Image;
        public int Video;
        public int Load;
        public int Website;
        public int Unkown;
    }

    public partial class Menu : Window
    {
        const string FOLDER_NAME = "playlist";
        const string SETTINGS_NAME = "Settings.txt";

        private DispatcherTimer Player;
        private Screen Screen;
        private Settings Settings;
        private int Index;
        private string[] Files;
        private BaseFrame[] Frames;

        public Menu()
        {
            InitializeComponent();

            Screen = new Screen();
            Screen.Show();

            Settings = readSettings();
            Files = readFolder();
            Frames = createFrames(Files);
            Index = 0;
            Screen.setFrame(Frames[Index]);
            updatePlaylist(Index);
            updateSettings();

            Player = new DispatcherTimer();
            Player.Interval = new TimeSpan(0, 0, 0, 0, 30);
            Player.Tick += update;
            Player.Start();
        }

        private void update(object sender, EventArgs e)
        {
            try
            {
                if (Frames[Index].isEnd())
                {
                    Frames[Index].clearLayer();
                    Index = (Index + 1 >= Frames.Length) ? 0 : Index + 1;
                    Screen.setFrame(Frames[Index]);
                    updatePlaylist(Index);
                }
                Frames[Index].drawFrame();
            }catch(Exception ex)
            {
                console("Media resource corrupted: " + Files[Index], true);
                console(ex.Message, true);
                Frames[Index].clearLayer();
                Index = (Index + 1 >= Frames.Length) ? 0 : Index + 1;
                Screen.setFrame(Frames[Index]);
                updatePlaylist(Index);
            }
        }

        private string[] readFolder()
        {
            string path = Environment.CurrentDirectory + '\\' + FOLDER_NAME;
            console("Searching for " + path + "...");
            if (!Directory.Exists(path))
            {
                console("Can't find playlist. Create default folder.", true);
                Directory.CreateDirectory(path);
            }
            console("Read playlist from: " + path + ".");
            string[] files = Directory.GetFiles(path); ;
            if (files.Length == 0)
            {
                console("Playlist is empty. Add files and click refresh button.", true);
                return new string[0];
            }
            return files;
        }

        private Settings readSettings()
        {
            // Init settings with default value.
            var config = new Settings();
                config.Image = 5;
                config.Video = 10;
                config.Load = 10;
                config.Website = 5;
                config.Unkown = 1;
            string filepath = Environment.CurrentDirectory + '\\' + SETTINGS_NAME;
            if (!File.Exists(filepath))
            {
                console("Settings is missing. Create deafult settings file.");
                resetSettings(config);
            }
            StreamReader file = new StreamReader(filepath);
            try
            {
                string line = "";
                while( (line = file.ReadLine()) != null )
                {
                    string[] option = line.Split('=');
                    switch( option[0] )
                    {
                        case "IMAGE_DURATION":
                            config.Image = int.Parse(option[1]);
                            break;

                        case "VIDEO_LOAD_TIME":
                            config.Video = int.Parse(option[1]);
                            break;

                        case "MAX_LOAD_TIME":
                            config.Load = int.Parse(option[1]);
                            break;

                        case "WEBSITE_DURATION":
                            config.Website = int.Parse(option[1]);
                            break;

                        case "UNKNOWN_DURATION":
                            config.Unkown = int.Parse(option[1]);
                            break;

                        default:
                            throw new Exception("Unknow settings.");
                    }
                }
                file.Close();
            }
            catch (Exception e)
            {
                file.Close();
                console("Settings is wrong. Proceed to reset config file.", true);
                console(e.Message, true);
                resetSettings(config);
            }
            return config;
        }

        private void resetSettings(Settings settings)
        {
            string path = Environment.CurrentDirectory + '\\' + SETTINGS_NAME;
            StreamWriter config = new StreamWriter(path);
                config.WriteLine("IMAGE_DURATION="+ settings.Image);
                config.WriteLine("VIDEO_LOAD_TIME="+ settings.Video);
                config.WriteLine("MAX_LOAD_TIME="+ settings.Load);
                config.WriteLine("WEBSITE_DURATION="+ settings.Website);
                config.WriteLine("UNKNOWN_DURATION="+ settings.Unkown);
                config.Close();
            console("DONE. New settings file saved.");
        }

        private BaseFrame getFrame(string filepath)
        {
            FileInfo file = new FileInfo(filepath);
            switch( file.Extension.ToLower())
            {
                case ".bmp":
                case ".png":
                case ".jpg":
                    return new Frame.Image(Screen, file, Settings.Image);

                case ".mp4":
                case ".mov":
                case ".wmv":
                case ".avi":
                    return new Frame.Video(Screen, file, Settings.Video);

                case ".url":
                    return new Frame.Website(Screen, file, Settings.Load, Settings.Website);

                default:
                    return new Frame.Unknown(Screen, file, Settings.Unkown);
            }
        }

        private BaseFrame[] createFrames(string[] files)
        {
            if( files.Length == 0)
            {
                return new BaseFrame[1] { new Frame.Empty(Screen, 1) };
            }
            BaseFrame[] frames = new BaseFrame[files.Length];
            for(var i=0; i < files.Length; i++)
            {
                frames[i] = getFrame(files[i]);
            }
            return frames;
        }

        public void updatePlaylist(int index)
        {
            string playlist = "";
            for(var i = 0; i < Files.Length; i++ )
            {
                playlist += (i == index) ? "\u25B6 " + Files[i] + "\n" : "    " + Files[i] + "\n";
            }
            Playlist.Text = playlist;
        }

        public void updateSettings()
        {
            imageDuration.Text = Settings.Image + "s.";
            websiteLoadTime.Text = Settings.Load + "s.";
            websiteDuration.Text = Settings.Website + "s.";
            unknownDuration.Text = Settings.Unkown + "s.";
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            console("Refresh playlist...");
            Frames[Index].clearLayer();
            Settings = readSettings();
            Files = readFolder();
            Frames = createFrames(Files);
            Screen.reset();
            Index = 0;
            updatePlaylist(Index);
            updateSettings();
            Screen.setFrame(Frames[Index]);
            console("Done. Restart playback.");
        }

        private void ScreenOnOff(object sender, RoutedEventArgs e)
        {
            Screen.changeFullScreenMode();
        }

        private void console(string msg, bool error = false)
        {
            Console.Text += DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ((error) ? " [ ! ] " : "       ") + msg + "\n";
            ConsoleWindow.ScrollToEnd();
        }
    }
}
