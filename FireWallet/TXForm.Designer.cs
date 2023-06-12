namespace FireWallet
{
    partial class TXForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TXForm));
            labelHash = new Label();
            groupBoxInputs = new GroupBox();
            groupBoxOutputs = new GroupBox();
            buttonExplorer = new Button();
            panelInputs = new Panel();
            panelOutputs = new Panel();
            groupBoxInputs.SuspendLayout();
            groupBoxOutputs.SuspendLayout();
            SuspendLayout();
            // 
            // labelHash
            // 
            labelHash.AutoSize = true;
            labelHash.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelHash.Location = new Point(12, 9);
            labelHash.Name = "labelHash";
            labelHash.Size = new Size(48, 21);
            labelHash.TabIndex = 0;
            labelHash.Text = "Hash:";
            // 
            // groupBoxInputs
            // 
            groupBoxInputs.Controls.Add(panelInputs);
            groupBoxInputs.Location = new Point(12, 60);
            groupBoxInputs.Name = "groupBoxInputs";
            groupBoxInputs.Size = new Size(277, 378);
            groupBoxInputs.TabIndex = 1;
            groupBoxInputs.TabStop = false;
            groupBoxInputs.Text = "Inputs";
            // 
            // groupBoxOutputs
            // 
            groupBoxOutputs.Controls.Add(panelOutputs);
            groupBoxOutputs.Location = new Point(295, 60);
            groupBoxOutputs.Name = "groupBoxOutputs";
            groupBoxOutputs.Size = new Size(493, 378);
            groupBoxOutputs.TabIndex = 0;
            groupBoxOutputs.TabStop = false;
            groupBoxOutputs.Text = "Outputs";
            // 
            // buttonExplorer
            // 
            buttonExplorer.FlatStyle = FlatStyle.Flat;
            buttonExplorer.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonExplorer.Location = new Point(699, 12);
            buttonExplorer.Name = "buttonExplorer";
            buttonExplorer.Size = new Size(89, 37);
            buttonExplorer.TabIndex = 0;
            buttonExplorer.Text = "Explorer";
            buttonExplorer.UseVisualStyleBackColor = true;
            buttonExplorer.Click += Explorer_Click;
            // 
            // panelInputs
            // 
            panelInputs.AutoScroll = true;
            panelInputs.Dock = DockStyle.Fill;
            panelInputs.Location = new Point(3, 19);
            panelInputs.Name = "panelInputs";
            panelInputs.Size = new Size(271, 356);
            panelInputs.TabIndex = 0;
            // 
            // panelOutputs
            // 
            panelOutputs.AutoScroll = true;
            panelOutputs.Dock = DockStyle.Fill;
            panelOutputs.Location = new Point(3, 19);
            panelOutputs.Name = "panelOutputs";
            panelOutputs.Size = new Size(487, 356);
            panelOutputs.TabIndex = 0;
            // 
            // TXForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonExplorer);
            Controls.Add(groupBoxOutputs);
            Controls.Add(groupBoxInputs);
            Controls.Add(labelHash);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "TXForm";
            Text = "TXForm";
            Load += TXForm_Load;
            groupBoxInputs.ResumeLayout(false);
            groupBoxOutputs.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelHash;
        private GroupBox groupBoxInputs;
        private GroupBox groupBoxOutputs;
        private Button buttonExplorer;
        private Panel panelInputs;
        private Panel panelOutputs;
    }
}