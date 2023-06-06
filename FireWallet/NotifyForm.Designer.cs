namespace FireWallet
{
    partial class NotifyForm
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
            labelmessage = new Label();
            button1 = new Button();
            SuspendLayout();
            // 
            // labelmessage
            // 
            labelmessage.Dock = DockStyle.Fill;
            labelmessage.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            labelmessage.Location = new Point(0, 0);
            labelmessage.Name = "labelmessage";
            labelmessage.Size = new Size(382, 170);
            labelmessage.TabIndex = 0;
            labelmessage.Text = "Message";
            labelmessage.TextAlign = ContentAlignment.TopCenter;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            button1.Location = new Point(271, 120);
            button1.Name = "button1";
            button1.Size = new Size(99, 38);
            button1.TabIndex = 1;
            button1.Text = "Ok";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // NotifyForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(382, 170);
            Controls.Add(button1);
            Controls.Add(labelmessage);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "NotifyForm";
            Text = "FireWallet";
            Load += NotifyForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private Label labelmessage;
        private Button button1;
    }
}