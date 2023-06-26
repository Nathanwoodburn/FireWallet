namespace FireWallet
{
    partial class ImportTXForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportTXForm));
            groupBoxIn = new GroupBox();
            panelIn = new Panel();
            groupBoxOut = new GroupBox();
            panelOut = new Panel();
            buttonSign = new Button();
            Cancelbutton2 = new Button();
            label1 = new Label();
            labelSigsTotal = new Label();
            labelSigsReq = new Label();
            labelSigsSigned = new Label();
            labelSigInfo = new Label();
            buttonExport = new Button();
            buttonSend = new Button();
            groupBoxIn.SuspendLayout();
            groupBoxOut.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxIn
            // 
            groupBoxIn.Controls.Add(panelIn);
            groupBoxIn.Location = new Point(12, 83);
            groupBoxIn.Name = "groupBoxIn";
            groupBoxIn.Size = new Size(376, 355);
            groupBoxIn.TabIndex = 3;
            groupBoxIn.TabStop = false;
            groupBoxIn.Text = "Inputs";
            // 
            // panelIn
            // 
            panelIn.AutoScroll = true;
            panelIn.Dock = DockStyle.Fill;
            panelIn.Location = new Point(3, 19);
            panelIn.Name = "panelIn";
            panelIn.Size = new Size(370, 333);
            panelIn.TabIndex = 0;
            // 
            // groupBoxOut
            // 
            groupBoxOut.Controls.Add(panelOut);
            groupBoxOut.Location = new Point(391, 80);
            groupBoxOut.Name = "groupBoxOut";
            groupBoxOut.Size = new Size(484, 355);
            groupBoxOut.TabIndex = 0;
            groupBoxOut.TabStop = false;
            groupBoxOut.Text = "Outputs";
            // 
            // panelOut
            // 
            panelOut.AutoScroll = true;
            panelOut.Dock = DockStyle.Fill;
            panelOut.Location = new Point(3, 19);
            panelOut.Name = "panelOut";
            panelOut.Size = new Size(478, 333);
            panelOut.TabIndex = 0;
            // 
            // buttonSign
            // 
            buttonSign.FlatStyle = FlatStyle.Flat;
            buttonSign.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonSign.Location = new Point(700, 444);
            buttonSign.Name = "buttonSign";
            buttonSign.Size = new Size(83, 36);
            buttonSign.TabIndex = 2;
            buttonSign.Text = "Sign";
            buttonSign.UseVisualStyleBackColor = true;
            buttonSign.Click += buttonSign_Click;
            // 
            // Cancelbutton2
            // 
            Cancelbutton2.FlatStyle = FlatStyle.Flat;
            Cancelbutton2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Cancelbutton2.Location = new Point(12, 441);
            Cancelbutton2.Name = "Cancelbutton2";
            Cancelbutton2.Size = new Size(83, 36);
            Cancelbutton2.TabIndex = 2;
            Cancelbutton2.Text = "Cancel";
            Cancelbutton2.UseVisualStyleBackColor = true;
            Cancelbutton2.Click += Cancel;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(87, 21);
            label1.TabIndex = 4;
            label1.Text = "Signatures:";
            // 
            // labelSigsTotal
            // 
            labelSigsTotal.BackColor = Color.Blue;
            labelSigsTotal.Location = new Point(105, 7);
            labelSigsTotal.Name = "labelSigsTotal";
            labelSigsTotal.Size = new Size(250, 32);
            labelSigsTotal.TabIndex = 5;
            // 
            // labelSigsReq
            // 
            labelSigsReq.BackColor = Color.FromArgb(255, 128, 0);
            labelSigsReq.Location = new Point(105, 7);
            labelSigsReq.Name = "labelSigsReq";
            labelSigsReq.Size = new Size(191, 32);
            labelSigsReq.TabIndex = 6;
            // 
            // labelSigsSigned
            // 
            labelSigsSigned.BackColor = Color.Lime;
            labelSigsSigned.Location = new Point(105, 7);
            labelSigsSigned.Name = "labelSigsSigned";
            labelSigsSigned.Size = new Size(100, 32);
            labelSigsSigned.TabIndex = 7;
            // 
            // labelSigInfo
            // 
            labelSigInfo.AutoSize = true;
            labelSigInfo.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelSigInfo.Location = new Point(361, 7);
            labelSigInfo.Name = "labelSigInfo";
            labelSigInfo.Size = new Size(19, 21);
            labelSigInfo.TabIndex = 8;
            labelSigInfo.Text = "#";
            // 
            // buttonExport
            // 
            buttonExport.Enabled = false;
            buttonExport.FlatStyle = FlatStyle.Flat;
            buttonExport.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonExport.Location = new Point(611, 444);
            buttonExport.Name = "buttonExport";
            buttonExport.Size = new Size(83, 36);
            buttonExport.TabIndex = 2;
            buttonExport.Text = "Export";
            buttonExport.UseVisualStyleBackColor = true;
            buttonExport.Click += buttonExport_Click;
            // 
            // buttonSend
            // 
            buttonSend.FlatStyle = FlatStyle.Flat;
            buttonSend.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonSend.Location = new Point(789, 444);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(83, 36);
            buttonSend.TabIndex = 2;
            buttonSend.Text = "Send";
            buttonSend.UseVisualStyleBackColor = true;
            buttonSend.Click += buttonSend_Click;
            // 
            // ImportTXForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(887, 485);
            Controls.Add(labelSigInfo);
            Controls.Add(labelSigsSigned);
            Controls.Add(labelSigsReq);
            Controls.Add(labelSigsTotal);
            Controls.Add(label1);
            Controls.Add(groupBoxOut);
            Controls.Add(groupBoxIn);
            Controls.Add(buttonSend);
            Controls.Add(buttonExport);
            Controls.Add(Cancelbutton2);
            Controls.Add(buttonSign);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "ImportTXForm";
            Text = "Import TX";
            Load += ImportTXForm_Load;
            groupBoxIn.ResumeLayout(false);
            groupBoxOut.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private GroupBox groupBoxIn;
        private GroupBox groupBoxOut;
        private Button buttonSign;
        private Button Cancelbutton2;
        private Panel panelIn;
        private Panel panelOut;
        private Label label1;
        private Label labelSigsTotal;
        private Label labelSigsReq;
        private Label labelSigsSigned;
        private Label labelSigInfo;
        private Button buttonExport;
        private Button buttonSend;
    }
}