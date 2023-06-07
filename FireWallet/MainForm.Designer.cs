using Color = System.Drawing.Color;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using SizeF = System.Drawing.SizeF;

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
            labelloginprompt = new Label();
            comboBoxaccount = new ComboBox();
            textBoxaccountpassword = new TextBox();
            buttonaccountlogin = new Button();
            labelaccountpassword = new Label();
            labelaccountusername = new Label();
            buttonaccountnew = new Button();
            panelNav = new Panel();
            buttonNavReceive = new Button();
            buttonNavSend = new Button();
            buttonNavPortfolio = new Button();
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
            checkBoxSendSubFee = new CheckBox();
            buttonSendMax = new Button();
            buttonSendHNS = new Button();
            labelSendingError = new Label();
            labelSendingFee = new Label();
            textBoxSendingAmount = new TextBox();
            textBoxSendingTo = new TextBox();
            labelSendingMax = new Label();
            labelSendingAmount = new Label();
            labelSendingTo = new Label();
            labelSendPrompt = new Label();
            panelRecieve = new Panel();
            pictureBoxReceiveQR = new PictureBox();
            labelReceive2 = new Label();
            textBoxReceiveAddress = new TextBox();
            labelReceive1 = new Label();
            statusStripmain.SuspendLayout();
            panelaccount.SuspendLayout();
            groupBoxaccount.SuspendLayout();
            panelNav.SuspendLayout();
            panelPortfolio.SuspendLayout();
            groupBoxinfo.SuspendLayout();
            groupBoxbalance.SuspendLayout();
            panelSend.SuspendLayout();
            panelRecieve.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxReceiveQR).BeginInit();
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
            groupBoxaccount.Controls.Add(labelloginprompt);
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
            // labelloginprompt
            // 
            labelloginprompt.AutoSize = true;
            labelloginprompt.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            labelloginprompt.Location = new Point(6, 19);
            labelloginprompt.Name = "labelloginprompt";
            labelloginprompt.Size = new Size(281, 30);
            labelloginprompt.TabIndex = 7;
            labelloginprompt.Text = "Please Login to your account";
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
            panelNav.Controls.Add(buttonNavReceive);
            panelNav.Controls.Add(buttonNavSend);
            panelNav.Controls.Add(buttonNavPortfolio);
            panelNav.Dock = DockStyle.Left;
            panelNav.Location = new Point(0, 22);
            panelNav.Name = "panelNav";
            panelNav.Size = new Size(114, 553);
            panelNav.TabIndex = 6;
            // 
            // buttonNavReceive
            // 
            buttonNavReceive.FlatStyle = FlatStyle.Flat;
            buttonNavReceive.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonNavReceive.Location = new Point(12, 134);
            buttonNavReceive.Name = "buttonNavReceive";
            buttonNavReceive.Size = new Size(89, 30);
            buttonNavReceive.TabIndex = 1;
            buttonNavReceive.TabStop = false;
            buttonNavReceive.Text = "Receive";
            buttonNavReceive.UseVisualStyleBackColor = true;
            buttonNavReceive.Click += ReceivePanel_Click;
            // 
            // buttonNavSend
            // 
            buttonNavSend.FlatStyle = FlatStyle.Flat;
            buttonNavSend.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonNavSend.Location = new Point(12, 80);
            buttonNavSend.Name = "buttonNavSend";
            buttonNavSend.Size = new Size(89, 30);
            buttonNavSend.TabIndex = 1;
            buttonNavSend.TabStop = false;
            buttonNavSend.Text = "Send";
            buttonNavSend.UseVisualStyleBackColor = true;
            buttonNavSend.Click += SendPanel_Click;
            // 
            // buttonNavPortfolio
            // 
            buttonNavPortfolio.FlatStyle = FlatStyle.Flat;
            buttonNavPortfolio.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonNavPortfolio.Location = new Point(12, 25);
            buttonNavPortfolio.Name = "buttonNavPortfolio";
            buttonNavPortfolio.Size = new Size(89, 30);
            buttonNavPortfolio.TabIndex = 0;
            buttonNavPortfolio.TabStop = false;
            buttonNavPortfolio.Text = "Portfolio";
            buttonNavPortfolio.UseVisualStyleBackColor = true;
            buttonNavPortfolio.Click += PortfolioPanel_Click;
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
            panelSend.Controls.Add(checkBoxSendSubFee);
            panelSend.Controls.Add(buttonSendMax);
            panelSend.Controls.Add(buttonSendHNS);
            panelSend.Controls.Add(labelSendingError);
            panelSend.Controls.Add(labelSendingFee);
            panelSend.Controls.Add(textBoxSendingAmount);
            panelSend.Controls.Add(textBoxSendingTo);
            panelSend.Controls.Add(labelSendingMax);
            panelSend.Controls.Add(labelSendingAmount);
            panelSend.Controls.Add(labelSendingTo);
            panelSend.Controls.Add(labelSendPrompt);
            panelSend.Location = new Point(448, 170);
            panelSend.Name = "panelSend";
            panelSend.Size = new Size(974, 521);
            panelSend.TabIndex = 2;
            panelSend.Visible = false;
            // 
            // checkBoxSendSubFee
            // 
            checkBoxSendSubFee.AutoSize = true;
            checkBoxSendSubFee.CheckAlign = ContentAlignment.MiddleRight;
            checkBoxSendSubFee.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            checkBoxSendSubFee.Location = new Point(254, 258);
            checkBoxSendSubFee.Name = "checkBoxSendSubFee";
            checkBoxSendSubFee.Size = new Size(206, 25);
            checkBoxSendSubFee.TabIndex = 16;
            checkBoxSendSubFee.TabStop = false;
            checkBoxSendSubFee.Text = "Subtract Fee from Output";
            checkBoxSendSubFee.UseVisualStyleBackColor = true;
            // 
            // buttonSendMax
            // 
            buttonSendMax.FlatStyle = FlatStyle.Flat;
            buttonSendMax.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonSendMax.Location = new Point(609, 159);
            buttonSendMax.Name = "buttonSendMax";
            buttonSendMax.Size = new Size(81, 29);
            buttonSendMax.TabIndex = 15;
            buttonSendMax.TabStop = false;
            buttonSendMax.Text = "Max";
            buttonSendMax.UseVisualStyleBackColor = true;
            buttonSendMax.Click += buttonSendMax_Click;
            // 
            // buttonSendHNS
            // 
            buttonSendHNS.FlatStyle = FlatStyle.Flat;
            buttonSendHNS.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonSendHNS.Location = new Point(361, 315);
            buttonSendHNS.Name = "buttonSendHNS";
            buttonSendHNS.Size = new Size(150, 46);
            buttonSendHNS.TabIndex = 3;
            buttonSendHNS.Text = "Send";
            buttonSendHNS.UseVisualStyleBackColor = true;
            buttonSendHNS.Click += buttonSendHNS_Click;
            // 
            // labelSendingError
            // 
            labelSendingError.AutoSize = true;
            labelSendingError.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelSendingError.Location = new Point(615, 131);
            labelSendingError.Name = "labelSendingError";
            labelSendingError.Size = new Size(52, 21);
            labelSendingError.TabIndex = 13;
            labelSendingError.Text = "label1";
            labelSendingError.Visible = false;
            // 
            // labelSendingFee
            // 
            labelSendingFee.AutoSize = true;
            labelSendingFee.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelSendingFee.Location = new Point(254, 234);
            labelSendingFee.Name = "labelSendingFee";
            labelSendingFee.Size = new Size(109, 21);
            labelSendingFee.TabIndex = 12;
            labelSendingFee.Text = "Estimated Fee:";
            // 
            // textBoxSendingAmount
            // 
            textBoxSendingAmount.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            textBoxSendingAmount.Location = new Point(346, 159);
            textBoxSendingAmount.Name = "textBoxSendingAmount";
            textBoxSendingAmount.Size = new Size(344, 29);
            textBoxSendingAmount.TabIndex = 2;
            textBoxSendingAmount.Leave += textBoxSendingAmount_Leave;
            // 
            // textBoxSendingTo
            // 
            textBoxSendingTo.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            textBoxSendingTo.Location = new Point(346, 98);
            textBoxSendingTo.Name = "textBoxSendingTo";
            textBoxSendingTo.Size = new Size(344, 29);
            textBoxSendingTo.TabIndex = 1;
            textBoxSendingTo.Leave += textBoxSendingTo_Leave;
            // 
            // labelSendingMax
            // 
            labelSendingMax.AutoSize = true;
            labelSendingMax.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelSendingMax.Location = new Point(254, 204);
            labelSendingMax.Name = "labelSendingMax";
            labelSendingMax.Size = new Size(99, 21);
            labelSendingMax.TabIndex = 10;
            labelSendingMax.Text = "Max Amount";
            // 
            // labelSendingAmount
            // 
            labelSendingAmount.AutoSize = true;
            labelSendingAmount.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelSendingAmount.Location = new Point(254, 162);
            labelSendingAmount.Name = "labelSendingAmount";
            labelSendingAmount.Size = new Size(66, 21);
            labelSendingAmount.TabIndex = 9;
            labelSendingAmount.Text = "Amount";
            // 
            // labelSendingTo
            // 
            labelSendingTo.AutoSize = true;
            labelSendingTo.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelSendingTo.Location = new Point(254, 101);
            labelSendingTo.Name = "labelSendingTo";
            labelSendingTo.Size = new Size(86, 21);
            labelSendingTo.TabIndex = 8;
            labelSendingTo.Text = "Sending To";
            // 
            // labelSendPrompt
            // 
            labelSendPrompt.AutoSize = true;
            labelSendPrompt.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            labelSendPrompt.Location = new Point(395, 38);
            labelSendPrompt.Name = "labelSendPrompt";
            labelSendPrompt.Size = new Size(101, 28);
            labelSendPrompt.TabIndex = 0;
            labelSendPrompt.Text = "Send HNS";
            // 
            // panelRecieve
            // 
            panelRecieve.Controls.Add(pictureBoxReceiveQR);
            panelRecieve.Controls.Add(labelReceive2);
            panelRecieve.Controls.Add(textBoxReceiveAddress);
            panelRecieve.Controls.Add(labelReceive1);
            panelRecieve.Location = new Point(120, 25);
            panelRecieve.Name = "panelRecieve";
            panelRecieve.Size = new Size(995, 523);
            panelRecieve.TabIndex = 17;
            panelRecieve.Visible = false;
            // 
            // pictureBoxReceiveQR
            // 
            pictureBoxReceiveQR.Location = new Point(391, 190);
            pictureBoxReceiveQR.Name = "pictureBoxReceiveQR";
            pictureBoxReceiveQR.Size = new Size(300, 300);
            pictureBoxReceiveQR.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBoxReceiveQR.TabIndex = 20;
            pictureBoxReceiveQR.TabStop = false;
            // 
            // labelReceive2
            // 
            labelReceive2.AutoSize = true;
            labelReceive2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelReceive2.Location = new Point(463, 148);
            labelReceive2.Name = "labelReceive2";
            labelReceive2.Size = new Size(205, 21);
            labelReceive2.TabIndex = 19;
            labelReceive2.Text = "Click your address to copy it";
            // 
            // textBoxReceiveAddress
            // 
            textBoxReceiveAddress.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            textBoxReceiveAddress.Location = new Point(299, 110);
            textBoxReceiveAddress.Name = "textBoxReceiveAddress";
            textBoxReceiveAddress.ReadOnly = true;
            textBoxReceiveAddress.Size = new Size(464, 32);
            textBoxReceiveAddress.TabIndex = 18;
            textBoxReceiveAddress.Click += textBoxRecieveAddress_Click;
            // 
            // labelReceive1
            // 
            labelReceive1.AutoSize = true;
            labelReceive1.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            labelReceive1.Location = new Point(361, 77);
            labelReceive1.Name = "labelReceive1";
            labelReceive1.Size = new Size(252, 25);
            labelReceive1.TabIndex = 0;
            labelReceive1.Text = "Here is your receive address:";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1152, 575);
            Controls.Add(panelRecieve);
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
            panelSend.ResumeLayout(false);
            panelSend.PerformLayout();
            panelRecieve.ResumeLayout(false);
            panelRecieve.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxReceiveQR).EndInit();
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
        private Label labelloginprompt;
        private ComboBox comboBoxaccount;
        private TextBox textBoxaccountpassword;
        private ToolStripStatusLabel toolStripStatusLabelstatus;
        private ToolStripSplitButton toolStripSplitButtonlogout;
        private Panel panelNav;
        private Button buttonNavPortfolio;
        private Button buttonNavSend;
        private Button buttonNavReceive;
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
        private Label labelSendPrompt;
        private Label labelSendingMax;
        private Label labelSendingAmount;
        private Label labelSendingTo;
        private TextBox textBoxSendingTo;
        private TextBox textBoxSendingAmount;
        private Label labelSendingFee;
        private Label labelSendingError;
        private Button buttonSendHNS;
        private Button buttonSendMax;
        private CheckBox checkBoxSendSubFee;
        private Panel panelRecieve;
        private Label labelReceive1;
        private TextBox textBoxReceiveAddress;
        private Label labelReceive2;
        private PictureBox pictureBoxReceiveQR;
    }
}