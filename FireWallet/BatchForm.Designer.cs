namespace FireWallet
{
    partial class BatchForm
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
            buttonCancel = new Button();
            buttonSend = new Button();
            groupBoxTransactions = new GroupBox();
            panelTXs = new Panel();
            buttonImport = new Button();
            buttonExport = new Button();
            groupBoxTransactions.SuspendLayout();
            SuspendLayout();
            // 
            // buttonCancel
            // 
            buttonCancel.FlatStyle = FlatStyle.Flat;
            buttonCancel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonCancel.Location = new System.Drawing.Point(707, 401);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new System.Drawing.Size(81, 37);
            buttonCancel.TabIndex = 0;
            buttonCancel.TabStop = false;
            buttonCancel.Text = "Cancel Batch";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonSend
            // 
            buttonSend.FlatStyle = FlatStyle.Flat;
            buttonSend.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonSend.Location = new System.Drawing.Point(620, 401);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new System.Drawing.Size(81, 37);
            buttonSend.TabIndex = 1;
            buttonSend.TabStop = false;
            buttonSend.Text = "Send";
            buttonSend.UseVisualStyleBackColor = true;
            buttonSend.Click += buttonSend_Click;
            // 
            // groupBoxTransactions
            // 
            groupBoxTransactions.Controls.Add(panelTXs);
            groupBoxTransactions.Location = new System.Drawing.Point(0, 0);
            groupBoxTransactions.Name = "groupBoxTransactions";
            groupBoxTransactions.Size = new System.Drawing.Size(614, 438);
            groupBoxTransactions.TabIndex = 2;
            groupBoxTransactions.TabStop = false;
            groupBoxTransactions.Text = "Transactions";
            // 
            // panelTXs
            // 
            panelTXs.AutoScroll = true;
            panelTXs.Dock = DockStyle.Fill;
            panelTXs.Location = new System.Drawing.Point(3, 19);
            panelTXs.Name = "panelTXs";
            panelTXs.Size = new System.Drawing.Size(608, 416);
            panelTXs.TabIndex = 3;
            // 
            // buttonImport
            // 
            buttonImport.FlatStyle = FlatStyle.Flat;
            buttonImport.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonImport.Location = new System.Drawing.Point(620, 19);
            buttonImport.Name = "buttonImport";
            buttonImport.Size = new System.Drawing.Size(81, 37);
            buttonImport.TabIndex = 3;
            buttonImport.TabStop = false;
            buttonImport.Text = "Import";
            buttonImport.UseVisualStyleBackColor = true;
            buttonImport.Click += buttonImport_Click;
            // 
            // buttonExport
            // 
            buttonExport.FlatStyle = FlatStyle.Flat;
            buttonExport.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonExport.Location = new System.Drawing.Point(707, 19);
            buttonExport.Name = "buttonExport";
            buttonExport.Size = new System.Drawing.Size(81, 37);
            buttonExport.TabIndex = 4;
            buttonExport.TabStop = false;
            buttonExport.Text = "Export";
            buttonExport.UseVisualStyleBackColor = true;
            buttonExport.Click += buttonExport_Click;
            // 
            // BatchForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(buttonExport);
            Controls.Add(buttonImport);
            Controls.Add(groupBoxTransactions);
            Controls.Add(buttonSend);
            Controls.Add(buttonCancel);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            Name = "BatchForm";
            Text = "Batch";
            FormClosing += BatchForm_FormClosing;
            Load += BatchForm_Load;
            groupBoxTransactions.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button buttonCancel;
        private Button buttonSend;
        private GroupBox groupBoxTransactions;
        private Panel panelTXs;
        private Button buttonImport;
        private Button buttonExport;
    }
}