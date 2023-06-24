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
using DnsClient.Protocol;
using DnsClient;
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
            if (MainForm.Theme.ContainsKey("error"))
            {
                labelError.ForeColor = ColorTranslator.FromHtml(MainForm.Theme["error"]);
            }
            if (MainForm.WatchOnly)
            {
                buttonTransfer.Enabled = false; // watch only wallet only batch
            }

            // Theme
            this.BackColor = ColorTranslator.FromHtml(MainForm.Theme["background"]);
            this.ForeColor = ColorTranslator.FromHtml(MainForm.Theme["foreground"]);
            foreach (Control c in Controls)
            {
                MainForm.ThemeControl(c);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        string address = "";
        private async void buttonTransfer_Click(object sender, EventArgs e)
        {
            updateAddress();

            if (!await MainForm.ValidAddress(address))
            {
                labelError.Show();
                return;
            }

            string content = "{\"method\": \"sendtransfer\",\"params\": [ \"" + Domain + "\", \"" +
    address + "\"]}";
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
            string link = MainForm.UserSettings["explorer-tx"] + hash;
            NotifyForm notifySuccess = new NotifyForm("Transaction Sent\nThis transaction could take up to 20 minutes to mine",
                "Explorer", link);
            notifySuccess.ShowDialog();
            notifySuccess.Dispose();
            this.Close();
        }

        private async void buttonBatch_Click(object sender, EventArgs e)
        {
            updateAddress();

            if (!await MainForm.ValidAddress(address))
            {
                labelError.Show();
                return;
            }

            MainForm.AddBatch(Domain, "TRANSFER", address);
            this.Close();
        }

        private async void updateAddress()
        {
            labelError.Hide();

            if (textBoxAddress.Text.Length < 1)
            {
                address = "";
                return;
            }
            if (textBoxAddress.Text.Substring(0, 1) == "@")
            {
                string domain = textBoxAddress.Text.Substring(1);

                string address = await MainForm.HIP02Lookup(domain);
                if (address == "ERROR")
                {
                    labelError.Show();
                    labelError.Text = "HIP-02 lookup failed";
                } else
                {
                    labelSendingHIPAddress.Text = address;
                    this.address = address;
                    labelSendingHIPAddress.Show();
                    labelHIPArrow.Show();
                }
            } else
            {
                address = textBoxAddress.Text;
            }

        }
    }
}
