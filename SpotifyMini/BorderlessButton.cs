using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpotifyViewer
{
    public partial class BorderlessButton : Button
    {
        protected override bool ShowFocusCues
        {
            get { return false; }
        }

        public BorderlessButton()
        {
            this.TabStop = false;
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255); //transparent
        }
    }
}
