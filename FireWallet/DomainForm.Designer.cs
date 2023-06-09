﻿namespace FireWallet
{
    partial class DomainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DomainForm));
            labelTitle = new Label();
            groupBoxStatus = new GroupBox();
            labelStatusTimeToNext = new Label();
            labelStatusNextState = new Label();
            labelStatusTransferring = new Label();
            labelStatus5 = new Label();
            labelStatusPaid = new Label();
            labelStatus4 = new Label();
            labelStatusHighest = new Label();
            labelStatus3 = new Label();
            labelStatusReserved = new Label();
            labelStatus2 = new Label();
            labelStatusMain = new Label();
            labelStatus1 = new Label();
            groupBoxDNS = new GroupBox();
            panelDNS = new Panel();
            groupBoxBids = new GroupBox();
            panelBids = new Panel();
            groupBoxAction = new GroupBox();
            buttonTransfer = new Button();
            buttonRenew = new Button();
            textBoxBlind = new TextBox();
            textBoxBid = new TextBox();
            labelBlind = new Label();
            labelBid = new Label();
            buttonActionAlt = new Button();
            buttonActionMain = new Button();
            buttonExplorer = new Button();
            groupBoxStatus.SuspendLayout();
            groupBoxDNS.SuspendLayout();
            groupBoxBids.SuspendLayout();
            groupBoxAction.SuspendLayout();
            SuspendLayout();
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point);
            labelTitle.Location = new Point(12, 9);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(126, 37);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "labelTitle";
            // 
            // groupBoxStatus
            // 
            groupBoxStatus.Controls.Add(labelStatusTimeToNext);
            groupBoxStatus.Controls.Add(labelStatusNextState);
            groupBoxStatus.Controls.Add(labelStatusTransferring);
            groupBoxStatus.Controls.Add(labelStatus5);
            groupBoxStatus.Controls.Add(labelStatusPaid);
            groupBoxStatus.Controls.Add(labelStatus4);
            groupBoxStatus.Controls.Add(labelStatusHighest);
            groupBoxStatus.Controls.Add(labelStatus3);
            groupBoxStatus.Controls.Add(labelStatusReserved);
            groupBoxStatus.Controls.Add(labelStatus2);
            groupBoxStatus.Controls.Add(labelStatusMain);
            groupBoxStatus.Controls.Add(labelStatus1);
            groupBoxStatus.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            groupBoxStatus.Location = new Point(12, 68);
            groupBoxStatus.Name = "groupBoxStatus";
            groupBoxStatus.Size = new Size(378, 173);
            groupBoxStatus.TabIndex = 1;
            groupBoxStatus.TabStop = false;
            groupBoxStatus.Text = "Info";
            // 
            // labelStatusTimeToNext
            // 
            labelStatusTimeToNext.AutoSize = true;
            labelStatusTimeToNext.Location = new Point(112, 130);
            labelStatusTimeToNext.Name = "labelStatusTimeToNext";
            labelStatusTimeToNext.Size = new Size(67, 21);
            labelStatusTimeToNext.TabIndex = 11;
            labelStatusTimeToNext.Text = "0 Blocks";
            // 
            // labelStatusNextState
            // 
            labelStatusNextState.AutoSize = true;
            labelStatusNextState.Location = new Point(27, 130);
            labelStatusNextState.Name = "labelStatusNextState";
            labelStatusNextState.Size = new Size(79, 21);
            labelStatusNextState.TabIndex = 10;
            labelStatusNextState.Text = "Expires in:";
            // 
            // labelStatusTransferring
            // 
            labelStatusTransferring.AutoSize = true;
            labelStatusTransferring.Location = new Point(112, 109);
            labelStatusTransferring.Name = "labelStatusTransferring";
            labelStatusTransferring.Size = new Size(31, 21);
            labelStatusTransferring.TabIndex = 9;
            labelStatusTransferring.Text = "No";
            // 
            // labelStatus5
            // 
            labelStatus5.AutoSize = true;
            labelStatus5.Location = new Point(9, 109);
            labelStatus5.Name = "labelStatus5";
            labelStatus5.Size = new Size(97, 21);
            labelStatus5.TabIndex = 8;
            labelStatus5.Text = "Transferring:";
            // 
            // labelStatusPaid
            // 
            labelStatusPaid.AutoSize = true;
            labelStatusPaid.Location = new Point(112, 88);
            labelStatusPaid.Name = "labelStatusPaid";
            labelStatusPaid.Size = new Size(95, 21);
            labelStatusPaid.TabIndex = 7;
            labelStatusPaid.Text = "Not Opened";
            // 
            // labelStatus4
            // 
            labelStatus4.AutoSize = true;
            labelStatus4.Location = new Point(26, 88);
            labelStatus4.Name = "labelStatus4";
            labelStatus4.Size = new Size(80, 21);
            labelStatus4.TabIndex = 6;
            labelStatus4.Text = "Price Paid:";
            // 
            // labelStatusHighest
            // 
            labelStatusHighest.AutoSize = true;
            labelStatusHighest.Location = new Point(112, 67);
            labelStatusHighest.Name = "labelStatusHighest";
            labelStatusHighest.Size = new Size(95, 21);
            labelStatusHighest.TabIndex = 5;
            labelStatusHighest.Text = "Not Opened";
            // 
            // labelStatus3
            // 
            labelStatus3.AutoSize = true;
            labelStatus3.Location = new Point(14, 67);
            labelStatus3.Name = "labelStatus3";
            labelStatus3.Size = new Size(92, 21);
            labelStatus3.TabIndex = 4;
            labelStatus3.Text = "Highest Bid:";
            // 
            // labelStatusReserved
            // 
            labelStatusReserved.AutoSize = true;
            labelStatusReserved.Location = new Point(112, 25);
            labelStatusReserved.Name = "labelStatusReserved";
            labelStatusReserved.Size = new Size(40, 21);
            labelStatusReserved.TabIndex = 3;
            labelStatusReserved.Text = "True";
            // 
            // labelStatus2
            // 
            labelStatus2.AutoSize = true;
            labelStatus2.Location = new Point(29, 25);
            labelStatus2.Name = "labelStatus2";
            labelStatus2.Size = new Size(77, 21);
            labelStatus2.TabIndex = 2;
            labelStatus2.Text = "Reserved:";
            // 
            // labelStatusMain
            // 
            labelStatusMain.AutoSize = true;
            labelStatusMain.Location = new Point(112, 46);
            labelStatusMain.Name = "labelStatusMain";
            labelStatusMain.Size = new Size(57, 21);
            labelStatusMain.TabIndex = 1;
            labelStatusMain.Text = "Closed";
            // 
            // labelStatus1
            // 
            labelStatus1.AutoSize = true;
            labelStatus1.Location = new Point(51, 46);
            labelStatus1.Name = "labelStatus1";
            labelStatus1.Size = new Size(55, 21);
            labelStatus1.TabIndex = 0;
            labelStatus1.Text = "Status:";
            // 
            // groupBoxDNS
            // 
            groupBoxDNS.Controls.Add(panelDNS);
            groupBoxDNS.Location = new Point(12, 247);
            groupBoxDNS.Name = "groupBoxDNS";
            groupBoxDNS.Size = new Size(962, 313);
            groupBoxDNS.TabIndex = 12;
            groupBoxDNS.TabStop = false;
            groupBoxDNS.Text = "DNS";
            // 
            // panelDNS
            // 
            panelDNS.Dock = DockStyle.Fill;
            panelDNS.Location = new Point(3, 19);
            panelDNS.Name = "panelDNS";
            panelDNS.Size = new Size(956, 291);
            panelDNS.TabIndex = 0;
            // 
            // groupBoxBids
            // 
            groupBoxBids.Controls.Add(panelBids);
            groupBoxBids.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            groupBoxBids.Location = new Point(12, 247);
            groupBoxBids.Name = "groupBoxBids";
            groupBoxBids.Size = new Size(962, 313);
            groupBoxBids.TabIndex = 13;
            groupBoxBids.TabStop = false;
            groupBoxBids.Text = "Bids";
            groupBoxBids.Visible = false;
            // 
            // panelBids
            // 
            panelBids.Dock = DockStyle.Fill;
            panelBids.Location = new Point(3, 19);
            panelBids.Name = "panelBids";
            panelBids.Size = new Size(956, 291);
            panelBids.TabIndex = 0;
            // 
            // groupBoxAction
            // 
            groupBoxAction.Controls.Add(buttonTransfer);
            groupBoxAction.Controls.Add(buttonRenew);
            groupBoxAction.Controls.Add(textBoxBlind);
            groupBoxAction.Controls.Add(textBoxBid);
            groupBoxAction.Controls.Add(labelBlind);
            groupBoxAction.Controls.Add(labelBid);
            groupBoxAction.Controls.Add(buttonActionAlt);
            groupBoxAction.Controls.Add(buttonActionMain);
            groupBoxAction.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            groupBoxAction.Location = new Point(396, 68);
            groupBoxAction.Name = "groupBoxAction";
            groupBoxAction.Size = new Size(575, 173);
            groupBoxAction.TabIndex = 14;
            groupBoxAction.TabStop = false;
            groupBoxAction.Text = "Bid";
            groupBoxAction.Visible = false;
            // 
            // buttonTransfer
            // 
            buttonTransfer.FlatStyle = FlatStyle.Flat;
            buttonTransfer.Location = new Point(253, 22);
            buttonTransfer.Name = "buttonTransfer";
            buttonTransfer.Size = new Size(155, 37);
            buttonTransfer.TabIndex = 6;
            buttonTransfer.Text = "Transfer";
            buttonTransfer.UseVisualStyleBackColor = true;
            buttonTransfer.Visible = false;
            buttonTransfer.Click += buttonTransfer_Click;
            // 
            // buttonRenew
            // 
            buttonRenew.FlatStyle = FlatStyle.Flat;
            buttonRenew.Location = new Point(414, 22);
            buttonRenew.Name = "buttonRenew";
            buttonRenew.Size = new Size(155, 37);
            buttonRenew.TabIndex = 6;
            buttonRenew.Text = "Renew";
            buttonRenew.UseVisualStyleBackColor = true;
            buttonRenew.Visible = false;
            buttonRenew.Click += buttonRenew_Click;
            // 
            // textBoxBlind
            // 
            textBoxBlind.Location = new Point(60, 64);
            textBoxBlind.Name = "textBoxBlind";
            textBoxBlind.Size = new Size(180, 29);
            textBoxBlind.TabIndex = 5;
            textBoxBlind.Visible = false;
            textBoxBlind.TextChanged += BidBlind_TextChanged;
            // 
            // textBoxBid
            // 
            textBoxBid.Location = new Point(60, 22);
            textBoxBid.Name = "textBoxBid";
            textBoxBid.Size = new Size(180, 29);
            textBoxBid.TabIndex = 4;
            textBoxBid.Visible = false;
            textBoxBid.TextChanged += BidBlind_TextChanged;
            // 
            // labelBlind
            // 
            labelBlind.AutoSize = true;
            labelBlind.Location = new Point(6, 67);
            labelBlind.Name = "labelBlind";
            labelBlind.Size = new Size(48, 21);
            labelBlind.TabIndex = 3;
            labelBlind.Text = "Blind:";
            labelBlind.Visible = false;
            // 
            // labelBid
            // 
            labelBid.AutoSize = true;
            labelBid.Location = new Point(19, 25);
            labelBid.Name = "labelBid";
            labelBid.Size = new Size(35, 21);
            labelBid.TabIndex = 2;
            labelBid.Text = "Bid:";
            labelBid.Visible = false;
            // 
            // buttonActionAlt
            // 
            buttonActionAlt.FlatStyle = FlatStyle.Flat;
            buttonActionAlt.Location = new Point(253, 130);
            buttonActionAlt.Name = "buttonActionAlt";
            buttonActionAlt.Size = new Size(155, 37);
            buttonActionAlt.TabIndex = 1;
            buttonActionAlt.Text = "Bid in Batch";
            buttonActionAlt.UseVisualStyleBackColor = true;
            buttonActionAlt.Click += buttonActionAlt_Click;
            // 
            // buttonActionMain
            // 
            buttonActionMain.FlatStyle = FlatStyle.Flat;
            buttonActionMain.Location = new Point(414, 130);
            buttonActionMain.Name = "buttonActionMain";
            buttonActionMain.Size = new Size(155, 37);
            buttonActionMain.TabIndex = 0;
            buttonActionMain.Text = "Send Bid";
            buttonActionMain.UseVisualStyleBackColor = true;
            buttonActionMain.Click += buttonActionMain_Click;
            // 
            // buttonExplorer
            // 
            buttonExplorer.FlatStyle = FlatStyle.Flat;
            buttonExplorer.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonExplorer.Location = new Point(876, 12);
            buttonExplorer.Name = "buttonExplorer";
            buttonExplorer.Size = new Size(98, 34);
            buttonExplorer.TabIndex = 15;
            buttonExplorer.TabStop = false;
            buttonExplorer.Text = "Explorer";
            buttonExplorer.UseVisualStyleBackColor = true;
            buttonExplorer.Click += Explorer_Click;
            // 
            // DomainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(986, 572);
            Controls.Add(buttonExplorer);
            Controls.Add(groupBoxAction);
            Controls.Add(groupBoxBids);
            Controls.Add(groupBoxDNS);
            Controls.Add(groupBoxStatus);
            Controls.Add(labelTitle);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "DomainForm";
            Text = "DomainForm";
            Load += DomainForm_Load;
            groupBoxStatus.ResumeLayout(false);
            groupBoxStatus.PerformLayout();
            groupBoxDNS.ResumeLayout(false);
            groupBoxBids.ResumeLayout(false);
            groupBoxAction.ResumeLayout(false);
            groupBoxAction.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelTitle;
        private GroupBox groupBoxStatus;
        private Label labelStatusMain;
        private Label labelStatus1;
        private Label labelStatusReserved;
        private Label labelStatus2;
        private Label labelStatusHighest;
        private Label labelStatus3;
        private Label labelStatusPaid;
        private Label labelStatus4;
        private Label labelStatus5;
        private Label labelStatusTransferring;
        private Label labelStatusTimeToNext;
        private Label labelStatusNextState;
        private GroupBox groupBoxDNS;
        private Panel panelDNS;
        private GroupBox groupBoxBids;
        private Panel panelBids;
        private GroupBox groupBoxAction;
        private TextBox textBoxBlind;
        private TextBox textBoxBid;
        private Label labelBlind;
        private Label labelBid;
        private Button buttonActionAlt;
        private Button buttonActionMain;
        private Button buttonExplorer;
        private Button buttonRenew;
        private Button buttonTransfer;
    }
}