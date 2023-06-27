namespace FireWallet
{
    partial class MultisigSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultisigSettingsForm));
            groupBoxSigners = new GroupBox();
            panelSigners = new Panel();
            labelSigners = new Label();
            groupBoxAddSig = new GroupBox();
            buttoAddSigner = new Button();
            textBoxAddSig = new TextBox();
            labelAddXpub = new Label();
            groupBoxSigners.SuspendLayout();
            groupBoxAddSig.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxSigners
            // 
            groupBoxSigners.Controls.Add(panelSigners);
            groupBoxSigners.Location = new Point(0, 0);
            groupBoxSigners.Name = "groupBoxSigners";
            groupBoxSigners.Size = new Size(418, 463);
            groupBoxSigners.TabIndex = 0;
            groupBoxSigners.TabStop = false;
            groupBoxSigners.Text = "Signers";
            // 
            // panelSigners
            // 
            panelSigners.Dock = DockStyle.Fill;
            panelSigners.Location = new Point(3, 19);
            panelSigners.Name = "panelSigners";
            panelSigners.Size = new Size(412, 441);
            panelSigners.TabIndex = 0;
            // 
            // labelSigners
            // 
            labelSigners.AutoSize = true;
            labelSigners.Location = new Point(424, 9);
            labelSigners.Name = "labelSigners";
            labelSigners.Size = new Size(48, 15);
            labelSigners.TabIndex = 1;
            labelSigners.Text = "Signers:";
            // 
            // groupBoxAddSig
            // 
            groupBoxAddSig.Controls.Add(buttoAddSigner);
            groupBoxAddSig.Controls.Add(textBoxAddSig);
            groupBoxAddSig.Controls.Add(labelAddXpub);
            groupBoxAddSig.Location = new Point(421, 255);
            groupBoxAddSig.Name = "groupBoxAddSig";
            groupBoxAddSig.Size = new Size(381, 208);
            groupBoxAddSig.TabIndex = 2;
            groupBoxAddSig.TabStop = false;
            groupBoxAddSig.Text = "Add Signer";
            // 
            // buttoAddSigner
            // 
            buttoAddSigner.FlatStyle = FlatStyle.Flat;
            buttoAddSigner.Location = new Point(6, 45);
            buttoAddSigner.Name = "buttoAddSigner";
            buttoAddSigner.Size = new Size(75, 23);
            buttoAddSigner.TabIndex = 2;
            buttoAddSigner.Text = "Add";
            buttoAddSigner.UseVisualStyleBackColor = true;
            buttoAddSigner.Click += buttoAddSigner_Click;
            // 
            // textBoxAddSig
            // 
            textBoxAddSig.Location = new Point(51, 16);
            textBoxAddSig.Name = "textBoxAddSig";
            textBoxAddSig.Size = new Size(324, 23);
            textBoxAddSig.TabIndex = 1;
            textBoxAddSig.KeyDown += textBoxAddSig_KeyDown;
            // 
            // labelAddXpub
            // 
            labelAddXpub.AutoSize = true;
            labelAddXpub.Location = new Point(6, 19);
            labelAddXpub.Name = "labelAddXpub";
            labelAddXpub.Size = new Size(39, 15);
            labelAddXpub.TabIndex = 0;
            labelAddXpub.Text = "XPUB:";
            // 
            // MultisigSettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(814, 475);
            Controls.Add(groupBoxAddSig);
            Controls.Add(labelSigners);
            Controls.Add(groupBoxSigners);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MultisigSettingsForm";
            Text = "Multisig Settings";
            Load += MultisigSettingsForm_Load;
            groupBoxSigners.ResumeLayout(false);
            groupBoxAddSig.ResumeLayout(false);
            groupBoxAddSig.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBoxSigners;
        private Label labelSigners;
        private Panel panelSigners;
        private GroupBox groupBoxAddSig;
        private Button buttoAddSigner;
        private TextBox textBoxAddSig;
        private Label labelAddXpub;
    }
}