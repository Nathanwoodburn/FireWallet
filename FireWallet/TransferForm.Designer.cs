namespace FireWallet
{
    partial class TransferForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransferForm));
            buttonTransfer = new Button();
            buttonBatch = new Button();
            buttonCancel = new Button();
            label1 = new Label();
            label2 = new Label();
            textBoxAddress = new TextBox();
            labelError = new Label();
            labelSendingHIPAddress = new Label();
            labelHIPArrow = new Label();
            SuspendLayout();
            // 
            // buttonTransfer
            // 
            buttonTransfer.FlatStyle = FlatStyle.Flat;
            buttonTransfer.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonTransfer.Location = new Point(451, 285);
            buttonTransfer.Name = "buttonTransfer";
            buttonTransfer.Size = new Size(139, 43);
            buttonTransfer.TabIndex = 0;
            buttonTransfer.Text = "Transfer";
            buttonTransfer.UseVisualStyleBackColor = true;
            buttonTransfer.Click += buttonTransfer_Click;
            // 
            // buttonBatch
            // 
            buttonBatch.FlatStyle = FlatStyle.Flat;
            buttonBatch.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonBatch.Location = new Point(271, 285);
            buttonBatch.Name = "buttonBatch";
            buttonBatch.Size = new Size(139, 43);
            buttonBatch.TabIndex = 1;
            buttonBatch.Text = "Transfer in Batch";
            buttonBatch.UseVisualStyleBackColor = true;
            buttonBatch.Click += buttonBatch_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.FlatStyle = FlatStyle.Flat;
            buttonCancel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonCancel.Location = new Point(12, 285);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(95, 43);
            buttonCancel.TabIndex = 2;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(133, 21);
            label1.TabIndex = 3;
            label1.Text = "Transfer {domain}";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(12, 110);
            label2.Name = "label2";
            label2.Size = new Size(28, 21);
            label2.TabIndex = 4;
            label2.Text = "To:";
            // 
            // textBoxAddress
            // 
            textBoxAddress.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            textBoxAddress.Location = new Point(46, 107);
            textBoxAddress.Name = "textBoxAddress";
            textBoxAddress.Size = new Size(545, 29);
            textBoxAddress.TabIndex = 5;
            // 
            // labelError
            // 
            labelError.AutoSize = true;
            labelError.Location = new Point(492, 139);
            labelError.Name = "labelError";
            labelError.Size = new Size(98, 15);
            labelError.TabIndex = 6;
            labelError.Text = "Address not valid";
            labelError.Visible = false;
            // 
            // labelSendingHIPAddress
            // 
            labelSendingHIPAddress.AutoSize = true;
            labelSendingHIPAddress.Location = new Point(75, 150);
            labelSendingHIPAddress.Name = "labelSendingHIPAddress";
            labelSendingHIPAddress.Size = new Size(64, 15);
            labelSendingHIPAddress.TabIndex = 19;
            labelSendingHIPAddress.Text = "To Address";
            labelSendingHIPAddress.Visible = false;
            // 
            // labelHIPArrow
            // 
            labelHIPArrow.AutoSize = true;
            labelHIPArrow.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            labelHIPArrow.Location = new Point(46, 139);
            labelHIPArrow.Name = "labelHIPArrow";
            labelHIPArrow.Size = new Size(32, 32);
            labelHIPArrow.TabIndex = 20;
            labelHIPArrow.Text = "⮡ ";
            labelHIPArrow.Visible = false;
            // 
            // TransferForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(602, 340);
            Controls.Add(labelSendingHIPAddress);
            Controls.Add(labelHIPArrow);
            Controls.Add(labelError);
            Controls.Add(textBoxAddress);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(buttonCancel);
            Controls.Add(buttonBatch);
            Controls.Add(buttonTransfer);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "TransferForm";
            Text = "Transfer | FireWallet";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonTransfer;
        private Button buttonBatch;
        private Button buttonCancel;
        private Label label1;
        private Label label2;
        private TextBox textBoxAddress;
        private Label labelError;
        private Label labelSendingHIPAddress;
        private Label labelHIPArrow;
    }
}