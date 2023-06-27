using Newtonsoft.Json.Linq;

namespace FireWallet
{
    public partial class MultisigSettingsForm : Form
    {
        MainForm mainForm;
        int n;
        int m;
        int current;
        string OwnKey;
        public MultisigSettingsForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.BackColor = ColorTranslator.FromHtml(mainForm.Theme["background"]);
            this.ForeColor = ColorTranslator.FromHtml(mainForm.Theme["foreground"]);
            foreach (Control c in Controls)
            {
                mainForm.ThemeControl(c);
            }
        }

        private void MultisigSettingsForm_Load(object sender, EventArgs e)
        {
            UpdateInfo();
        }
        private async void UpdateInfo()
        {
            // Get multisig info
            string infoResp = await mainForm.APIGet("wallet/" + mainForm.Account + "/account/default", true);
            mainForm.AddLog(infoResp);

            if (infoResp == "Error")
            {
                NotifyForm notifyForm = new NotifyForm("Error getting multisig info");
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                this.Close();
            }
            JObject info = JObject.Parse(infoResp);
            n = int.Parse(info["n"].ToString());
            m = int.Parse(info["m"].ToString());
            labelSigners.Text = "Signers: " + m + "/" + n;

            panelSigners.Controls.Clear();
            Panel selfInfo = new Panel();
            selfInfo.Width = panelSigners.Width - SystemInformation.VerticalScrollBarWidth;
            selfInfo.Height = 50;
            selfInfo.Location = new Point(0, 0);
            selfInfo.BackColor = ColorTranslator.FromHtml(mainForm.Theme["background"]);
            selfInfo.ForeColor = ColorTranslator.FromHtml(mainForm.Theme["foreground"]);
            selfInfo.BorderStyle = BorderStyle.FixedSingle;

            Label selfLabel = new Label();
            selfLabel.Text = "Own Key";
            selfLabel.Location = new Point(10, 10);
            selfLabel.AutoSize = true;
            selfInfo.Controls.Add(selfLabel);

            Label pubKey = new Label();
            OwnKey = info["accountKey"].ToString();
            string pukKeyShort = info["accountKey"].ToString().Substring(0, 10) + "..." + info["accountKey"].ToString().Substring(info["accountKey"].ToString().Length - 10);

            pubKey.Text = pukKeyShort;
            pubKey.Location = new Point(10, 30);
            pubKey.AutoSize = true;
            selfInfo.Controls.Add(pubKey);

            Button copyPubKey = new Button();
            copyPubKey.Text = "Copy";
            copyPubKey.AutoSize = true;
            copyPubKey.FlatStyle = FlatStyle.Flat;
            copyPubKey.Location = new Point(selfInfo.Width - copyPubKey.Width - 10, 10);
            copyPubKey.Click += (s, e) =>
            {
                Clipboard.SetText(OwnKey);
            };
            selfInfo.Controls.Add(copyPubKey);
            panelSigners.Controls.Add(selfInfo);

            // Get signers
            int y = 50;
            JArray signers = JArray.Parse(info["keys"].ToString());
            current = signers.Count;
            foreach (string sig in signers)
            {
                Panel signerInfo = new Panel();
                signerInfo.Width = panelSigners.Width - SystemInformation.VerticalScrollBarWidth;
                signerInfo.Height = 50;
                signerInfo.Location = new Point(0, y);
                signerInfo.BackColor = ColorTranslator.FromHtml(mainForm.Theme["background"]);
                signerInfo.ForeColor = ColorTranslator.FromHtml(mainForm.Theme["foreground"]);
                signerInfo.BorderStyle = BorderStyle.FixedSingle;

                Label signerLabel = new Label();
                signerLabel.Text = "Signer";
                signerLabel.Location = new Point(10, 10);
                signerLabel.AutoSize = true;
                signerInfo.Controls.Add(signerLabel);

                Label signerKey = new Label();
                string signerKeyShort = sig.Substring(0, 10) + "..." + sig.Substring(sig.Length - 10);
                signerKey.Text = signerKeyShort;
                signerKey.Location = new Point(10, 30);
                signerKey.AutoSize = true;
                signerInfo.Controls.Add(signerKey);

                Button DelSignerKey = new Button();
                DelSignerKey.Text = "Remove";
                DelSignerKey.AutoSize = true;
                DelSignerKey.FlatStyle = FlatStyle.Flat;
                DelSignerKey.Location = new Point(signerInfo.Width - DelSignerKey.Width - 10, 10);
                DelSignerKey.Click += (s, e) =>
                {
                    RemoveSig(sig);
                };
                signerInfo.Controls.Add(DelSignerKey);
                panelSigners.Controls.Add(signerInfo);
                y += 50;
            }


        }
        private async Task RemoveSig(string sig)
        {
            string path = "wallet/" + mainForm.Account + "/shared-key";
            string content = "{\"accountKey\": \"" + sig + "\",\"account\":\"default\"}";
            string resp = await APIPut(path, true, content);
            mainForm.AddLog(resp);
            UpdateInfo();

        }
        private async Task AddSigAsync(string sig)
        {
            string path = "wallet/" + mainForm.Account + "/shared-key";
            string content = "{\"accountKey\": \"" + sig + "\",\"account\":\"default\"}";
            string resp = await APIPut(path, true, content);
            mainForm.AddLog(resp);
            UpdateInfo();
        }

        private async void buttoAddSigner_Click(object sender, EventArgs e)
        {
            if (textBoxAddSig.Text.Length < 5)
            {
                NotifyForm notifyForm = new NotifyForm("You need to add a XPUB key");
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                return;
            }

            await AddSigAsync(textBoxAddSig.Text);

            textBoxAddSig.Text = "";
        }

        HttpClient httpClient = new HttpClient();
        /// <summary>
        /// Put to HSD API
        /// </summary>
        /// <param name="path">Path to put to</param>
        /// <param name="wallet">Whether to use port 12039</param>
        /// <param name="content">Content to put</param>
        /// <returns></returns>
        public async Task<string> APIPut(string path, bool wallet, string content)
        {
            if (content == "{\"passphrase\": \"\",\"timeout\": 60}")
            {
                return "";
            }
            string key = mainForm.NodeSettings["Key"];
            string ip = mainForm.NodeSettings["IP"];
            string port = "1203";
            if (mainForm.HSDNetwork == 1)
            {
                port = "1303";
            }
            if (wallet) port = port + "9";
            else port = port + "7";

            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Put, "http://" + ip + ":" + port + "/" + path);
            req.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes("x:" + key)));
            req.Content = new StringContent(content);
            // Send request


            try
            {
                HttpResponseMessage resp = await httpClient.SendAsync(req);
                if (!resp.IsSuccessStatusCode)
                {
                    mainForm.AddLog("Post Error: " + resp.StatusCode);
                    mainForm.AddLog(await resp.Content.ReadAsStringAsync());
                    return "Error";
                }
                return await resp.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                mainForm.AddLog("Post Error: " + ex.Message);
                return "Error";
            }
        }

        private void textBoxAddSig_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttoAddSigner_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }
    }
}
