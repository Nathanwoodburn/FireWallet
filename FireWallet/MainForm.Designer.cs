namespace FireWallet
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            statusStripmain = new StatusStrip();
            toolStripStatusLabelNetwork = new ToolStripStatusLabel();
            toolStripStatusLabelstatus = new ToolStripStatusLabel();
            timerNodeStatus = new System.Windows.Forms.Timer(components);
            panelaccount = new Panel();
            groupBoxaccount = new GroupBox();
            label1 = new Label();
            comboBoxaccount = new ComboBox();
            textBoxaccountpassword = new TextBox();
            buttonaccountlogin = new Button();
            labelaccountpassword = new Label();
            labelaccountusername = new Label();
            buttonaccountnew = new Button();
            statusStripmain.SuspendLayout();
            panelaccount.SuspendLayout();
            groupBoxaccount.SuspendLayout();
            SuspendLayout();
            // 
            // statusStripmain
            // 
            statusStripmain.Dock = DockStyle.Top;
            statusStripmain.Items.AddRange(new ToolStripItem[] { toolStripStatusLabelNetwork, toolStripStatusLabelstatus });
            statusStripmain.Location = new Point(0, 0);
            statusStripmain.Name = "statusStripmain";
            statusStripmain.Size = new Size(1074, 22);
            statusStripmain.SizingGrip = false;
            statusStripmain.TabIndex = 0;
            statusStripmain.Text = "statusStrip1";
            // 
            // toolStripStatusLabelNetwork
            // 
            toolStripStatusLabelNetwork.Margin = new Padding(0, 3, 50, 2);
            toolStripStatusLabelNetwork.Name = "toolStripStatusLabelNetwork";
            toolStripStatusLabelNetwork.Size = new Size(58, 17);
            toolStripStatusLabelNetwork.Text = "Network: ";
            // 
            // toolStripStatusLabelstatus
            // 
            toolStripStatusLabelstatus.Margin = new Padding(0, 3, 50, 2);
            toolStripStatusLabelstatus.Name = "toolStripStatusLabelstatus";
            toolStripStatusLabelstatus.Size = new Size(126, 17);
            toolStripStatusLabelstatus.Text = "Status: Not Connected";
            // 
            // timerNodeStatus
            // 
            timerNodeStatus.Enabled = true;
            timerNodeStatus.Interval = 10000;
            timerNodeStatus.Tick += timerNodeStatus_Tick;
            // 
            // panelaccount
            // 
            panelaccount.BackColor = Color.Transparent;
            panelaccount.Controls.Add(groupBoxaccount);
            panelaccount.Dock = DockStyle.Fill;
            panelaccount.Location = new Point(0, 22);
            panelaccount.Name = "panelaccount";
            panelaccount.Size = new Size(1074, 642);
            panelaccount.TabIndex = 1;
            // 
            // groupBoxaccount
            // 
            groupBoxaccount.Controls.Add(label1);
            groupBoxaccount.Controls.Add(comboBoxaccount);
            groupBoxaccount.Controls.Add(textBoxaccountpassword);
            groupBoxaccount.Controls.Add(buttonaccountlogin);
            groupBoxaccount.Controls.Add(labelaccountpassword);
            groupBoxaccount.Controls.Add(labelaccountusername);
            groupBoxaccount.Controls.Add(buttonaccountnew);
            groupBoxaccount.FlatStyle = FlatStyle.Popup;
            groupBoxaccount.Location = new Point(458, 155);
            groupBoxaccount.Name = "groupBoxaccount";
            groupBoxaccount.Size = new Size(308, 241);
            groupBoxaccount.TabIndex = 5;
            groupBoxaccount.TabStop = false;
            groupBoxaccount.Text = "Login";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(6, 19);
            label1.Name = "label1";
            label1.Size = new Size(281, 30);
            label1.TabIndex = 7;
            label1.Text = "Please Login to your account";
            // 
            // comboBoxusername
            // 
            comboBoxaccount.FlatStyle = FlatStyle.Popup;
            comboBoxaccount.FormattingEnabled = true;
            comboBoxaccount.Location = new Point(97, 67);
            comboBoxaccount.Name = "comboBoxusername";
            comboBoxaccount.Size = new Size(190, 23);
            comboBoxaccount.TabIndex = 6;
            // 
            // textBoxaccountpassword
            // 
            textBoxaccountpassword.Location = new Point(97, 101);
            textBoxaccountpassword.Name = "textBoxaccountpassword";
            textBoxaccountpassword.Size = new Size(190, 23);
            textBoxaccountpassword.TabIndex = 5;
            textBoxaccountpassword.UseSystemPasswordChar = true;
            // 
            // buttonaccountlogin
            // 
            buttonaccountlogin.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonaccountlogin.Location = new Point(188, 164);
            buttonaccountlogin.Name = "buttonaccountlogin";
            buttonaccountlogin.Size = new Size(99, 41);
            buttonaccountlogin.TabIndex = 3;
            buttonaccountlogin.Text = "Login";
            buttonaccountlogin.UseVisualStyleBackColor = true;
            buttonaccountlogin.Click += LoginClick;
            // 
            // labelaccountpassword
            // 
            labelaccountpassword.AutoSize = true;
            labelaccountpassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelaccountpassword.Location = new Point(12, 103);
            labelaccountpassword.Name = "labelaccountpassword";
            labelaccountpassword.Size = new Size(79, 21);
            labelaccountpassword.TabIndex = 4;
            labelaccountpassword.Text = "Password:";
            // 
            // labelaccountusername
            // 
            labelaccountusername.AutoSize = true;
            labelaccountusername.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelaccountusername.Location = new Point(12, 67);
            labelaccountusername.Name = "labelaccountusername";
            labelaccountusername.Size = new Size(69, 21);
            labelaccountusername.TabIndex = 1;
            labelaccountusername.Text = "Account:";
            // 
            // buttonaccountnew
            // 
            buttonaccountnew.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonaccountnew.Location = new Point(12, 164);
            buttonaccountnew.Name = "buttonaccountnew";
            buttonaccountnew.Size = new Size(99, 41);
            buttonaccountnew.TabIndex = 2;
            buttonaccountnew.Text = "New";
            buttonaccountnew.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1074, 664);
            Controls.Add(panelaccount);
            Controls.Add(statusStripmain);
            Name = "MainForm";
            Text = "FireWallet";
            FormClosing += MainForm_Closing;
            Load += MainForm_Load;
            Resize += Form1_Resize;
            statusStripmain.ResumeLayout(false);
            statusStripmain.PerformLayout();
            panelaccount.ResumeLayout(false);
            groupBoxaccount.ResumeLayout(false);
            groupBoxaccount.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip statusStripmain;
        private ToolStripStatusLabel toolStripStatusLabelNetwork;
        private ToolStripStatusLabel toolStripStatusLabelstatus;
        private System.Windows.Forms.Timer timerNodeStatus;
        private Panel panelaccount;
        private Button buttonaccountlogin;
        private Button buttonaccountnew;
        private Label labelaccountusername;
        private Label labelaccountpassword;
        private GroupBox groupBoxaccount;
        private Label label1;
        private ComboBox comboBoxaccount;
        private TextBox textBoxaccountpassword;
    }
}