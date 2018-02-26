namespace SpotifyViewer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.lblCurrentTrack = new System.Windows.Forms.Label();
			this.lblCurrentArtist = new System.Windows.Forms.Label();
			this.lblAlbum = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.txtLog = new System.Windows.Forms.TextBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.chkTopMost = new System.Windows.Forms.CheckBox();
			this.timer2 = new System.Windows.Forms.Timer(this.components);
			this.trackPosition = new SpotifyViewer.SmoothProgressBar();
			this.btnExpand = new SpotifyViewer.BorderlessButton();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// lblCurrentTrack
			// 
			this.lblCurrentTrack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblCurrentTrack.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCurrentTrack.ForeColor = System.Drawing.Color.DarkGray;
			this.lblCurrentTrack.Location = new System.Drawing.Point(193, 22);
			this.lblCurrentTrack.Name = "lblCurrentTrack";
			this.lblCurrentTrack.Size = new System.Drawing.Size(418, 20);
			this.lblCurrentTrack.TabIndex = 0;
			this.lblCurrentTrack.Text = "Track";
			// 
			// lblCurrentArtist
			// 
			this.lblCurrentArtist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblCurrentArtist.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCurrentArtist.ForeColor = System.Drawing.Color.DarkGray;
			this.lblCurrentArtist.Location = new System.Drawing.Point(193, 59);
			this.lblCurrentArtist.Name = "lblCurrentArtist";
			this.lblCurrentArtist.Size = new System.Drawing.Size(418, 20);
			this.lblCurrentArtist.TabIndex = 1;
			this.lblCurrentArtist.Text = "Artist";
			// 
			// lblAlbum
			// 
			this.lblAlbum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblAlbum.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblAlbum.ForeColor = System.Drawing.Color.DarkGray;
			this.lblAlbum.Location = new System.Drawing.Point(193, 96);
			this.lblAlbum.Name = "lblAlbum";
			this.lblAlbum.Size = new System.Drawing.Size(418, 20);
			this.lblAlbum.TabIndex = 3;
			this.lblAlbum.Text = "Album";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(12, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(160, 160);
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			// 
			// txtLog
			// 
			this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtLog.BackColor = System.Drawing.SystemColors.ControlDark;
			this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtLog.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.txtLog.Location = new System.Drawing.Point(12, 257);
			this.txtLog.Multiline = true;
			this.txtLog.Name = "txtLog";
			this.txtLog.ReadOnly = true;
			this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtLog.Size = new System.Drawing.Size(599, 451);
			this.txtLog.TabIndex = 4;
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// chkTopMost
			// 
			this.chkTopMost.AutoSize = true;
			this.chkTopMost.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkTopMost.ForeColor = System.Drawing.Color.DarkGray;
			this.chkTopMost.Location = new System.Drawing.Point(12, 228);
			this.chkTopMost.Name = "chkTopMost";
			this.chkTopMost.Size = new System.Drawing.Size(114, 21);
			this.chkTopMost.TabIndex = 9;
			this.chkTopMost.Text = "Always on top";
			this.chkTopMost.UseVisualStyleBackColor = true;
			this.chkTopMost.CheckedChanged += new System.EventHandler(this.chkTopMost_CheckedChanged);
			// 
			// timer2
			// 
			this.timer2.Interval = 10000;
			this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
			// 
			// trackPosition
			// 
			this.trackPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.trackPosition.BorderColor = System.Drawing.Color.Black;
			this.trackPosition.Location = new System.Drawing.Point(197, 171);
			this.trackPosition.Maximum = 100;
			this.trackPosition.Minimum = 0;
			this.trackPosition.Name = "trackPosition";
			this.trackPosition.ProgressBarColor = System.Drawing.Color.Gray;
			this.trackPosition.Size = new System.Drawing.Size(414, 10);
			this.trackPosition.TabIndex = 8;
			this.trackPosition.Text = "smoothProgressBar1";
			this.trackPosition.Value = 0;
			// 
			// btnExpand
			// 
			this.btnExpand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.btnExpand.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnExpand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnExpand.Location = new System.Drawing.Point(0, 714);
			this.btnExpand.Name = "btnExpand";
			this.btnExpand.Size = new System.Drawing.Size(623, 23);
			this.btnExpand.TabIndex = 5;
			this.btnExpand.TabStop = false;
			this.btnExpand.Text = "▼";
			this.btnExpand.UseVisualStyleBackColor = false;
			this.btnExpand.Click += new System.EventHandler(this.btnExpand_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.ClientSize = new System.Drawing.Size(623, 737);
			this.Controls.Add(this.chkTopMost);
			this.Controls.Add(this.trackPosition);
			this.Controls.Add(this.btnExpand);
			this.Controls.Add(this.txtLog);
			this.Controls.Add(this.lblAlbum);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.lblCurrentArtist);
			this.Controls.Add(this.lblCurrentTrack);
			this.ForeColor = System.Drawing.Color.White;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximumSize = new System.Drawing.Size(1000, 1000);
			this.Name = "Form1";
			this.Text = "Spotify Mini";
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCurrentTrack;
        private System.Windows.Forms.Label lblCurrentArtist;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblAlbum;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.ToolTip toolTip1;
        private SmoothProgressBar trackPosition;
        private System.Windows.Forms.Timer timer1;
        private BorderlessButton btnExpand;
        private System.Windows.Forms.CheckBox chkTopMost;
		private System.Windows.Forms.Timer timer2;
	}
}

