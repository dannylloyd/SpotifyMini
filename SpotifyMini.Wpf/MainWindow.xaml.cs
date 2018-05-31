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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FontAwesome.Sharp;
using NLog;
using SpotifyAPI.Local;
using SpotifyAPI.Local.Enums;
using SpotifyAPI.Local.Models;
using Application = System.Windows.Application;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Timer = System.Timers.Timer;

namespace SpotifyMini.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        SpotifyLocalAPI Spotify = new SpotifyLocalAPI();
        private int ReconnectAttempts = 0;
        private Track CurrentTrack;
        private Timer ReconnectTimer = new Timer(500);
        private Timer SettingsSaver = new Timer(5 * 60 * 1000);
        public LogWindow LoggerWindow;
        public Settings AppSettings;

        public MainWindow()
        {
            InitializeComponent();
            pictureBox1.Height = Double.NaN;
            pictureBox1.Width = Double.NaN;
            SetSettings();
            Startup();

            ReconnectTimer.Enabled = false;
            ReconnectTimer.Elapsed += (sender, args) => Startup();
            this.Closing += (sender, args) =>LoggerWindow?.Close();
            SettingsSaver.Elapsed += (sender, args) => Save();

            Spotify.OnPlayStateChange += (sender, args) =>
			{
				Debug.WriteLine("OnPlayStateChange, Playing:{0}", args.Playing);
				Logger.Debug("OnPlayStateChange, Playing:{0}", args.Playing);
                Log("PlayStateChange", $"Playing:{args.Playing}");
                SetPlayPause(args.Playing);
            };
            Spotify.OnTrackChange += (sender, args) =>
            {
                if (args.NewTrack?.TrackResource != null)
				{
					Debug.WriteLine("OnTrackChange, NewTrack:{0}", args.NewTrack.TrackResource.Name);
					Logger.Debug("OnTrackChange, NewTrack:{0}", args.NewTrack.TrackResource.Name);
                    var oldTrack = "None";
                    if (args.OldTrack.TrackResource != null)
                        oldTrack = args.OldTrack.TrackResource.Name;
                    Log("TrackChange", $"Old Track: {oldTrack} New Track: {args.NewTrack.TrackResource.Name}");
                    CurrentTrack = Spotify.GetStatus().Track;
                    DisplayCurrentSong(CurrentTrack);
                }
                else
				{
					Debug.WriteLine("TrackChange", "Disconnected from Spotify, trying to reconnect");
					Log("TrackChange", "Disconnected from Spotify, trying to reconnect");
                    Startup();
                }
            };

            Spotify.OnTrackTimeChange += (sender, args) =>
            {
	            if (Math.Abs(args.TrackTime) < .01)
	            {
					Log("OnTrackTimeChange", $"Track probably changed, we should get the song info again. Track pos {args.TrackTime}");
		            Startup();
				}
                //Log("TrackTimeChange", $"TrackTime: {args.TrackTime}");

                ChangeTrackPosition(args.TrackTime);
            };

            Spotify.OnVolumeChange += (sender, args) =>
            {
                Log("TrackTimeChange", $"OldVolume: {args.OldVolume} NewVolume: {args.NewVolume}");
            };

            Spotify.ListenForEvents = true;
        }



        public void SetSettings()
        {
            var fileName = $"{System.IO.Directory.GetCurrentDirectory()}\\Settings.json";
            if (System.IO.File.Exists(fileName))
            {
                var settings = Newtonsoft.Json.JsonConvert.DeserializeObject<Settings>(System.IO.File.ReadAllText(fileName));
                AppSettings = settings;
                this.Height = settings.Height;
                this.Width = settings.Width;
                this.Top = settings.Top;
                this.Left = settings.Left;
                this.Topmost = settings.TopMost;
                this.chkTopMost.IsChecked = settings.TopMost;
            }
            else
            {
                System.IO.File.WriteAllText(fileName, Newtonsoft.Json.JsonConvert.SerializeObject(new Settings()));
            }
        }

        public void Startup()
        {
            SpotifyLocalAPI.RunSpotify();

            try
            {
                Spotify.Connect();
                Logger.Debug("Connected to spotify");

                Logger.Debug("Getting status");
                var status = Spotify.GetStatus();
                Logger.Debug("Done getting status");
	            if (CurrentTrack == null || CurrentTrack != status.Track)
	            {
					CurrentTrack = status.Track;
					DisplayCurrentSong(status.Track);
				}
				SetPlayPause(status.Playing);

                ChangeTrackPosition(status.PlayingPosition);
                ReconnectAttempts = 0;
            }
            catch (System.Net.WebException ex)
            {
                //"The remote server returned an error: (503) Server Unavailable."
                //ex
                //		ex.Status	ProtocolError	System.Net.WebExceptionStatus
                ReconnectAttempts++;
                Log("Startup", $"Error connecting to spotify:{ex.Status.ToString()}. Starting timer to reconnect");
                if (ReconnectAttempts < 10)
                    ReconnectTimer.Enabled = true;
                else
                {
                    Log("Startup", "Reconnect tried more than 10 times, stopping. Please Restart SpotifyMini.");
                }
            }
        }

        delegate void TrackPositionArgReturningVoidDelegate(double trackTime);
        public void ChangeTrackPosition(double trackTime)
        {
            if (!CheckAccess())
            {
                var d = new TrackPositionArgReturningVoidDelegate(ChangeTrackPosition);
                Dispatcher.Invoke(d, new object[] { trackTime });
                return;
            }
            else
            {
                var currentTimeSpan = new TimeSpan(0, 0, (int) Math.Round(trackTime, 0));
                var totalTimeSpan = new TimeSpan(0, 0, CurrentTrack.Length);
                var timeLeft = new TimeSpan(0, 0, (int)Math.Round((totalTimeSpan - currentTimeSpan).TotalSeconds, 0));
                lblCurrentTime.Content = FormatTimeSpan(currentTimeSpan);// $" {currentTimeSpan.Minutes}:{currentTimeSpan.Seconds.ToString().PadLeft(2, '0')}";
                lblTimeLeft.Content = "-" + FormatTimeSpan(timeLeft);// $"-{timeLeft.Minutes}:{timeLeft.Seconds.ToString().PadLeft(2, '0')}";
                lblTotalTime.Content = FormatTimeSpan(totalTimeSpan);// $"{totalTimeSpan.Minutes}:{totalTimeSpan.Seconds.ToString().PadLeft(2, '0')}";


                trackPosition.Maximum = CurrentTrack.Length;
                trackPosition.Minimum = 0;
                trackPosition.Value = currentTimeSpan.TotalSeconds;
            }

        }

        string FormatTimeSpan(TimeSpan span)
        {
            return $"{span.Minutes}:{span.Seconds.ToString().PadLeft(2, '0')}";
        }

        delegate void LogArgReturningVoidDelegate(string action, string msg);
        public void Log(string action, string msg)
        {
            if (LoggerWindow != null)
            {
                if (!CheckAccess())
                {
                    LogArgReturningVoidDelegate d = new LogArgReturningVoidDelegate(Log);
                    Dispatcher.Invoke(d, new object[] {action, msg});
                }
                else
                {
                    LoggerWindow.Log.Text += $"{action} - {msg}{Environment.NewLine}";
                }
            }

        }

        delegate void StringArgReturningVoidDelegate(Track text);
        private void DisplayCurrentSong(SpotifyAPI.Local.Models.Track track)
        {
            if (track == null)
            {
                lblCurrentArtist.Content = "Error getting artist";
                lblCurrentTrack.Content = "Error getting track";
            }
            else
            {
                //var trackInfo = $"Track: {track.TrackResource.Name}";
                SetTrackInfo(track);
                SetArtistInfo(track);
                SetAlbumInfo(track);
                SetAlbumImage(track);

                //lblCurrentArtist.Text = $"Artist: {track.ArtistResource.Name}";
                //lblAlbum.Text = $"Album: {track.AlbumResource.Name}";
                //pictureBox1.Image = track.GetAlbumArt(AlbumArtSize.Size160);
            }
        }

        private void SetTrackInfo(Track track)
        {
            if (!CheckAccess())
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetTrackInfo);
                Dispatcher.Invoke(d, new object[] { track });
            }
            else
            {
                if (track.TrackResource != null)
                    lblCurrentTrack.Content = $"{track.TrackResource.Name}";
                else
                    lblCurrentTrack.Content = "Error getting track name";
            }
        }
        private void SetArtistInfo(Track track)
        {
            if (!CheckAccess())
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetArtistInfo);
                Dispatcher.Invoke(d, new object[] { track });
            }
            else
            {
                if (track.ArtistResource != null)
                    lblCurrentArtist.Content = $"{track.ArtistResource.Name}";
                else
                    lblCurrentArtist.Content = "Error getting track name";
            }
        }
        private void SetAlbumInfo(Track track)
        {
            if (!CheckAccess())
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetAlbumInfo);
                Dispatcher.Invoke(d, new object[] { track });
            }
            else
            {
                if (track.AlbumResource != null)
                    lblCurrentAlbum.Content = $"{track.AlbumResource.Name}";
                else
                    lblCurrentAlbum.Content = "Error getting album name";
            }
        }
        private void SetAlbumImage(Track track)
        {
            if (!CheckAccess())
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetAlbumImage);
                Dispatcher.Invoke(d, new object[] { track });
            }
            else
            {
                try
                {
                    pictureBox1.Source = Convert(track.GetAlbumArt(AlbumArtSize.Size640));
                    pictureBox1.Visibility = Visibility.Visible;
                    chkHideAlbumArt.IsChecked = false;
                }
                catch (Exception e)
                {
                    pictureBox1.Source = null;
                    Log("Set album image", "Error getting album image");
                    Logger.Error(e, "Error getting album image");
                }
            }
        }

        public BitmapSource Convert(System.Drawing.Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height,
                bitmap.HorizontalResolution, bitmap.VerticalResolution,
                PixelFormats.Bgr24, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }
        delegate void BoolArgReturningVoidDelegate(bool bol);
        private void SetPlayPause(bool playing)
        {
            if (!CheckAccess())
            {
                BoolArgReturningVoidDelegate d = new BoolArgReturningVoidDelegate(SetPlayPause);
                Dispatcher.Invoke(d, new object[] { playing });
            }
            else
            {
                SetPlayPauseButtons(playing);
            }
        }

        //public const int WM_NCLBUTTONDOWN = 0xA1;
        //public const int HT_CAPTION = 0x2;

        //[System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        //public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        //[System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        //public static extern bool ReleaseCapture();

        //private void Form1_MouseDown(object sender, MouseEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        ReleaseCapture();
        //        SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        //    }
        //}

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    Application.Exit();
        //    //⏸
        //}

        private void pbPrevious_Click(object sender, EventArgs e)
        {
            Spotify.Previous();
        }

        private void pbNext_Click(object sender, EventArgs e)
        {
            Spotify.Skip();
        }

        //private void chkTopMost_CheckedChanged(object sender, EventArgs e)
        //{
        //    this.TopMost = chkTopMost.Checked;
        //}

        //private void timer2_Tick(object sender, EventArgs e)
        //{
        //    timer2.Enabled = false;
        //    Startup();
        //}

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                App.Current.MainWindow.DragMove();
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.Close();
        }

        private void LblTotalTime_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            lblTotalTime.Visibility = Visibility.Collapsed;
            lblTimeLeft.Visibility = Visibility.Visible;
        }

        private void LblTimeLeft_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            lblTotalTime.Visibility = Visibility.Visible;
            lblTimeLeft.Visibility = Visibility.Collapsed;
        }
        private void Previous_OnClick(object sender, RoutedEventArgs e)
        {
            Spotify.Previous();
        }

        private async void PlayPause_OnClick(object sender, RoutedEventArgs e)
        {
            if (Spotify.GetStatus().Playing)
            {
                await Spotify.Pause();
            }
            else
            {
                await Spotify.Play();
            }
            SetPlayPauseButtons(Spotify.GetStatus().Playing);
        }

        private void SetPlayPauseButtons(bool playing)
        {
            if (playing)
            {
                btnPlay.Visibility = Visibility.Collapsed;
                btnPause.Visibility = Visibility.Visible;
            }
            else
            {
                btnPlay.Visibility = Visibility.Visible;
                btnPause.Visibility = Visibility.Collapsed;
            }
        }

        private void Next_OnClick(object sender, RoutedEventArgs e)
        {
            Spotify.Skip();
        }

        private void chkTopMost_Checked(object sender, RoutedEventArgs e)
        {
            this.Topmost = chkTopMost.IsChecked;
        }

        private void BtnShowLog_OnClick(object sender, RoutedEventArgs e)
        {
            LoggerWindow = new LogWindow();
            LoggerWindow.Left = this.Left + this.ActualWidth;
            LoggerWindow.Top = this.Top;
            LoggerWindow.Show();
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ChkHideAlbumArt_OnClick(object sender, RoutedEventArgs e)
        {
            pictureBox1.Visibility = chkHideAlbumArt.IsChecked ? Visibility.Hidden : Visibility.Visible;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Save();
        }

        public void Save()
        {
            AppSettings.Top = this.Top;
            AppSettings.Left = this.Left;
            AppSettings.Width = this.Width;
            AppSettings.Height = this.Height;
            AppSettings.TopMost = this.Topmost;

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(AppSettings);
            System.IO.File.WriteAllText($"{System.IO.Directory.GetCurrentDirectory()}\\Settings.json", json);
        }
    }
}
