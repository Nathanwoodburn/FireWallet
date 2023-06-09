using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace FireWallet
{
    public partial class TransferForm : Form
    {
        MainForm MainForm;
        string Domain;
        public TransferForm(MainForm main, string domain)
        {
            InitializeComponent();
            MainForm = main;
            Domain = domain;
            this.Text = "Transfer " + Domain + " | FireWallet";
            label1.Text = "Transfer " + Domain;
            if (MainForm.theme.ContainsKey("error"))
            {
                labelError.ForeColor = ColorTranslator.FromHtml(MainForm.theme["error"]);
            }
            if (MainForm.watchOnly)
            {
                buttonTransfer.Enabled = false; // watch only wallet only batch
            }

            // Theme
            this.BackColor = ColorTranslator.FromHtml(MainForm.theme["background"]);
            this.ForeColor = ColorTranslator.FromHtml(MainForm.theme["foreground"]);
            foreach (Control c in Controls)
            {
                MainForm.ThemeControl(c);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void buttonTransfer_Click(object sender, EventArgs e)
        {
            if (!await MainForm.ValidAddress(textBoxAddress.Text))
            {
                labelError.Show();
                return;
            }

            string content = "{\"method\": \"sendtransfer\",\"params\": [ \"" + Domain + "\", \"" +
    textBoxAddress.Text + "\"]}";
            string output = await MainForm.APIPost("", true, content);
            JObject APIresp = JObject.Parse(output);
            if (APIresp["error"].ToString() != "")
            {
                MainForm.AddLog("Failed:");
                MainForm.AddLog(APIresp.ToString());
                NotifyForm notify = new NotifyForm("Error Transaction Failed");
                notify.ShowDialog();
                return;
            }
            JObject result = JObject.Parse(APIresp["result"].ToString());
            string hash = result["hash"].ToString();
            string link = MainForm.userSettings["explorer-tx"] + hash;
            NotifyForm notifySuccess = new NotifyForm("Transaction Sent\nThis transaction could take up to 20 minutes to mine",
                "Explorer", link);
            notifySuccess.ShowDialog();
            notifySuccess.Dispose();
            this.Close();
        }

        private async void buttonBatch_Click(object sender, EventArgs e)
        {
            if (!await MainForm.ValidAddress(textBoxAddress.Text))
            {
                labelError.Show();
                return;
            }

            MainForm.AddBatch(Domain, "TRANSFER", textBoxAddress.Text);
            this.Close();
        }
    }
}
