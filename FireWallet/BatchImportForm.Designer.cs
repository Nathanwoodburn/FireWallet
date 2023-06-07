namespace FireWallet
{
    partial class BatchImportForm
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
            listBoxDomains = new ListBox();
            label1 = new Label();
            comboBoxMode = new ComboBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            textBoxBid = new TextBox();
            textBoxBlind = new TextBox();
            buttonImport = new Button();
            buttonCancel = new Button();
            SuspendLayout();
            // 
            // listBoxDomains
            // 
            listBoxDomains.FormattingEnabled = true;
            listBoxDomains.ItemHeight = 15;
            listBoxDomains.Location = new System.Drawing.Point(12, 42);
            listBoxDomains.Name = "listBoxDomains";
            listBoxDomains.Size = new System.Drawing.Size(241, 484);
            listBoxDomains.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new System.Drawing.Point(12, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(75, 21);
            label1.TabIndex = 1;
            label1.Text = "Domains:";
            // 
            // comboBoxMode
            // 
            comboBoxMode.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxMode.FlatStyle = FlatStyle.Flat;
            comboBoxMode.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            comboBoxMode.FormattingEnabled = true;
            comboBoxMode.Items.AddRange(new object[] { "OPEN", "BID", "REVEAL", "REDEEM", "RENEW" });
            comboBoxMode.Location = new System.Drawing.Point(346, 42);
            comboBoxMode.Name = "comboBoxMode";
            comboBoxMode.Size = new System.Drawing.Size(226, 29);
            comboBoxMode.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new System.Drawing.Point(287, 45);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(53, 21);
            label2.TabIndex = 3;
            label2.Text = "Mode:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new System.Drawing.Point(305, 111);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(35, 21);
            label3.TabIndex = 3;
            label3.Text = "Bid:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new System.Drawing.Point(292, 159);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(48, 21);
            label4.TabIndex = 3;
            label4.Text = "Blind:";
            // 
            // textBoxBid
            // 
            textBoxBid.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            textBoxBid.Location = new System.Drawing.Point(346, 108);
            textBoxBid.Name = "textBoxBid";
            textBoxBid.Size = new System.Drawing.Size(226, 29);
            textBoxBid.TabIndex = 4;
            // 
            // textBoxBlind
            // 
            textBoxBlind.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            textBoxBlind.Location = new System.Drawing.Point(346, 156);
            textBoxBlind.Name = "textBoxBlind";
            textBoxBlind.Size = new System.Drawing.Size(226, 29);
            textBoxBlind.TabIndex = 5;
            // 
            // buttonImport
            // 
            buttonImport.FlatStyle = FlatStyle.Flat;
            buttonImport.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonImport.Location = new System.Drawing.Point(851, 485);
            buttonImport.Name = "buttonImport";
            buttonImport.Size = new System.Drawing.Size(87, 38);
            buttonImport.TabIndex = 6;
            buttonImport.Text = "Import";
            buttonImport.UseVisualStyleBackColor = true;
            buttonImport.Click += buttonImport_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.FlatStyle = FlatStyle.Flat;
            buttonCancel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonCancel.Location = new System.Drawing.Point(731, 485);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new System.Drawing.Size(87, 38);
            buttonCancel.TabIndex = 6;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // BatchImportForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(950, 535);
            Controls.Add(buttonCancel);
            Controls.Add(buttonImport);
            Controls.Add(textBoxBlind);
            Controls.Add(textBoxBid);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(comboBoxMode);
            Controls.Add(label1);
            Controls.Add(listBoxDomains);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            Name = "BatchImportForm";
            Text = "Import";
            Load += BatchImportForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBoxDomains;
        private Label label1;
        private ComboBox comboBoxMode;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox textBoxBid;
        private TextBox textBoxBlind;
        private Button buttonImport;
        private Button buttonCancel;
    }
}