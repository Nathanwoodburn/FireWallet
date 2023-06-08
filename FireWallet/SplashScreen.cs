using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FireWallet
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
            close = false;
        }
        bool close;
        private void timerSplashDelay_Tick(object sender, EventArgs e)
        {
            close = true;
            this.Close();
        }

        private void SplashScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!close)
            {
                e.Cancel = true;
            }
        }
    }
}
