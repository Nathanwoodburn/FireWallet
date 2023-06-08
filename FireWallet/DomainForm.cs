using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;

namespace FireWallet
{
    public partial class DomainForm : Form
    {
        string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FireWallet\\";
        Dictionary<string, string> theme;
        Dictionary<string, string> nodeSettings;
        string domain;
        int network;
        int height;
        bool own;
        string state;
        string explorerTX;
        string explorerName;
        int TransferEnd;


        private MainForm mainForm;

        public DomainForm(MainForm mainForm, string domain, string explorerTX, string explorerName)
        {
            InitializeComponent();
            this.Text = domain + "/ | FireWallet";
            labelTitle.Text = domain + "/";
            this.domain = domain;
            this.explorerTX = explorerTX;
            this.explorerName = explorerName;
            this.mainForm = mainForm;
        }

        #region Theming
        private void UpdateTheme()
        {
            // Check if file exists
            if (!Directory.Exists(dir))
            {
                CreateConfig(dir);
            }
            if (!File.Exists(dir + "theme.txt"))
            {
                CreateConfig(dir);
            }

            // Read file
            StreamReader sr = new StreamReader(dir + "theme.txt");
            theme = new Dictionary<string, string>();
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] split = line.Split(':');
                theme.Add(split[0].Trim(), split[1].Trim());
            }
            sr.Dispose();

            if (!theme.ContainsKey("background") || !theme.ContainsKey("background-alt") || !theme.ContainsKey("foreground") || !theme.ContainsKey("foreground-alt"))
            {
                return;
            }

            // Apply theme
            this.BackColor = ColorTranslator.FromHtml(theme["background"]);

            // Foreground
            this.ForeColor = ColorTranslator.FromHtml(theme["foreground"]);


            // Need to specify this for each groupbox to override the black text
            foreach (Control c in Controls)
            {
                ThemeControl(c);
            }




            // Transparancy
            applyTransparency(theme);


        }
        private void ThemeControl(Control c)
        {
            if (c.GetType() == typeof(GroupBox) || c.GetType() == typeof(Panel))
            {
                c.ForeColor = ColorTranslator.FromHtml(theme["foreground"]);
                foreach (Control sub in c.Controls)
                {
                    ThemeControl(sub);
                }
            }
            if (c.GetType() == typeof(TextBox) || c.GetType() == typeof(Button)
                || c.GetType() == typeof(ComboBox) || c.GetType() == typeof(StatusStrip))
            {
                c.ForeColor = ColorTranslator.FromHtml(theme["foreground-alt"]);
                c.BackColor = ColorTranslator.FromHtml(theme["background-alt"]);
            }
        }

        private void applyTransparency(Dictionary<string, string> theme)
        {
            if (theme.ContainsKey("transparent-mode"))
            {
                switch (theme["transparent-mode"])
                {
                    case "mica":
                        var accent = new AccentPolicy { AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND };
                        var accentStructSize = Marshal.SizeOf(accent);
                        var accentPtr = Marshal.AllocHGlobal(accentStructSize);
                        Marshal.StructureToPtr(accent, accentPtr, false);
                        var data = new WindowCompositionAttributeData
                        {
                            Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                            SizeOfData = accentStructSize,
                            Data = accentPtr
                        };
                        User32.SetWindowCompositionAttribute(Handle, ref data);
                        Marshal.FreeHGlobal(accentPtr);
                        break;
                    case "key":
                        if (theme.ContainsKey("transparency-key"))
                        {
                            switch (theme["transparency-key"])
                            {
                                case "alt":
                                    this.TransparencyKey = ColorTranslator.FromHtml(theme["background-alt"]);
                                    break;
                                case "main":
                                    this.TransparencyKey = ColorTranslator.FromHtml(theme["background"]);
                                    break;
                                default:
                                    this.TransparencyKey = ColorTranslator.FromHtml(theme["transparency-key"]);
                                    break;
                            }
                        }
                        break;
                    case "percent":
                        if (theme.ContainsKey("transparency-percent"))
                        {
                            Opacity = Convert.ToDouble(theme["transparency-percent"]) / 100;
                        }
                        break;
                }
            }
        }

        private void CreateConfig(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            StreamWriter sw = new StreamWriter(dir + "theme.txt");
            sw.WriteLine("background: #000000");
            sw.WriteLine("foreground: #8e05c2");
            sw.WriteLine("background-alt: #3e065f");
            sw.WriteLine("foreground-alt: #ffffff");
            sw.WriteLine("transparent-mode: off");
            sw.WriteLine("transparency-key: main");
            sw.WriteLine("transparency-percent: 90");

            sw.Dispose();

        }

        // Required for mica effect
        internal enum AccentState
        {
            ACCENT_DISABLED = 0,
            ACCENT_ENABLE_GRADIENT = 1,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_INVALID_STATE = 4
        }

        internal enum WindowCompositionAttribute
        {
            WCA_ACCENT_POLICY = 19
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct AccentPolicy
        {
            public AccentState AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        internal static class User32
        {
            [DllImport("user32.dll")]
            internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);
        }
        #endregion

        private void DomainForm_Load(object sender, EventArgs e)
        {
            UpdateTheme();
            own = false;
            StreamReader sr = new StreamReader(dir + "node.txt");
            nodeSettings = new Dictionary<string, string>();
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] split = line.Split(':');
                nodeSettings.Add(split[0].Trim(), split[1].Trim());

            }
            sr.Dispose();

            if (!nodeSettings.ContainsKey("Network") || !nodeSettings.ContainsKey("Key") || !nodeSettings.ContainsKey("IP"))
            {
                this.Close();
                return;
            }
            network = Convert.ToInt32(nodeSettings["Network"]);
            GetName();
        }
        #region API
        private async void GetName()
        {
            try
            {
                string content = "{\"method\": \"getnameinfo\", \"params\": [\"" + domain + "\"]}";
                string response = await APIPost("", false, content);
                JObject jObject = JObject.Parse(response);
                // Get block height
                string Nodeinfo = await APIGet("", false);
                JObject jObjectInfo = JObject.Parse(Nodeinfo);
                JObject chain = (JObject)jObjectInfo["chain"];
                height = Convert.ToInt32(chain["height"]);

                if (jObject.ContainsKey("result"))
                {
                    JObject result = (JObject)jObject["result"];

                    JObject start = (JObject)result["start"];
                    labelStatusReserved.Text = start["reserved"].ToString();


                    if (result.ContainsKey("info"))
                    {
                        try
                        {
                            JObject info = (JObject)result["info"];
                            string state = info["state"].ToString();
                            labelStatusMain.Text = state;
                            labelStatusHighest.Text = convertHNS(info["highest"].ToString()) + " HNS";
                            labelStatusPaid.Text = convertHNS(info["value"].ToString()) + " HNS";

                            JObject stats = (JObject)info["stats"];

                            if (info["transfer"].ToString() == "0")
                            {
                                labelStatusTransferring.Text = "No";
                                TransferEnd = 0;
                            }
                            else
                            {
                                labelStatusTransferring.Text = "Yes";
                                TransferEnd = Convert.ToInt32(stats["transferLockupEnd"].ToString());
                            }

                            if (state == "CLOSED")
                            {
                                string expires = stats["blocksUntilExpire"].ToString() + " Blocks (~" + stats["daysUntilExpire"].ToString() + " days)";
                                labelStatusTimeToNext.Text = expires;
                            }
                            else if (state == "BIDDING")
                            {
                                string bidding = stats["blocksUntilReveal"].ToString() + " Blocks (~" + stats["hoursUntilReveal"].ToString() + " hrs)";
                                labelStatusTimeToNext.Text = bidding;
                                labelStatusNextState.Text = "Reveal in:";
                            }
                            else if (state == "REVEAL")
                            {
                                string reveal = stats["blocksUntilClose"].ToString() + " Blocks (~" + stats["hoursUntilClose"].ToString() + " hrs)";
                                labelStatusTimeToNext.Text = reveal;
                                labelStatusNextState.Text = "Closing in:";
                            }
                            else
                            {
                                AddLog("State not added yet: " + state);
                                AddLog(stats.ToString());
                            }


                            // Get DNS if the domain isn't in auction
                            if (state == "CLOSED") GetDNS();
                            else if (state == "BIDDING" || state == "REVEAL") GetBids(state);
                            else groupBoxDNS.Visible = false;

                            //Setup action box
                            ActionSetupAsync(state);
                        }
                        catch (Exception ex)
                        {
                            // No info -> Domain not yet auctioned
                            labelStatusMain.Text = "Available";
                            ActionSetupAsync("AVAILABLE");
                        }
                    }
                }
                else
                {
                    labelStatusMain.Text = "Error";
                }
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }

        }

        private async void GetDNS()
        {
            // Get DNS records
            string contentDNS = "{\"method\": \"getnameresource\", \"params\": [\"" + domain + "\"]}";
            string responseDNS = await APIPost("", false, contentDNS);
            JObject jObjectDNS = JObject.Parse(responseDNS);
            JObject result = (JObject)jObjectDNS["result"];
            JArray records = (JArray)result["records"];
            // For each record
            int i = 0;
            foreach (JObject record in records)
            {
                Panel DNSPanel = new Panel();
                // Count for scroll width
                DNSPanel.Width = panelDNS.Width - SystemInformation.VerticalScrollBarWidth - 2;
                DNSPanel.Height = 60;
                DNSPanel.BorderStyle = BorderStyle.FixedSingle;
                DNSPanel.Top = 62 * i;

                Label DNSType = new Label();
                DNSType.Text = record["type"].ToString();
                DNSType.Location = new System.Drawing.Point(10, 10);
                DNSType.AutoSize = true;
                DNSType.Font = new Font(DNSType.Font.FontFamily, 11.0f, FontStyle.Bold);
                DNSPanel.Controls.Add(DNSType);


                switch (DNSType.Text)
                {
                    case "NS":
                        Label DNSNS = new Label();
                        DNSNS.Text = record["ns"].ToString();
                        DNSNS.Location = new System.Drawing.Point(10, 30);
                        DNSNS.AutoSize = true;
                        DNSPanel.Controls.Add(DNSNS);
                        break;
                    case "GLUE4":
                    case "GLUE6":
                        Label DNSNS1 = new Label();
                        DNSNS1.Text = record["ns"].ToString();
                        DNSNS1.Location = new System.Drawing.Point(10, 30);
                        DNSNS1.AutoSize = true;
                        DNSPanel.Controls.Add(DNSNS1);
                        Label address = new Label();
                        address.Text = record["address"].ToString();
                        address.Location = new System.Drawing.Point(DNSNS1.Left + DNSNS1.Width + 20, 30);
                        address.AutoSize = true;
                        DNSPanel.Controls.Add(address);
                        break;
                    case "DS":
                        Label keyTag = new Label();
                        keyTag.Text = record["keyTag"].ToString();
                        keyTag.Location = new System.Drawing.Point(10, 30);
                        keyTag.AutoSize = true;
                        DNSPanel.Controls.Add(keyTag);
                        Label algorithm = new Label();
                        algorithm.Text = record["algorithm"].ToString();
                        algorithm.Location = new System.Drawing.Point(keyTag.Left + keyTag.Width + 10, 30);
                        algorithm.AutoSize = true;
                        DNSPanel.Controls.Add(algorithm);
                        Label digestType = new Label();
                        digestType.Text = record["digestType"].ToString();
                        digestType.Location = new System.Drawing.Point(algorithm.Left + algorithm.Width + 10, 30);
                        digestType.AutoSize = true;
                        DNSPanel.Controls.Add(digestType);
                        Label digest = new Label();
                        digest.Text = record["digest"].ToString();
                        digest.Location = new System.Drawing.Point(digestType.Left + digestType.Width + 10, 30);
                        digest.AutoSize = true;
                        DNSPanel.Controls.Add(digest);
                        break;
                    case "TXT":
                        JArray txts = (JArray)record["txt"];
                        int j = 0;
                        foreach (string txt in txts)
                        {
                            Label DNSTXT = new Label();
                            DNSTXT.Text = txt;
                            DNSTXT.Location = new System.Drawing.Point(10, 30 + (j * 20));
                            DNSTXT.AutoSize = true;
                            DNSPanel.Controls.Add(DNSTXT);
                            DNSPanel.Height = 60 + (j * 20);
                            j++;
                        }
                        break;

                }
                panelDNS.Controls.Add(DNSPanel);
                i++;
            }
            panelDNS.AutoScroll = true;
        }
        private async void GetBids(string state)
        {

            groupBoxBids.Visible = true;
            panelBids.AutoScroll = true;
            // Get Bids
            string contentBids = "{\"method\": \"getauctioninfo\", \"params\": [\"" + domain + "\"]}";
            string response = await APIPost("", true, contentBids);
            if (!response.Contains("\"error\":null"))
            {
                AddLog("Syncing Domain");
                Label syncingLabel = new Label();
                syncingLabel.Text = "Syncing Bids...";
                syncingLabel.Location = new System.Drawing.Point(10, 10);
                syncingLabel.AutoSize = true;
                panelBids.Controls.Add(syncingLabel);

                // Error
                // Try scanning for auction
                contentBids = "{\"method\": \"importname\", \"params\": [\"" + domain + "\", " + (height - 2000) + "]}";
                await APIPost("", true, contentBids);
                contentBids = "{\"method\": \"getauctioninfo\", \"params\": [\"" + domain + "\"]}";
                response = await APIPost("", true, contentBids);
                panelBids.Controls.Clear();
            }

            if (state == "BIDDING")
            {
                JObject resp = JObject.Parse(response);
                JObject result = (JObject)resp["result"];
                JArray bids = (JArray)result["bids"];
                int i = 1;
                foreach (JObject bid in bids)
                {
                    Panel bidPanel = new Panel();
                    // Count for scroll width
                    bidPanel.Width = panelBids.Width - SystemInformation.VerticalScrollBarWidth - 2;
                    bidPanel.Height = 60;
                    bidPanel.BorderStyle = BorderStyle.FixedSingle;
                    bidPanel.Top = (62 * i) - 60;
                    Label bidNumber = new Label();
                    bidNumber.Text = i.ToString();
                    bidNumber.Location = new System.Drawing.Point(10, 10);
                    bidNumber.AutoSize = true;
                    bidNumber.Font = new Font(bidNumber.Font.FontFamily, 11.0f, FontStyle.Bold);
                    bidPanel.Controls.Add(bidNumber);
                    Label bidAmount = new Label();
                    bidAmount.Text = convertHNS(bid["lockup"].ToString()) + " HNS";
                    bidAmount.Location = new System.Drawing.Point(10, 30);
                    bidAmount.AutoSize = true;
                    bidPanel.Controls.Add(bidAmount);
                    if (bid["own"].ToString() == "True")
                    {
                        Label ownBid = new Label();
                        ownBid.Text = "Own Bid";
                        ownBid.Location = new System.Drawing.Point(bidAmount.Left + bidAmount.Width + 10, 30);
                        ownBid.AutoSize = true;
                        bidPanel.Controls.Add(ownBid);
                    }

                    panelBids.Controls.Add(bidPanel);
                    i++;
                }
            }
            else if (state == "REVEAL")
            {
                //! TODO Add reveal info
                JObject resp = JObject.Parse(response);
                JObject result = (JObject)resp["result"];
                JArray bids = (JArray)result["bids"];
                JArray reveals = (JArray)result["reveals"];
                int i = 1;
                foreach (JObject bid in bids)
                {

                    Panel bidPanel = new Panel();
                    // Count for scroll width
                    bidPanel.Width = panelBids.Width - SystemInformation.VerticalScrollBarWidth - 2;
                    bidPanel.Height = 60;
                    bidPanel.BorderStyle = BorderStyle.FixedSingle;
                    bidPanel.Top = (62 * i) - 60;
                    Label bidNumber = new Label();
                    bidNumber.Text = i.ToString();
                    bidNumber.Location = new System.Drawing.Point(10, 10);
                    bidNumber.AutoSize = true;
                    bidNumber.Font = new Font(bidNumber.Font.FontFamily, 11.0f, FontStyle.Bold);
                    bidPanel.Controls.Add(bidNumber);
                    Label bidAmount = new Label();
                    bidAmount.Text = convertHNS(bid["lockup"].ToString()) + " HNS";
                    bidAmount.Location = new System.Drawing.Point(10, 30);
                    bidAmount.AutoSize = true;
                    bidPanel.Controls.Add(bidAmount);

                    if (bid["own"].ToString() == "true")
                    {
                        Label ownBid = new Label();
                        ownBid.Text = "Own Bid";
                        ownBid.Location = new System.Drawing.Point(bidAmount.Left + bidAmount.Width + 10, 30);
                        ownBid.AutoSize = true;
                        bidPanel.Controls.Add(ownBid);
                    }

                    panelBids.Controls.Add(bidPanel);
                    i++;
                }
            }



        }
        private string convertHNS(string dollarydoos)
        {
            decimal hns = Convert.ToDecimal(dollarydoos);
            hns = hns / 1000000;
            return decimal.Round(hns, 2).ToString();
        }



        HttpClient httpClient = new HttpClient();
        /// <summary>
        /// Post to HSD API
        /// </summary>
        /// <param name="path">Path to post to</param>
        /// <param name="wallet">Whether to use port 12039</param>
        /// <param name="content">Content to post</param>
        /// <returns></returns>
        private async Task<string> APIPost(string path, bool wallet, string content)
        {
            string key = nodeSettings["Key"];
            string ip = nodeSettings["IP"];
            string port = "1203";
            if (network == 1)
            {
                port = "1303";
            }
            if (wallet) port = port + "9";
            else port = port + "7";

            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, "http://" + ip + ":" + port + "/" + path);
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
                AddLog("Post Error: " + ex.Message);
                AddLog("Post Error: " + await resp.Content.ReadAsStringAsync());
                return "Error";
            }

            return await resp.Content.ReadAsStringAsync();
        }
        /// <summary>
        /// Get from HSD API
        /// </summary>
        /// <param name="path">Path to get</param>
        /// <param name="wallet">Whether to use port 12039</param>
        /// <returns></returns>
        private async Task<string> APIGet(string path, bool wallet)
        {
            string key = nodeSettings["Key"];
            string ip = nodeSettings["IP"];

            string port = "1203";
            if (network == 1)
            {
                port = "1303";
            }
            if (wallet) port = port + "9";
            else port = port + "7";

            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://" + ip + ":" + port + "/" + path);
                // Add API key to header
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes("x:" + key)));
                // Send request and log response
                HttpResponseMessage response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();

            }
            // Log errors to log textbox
            catch (Exception ex)
            {
                AddLog("Get Error: " + ex.Message);
                return "Error";
            }
        }
        private void AddLog(string message)
        {
            StreamWriter sw = new StreamWriter(dir + "log.txt", true);
            sw.WriteLine(DateTime.Now.ToString() + ": " + message);
            sw.Dispose();
        }
        #endregion
        private bool DomainOwned()
        {
            foreach (string d in mainForm.Domains)
            {
                if (d == domain)
                {
                    return true;
                }
            }
            return false;
        }

        private void ActionSetupAsync(string state)
        {
            own = DomainOwned();
            this.state = state;
            switch (state)
            {
                case "AVAILABLE":
                    groupBoxAction.Show();
                    groupBoxAction.Text = "Open Auction";
                    buttonActionMain.Text = "Send Open";
                    buttonActionAlt.Text = "Open in Batch";
                    break;
                case "BIDDING":
                    groupBoxAction.Show();
                    labelBid.Show();
                    textBoxBid.Show();
                    labelBlind.Show();
                    textBoxBlind.Show();
                    break;
                case "REVEAL":
                    groupBoxAction.Show();
                    groupBoxAction.Text = "Reveal Bid";
                    buttonActionMain.Text = "Send Reveal";
                    buttonActionAlt.Text = "Reveal in Batch";
                    break;
                case "CLOSED":
                    if (own)
                    {
                        if (labelStatusTransferring.Text == "Yes")
                        {
                            groupBoxAction.Show();
                            groupBoxAction.Text = "Finalize";

                            // Check if can finalize
                            if (height >= TransferEnd)
                            {
                                buttonActionMain.Text = "Send Finalize";
                                buttonActionAlt.Text = "Finalize in Batch";
                            }
                            else
                            {
                                labelBid.Text = "Finalize transfer in " + (TransferEnd - height).ToString() + " blocks";
                                labelBid.Show();
                                buttonActionMain.Text = "Cancel Transfer";
                                buttonActionAlt.Text = "Cancel in Batch";
                            }
                        }
                        else
                        {
                            groupBoxAction.Show();
                            groupBoxAction.Text = "Edit";
                            buttonActionMain.Text = "Edit DNS";
                            buttonActionAlt.Text = "Edit in Batch";
                            buttonRenew.Show();
                            buttonTransfer.Show();
                        }
                    }
                    break;
            }
        }
        private void BidBlind_TextChanged(object sender, EventArgs e)
        {
            string cleanedText = Regex.Replace(textBoxBid.Text, "[^0-9.]", "");
            textBoxBid.Text = cleanedText;
            cleanedText = Regex.Replace(textBoxBlind.Text, "[^0-9.]", "");
            textBoxBlind.Text = cleanedText;
        }

        private async void buttonActionMain_Click(object sender, EventArgs e)
        {
            if (state == "BIDDING")
            {
                int count = textBoxBid.Text.Count(c => c == '.');
                int count2 = textBoxBlind.Text.Count(c => c == '.');
                if (count > 1 || count2 > 1)
                {
                    NotifyForm notifyForm = new NotifyForm("Invalid bid amount");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                    return;
                }
                if (textBoxBid.Text == "" || textBoxBid.Text == ".")
                {
                    textBoxBid.Text = "0";
                }
                if (textBoxBlind.Text == "" || textBoxBlind.Text == ".")
                {
                    textBoxBlind.Text = "0";
                }
                decimal bid = Convert.ToDecimal(textBoxBid.Text);
                decimal blind = Convert.ToDecimal(textBoxBlind.Text);
                decimal lockup = bid + blind;
                string content = "{\"method\": \"sendbid\", \"params\": [\"" + domain + "\", " + bid.ToString() + ", " + lockup.ToString() + "]}";

                string response = await APIPost("", true, content);

                if (response == "Error")
                {
                    NotifyForm notifyForm = new NotifyForm("Error sending bid");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                }
                else
                {
                    JObject jObject = JObject.Parse(response);
                    if (jObject["result"].ToString() == "")
                    {
                        JObject error = (JObject)jObject["error"];
                        string message = (string)error["message"];
                        NotifyForm notifyForm2 = new NotifyForm("Error sending bid: \n" + message);
                        notifyForm2.ShowDialog();
                        notifyForm2.Dispose();
                        return;
                    }
                    JObject result = (JObject)jObject["result"];
                    string hash = (string)result["hash"];

                    NotifyForm notifyForm = new NotifyForm("Bid sent: " + hash, "Explorer", explorerTX + hash);
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();

                }

            }
            else if (state == "REVEAL")
            {
                decimal bid = Convert.ToDecimal(textBoxBid.Text);
                decimal blind = Convert.ToDecimal(textBoxBlind.Text);
                decimal lockup = bid + blind;
                string content = "{\"method\": \"sendreveal\", \"params\": [\"" + domain + "\"]}";

                string response = await APIPost("", true, content);

                if (response == "Error")
                {
                    NotifyForm notifyForm = new NotifyForm("Error sending reveal");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                }
                else
                {
                    JObject jObject = JObject.Parse(response);
                    if (jObject["result"].ToString() == "")
                    {
                        JObject error = (JObject)jObject["error"];
                        string message = (string)error["message"];
                        NotifyForm notifyForm2 = new NotifyForm("Error sending reveal: \n" + message);
                        notifyForm2.ShowDialog();
                        notifyForm2.Dispose();
                        return;
                    }
                    JObject result = (JObject)jObject["result"];
                    string hash = (string)result["hash"];

                    NotifyForm notifyForm = new NotifyForm("Reveal sent: " + hash, "Explorer", explorerTX + hash);
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();

                }
            }
            else if (state == "AVAILABLE")
            {
                decimal bid = Convert.ToDecimal(textBoxBid.Text);
                decimal blind = Convert.ToDecimal(textBoxBlind.Text);
                decimal lockup = bid + blind;
                string content = "{\"method\": \"sendopen\", \"params\": [\"" + domain + "\"]}";

                string response = await APIPost("", true, content);

                if (response == "Error")
                {
                    NotifyForm notifyForm = new NotifyForm("Error sending open");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                }
                else
                {
                    JObject jObject = JObject.Parse(response);
                    if (jObject["result"].ToString() == "")
                    {
                        JObject error = (JObject)jObject["error"];
                        string message = (string)error["message"];
                        NotifyForm notifyForm2 = new NotifyForm("Error sending open: \n" + message);
                        notifyForm2.ShowDialog();
                        notifyForm2.Dispose();
                        return;
                    }
                    JObject result = (JObject)jObject["result"];
                    string hash = (string)result["hash"];

                    NotifyForm notifyForm = new NotifyForm("Open sent: " + hash, "Explorer", explorerTX + hash);
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                }
            }
            else if (state == "CLOSED")
            {
                if (labelStatusTransferring.Text == "Yes")
                {
                    string content = "{\"method\": \"sendfinalize\", \"params\": [\"" + domain + "\"]}";
                    if (height < TransferEnd)
                    {
                        content = "{\"method\": \"sendcancel\", \"params\": [\"" + domain + "\"]}";
                    }
                    string response = await APIPost("", true, content);

                    if (response == "Error")
                    {
                        NotifyForm notifyForm = new NotifyForm("Error sending finalize");
                        notifyForm.ShowDialog();
                        notifyForm.Dispose();
                    }
                    else
                    {
                        JObject jObject = JObject.Parse(response);
                        if (jObject["result"].ToString() == "")
                        {
                            JObject error = (JObject)jObject["error"];
                            string message = (string)error["message"];
                            NotifyForm notifyForm2 = new NotifyForm("Error sending finalize: \n" + message);
                            notifyForm2.ShowDialog();
                            notifyForm2.Dispose();
                            return;
                        }
                        JObject result = (JObject)jObject["result"];
                        string hash = (string)result["hash"];

                        NotifyForm notifyForm = new NotifyForm("Finalize sent: " + hash, "Explorer", explorerTX + hash);
                        notifyForm.ShowDialog();
                        notifyForm.Dispose();

                    }
                }
                else
                {
                    DNSForm DNSEdit = new DNSForm(mainForm, domain);
                    DNSEdit.ShowDialog();

                    if (!DNSEdit.cancel)
                    {
                        string records = string.Join(", ", DNSEdit.DNSrecords.Select(record => record.ToString()));

                        string content = "{\"method\": \"sendupdate\", \"params\": [\"" + domain + "\", {\"records\": [" + records + "]}]}";
                        AddLog(content);
                        string response = await APIPost("", true, content);

                        if (response == "Error")
                        {
                            NotifyForm notifyForm = new NotifyForm("Error sending update");
                            notifyForm.ShowDialog();
                            notifyForm.Dispose();
                        }
                        else
                        {
                            JObject jObject = JObject.Parse(response);
                            if (jObject["result"].ToString() == "")
                            {
                                JObject error = (JObject)jObject["error"];
                                string message = (string)error["message"];
                                NotifyForm notifyForm2 = new NotifyForm("Error sending update: \n" + message);
                                notifyForm2.ShowDialog();
                                notifyForm2.Dispose();
                                return;
                            }
                            JObject result = (JObject)jObject["result"];
                            string hash = (string)result["hash"];

                            NotifyForm notifyForm = new NotifyForm("Update sent: " + hash, "Explorer", explorerTX + hash);
                            notifyForm.ShowDialog();
                            notifyForm.Dispose();

                        }

                    }

                    DNSEdit.Dispose();
                }

            }
        }

        private void Explorer_Click(object sender, EventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = explorerName + domain,
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void buttonActionAlt_Click(object sender, EventArgs e)
        {
            if (state == "BIDDING")
            {
                int count = textBoxBid.Text.Count(c => c == '.');
                int count2 = textBoxBlind.Text.Count(c => c == '.');
                if (count > 1 || count2 > 1)
                {
                    NotifyForm notifyForm = new NotifyForm("Invalid bid amount");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                    return;
                }
                if (textBoxBid.Text == "" || textBoxBid.Text == ".")
                {
                    textBoxBid.Text = "0";
                }
                if (textBoxBlind.Text == "" || textBoxBlind.Text == ".")
                {
                    textBoxBlind.Text = "0";
                }
                decimal bid = Convert.ToDecimal(textBoxBid.Text);
                decimal blind = Convert.ToDecimal(textBoxBlind.Text);
                decimal lockup = bid + blind;

                mainForm.AddBatch(domain, "BID", bid, lockup);
                this.Close();
            }
            else if (state == "CLOSED")
            {
                if (labelStatusTransferring.Text == "Yes")
                {
                    if (height >= TransferEnd)
                    {
                        mainForm.AddBatch(domain, "FINALIZE");
                        this.Close();
                    }
                    else
                    {
                        mainForm.AddBatch(domain, "CANCEL");
                        this.Close();
                    }
                }
                else
                {
                    DNSForm dnsEdit = new DNSForm(mainForm, domain);
                    dnsEdit.ShowDialog();
                    if (!dnsEdit.cancel)
                    {
                        mainForm.AddBatch(domain, "UPDATE", dnsEdit.DNSrecords);
                    }
                    dnsEdit.Dispose();
                }
            }
            else
            {
                switch (state)
                {
                    case "REVEAL":
                        mainForm.AddBatch(domain, "REVEAL");
                        this.Close();
                        break;
                    case "AVAILABLE":
                        mainForm.AddBatch(domain, "OPEN");
                        this.Close();
                        break;
                }
            }
        }

        private void buttonRenew_Click(object sender, EventArgs e)
        {
            mainForm.AddBatch(domain, "RENEW");
        }

        private void buttonTransfer_Click(object sender, EventArgs e)
        {
            // Transfer
        }
    }
}
