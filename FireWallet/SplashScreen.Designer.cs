namespace FireWallet
{
    partial class SplashScreen
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashScreen));
            label1 = new Label();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            panelNew = new Panel();
            pictureBoxNew = new PictureBox();
            timerIn = new System.Windows.Forms.Timer(components);
            timerOut = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panelNew.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxNew).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(80, 9);
            label1.Name = "label1";
            label1.Size = new Size(284, 37);
            label1.TabIndex = 0;
            label1.Text = "Welcome to FireWallet";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 58);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(420, 330);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(154, 400);
            label2.Name = "label2";
            label2.Size = new Size(136, 20);
            label2.TabIndex = 2;
            label2.Text = "Nathan.Woodburn/";
            label2.Click += label2_Click;
            // 
            // panelNew
            // 
            panelNew.Controls.Add(pictureBoxNew);
            panelNew.Dock = DockStyle.Fill;
            panelNew.Location = new Point(0, 0);
            panelNew.Name = "panelNew";
            panelNew.Size = new Size(450, 450);
            panelNew.TabIndex = 3;
            // 
            // pictureBoxNew
            // 
            pictureBoxNew.BackColor = Color.Black;
            pictureBoxNew.Dock = DockStyle.Fill;
            pictureBoxNew.Image = Properties.Resources.FWSplash;
            pictureBoxNew.InitialImage = null;
            pictureBoxNew.Location = new Point(0, 0);
            pictureBoxNew.Name = "pictureBoxNew";
            pictureBoxNew.Size = new Size(450, 450);
            pictureBoxNew.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxNew.TabIndex = 0;
            pictureBoxNew.TabStop = false;
            pictureBoxNew.Visible = false;
            // 
            // timerIn
            // 
            timerIn.Enabled = true;
            timerIn.Tick += timerIn_Tick;
            // 
            // timerOut
            // 
            timerOut.Tick += timerOut_Tick;
            // 
            // SplashScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(450, 450);
            Controls.Add(panelNew);
            Controls.Add(label2);
            Controls.Add(pictureBox1);
            Controls.Add(label1);
            ForeColor = Color.FromArgb(142, 5, 194);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "SplashScreen";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FireWallet";
            TopMost = true;
            FormClosing += SplashScreen_FormClosing;
            Load += SplashScreen_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panelNew.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxNew).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private PictureBox pictureBox1;
        private Label label2;
        private Panel panelNew;
        private PictureBox pictureBoxNew;
        private System.Windows.Forms.Timer timerIn;
        private System.Windows.Forms.Timer timerOut;
    }
}