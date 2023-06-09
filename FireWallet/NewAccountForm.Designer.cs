namespace FireWallet
{
    partial class NewAccountForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewAccountForm));
            buttonNext = new Button();
            buttonCancel = new Button();
            groupBoxMode = new GroupBox();
            label1 = new Label();
            buttonCold = new Button();
            buttonImport = new Button();
            buttonNew = new Button();
            groupBoxNew = new GroupBox();
            textBoxNewPass2 = new TextBox();
            textBoxNewPass1 = new TextBox();
            label4 = new Label();
            textBoxNewName = new TextBox();
            label5 = new Label();
            label3 = new Label();
            label2 = new Label();
            groupBoxSeed = new GroupBox();
            textBoxSeedPhrase = new TextBox();
            groupBoxMode.SuspendLayout();
            groupBoxNew.SuspendLayout();
            groupBoxSeed.SuspendLayout();
            SuspendLayout();
            // 
            // buttonNext
            // 
            buttonNext.FlatStyle = FlatStyle.Flat;
            buttonNext.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonNext.Location = new Point(576, 372);
            buttonNext.Name = "buttonNext";
            buttonNext.Size = new Size(92, 46);
            buttonNext.TabIndex = 0;
            buttonNext.TabStop = false;
            buttonNext.Text = "Next";
            buttonNext.UseVisualStyleBackColor = true;
            buttonNext.Visible = false;
            buttonNext.Click += buttonNext_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.FlatStyle = FlatStyle.Flat;
            buttonCancel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonCancel.Location = new Point(12, 372);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(92, 46);
            buttonCancel.TabIndex = 0;
            buttonCancel.TabStop = false;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // groupBoxMode
            // 
            groupBoxMode.Controls.Add(label1);
            groupBoxMode.Controls.Add(buttonCold);
            groupBoxMode.Controls.Add(buttonImport);
            groupBoxMode.Controls.Add(buttonNew);
            groupBoxMode.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            groupBoxMode.Location = new Point(125, 22);
            groupBoxMode.Name = "groupBoxMode";
            groupBoxMode.Size = new Size(450, 319);
            groupBoxMode.TabIndex = 1;
            groupBoxMode.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(29, 19);
            label1.Name = "label1";
            label1.Size = new Size(391, 42);
            label1.TabIndex = 3;
            label1.Text = "Welcome!\r\nYou can either create a new wallet or import an old one";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // buttonCold
            // 
            buttonCold.FlatStyle = FlatStyle.Flat;
            buttonCold.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonCold.Location = new Point(147, 239);
            buttonCold.Name = "buttonCold";
            buttonCold.Size = new Size(156, 62);
            buttonCold.TabIndex = 2;
            buttonCold.TabStop = false;
            buttonCold.Text = "Connect Ledger";
            buttonCold.UseVisualStyleBackColor = true;
            buttonCold.Click += buttonCold_Click;
            // 
            // buttonImport
            // 
            buttonImport.FlatStyle = FlatStyle.Flat;
            buttonImport.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonImport.Location = new Point(147, 171);
            buttonImport.Name = "buttonImport";
            buttonImport.Size = new Size(156, 62);
            buttonImport.TabIndex = 1;
            buttonImport.TabStop = false;
            buttonImport.Text = "Import Wallet";
            buttonImport.UseVisualStyleBackColor = true;
            buttonImport.Click += buttonImport_Click;
            // 
            // buttonNew
            // 
            buttonNew.FlatStyle = FlatStyle.Flat;
            buttonNew.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonNew.Location = new Point(147, 103);
            buttonNew.Name = "buttonNew";
            buttonNew.Size = new Size(156, 62);
            buttonNew.TabIndex = 0;
            buttonNew.TabStop = false;
            buttonNew.Text = "Create New Wallet";
            buttonNew.UseVisualStyleBackColor = true;
            buttonNew.Click += buttonNew_Click;
            // 
            // groupBoxNew
            // 
            groupBoxNew.Controls.Add(textBoxNewPass2);
            groupBoxNew.Controls.Add(textBoxNewPass1);
            groupBoxNew.Controls.Add(label4);
            groupBoxNew.Controls.Add(textBoxNewName);
            groupBoxNew.Controls.Add(label5);
            groupBoxNew.Controls.Add(label3);
            groupBoxNew.Controls.Add(label2);
            groupBoxNew.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            groupBoxNew.Location = new Point(125, 22);
            groupBoxNew.Name = "groupBoxNew";
            groupBoxNew.Size = new Size(450, 319);
            groupBoxNew.TabIndex = 4;
            groupBoxNew.TabStop = false;
            groupBoxNew.Text = "New Wallet";
            groupBoxNew.Visible = false;
            // 
            // textBoxNewPass2
            // 
            textBoxNewPass2.Location = new Point(128, 136);
            textBoxNewPass2.Name = "textBoxNewPass2";
            textBoxNewPass2.Size = new Size(316, 29);
            textBoxNewPass2.TabIndex = 8;
            textBoxNewPass2.UseSystemPasswordChar = true;
            // 
            // textBoxNewPass1
            // 
            textBoxNewPass1.Location = new Point(128, 90);
            textBoxNewPass1.Name = "textBoxNewPass1";
            textBoxNewPass1.Size = new Size(316, 29);
            textBoxNewPass1.TabIndex = 7;
            textBoxNewPass1.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(128, 57);
            label4.Name = "label4";
            label4.Size = new Size(195, 15);
            label4.TabIndex = 6;
            label4.Text = "Only lowercase letters and numbers";
            // 
            // textBoxNewName
            // 
            textBoxNewName.Location = new Point(128, 25);
            textBoxNewName.Name = "textBoxNewName";
            textBoxNewName.Size = new Size(316, 29);
            textBoxNewName.TabIndex = 5;
            textBoxNewName.TextChanged += textBoxNewName_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(38, 139);
            label5.Name = "label5";
            label5.Size = new Size(70, 21);
            label5.TabIndex = 1;
            label5.Text = "Confirm:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(29, 93);
            label3.Name = "label3";
            label3.Size = new Size(79, 21);
            label3.TabIndex = 1;
            label3.Text = "Password:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(53, 28);
            label2.Name = "label2";
            label2.Size = new Size(55, 21);
            label2.TabIndex = 0;
            label2.Text = "Name:";
            // 
            // groupBoxSeed
            // 
            groupBoxSeed.Controls.Add(textBoxSeedPhrase);
            groupBoxSeed.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            groupBoxSeed.Location = new Point(125, 22);
            groupBoxSeed.Name = "groupBoxSeed";
            groupBoxSeed.Size = new Size(450, 319);
            groupBoxSeed.TabIndex = 5;
            groupBoxSeed.TabStop = false;
            groupBoxSeed.Text = "Seed Phrase";
            groupBoxSeed.Visible = false;
            // 
            // textBoxSeedPhrase
            // 
            textBoxSeedPhrase.Location = new Point(6, 25);
            textBoxSeedPhrase.Multiline = true;
            textBoxSeedPhrase.Name = "textBoxSeedPhrase";
            textBoxSeedPhrase.PlaceholderText = "pistol air cabbage high conduct party powder inject jungle knee spell derive";
            textBoxSeedPhrase.Size = new Size(438, 288);
            textBoxSeedPhrase.TabIndex = 0;
            // 
            // NewAccountForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(680, 430);
            Controls.Add(groupBoxSeed);
            Controls.Add(buttonCancel);
            Controls.Add(buttonNext);
            Controls.Add(groupBoxNew);
            Controls.Add(groupBoxMode);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "NewAccountForm";
            Text = "New Account | FireWallet";
            Load += NewAccountForm_Load;
            groupBoxMode.ResumeLayout(false);
            groupBoxMode.PerformLayout();
            groupBoxNew.ResumeLayout(false);
            groupBoxNew.PerformLayout();
            groupBoxSeed.ResumeLayout(false);
            groupBoxSeed.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button buttonNext;
        private Button buttonCancel;
        private GroupBox groupBoxMode;
        private Label label1;
        private Button buttonCold;
        private Button buttonImport;
        private Button buttonNew;
        private GroupBox groupBoxNew;
        private Label label3;
        private Label label2;
        private TextBox textBoxNewName;
        private Label label4;
        private TextBox textBoxNewPass2;
        private TextBox textBoxNewPass1;
        private Label label5;
        private GroupBox groupBoxSeed;
        private TextBox textBoxSeedPhrase;
    }
}