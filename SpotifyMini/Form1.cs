using SpotifyAPI.Local;
using SpotifyAPI.Web;
using SpotifyAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FontAwesome.Sharp;
using NLog;
using SpotifyAPI.Local.Enums;
using SpotifyAPI.Local.Models;

namespace SpotifyViewer
{
    public partial class Form1 : Form
    {
        Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        SpotifyLocalAPI Spotify = new SpotifyLocalAPI();
        IconButton playButton = new IconButton();
        IconButton nextButton = new IconButton();
        IconButton previousButton = new IconButton();
        private Track CurrentTrack;
        public Form1()
        {
            InitializeComponent();
            AddPlayerControlButtons();
            
            trackPosition.BorderColor = Color.Transparent;
            CollapseForm();

            Startup();

            Spotify.OnPlayStateChange += (sender, args) =>
            {
                Logger.Debug("OnPlayStateChange, Playing:{0}", args.Playing);
                Log("PlayStateChange", $"Playing:{args.Playing}");
                SetPlayPause(args.Playing);
            };

            Spotify.OnTrackChange += (sender, args) =>
            {
                Logger.Debug("OnTrackChange, NewTrack:{0}", args.NewTrack.TrackResource.Name);
                var oldTrack = "None";
                if (args.OldTrack.TrackResource != null)
                    oldTrack = args.OldTrack.TrackResource.Name;
                Log("TrackChange", $"Old Track: {oldTrack} New Track: {args.NewTrack.TrackResource.Name}");
                CurrentTrack = Spotify.GetStatus().Track;
                DisplayCurrentSong(CurrentTrack);
            };

            Spotify.OnTrackTimeChange += (sender, args) =>
            {

                //Log("TrackTimeChange", $"TrackTime: {args.TrackTime}");
                
                ChangeTrackPosition(args.TrackTime);
            };

            Spotify.OnVolumeChange += (sender, args) =>
            {
                Log("TrackTimeChange", $"OldVolume: {args.OldVolume} NewVolume: {args.NewVolume}");
            };

            Spotify.ListenForEvents = true;
        }

        public void Startup()
        {
            SpotifyLocalAPI.RunSpotify();

            Spotify.Connect();
            Logger.Debug("Connected to spotify");

            Logger.Debug("Getting status");
            var status = Spotify.GetStatus();
            Logger.Debug("Done getting status");
            CurrentTrack = status.Track;
            DisplayCurrentSong(status.Track);
            SetPlayPause(status.Playing);

            ChangeTrackPosition(status.PlayingPosition);
        }


        delegate void TrackPositionArgReturningVoidDelegate(double trackTime);
        public void ChangeTrackPosition(double trackTime)
        {
            if (trackPosition.InvokeRequired)
            {
                var d = new TrackPositionArgReturningVoidDelegate(ChangeTrackPosition);
                this.trackPosition.Invoke(d, new object[] { trackTime });
            }
            else
            {
                trackPosition.Maximum = CurrentTrack.Length;
                trackPosition.Minimum = 0;
                trackPosition.Value = (int)Math.Round(trackTime, 0);
            }

        }

        delegate void LogArgReturningVoidDelegate(string action, string msg);
        public void Log(string action, string msg)
        {
            if (txtLog.InvokeRequired)
            {
                LogArgReturningVoidDelegate d = new LogArgReturningVoidDelegate(Log);
                this.txtLog.Invoke(d, new object[] { action, msg });
            }
            else
            {
                txtLog.Text += $"{action} - {msg}{Environment.NewLine}";
                txtLog.SelectionStart = txtLog.Text.Length - 1;
            }
            
        }

        private void SetupButton(IconButton button, IconChar icon, int x, int y, EventHandler eventHandler)
        {
            button.IconColor = Color.White;
            button.IconChar = icon;
            button.IconSize = 25;
            button.Size = new Size(25, 25);
            button.Location = new Point(x, y);
            button.Click += eventHandler;

            button.TabStop = false;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255); //transparent
            this.Controls.Add(button);
        }

        private void AddPlayerControlButtons()
        {
            var x = lblAlbum.Location.X;
            var y = lblAlbum.Location.Y + (lblAlbum.Location.Y - lblCurrentArtist.Location.Y);
            SetupButton(previousButton, IconChar.StepBackward, x, y, pbPrevious_Click);
            SetupButton(playButton, IconChar.Play, x + 25, y, pbPlayPause_Click);
            SetupButton(nextButton, IconChar.StepForward, x + 50, y, pbNext_Click);
        }

        delegate void StringArgReturningVoidDelegate(Track text);
        private void DisplayCurrentSong(SpotifyAPI.Local.Models.Track track)
        {
            if (track == null)
            {
                lblCurrentArtist.Text = "Error getting artist";
                lblCurrentTrack.Text = "Error getting track";
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
            if (lblCurrentTrack.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetTrackInfo);
                this.lblCurrentTrack.Invoke(d, new object[] { track });
            }
            else
            {
                if (track.TrackResource != null)
                    lblCurrentTrack.Text = $"{track.TrackResource.Name}";
                else
                    lblCurrentTrack.Text = "Error getting track name";
            }
        }
        private void SetArtistInfo(Track track)
        {
            if (lblCurrentArtist.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetArtistInfo);
                this.lblCurrentArtist.Invoke(d, new object[] { track });
            }
            else
            {
                if (track.ArtistResource != null)
                    lblCurrentArtist.Text = $"{track.ArtistResource.Name}";
                else
                    lblCurrentArtist.Text = "Error getting track name";
            }
        }
        private void SetAlbumInfo(Track track)
        {
            if (lblAlbum.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetAlbumInfo);
                this.lblAlbum.Invoke(d, new object[] { track });
            }
            else
            {
                if (track.AlbumResource != null)
                    lblAlbum.Text = $"{track.AlbumResource.Name}";
                else
                    lblAlbum.Text = "Error getting album name";
            }
        }
        private void SetAlbumImage(Track track)
        {
            if (pictureBox1.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetAlbumImage);
                this.pictureBox1.Invoke(d, new object[] { track });
            }
            else
            {
                try
                {
                    pictureBox1.Image = track.GetAlbumArt(AlbumArtSize.Size160);
                }
                catch (Exception e)
                {
                    pictureBox1.Image = null;
                    Log("Set album image", "Error getting album image");
                    Logger.Error(e, "Error getting album image");
                }
            }
        }
        delegate void BoolArgReturningVoidDelegate(bool bol);
        private void SetPlayPause(bool playing)
        {
            if (playButton.InvokeRequired)
            {
                BoolArgReturningVoidDelegate d = new BoolArgReturningVoidDelegate(SetPlayPause);
                this.playButton.Invoke(d, new object[] { playing });
            }
            else
            {
                playButton.IconChar = playing ? IconChar.Pause : IconChar.Play;
            }
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
            //⏸
        }

        private async void pbPlayPause_Click(object sender, EventArgs e)
        {
            if (Spotify.GetStatus().Playing)
            {
                await Spotify.Pause();
                playButton.IconChar = IconChar.Play;
            }
            else
            {
                await Spotify.Play();
                playButton.IconChar = IconChar.Pause;
            }
        }

        private void pbPrevious_Click(object sender, EventArgs e)
        {
            Spotify.Previous();
        }

        private void pbNext_Click(object sender, EventArgs e)
        {
            Spotify.Skip();
        }

        private void btnExpand_Click(object sender, EventArgs e)
        {
            if (IsFormExpanded())
                CollapseForm();
            else
                ExpandForm();
        }

        private void ExpandForm()
        {
            btnExpand.Text = "▲";
            this.Height = 775;
            toolTip1.SetToolTip(btnExpand, "Collapse");
        }
        private void CollapseForm()
        {
            btnExpand.Text = "▼";
            this.Height = 250;
            toolTip1.SetToolTip(btnExpand, "Expand");
        }

        private bool IsFormExpanded()
        {
            return (this.Height > 300);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void chkTopMost_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = chkTopMost.Checked;
        }

        //▲
        //▼
    }
}
