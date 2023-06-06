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
            toolStripStatusLabelaccount = new ToolStripStatusLabel();
            toolStripSplitButtonlogout = new ToolStripSplitButton();
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
            panelNav = new Panel();
            buttonReceive = new Button();
            buttonSend = new Button();
            buttonPortfolio = new Button();
            panelPortfolio = new Panel();
            groupBoxTransactions = new GroupBox();
            groupBoxinfo = new GroupBox();
            labelPendingCount = new Label();
            labelSyncPercent = new Label();
            labelHeight = new Label();
            groupBoxbalance = new GroupBox();
            labelBalanceTotal = new Label();
            labelLocked = new Label();
            labelBalance = new Label();
            panelSend = new Panel();
            statusStripmain.SuspendLayout();
            panelaccount.SuspendLayout();
            groupBoxaccount.SuspendLayout();
            panelNav.SuspendLayout();
            panelPortfolio.SuspendLayout();
            groupBoxinfo.SuspendLayout();
            groupBoxbalance.SuspendLayout();
            SuspendLayout();
            // 
            // statusStripmain
            // 
            statusStripmain.Dock = DockStyle.Top;
            statusStripmain.Items.AddRange(new ToolStripItem[] { toolStripStatusLabelNetwork, toolStripStatusLabelstatus, toolStripStatusLabelaccount, toolStripSplitButtonlogout });
            statusStripmain.Location = new Point(0, 0);
            statusStripmain.Name = "statusStripmain";
            statusStripmain.Size = new Size(1152, 22);
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
            // toolStripStatusLabelaccount
            // 
            toolStripStatusLabelaccount.Margin = new Padding(0, 3, 50, 2);
            toolStripStatusLabelaccount.Name = "toolStripStatusLabelaccount";
            toolStripStatusLabelaccount.Size = new Size(55, 17);
            toolStripStatusLabelaccount.Text = "Account:";
            // 
            // toolStripSplitButtonlogout
            // 
            toolStripSplitButtonlogout.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripSplitButtonlogout.DropDownButtonWidth = 0;
            toolStripSplitButtonlogout.ImageTransparentColor = Color.Magenta;
            toolStripSplitButtonlogout.Name = "toolStripSplitButtonlogout";
            toolStripSplitButtonlogout.Size = new Size(53, 20);
            toolStripSplitButtonlogout.Text = "Log out";
            toolStripSplitButtonlogout.Visible = false;
            toolStripSplitButtonlogout.ButtonClick += Logout;
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
            panelaccount.Location = new Point(1082, 211);
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
            // comboBoxaccount
            // 
            comboBoxaccount.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxaccount.FlatStyle = FlatStyle.Popup;
            comboBoxaccount.FormattingEnabled = true;
            comboBoxaccount.Location = new Point(97, 67);
            comboBoxaccount.Name = "comboBoxaccount";
            comboBoxaccount.Size = new Size(190, 23);
            comboBoxaccount.TabIndex = 6;
            comboBoxaccount.DropDownClosed += AccountChoose;
            // 
            // textBoxaccountpassword
            // 
            textBoxaccountpassword.Location = new Point(97, 101);
            textBoxaccountpassword.Name = "textBoxaccountpassword";
            textBoxaccountpassword.Size = new Size(190, 23);
            textBoxaccountpassword.TabIndex = 5;
            textBoxaccountpassword.UseSystemPasswordChar = true;
            textBoxaccountpassword.KeyDown += PasswordEntered;
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
            // panelNav
            // 
            panelNav.Controls.Add(buttonReceive);
            panelNav.Controls.Add(buttonSend);
            panelNav.Controls.Add(buttonPortfolio);
            panelNav.Dock = DockStyle.Left;
            panelNav.Location = new Point(0, 22);
            panelNav.Name = "panelNav";
            panelNav.Size = new Size(114, 553);
            panelNav.TabIndex = 6;
            // 
            // buttonReceive
            // 
            buttonReceive.FlatStyle = FlatStyle.Flat;
            buttonReceive.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonReceive.Location = new Point(12, 134);
            buttonReceive.Name = "buttonReceive";
            buttonReceive.Size = new Size(89, 30);
            buttonReceive.TabIndex = 1;
            buttonReceive.Text = "Receive";
            buttonReceive.UseVisualStyleBackColor = true;
            // 
            // buttonSend
            // 
            buttonSend.FlatStyle = FlatStyle.Flat;
            buttonSend.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonSend.Location = new Point(12, 80);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(89, 30);
            buttonSend.TabIndex = 1;
            buttonSend.Text = "Send";
            buttonSend.UseVisualStyleBackColor = true;
            buttonSend.Click += buttonSend_Click;
            // 
            // buttonPortfolio
            // 
            buttonPortfolio.FlatStyle = FlatStyle.Flat;
            buttonPortfolio.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonPortfolio.Location = new Point(12, 25);
            buttonPortfolio.Name = "buttonPortfolio";
            buttonPortfolio.Size = new Size(89, 30);
            buttonPortfolio.TabIndex = 0;
            buttonPortfolio.Text = "Portfolio";
            buttonPortfolio.UseVisualStyleBackColor = true;
            buttonPortfolio.Click += buttonPortfolio_Click;
            // 
            // panelPortfolio
            // 
            panelPortfolio.Controls.Add(groupBoxTransactions);
            panelPortfolio.Controls.Add(groupBoxinfo);
            panelPortfolio.Controls.Add(groupBoxbalance);
            panelPortfolio.Location = new Point(448, 170);
            panelPortfolio.Name = "panelPortfolio";
            panelPortfolio.Size = new Size(956, 538);
            panelPortfolio.TabIndex = 7;
            panelPortfolio.Visible = false;
            // 
            // groupBoxTransactions
            // 
            groupBoxTransactions.Dock = DockStyle.Bottom;
            groupBoxTransactions.Location = new Point(0, 113);
            groupBoxTransactions.Name = "groupBoxTransactions";
            groupBoxTransactions.Size = new Size(956, 425);
            groupBoxTransactions.TabIndex = 8;
            groupBoxTransactions.TabStop = false;
            groupBoxTransactions.Text = "Transactions";
            // 
            // groupBoxinfo
            // 
            groupBoxinfo.Controls.Add(labelPendingCount);
            groupBoxinfo.Controls.Add(labelSyncPercent);
            groupBoxinfo.Controls.Add(labelHeight);
            groupBoxinfo.Location = new Point(281, 3);
            groupBoxinfo.Name = "groupBoxinfo";
            groupBoxinfo.Size = new Size(232, 104);
            groupBoxinfo.TabIndex = 8;
            groupBoxinfo.TabStop = false;
            groupBoxinfo.Text = "Info";
            // 
            // labelPendingCount
            // 
            labelPendingCount.AutoSize = true;
            labelPendingCount.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelPendingCount.Location = new Point(3, 56);
            labelPendingCount.Name = "labelPendingCount";
            labelPendingCount.Size = new Size(116, 21);
            labelPendingCount.TabIndex = 2;
            labelPendingCount.Text = "labelPendingTX";
            // 
            // labelSyncPercent
            // 
            labelSyncPercent.AutoSize = true;
            labelSyncPercent.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelSyncPercent.Location = new Point(133, 14);
            labelSyncPercent.Name = "labelSyncPercent";
            labelSyncPercent.Size = new Size(89, 21);
            labelSyncPercent.TabIndex = 1;
            labelSyncPercent.Text = "labelSync%";
            // 
            // labelHeight
            // 
            labelHeight.AutoSize = true;
            labelHeight.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelHeight.Location = new Point(3, 14);
            labelHeight.Name = "labelHeight";
            labelHeight.Size = new Size(89, 21);
            labelHeight.TabIndex = 0;
            labelHeight.Text = "labelHeight";
            // 
            // groupBoxbalance
            // 
            groupBoxbalance.Controls.Add(labelBalanceTotal);
            groupBoxbalance.Controls.Add(labelLocked);
            groupBoxbalance.Controls.Add(labelBalance);
            groupBoxbalance.Location = new Point(12, 3);
            groupBoxbalance.Name = "groupBoxbalance";
            groupBoxbalance.Size = new Size(263, 104);
            groupBoxbalance.TabIndex = 2;
            groupBoxbalance.TabStop = false;
            groupBoxbalance.Text = "Balance";
            // 
            // labelBalanceTotal
            // 
            labelBalanceTotal.AutoSize = true;
            labelBalanceTotal.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelBalanceTotal.Location = new Point(21, 60);
            labelBalanceTotal.Name = "labelBalanceTotal";
            labelBalanceTotal.Size = new Size(75, 21);
            labelBalanceTotal.TabIndex = 2;
            labelBalanceTotal.Text = "labelTotal";
            // 
            // labelLocked
            // 
            labelLocked.AutoSize = true;
            labelLocked.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelLocked.Location = new Point(21, 39);
            labelLocked.Name = "labelLocked";
            labelLocked.Size = new Size(92, 21);
            labelLocked.TabIndex = 1;
            labelLocked.Text = "labelLocked";
            // 
            // labelBalance
            // 
            labelBalance.AutoSize = true;
            labelBalance.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelBalance.Location = new Point(21, 18);
            labelBalance.Name = "labelBalance";
            labelBalance.Size = new Size(96, 21);
            labelBalance.TabIndex = 0;
            labelBalance.Text = "labelBalance";
            // 
            // panelSend
            // 
            panelSend.Location = new Point(120, 25);
            panelSend.Name = "panelSend";
            panelSend.Size = new Size(200, 100);
            panelSend.TabIndex = 2;
            panelSend.Visible = false;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1152, 575);
            Controls.Add(panelSend);
            Controls.Add(panelPortfolio);
            Controls.Add(panelNav);
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
            panelNav.ResumeLayout(false);
            panelPortfolio.ResumeLayout(false);
            groupBoxinfo.ResumeLayout(false);
            groupBoxinfo.PerformLayout();
            groupBoxbalance.ResumeLayout(false);
            groupBoxbalance.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip statusStripmain;
        private ToolStripStatusLabel toolStripStatusLabelNetwork;
        private ToolStripStatusLabel toolStripStatusLabelaccount;
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
        private ToolStripStatusLabel toolStripStatusLabelstatus;
        private ToolStripSplitButton toolStripSplitButtonlogout;
        private Panel panelNav;
        private Button buttonPortfolio;
        private Button buttonSend;
        private Button buttonReceive;
        private Panel panelPortfolio;
        private Label labelLocked;
        private Label labelBalance;
        private GroupBox groupBoxbalance;
        private Label labelBalanceTotal;
        private GroupBox groupBoxinfo;
        private Label labelHeight;
        private Label labelPendingCount;
        private Label labelSyncPercent;
        private GroupBox groupBoxTransactions;
        private Panel panelSend;
    }
}