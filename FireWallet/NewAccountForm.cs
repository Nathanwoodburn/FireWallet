using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;

namespace FireWallet
{
    public partial class NewAccountForm : Form
    {
        MainForm mainForm;
        private int page;
        public NewAccountForm(MainForm main)
        {
            InitializeComponent();
            mainForm = main;
        }
        private void NewAccountForm_Load(object sender, EventArgs e)
        {
            page = 0;
            Dictionary<string, string> theme = mainForm.theme;
            this.BackColor = ColorTranslator.FromHtml(theme["background"]);
            this.ForeColor = ColorTranslator.FromHtml(theme["foreground"]);
            foreach (Control c in Controls)
            {
                mainForm.ThemeControl(c);
            }


        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            page = 1;
            groupBoxNew.Show();
            buttonNext.Show();
            buttonNext.Enabled = false;
            buttonNext.Text = "Create";
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            page = 2;
            groupBoxNew.Show();
            buttonNext.Show();
            buttonNext.Enabled = false;
            groupBoxNew.Text = "Import Wallet";
        }

        private async void buttonCold_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(mainForm.dir + "hsd-ledger"))
            {
                if (mainForm.CheckNodeInstalled() == false)
                {
                    mainForm.AddLog("Node not installed");
                    NotifyForm notify1 = new NotifyForm("Node not installed\nPlease install it to use Ledger");
                    notify1.ShowDialog();
                    notify1.Dispose();
                    return;
                }
                mainForm.AddLog("Installing hsd-ledger");

                // Try to install hsd-ledger
                try
                {
                    NotifyForm Notifyinstall = new NotifyForm("Installing hsd-ledger\nThis may take a few minutes\nDo not close FireWallet", false);
                    Notifyinstall.Show();
                    // Wait for the notification to show
                    await Task.Delay(1000);

                    string repositoryUrl = "https://github.com/handshake-org/hsd-ledger.git";
                    string destinationPath = mainForm.dir + "hsd-ledger";
                    mainForm.CloneRepository(repositoryUrl, destinationPath);

                    Notifyinstall.CloseNotification();
                    Notifyinstall.Dispose();
                }
                catch (Exception ex)
                {
                    NotifyForm notifyError = new NotifyForm("Error installing hsd-ledger\n" + ex.Message);
                    mainForm.AddLog(ex.Message);
                    notifyError.ShowDialog();
                    notifyError.Dispose();
                    return;
                }
            }

            // Import HSD Wallet
            groupBoxNew.Show();
            groupBoxNew.Text = "Import Ledger";
            buttonNext.Show();
            page = 4;
        }

        private void textBoxNewName_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(textBoxNewName.Text, @"^[a-z0-9]+$"))
            {
                textBoxNewName.BackColor = Color.LightGreen;
                buttonNext.Enabled = true;
            }
            else
            {
                textBoxNewName.BackColor = Color.LightPink;
                buttonNext.Enabled = false;
            }
        }

        private async void buttonNext_Click(object sender, EventArgs e)
        {

            if (textBoxNewPass1 == null)
            {
                NotifyForm notify = new NotifyForm("Please enter a password");
                notify.ShowDialog();
                notify.Dispose();
                return;
            }
            if (textBoxNewPass1.Text.Length < 8)
            {
                NotifyForm notify = new NotifyForm("Password must be at least 8 characters");
                notify.ShowDialog();
                notify.Dispose();
                return;
            }
            if (textBoxNewPass1.Text != textBoxNewPass2.Text)
            {
                NotifyForm notify = new NotifyForm("Passwords do not match");
                notify.ShowDialog();
                notify.Dispose();
                return;
            }

            if (page == 1)
            {
                // Create new wallet
                buttonNext.Enabled = false;
                string path = "wallet/" + textBoxNewName.Text;
                string content = "{\"passphrase\":\"" + textBoxNewPass1.Text + "\"}";
                string response = await APIPut(path, true, content);
                if (response == "Error")
                {
                    NotifyForm notify = new NotifyForm("Error creating wallet");
                    notify.ShowDialog();
                    notify.Dispose();
                    buttonNext.Enabled = true;
                    return;
                }
                mainForm.AddLog("Created wallet: " + textBoxNewName.Text);
                NotifyForm notify2 = new NotifyForm("Created wallet: " + textBoxNewName.Text);
                notify2.ShowDialog();
                notify2.Dispose();
                this.Close();
            }
            else if (page == 2)
            {
                groupBoxSeed.Show();
                buttonNext.Text = "Import";
                page = 3;

            }
            else if (page == 3)
            {
                // Create new wallet
                buttonNext.Enabled = false;
                string path = "wallet/" + textBoxNewName.Text;
                string content = "{\"passphrase\":\"" + textBoxNewPass1.Text + "\",\"mnemonic\":\"" + textBoxSeedPhrase.Text + "\"}";
                string response = await APIPut(path, true, content);
                if (response == "Error")
                {
                    NotifyForm notify = new NotifyForm("Error creating wallet");
                    notify.ShowDialog();
                    notify.Dispose();
                    buttonNext.Enabled = true;
                    return;
                }
                mainForm.AddLog("Created wallet: " + textBoxNewName.Text);
                NotifyForm notify2 = new NotifyForm("Imported wallet: " + textBoxNewName.Text);
                notify2.ShowDialog();
                notify2.Dispose();
                this.Close();
            }
            else if (page == 4)
            {
                // Import Ledger
                buttonNext.Enabled = false;


                var proc = new Process();
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.FileName = "node.exe";
                proc.StartInfo.Arguments = mainForm.dir + "hsd-ledger/bin/hsd-ledger createwallet " + textBoxNewPass1.Text + " --api-key " + mainForm.nodeSettings["Key"];
                var outputBuilder = new StringBuilder();

                // Event handler for capturing output data
                proc.OutputDataReceived += (sender, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        outputBuilder.AppendLine(args.Data);
                    }
                };

                proc.Start();
                proc.BeginOutputReadLine();
                proc.WaitForExit();

                mainForm.AddLog(outputBuilder.ToString());

            }


        }

        HttpClient httpClient = new HttpClient();
        private async Task<string> APIPut(string path, bool wallet, string content)
        {
            string key = mainForm.nodeSettings["Key"];
            string ip = mainForm.nodeSettings["IP"];
            string port = "1203";
            if (mainForm.network == 1)
            {
                port = "1303";
            }
            if (wallet) port = port + "9";
            else port = port + "7";

            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Put, "http://" + ip + ":" + port + "/" + path);
            req.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes("x:" + key)));
            req.Content = new StringContent(content);

            // Send request
            HttpResponseMessage resp = await httpClient.SendAsync(req);

            try
            {
                resp.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                mainForm.AddLog("Put Error: " + ex.Message);
                mainForm.AddLog(await resp.Content.ReadAsStringAsync());
                return "Error";
            }

            return await resp.Content.ReadAsStringAsync();
        }
    }
}
