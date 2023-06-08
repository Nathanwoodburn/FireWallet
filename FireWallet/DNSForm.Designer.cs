namespace FireWallet
{
    partial class DNSForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DNSForm));
            groupBoxRecords = new GroupBox();
            panelRecords = new Panel();
            buttonAddRecord = new Button();
            buttonCancel = new Button();
            buttonSend = new Button();
            label1 = new Label();
            groupBoxAddRecord = new GroupBox();
            labelRecordAlt = new Label();
            textBoxAlt = new TextBox();
            textBoxMain = new TextBox();
            labelRecordMain = new Label();
            comboBoxType = new ComboBox();
            groupBoxRecords.SuspendLayout();
            groupBoxAddRecord.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxRecords
            // 
            groupBoxRecords.Controls.Add(panelRecords);
            groupBoxRecords.Location = new Point(12, 12);
            groupBoxRecords.Name = "groupBoxRecords";
            groupBoxRecords.Size = new Size(542, 540);
            groupBoxRecords.TabIndex = 0;
            groupBoxRecords.TabStop = false;
            groupBoxRecords.Text = "DNS Records";
            // 
            // panelRecords
            // 
            panelRecords.AutoScroll = true;
            panelRecords.Dock = DockStyle.Fill;
            panelRecords.Location = new Point(3, 19);
            panelRecords.Name = "panelRecords";
            panelRecords.Size = new Size(536, 518);
            panelRecords.TabIndex = 0;
            // 
            // buttonAddRecord
            // 
            buttonAddRecord.FlatStyle = FlatStyle.Flat;
            buttonAddRecord.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonAddRecord.Location = new Point(6, 172);
            buttonAddRecord.Name = "buttonAddRecord";
            buttonAddRecord.Size = new Size(93, 46);
            buttonAddRecord.TabIndex = 1;
            buttonAddRecord.TabStop = false;
            buttonAddRecord.Text = "Add ";
            buttonAddRecord.UseVisualStyleBackColor = true;
            buttonAddRecord.Click += buttonAddRecord_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.FlatStyle = FlatStyle.Flat;
            buttonCancel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonCancel.Location = new Point(566, 500);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(102, 46);
            buttonCancel.TabIndex = 1;
            buttonCancel.TabStop = false;
            buttonCancel.Text = "Cancel Edit";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonSend
            // 
            buttonSend.FlatStyle = FlatStyle.Flat;
            buttonSend.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonSend.Location = new Point(879, 500);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(102, 46);
            buttonSend.TabIndex = 1;
            buttonSend.TabStop = false;
            buttonSend.Text = "Send Edit";
            buttonSend.UseVisualStyleBackColor = true;
            buttonSend.Click += buttonSend_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 28);
            label1.Name = "label1";
            label1.Size = new Size(34, 15);
            label1.TabIndex = 2;
            label1.Text = "Type:";
            // 
            // groupBoxAddRecord
            // 
            groupBoxAddRecord.Controls.Add(labelRecordAlt);
            groupBoxAddRecord.Controls.Add(textBoxAlt);
            groupBoxAddRecord.Controls.Add(textBoxMain);
            groupBoxAddRecord.Controls.Add(labelRecordMain);
            groupBoxAddRecord.Controls.Add(comboBoxType);
            groupBoxAddRecord.Controls.Add(label1);
            groupBoxAddRecord.Controls.Add(buttonAddRecord);
            groupBoxAddRecord.Location = new Point(560, 12);
            groupBoxAddRecord.Name = "groupBoxAddRecord";
            groupBoxAddRecord.Size = new Size(421, 224);
            groupBoxAddRecord.TabIndex = 3;
            groupBoxAddRecord.TabStop = false;
            groupBoxAddRecord.Text = "New Record";
            // 
            // labelRecordAlt
            // 
            labelRecordAlt.AutoSize = true;
            labelRecordAlt.Location = new Point(6, 105);
            labelRecordAlt.Name = "labelRecordAlt";
            labelRecordAlt.Size = new Size(52, 15);
            labelRecordAlt.TabIndex = 6;
            labelRecordAlt.Text = "Address:";
            // 
            // textBoxAlt
            // 
            textBoxAlt.Location = new Point(65, 102);
            textBoxAlt.Name = "textBoxAlt";
            textBoxAlt.Size = new Size(326, 23);
            textBoxAlt.TabIndex = 5;
            textBoxAlt.KeyDown += textBoxMain_KeyDown;
            // 
            // textBoxMain
            // 
            textBoxMain.Location = new Point(65, 65);
            textBoxMain.Name = "textBoxMain";
            textBoxMain.Size = new Size(326, 23);
            textBoxMain.TabIndex = 5;
            textBoxMain.KeyDown += textBoxMain_KeyDown;
            // 
            // labelRecordMain
            // 
            labelRecordMain.AutoSize = true;
            labelRecordMain.Location = new Point(6, 68);
            labelRecordMain.Name = "labelRecordMain";
            labelRecordMain.Size = new Size(53, 15);
            labelRecordMain.TabIndex = 4;
            labelRecordMain.Text = "Content:";
            // 
            // comboBoxType
            // 
            comboBoxType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxType.FlatStyle = FlatStyle.Flat;
            comboBoxType.FormattingEnabled = true;
            comboBoxType.Items.AddRange(new object[] { "NS", "DS", "GLUE4", "GLUE6", "TXT" });
            comboBoxType.Location = new Point(65, 25);
            comboBoxType.Name = "comboBoxType";
            comboBoxType.Size = new Size(129, 23);
            comboBoxType.TabIndex = 3;
            comboBoxType.SelectedIndexChanged += comboBoxType_SelectedIndexChanged;
            comboBoxType.DropDownClosed += comboBoxType_DropDownClosed;
            // 
            // DNSForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(993, 564);
            Controls.Add(groupBoxAddRecord);
            Controls.Add(buttonSend);
            Controls.Add(buttonCancel);
            Controls.Add(groupBoxRecords);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "DNSForm";
            Text = "DNSForm";
            Load += DNSForm_Load;
            groupBoxRecords.ResumeLayout(false);
            groupBoxAddRecord.ResumeLayout(false);
            groupBoxAddRecord.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBoxRecords;
        private Panel panelRecords;
        private Button buttonAddRecord;
        private Button buttonCancel;
        private Button buttonSend;
        private Label label1;
        private GroupBox groupBoxAddRecord;
        private ComboBox comboBoxType;
        private Label labelRecordMain;
        private Label labelRecordAlt;
        private TextBox textBoxAlt;
        private TextBox textBoxMain;
    }
}