using Color = System.Drawing.Color;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using SizeF = System.Drawing.SizeF;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotifyForm));
            labelmessage = new Label();
            buttonOK = new Button();
            buttonALT = new Button();
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
            // buttonOK
            // 
            buttonOK.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            buttonOK.Location = new Point(271, 120);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(99, 38);
            buttonOK.TabIndex = 0;
            buttonOK.Text = "Ok";
            buttonOK.UseVisualStyleBackColor = true;
            buttonOK.Click += OK_Click;
            // 
            // buttonALT
            // 
            buttonALT.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            buttonALT.Location = new Point(12, 120);
            buttonALT.Name = "buttonALT";
            buttonALT.Size = new Size(99, 38);
            buttonALT.TabIndex = 2;
            buttonALT.Text = "ALT";
            buttonALT.UseVisualStyleBackColor = true;
            buttonALT.Visible = false;
            buttonALT.Click += buttonALT_Click;
            // 
            // NotifyForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(382, 170);
            Controls.Add(buttonALT);
            Controls.Add(buttonOK);
            Controls.Add(labelmessage);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "NotifyForm";
            Text = "FireWallet";
            FormClosing += NotifyForm_FormClosing;
            Load += NotifyForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private Label labelmessage;
        private Button buttonOK;
        private Button buttonALT;
    }
}