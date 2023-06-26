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
            groupBoxIn.SuspendLayout();
            groupBoxOut.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxIn
            // 
            groupBoxIn.Controls.Add(panelIn);
            groupBoxIn.Location = new Point(12, 83);
            groupBoxIn.Name = "groupBoxIn";
            groupBoxIn.Size = new Size(341, 355);
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
            panelIn.Size = new Size(335, 333);
            panelIn.TabIndex = 0;
            // 
            // groupBoxOut
            // 
            groupBoxOut.Controls.Add(panelOut);
            groupBoxOut.Location = new Point(359, 83);
            groupBoxOut.Name = "groupBoxOut";
            groupBoxOut.Size = new Size(429, 355);
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
            panelOut.Size = new Size(423, 333);
            panelOut.TabIndex = 0;
            // 
            // buttonSign
            // 
            buttonSign.FlatStyle = FlatStyle.Flat;
            buttonSign.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonSign.Location = new Point(705, 444);
            buttonSign.Name = "buttonSign";
            buttonSign.Size = new Size(83, 36);
            buttonSign.TabIndex = 2;
            buttonSign.Text = "Sign";
            buttonSign.UseVisualStyleBackColor = true;
            // 
            // Cancelbutton2
            // 
            Cancelbutton2.FlatStyle = FlatStyle.Flat;
            Cancelbutton2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Cancelbutton2.Location = new Point(616, 444);
            Cancelbutton2.Name = "Cancelbutton2";
            Cancelbutton2.Size = new Size(83, 36);
            Cancelbutton2.TabIndex = 2;
            Cancelbutton2.Text = "Cancel";
            Cancelbutton2.UseVisualStyleBackColor = true;
            Cancelbutton2.Click += Cancelbutton2_Click;
            // 
            // ImportTXForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 485);
            Controls.Add(groupBoxOut);
            Controls.Add(groupBoxIn);
            Controls.Add(Cancelbutton2);
            Controls.Add(buttonSign);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "ImportTXForm";
            Text = "ImportTXForm";
            Load += ImportTXForm_Load;
            groupBoxIn.ResumeLayout(false);
            groupBoxOut.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private GroupBox groupBoxIn;
        private GroupBox groupBoxOut;
        private Button buttonSign;
        private Button Cancelbutton2;
        private Panel panelIn;
        private Panel panelOut;
    }
}