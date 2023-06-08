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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
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
            buttonNavSettings = new Button();
            buttonBatch = new Button();
            buttonNavDomains = new Button();
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
            panelDomains = new Panel();
            groupBoxDomains = new GroupBox();
            panelDomainList = new Panel();
            labelDomainSearch = new Label();
            textBoxDomainSearch = new TextBox();
            panelSettings = new Panel();
            labelSettingsSaved = new Label();
            buttonSettingsSave = new Button();
            groupBoxSettingsExplorer = new GroupBox();
            labelSettings1 = new Label();
            textBoxExName = new TextBox();
            labelSettings2 = new Label();
            textBoxExBlock = new TextBox();
            labelSettings3 = new Label();
            textBoxExAddr = new TextBox();
            labelSettings4 = new Label();
            textBoxExTX = new TextBox();
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
            panelDomains.SuspendLayout();
            groupBoxDomains.SuspendLayout();
            panelSettings.SuspendLayout();
            groupBoxSettingsExplorer.SuspendLayout();
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
            toolStripStatusLabelNetwork.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripStatusLabelNetwork.Margin = new Padding(0, 3, 50, 2);
            toolStripStatusLabelNetwork.Name = "toolStripStatusLabelNetwork";
            toolStripStatusLabelNetwork.Size = new Size(58, 17);
            toolStripStatusLabelNetwork.Text = "Network: ";
            // 
            // toolStripStatusLabelstatus
            // 
            toolStripStatusLabelstatus.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripStatusLabelstatus.Margin = new Padding(0, 3, 50, 2);
            toolStripStatusLabelstatus.Name = "toolStripStatusLabelstatus";
            toolStripStatusLabelstatus.Size = new Size(126, 17);
            toolStripStatusLabelstatus.Text = "Status: Not Connected";
            // 
            // toolStripStatusLabelaccount
            // 
            toolStripStatusLabelaccount.DisplayStyle = ToolStripItemDisplayStyle.Text;
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
            buttonaccountlogin.TabStop = false;
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
            buttonaccountnew.TabStop = false;
            buttonaccountnew.Text = "New";
            buttonaccountnew.UseVisualStyleBackColor = true;
            // 
            // panelNav
            // 
            panelNav.Controls.Add(buttonNavSettings);
            panelNav.Controls.Add(buttonBatch);
            panelNav.Controls.Add(buttonNavDomains);
            panelNav.Controls.Add(buttonNavReceive);
            panelNav.Controls.Add(buttonNavSend);
            panelNav.Controls.Add(buttonNavPortfolio);
            panelNav.Dock = DockStyle.Left;
            panelNav.Location = new Point(0, 22);
            panelNav.Name = "panelNav";
            panelNav.Size = new Size(114, 553);
            panelNav.TabIndex = 6;
            // 
            // buttonNavSettings
            // 
            buttonNavSettings.FlatStyle = FlatStyle.Flat;
            buttonNavSettings.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonNavSettings.Location = new Point(12, 508);
            buttonNavSettings.Name = "buttonNavSettings";
            buttonNavSettings.Size = new Size(89, 33);
            buttonNavSettings.TabIndex = 4;
            buttonNavSettings.TabStop = false;
            buttonNavSettings.Text = "Settings";
            buttonNavSettings.UseVisualStyleBackColor = true;
            buttonNavSettings.Click += buttonNavSettings_Click;
            // 
            // buttonBatch
            // 
            buttonBatch.FlatStyle = FlatStyle.Flat;
            buttonBatch.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonBatch.Location = new Point(12, 245);
            buttonBatch.Name = "buttonBatch";
            buttonBatch.Size = new Size(89, 30);
            buttonBatch.TabIndex = 3;
            buttonBatch.TabStop = false;
            buttonBatch.Text = "Batch";
            buttonBatch.UseVisualStyleBackColor = true;
            buttonBatch.Click += buttonBatch_Click;
            // 
            // buttonNavDomains
            // 
            buttonNavDomains.FlatStyle = FlatStyle.Flat;
            buttonNavDomains.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonNavDomains.Location = new Point(12, 189);
            buttonNavDomains.Name = "buttonNavDomains";
            buttonNavDomains.Size = new Size(89, 30);
            buttonNavDomains.TabIndex = 2;
            buttonNavDomains.TabStop = false;
            buttonNavDomains.Text = "Domains";
            buttonNavDomains.UseVisualStyleBackColor = true;
            buttonNavDomains.Click += buttonNavDomains_Click;
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
            panelPortfolio.Location = new Point(1085, 47);
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
            panelSend.Location = new Point(1113, 42);
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
            panelRecieve.Location = new Point(1057, 62);
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
            // panelDomains
            // 
            panelDomains.Controls.Add(groupBoxDomains);
            panelDomains.Controls.Add(labelDomainSearch);
            panelDomains.Controls.Add(textBoxDomainSearch);
            panelDomains.Location = new Point(1129, 22);
            panelDomains.Name = "panelDomains";
            panelDomains.Size = new Size(920, 536);
            panelDomains.TabIndex = 18;
            panelDomains.Visible = false;
            // 
            // groupBoxDomains
            // 
            groupBoxDomains.Controls.Add(panelDomainList);
            groupBoxDomains.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            groupBoxDomains.Location = new Point(18, 59);
            groupBoxDomains.Name = "groupBoxDomains";
            groupBoxDomains.Size = new Size(887, 466);
            groupBoxDomains.TabIndex = 2;
            groupBoxDomains.TabStop = false;
            groupBoxDomains.Text = "Domains";
            // 
            // panelDomainList
            // 
            panelDomainList.AutoScroll = true;
            panelDomainList.Dock = DockStyle.Fill;
            panelDomainList.Location = new Point(3, 25);
            panelDomainList.Name = "panelDomainList";
            panelDomainList.Size = new Size(881, 438);
            panelDomainList.TabIndex = 0;
            // 
            // labelDomainSearch
            // 
            labelDomainSearch.AutoSize = true;
            labelDomainSearch.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelDomainSearch.Location = new Point(18, 15);
            labelDomainSearch.Name = "labelDomainSearch";
            labelDomainSearch.Size = new Size(60, 21);
            labelDomainSearch.TabIndex = 1;
            labelDomainSearch.Text = "Search:";
            // 
            // textBoxDomainSearch
            // 
            textBoxDomainSearch.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            textBoxDomainSearch.Location = new Point(84, 12);
            textBoxDomainSearch.Name = "textBoxDomainSearch";
            textBoxDomainSearch.Size = new Size(261, 29);
            textBoxDomainSearch.TabIndex = 0;
            textBoxDomainSearch.TextChanged += textBoxDomainSearch_TextChanged;
            textBoxDomainSearch.KeyDown += textBoxDomainSearch_KeyDown;
            // 
            // panelSettings
            // 
            panelSettings.Controls.Add(labelSettingsSaved);
            panelSettings.Controls.Add(buttonSettingsSave);
            panelSettings.Controls.Add(groupBoxSettingsExplorer);
            panelSettings.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            panelSettings.Location = new Point(121, 21);
            panelSettings.Name = "panelSettings";
            panelSettings.Size = new Size(930, 550);
            panelSettings.TabIndex = 19;
            panelSettings.Visible = false;
            // 
            // labelSettingsSaved
            // 
            labelSettingsSaved.AutoSize = true;
            labelSettingsSaved.Location = new Point(109, 515);
            labelSettingsSaved.Name = "labelSettingsSaved";
            labelSettingsSaved.Size = new Size(52, 21);
            labelSettingsSaved.TabIndex = 6;
            labelSettingsSaved.Text = "Saved";
            labelSettingsSaved.Visible = false;
            // 
            // buttonSettingsSave
            // 
            buttonSettingsSave.FlatStyle = FlatStyle.Flat;
            buttonSettingsSave.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonSettingsSave.Location = new Point(14, 509);
            buttonSettingsSave.Name = "buttonSettingsSave";
            buttonSettingsSave.Size = new Size(89, 33);
            buttonSettingsSave.TabIndex = 4;
            buttonSettingsSave.TabStop = false;
            buttonSettingsSave.Text = "Save";
            buttonSettingsSave.UseVisualStyleBackColor = true;
            buttonSettingsSave.Click += buttonSettingsSave_Click;
            // 
            // groupBoxSettingsExplorer
            // 
            groupBoxSettingsExplorer.Controls.Add(labelSettings1);
            groupBoxSettingsExplorer.Controls.Add(textBoxExName);
            groupBoxSettingsExplorer.Controls.Add(labelSettings2);
            groupBoxSettingsExplorer.Controls.Add(textBoxExBlock);
            groupBoxSettingsExplorer.Controls.Add(labelSettings3);
            groupBoxSettingsExplorer.Controls.Add(textBoxExAddr);
            groupBoxSettingsExplorer.Controls.Add(labelSettings4);
            groupBoxSettingsExplorer.Controls.Add(textBoxExTX);
            groupBoxSettingsExplorer.Location = new Point(14, 16);
            groupBoxSettingsExplorer.Name = "groupBoxSettingsExplorer";
            groupBoxSettingsExplorer.Size = new Size(487, 188);
            groupBoxSettingsExplorer.TabIndex = 5;
            groupBoxSettingsExplorer.TabStop = false;
            groupBoxSettingsExplorer.Text = "Explorer Settings";
            // 
            // labelSettings1
            // 
            labelSettings1.AutoSize = true;
            labelSettings1.Location = new Point(43, 31);
            labelSettings1.Name = "labelSettings1";
            labelSettings1.Size = new Size(108, 21);
            labelSettings1.TabIndex = 0;
            labelSettings1.Text = "Explorer (TXs):";
            // 
            // textBoxExName
            // 
            textBoxExName.Location = new Point(157, 133);
            textBoxExName.Name = "textBoxExName";
            textBoxExName.Size = new Size(307, 29);
            textBoxExName.TabIndex = 4;
            // 
            // labelSettings2
            // 
            labelSettings2.AutoSize = true;
            labelSettings2.Location = new Point(11, 66);
            labelSettings2.Name = "labelSettings2";
            labelSettings2.Size = new Size(140, 21);
            labelSettings2.TabIndex = 0;
            labelSettings2.Text = "Explorer (Address):";
            // 
            // textBoxExBlock
            // 
            textBoxExBlock.Location = new Point(157, 98);
            textBoxExBlock.Name = "textBoxExBlock";
            textBoxExBlock.Size = new Size(307, 29);
            textBoxExBlock.TabIndex = 3;
            // 
            // labelSettings3
            // 
            labelSettings3.AutoSize = true;
            labelSettings3.Location = new Point(23, 101);
            labelSettings3.Name = "labelSettings3";
            labelSettings3.Size = new Size(128, 21);
            labelSettings3.TabIndex = 0;
            labelSettings3.Text = "Explorer (Blocks):";
            // 
            // textBoxExAddr
            // 
            textBoxExAddr.Location = new Point(157, 63);
            textBoxExAddr.Name = "textBoxExAddr";
            textBoxExAddr.Size = new Size(307, 29);
            textBoxExAddr.TabIndex = 2;
            // 
            // labelSettings4
            // 
            labelSettings4.AutoSize = true;
            labelSettings4.Location = new Point(5, 136);
            labelSettings4.Name = "labelSettings4";
            labelSettings4.Size = new Size(146, 21);
            labelSettings4.TabIndex = 0;
            labelSettings4.Text = "Explorer (Domains):";
            // 
            // textBoxExTX
            // 
            textBoxExTX.Location = new Point(157, 28);
            textBoxExTX.Name = "textBoxExTX";
            textBoxExTX.Size = new Size(307, 29);
            textBoxExTX.TabIndex = 1;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1152, 575);
            Controls.Add(panelSettings);
            Controls.Add(panelDomains);
            Controls.Add(panelRecieve);
            Controls.Add(panelSend);
            Controls.Add(panelPortfolio);
            Controls.Add(panelNav);
            Controls.Add(panelaccount);
            Controls.Add(statusStripmain);
            Icon = (Icon)resources.GetObject("$this.Icon");
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
            panelDomains.ResumeLayout(false);
            panelDomains.PerformLayout();
            groupBoxDomains.ResumeLayout(false);
            panelSettings.ResumeLayout(false);
            panelSettings.PerformLayout();
            groupBoxSettingsExplorer.ResumeLayout(false);
            groupBoxSettingsExplorer.PerformLayout();
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
        private Button buttonNavDomains;
        private Panel panelDomains;
        private Label labelDomainSearch;
        private TextBox textBoxDomainSearch;
        private Button buttonBatch;
        private GroupBox groupBoxDomains;
        private Panel panelDomainList;
        private Button buttonNavSettings;
        private Panel panelSettings;
        private Label labelSettings1;
        private Label labelSettings2;
        private TextBox textBoxExName;
        private TextBox textBoxExBlock;
        private TextBox textBoxExAddr;
        private TextBox textBoxExTX;
        private Label labelSettings4;
        private Label labelSettings3;
        private Button buttonSettingsSave;
        private GroupBox groupBoxSettingsExplorer;
        private Label labelSettingsSaved;
    }
}