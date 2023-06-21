using System.Diagnostics;
using System.Drawing.Imaging;
//using Microsoft.DirectX.AudioVideoPlayback;


namespace FireWallet
{
    public partial class SplashScreen : Form
    {
        public SplashScreen(bool timer)
        {
            InitializeComponent();
            close = false;
            IsClosed = false;
        }
        bool close;
        private void SplashScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!close)
            {
                e.Cancel = true;
            }
        }
        public bool IsClosed { get; set; }
        public void CloseSplash()
        {
            close = true;

            // Fade out
            timerIn.Stop();
            timerOut.Start();
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
        Bitmap splash = new Bitmap(Properties.Resources.FWSplash);
        private void SplashScreen_Load(object sender, EventArgs e)
        {
            pictureBoxNew.Visible = true;
            this.TransparencyKey = Color.FromArgb(0, 0, 0);
            pictureBoxNew.Invalidate();
        }
        public Image SetImageOpacity(Image image, float opacity)
        {
            try
            {
                //create a Bitmap the size of the image provided  
                Bitmap bmp = new Bitmap(image.Width, image.Height);

                //create a graphics object from the image  
                using (Graphics gfx = Graphics.FromImage(bmp))
                {

                    //create a color matrix object  
                    ColorMatrix matrix = new ColorMatrix();

                    //set the opacity  
                    matrix.Matrix33 = opacity;

                    //create image attributes  
                    ImageAttributes attributes = new ImageAttributes();

                    //set the color(opacity) of the image  
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    //now draw the image  
                    gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                }
                return bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        float opacity = 0.0f;

        private void timerIn_Tick(object sender, EventArgs e)
        {
            if (opacity >= 1)
            {
                timerIn.Stop();
                return;
            }
            opacity += 0.05f;
            pictureBoxNew.Image = SetImageOpacity(splash, opacity);
            pictureBoxNew.Invalidate();
        }

        private void timerOut_Tick(object sender, EventArgs e)
        {
            if (opacity <= 0)
            {
                timerOut.Stop();
                IsClosed = true;
                this.Close();
                return;
            }
            opacity -= 0.05f;
            pictureBoxNew.Image = SetImageOpacity(splash, opacity);
            pictureBoxNew.Invalidate();
        }
    }
}
