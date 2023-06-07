using Color = System.Drawing.Color;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using SizeF = System.Drawing.SizeF;

namespace FireWallet
{
    partial class CreateForm
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
            labelWelcome = new Label();
            groupBoxNode = new GroupBox();
            labelNodeStatus = new Label();
            comboBoxNodeNetwork = new ComboBox();
            buttonSave = new Button();
            buttonNodeTest = new Button();
            textBoxNodeKey = new TextBox();
            textBoxNodeIP = new TextBox();
            labelNodeKey = new Label();
            labelNodeNetowrk = new Label();
            labelNodeIP = new Label();
            groupBoxNode.SuspendLayout();
            SuspendLayout();
            // 
            // labelWelcome
            // 
            labelWelcome.AutoSize = true;
            labelWelcome.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            labelWelcome.Location = new Point(12, 9);
            labelWelcome.Name = "labelWelcome";
            labelWelcome.Size = new Size(358, 64);
            labelWelcome.TabIndex = 0;
            labelWelcome.Text = "Welcome to Fire Wallet\r\nPlease add your node settings";
            labelWelcome.TextAlign = ContentAlignment.TopCenter;
            // 
            // groupBoxNode
            // 
            groupBoxNode.Controls.Add(labelNodeStatus);
            groupBoxNode.Controls.Add(comboBoxNodeNetwork);
            groupBoxNode.Controls.Add(buttonSave);
            groupBoxNode.Controls.Add(buttonNodeTest);
            groupBoxNode.Controls.Add(textBoxNodeKey);
            groupBoxNode.Controls.Add(textBoxNodeIP);
            groupBoxNode.Controls.Add(labelNodeKey);
            groupBoxNode.Controls.Add(labelNodeNetowrk);
            groupBoxNode.Controls.Add(labelNodeIP);
            groupBoxNode.Location = new Point(12, 92);
            groupBoxNode.Name = "groupBoxNode";
            groupBoxNode.Size = new Size(402, 163);
            groupBoxNode.TabIndex = 1;
            groupBoxNode.TabStop = false;
            groupBoxNode.Text = "Node";
            // 
            // labelNodeStatus
            // 
            labelNodeStatus.AutoSize = true;
            labelNodeStatus.Location = new Point(87, 138);
            labelNodeStatus.Name = "labelNodeStatus";
            labelNodeStatus.Size = new Size(88, 15);
            labelNodeStatus.TabIndex = 9;
            labelNodeStatus.Text = "Not Connected";
            // 
            // comboBoxNodeNetwork
            // 
            comboBoxNodeNetwork.FlatStyle = FlatStyle.Popup;
            comboBoxNodeNetwork.FormattingEnabled = true;
            comboBoxNodeNetwork.Items.AddRange(new object[] { "Mainnet", "Regtest" });
            comboBoxNodeNetwork.Location = new Point(62, 45);
            comboBoxNodeNetwork.Name = "comboBoxNodeNetwork";
            comboBoxNodeNetwork.Size = new Size(121, 23);
            comboBoxNodeNetwork.TabIndex = 8;
            comboBoxNodeNetwork.Text = "Mainnet";
            // 
            // buttonSave
            // 
            buttonSave.FlatStyle = FlatStyle.Flat;
            buttonSave.Location = new Point(314, 134);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(75, 23);
            buttonSave.TabIndex = 7;
            buttonSave.Text = "Finish";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += SaveSettings;
            // 
            // buttonNodeTest
            // 
            buttonNodeTest.FlatStyle = FlatStyle.Flat;
            buttonNodeTest.Location = new Point(6, 134);
            buttonNodeTest.Name = "buttonNodeTest";
            buttonNodeTest.Size = new Size(75, 23);
            buttonNodeTest.TabIndex = 6;
            buttonNodeTest.Text = "Test";
            buttonNodeTest.UseVisualStyleBackColor = true;
            buttonNodeTest.Click += TestAPI;
            // 
            // textBoxNodeKey
            // 
            textBoxNodeKey.Location = new Point(62, 71);
            textBoxNodeKey.Name = "textBoxNodeKey";
            textBoxNodeKey.Size = new Size(327, 23);
            textBoxNodeKey.TabIndex = 5;
            // 
            // textBoxNodeIP
            // 
            textBoxNodeIP.Location = new Point(62, 16);
            textBoxNodeIP.Name = "textBoxNodeIP";
            textBoxNodeIP.Size = new Size(327, 23);
            textBoxNodeIP.TabIndex = 3;
            textBoxNodeIP.Text = "127.0.0.1";
            // 
            // labelNodeKey
            // 
            labelNodeKey.AutoSize = true;
            labelNodeKey.Location = new Point(6, 74);
            labelNodeKey.Name = "labelNodeKey";
            labelNodeKey.Size = new Size(50, 15);
            labelNodeKey.TabIndex = 2;
            labelNodeKey.Text = "API Key:";
            // 
            // labelNodeNetowrk
            // 
            labelNodeNetowrk.AutoSize = true;
            labelNodeNetowrk.Location = new Point(6, 48);
            labelNodeNetowrk.Name = "labelNodeNetowrk";
            labelNodeNetowrk.Size = new Size(32, 15);
            labelNodeNetowrk.TabIndex = 1;
            labelNodeNetowrk.Text = "Port:";
            // 
            // labelNodeIP
            // 
            labelNodeIP.AutoSize = true;
            labelNodeIP.Location = new Point(6, 19);
            labelNodeIP.Name = "labelNodeIP";
            labelNodeIP.Size = new Size(20, 15);
            labelNodeIP.TabIndex = 0;
            labelNodeIP.Text = "IP:";
            // 
            // CreateForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(438, 281);
            Controls.Add(groupBoxNode);
            Controls.Add(labelWelcome);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Name = "CreateForm";
            Text = "Setup";
            Load += CreateForm_Load;
            Resize += CreateForm_Resize;
            groupBoxNode.ResumeLayout(false);
            groupBoxNode.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelWelcome;
        private GroupBox groupBoxNode;
        private TextBox textBoxNodeKey;
        private TextBox textBoxNodeIP;
        private Label labelNodeKey;
        private Label labelNodeNetowrk;
        private Label labelNodeIP;
        private Button buttonNodeTest;
        private Button buttonSave;
        private ComboBox comboBoxNodeNetwork;
        private Label labelNodeStatus;
    }
}