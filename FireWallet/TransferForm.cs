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

        private void updateAddress()
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

                try
                {
                    IPAddress iPAddress = null;


                    // Create an instance of LookupClient using the custom options
                    NameServer nameServer = new NameServer(IPAddress.Parse("127.0.0.1"), 5350);
                    var options = new LookupClientOptions(nameServer);
                    options.EnableAuditTrail = true;
                    options.UseTcpOnly = true;
                    options.Recursion = true;
                    options.UseCache = false;
                    options.RequestDnsSecRecords = true;
                    options.Timeout = TimeSpan.FromSeconds(5);


                    var client = new LookupClient(options);


                    // Perform the DNS lookup for the specified domain using DNSSec

                    var result = client.Query(domain, QueryType.A);





                    // Display the DNS lookup results
                    foreach (var record in result.Answers.OfType<ARecord>())
                    {
                        iPAddress = record.Address;
                    }

                    if (iPAddress == null)
                    {
                        labelError.Show();
                        labelError.Text = "HIP-02 lookup failed";
                        return;
                    }

                    // Get TLSA record
                    var resultTLSA = client.Query("_443._tcp." + domain, QueryType.TLSA);
                    foreach (var record in resultTLSA.Answers.OfType<TlsaRecord>())
                    {
                        MainForm.TLSA = record.CertificateAssociationDataAsString;
                    }



                    string url = "https://" + iPAddress.ToString() + "/.well-known/wallets/HNS";
                    var handler = new HttpClientHandler();

                    handler.ServerCertificateCustomValidationCallback = MainForm.ValidateServerCertificate;

                    // Create an instance of HttpClient with the custom handler
                    using (var httpclient = new HttpClient(handler))
                    {
                        httpclient.DefaultRequestHeaders.Add("Host", domain);
                        // Send a GET request to the specified URL
                        HttpResponseMessage response = httpclient.GetAsync(url).Result;

                        // Response
                        string address = response.Content.ReadAsStringAsync().Result;

                        labelSendingHIPAddress.Text = address;
                        this.address = address;
                        labelSendingHIPAddress.Show();
                        labelHIPArrow.Show();
                    }

                }
                catch (Exception ex)
                {
                    MainForm.AddLog(ex.Message);
                    labelError.Show();
                    labelError.Text = "HIP-02 lookup failed";
                }
            } else
            {
                address = textBoxAddress.Text;
            }

        }
    }
}
