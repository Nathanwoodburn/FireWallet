namespace FireWallet
{
    partial class ExportTXForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportTXForm));
            label1 = new Label();
            buttonExport = new Button();
            groupBoxTXs = new GroupBox();
            buttonRefresh = new Button();
            numericUpDownLimit = new NumericUpDown();
            label2 = new Label();
            comboBoxFilter = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)numericUpDownLimit).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(37, 15);
            label1.TabIndex = 0;
            label1.Text = "Limit:";
            // 
            // buttonExport
            // 
            buttonExport.FlatStyle = FlatStyle.Flat;
            buttonExport.Location = new Point(694, 409);
            buttonExport.Name = "buttonExport";
            buttonExport.Size = new Size(94, 29);
            buttonExport.TabIndex = 1;
            buttonExport.Text = "Export";
            buttonExport.UseVisualStyleBackColor = true;
            buttonExport.Click += buttonExport_Click;
            // 
            // groupBoxTXs
            // 
            groupBoxTXs.Location = new Point(12, 57);
            groupBoxTXs.Name = "groupBoxTXs";
            groupBoxTXs.Size = new Size(553, 336);
            groupBoxTXs.TabIndex = 2;
            groupBoxTXs.TabStop = false;
            groupBoxTXs.Text = "TXs";
            // 
            // buttonRefresh
            // 
            buttonRefresh.FlatStyle = FlatStyle.Flat;
            buttonRefresh.Location = new Point(12, 409);
            buttonRefresh.Name = "buttonRefresh";
            buttonRefresh.Size = new Size(82, 29);
            buttonRefresh.TabIndex = 3;
            buttonRefresh.Text = "Refresh";
            buttonRefresh.UseVisualStyleBackColor = true;
            buttonRefresh.Click += buttonRefresh_Click;
            // 
            // numericUpDownLimit
            // 
            numericUpDownLimit.Location = new Point(55, 7);
            numericUpDownLimit.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numericUpDownLimit.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownLimit.Name = "numericUpDownLimit";
            numericUpDownLimit.Size = new Size(120, 23);
            numericUpDownLimit.TabIndex = 4;
            numericUpDownLimit.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(209, 9);
            label2.Name = "label2";
            label2.Size = new Size(36, 15);
            label2.TabIndex = 5;
            label2.Text = "Filter:";
            // 
            // comboBoxFilter
            // 
            comboBoxFilter.FormattingEnabled = true;
            comboBoxFilter.Items.AddRange(new object[] { "ALL", "NONE", "OPEN", "BID", "REVEAL", "REDEEM", "UPDATE", "TRANSFER", "FINALIZE" });
            comboBoxFilter.Location = new Point(251, 6);
            comboBoxFilter.Name = "comboBoxFilter";
            comboBoxFilter.Size = new Size(121, 23);
            comboBoxFilter.TabIndex = 6;
            // 
            // ExportTXForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(comboBoxFilter);
            Controls.Add(label2);
            Controls.Add(numericUpDownLimit);
            Controls.Add(buttonRefresh);
            Controls.Add(groupBoxTXs);
            Controls.Add(buttonExport);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "ExportTXForm";
            Text = "Export TXs";
            Load += ExportTXForm_Load;
            ((System.ComponentModel.ISupportInitialize)numericUpDownLimit).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button buttonExport;
        private GroupBox groupBoxTXs;
        private Button buttonRefresh;
        private NumericUpDown numericUpDownLimit;
        private Label label2;
        private ComboBox comboBoxFilter;
    }
}