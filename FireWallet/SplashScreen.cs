using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FireWallet
{
    public partial class SplashScreen : Form
    {
        public SplashScreen(bool timer)
        {
            InitializeComponent();
            close = false;
        }
        bool close;
        private void SplashScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!close)
            {
                e.Cancel = true;
            }
        }
        public void CloseSplash()
        {
            close = true;
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "https://nathan.woodburn.au",
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}
