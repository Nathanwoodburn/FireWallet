using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace FireWallet
{
    public partial class DNSForm : Form
    {
        private string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FireWallet\\";
        private Dictionary<string, string> theme;
        private Dictionary<string, string> nodeSettings;
        private string domain;
        MainForm mainForm;
        public bool cancel { get; set; }
        public DNS[] DNSrecords { get; set; }
        public DNSForm(MainForm mainForm, string domain)
        {
            InitializeComponent();
            this.domain = domain;
            this.mainForm = mainForm;
            nodeSettings = mainForm.NodeSettings;

            cancel = true;
            this.Text = domain + "/ DNS | FireWallet";
        }
        private void DNSForm_Load(object sender, EventArgs e)
        {
            UpdateTheme();
            GetDNS();

            comboBoxType.SelectedIndex = 0;
            textBoxMain.Focus();
        }



        private void UpdateDNSGroup()
        {
            panelRecords.Controls.Clear();
            int i = 0;
            foreach (DNS record in DNSrecords)
            {
                Panel DNSPanel = new Panel();
                // Count for scroll width
                DNSPanel.Width = panelRecords.Width - SystemInformation.VerticalScrollBarWidth - 2;
                DNSPanel.Height = 60;
                DNSPanel.BorderStyle = BorderStyle.FixedSingle;
                DNSPanel.Top = 62 * i;

                Label DNSType = new Label();
                DNSType.Text = record.type;
                DNSType.Location = new System.Drawing.Point(10, 10);
                DNSType.AutoSize = true;
                DNSType.Font = new Font(DNSType.Font.FontFamily, 11.0f, FontStyle.Bold);
                DNSPanel.Controls.Add(DNSType);


                switch (DNSType.Text)
                {
                    case "NS":
                        Label DNSNS = new Label();
                        DNSNS.Text = record.ns;
                        DNSNS.Location = new System.Drawing.Point(10, 30);
                        DNSNS.AutoSize = true;
                        DNSPanel.Controls.Add(DNSNS);
                        break;
                    case "GLUE4":
                    case "GLUE6":
                        Label DNSNS1 = new Label();
                        DNSNS1.Text = record.ns;
                        DNSNS1.Location = new System.Drawing.Point(10, 30);
                        DNSNS1.AutoSize = true;
                        DNSPanel.Controls.Add(DNSNS1);
                        Label address = new Label();
                        address.Text = record.address;
                        address.Location = new System.Drawing.Point(DNSNS1.Left + DNSNS1.Width + 20, 30);
                        address.AutoSize = true;
                        DNSPanel.Controls.Add(address);
                        break;
                    case "DS":
                        Label keyTag = new Label();
                        keyTag.Text = record.keyTag.ToString();
                        keyTag.Location = new System.Drawing.Point(10, 30);
                        keyTag.AutoSize = true;
                        DNSPanel.Controls.Add(keyTag);
                        Label algorithm = new Label();
                        algorithm.Text = record.algorithm.ToString();
                        algorithm.Location = new System.Drawing.Point(keyTag.Left + keyTag.Width + 10, 30);
                        algorithm.AutoSize = true;
                        DNSPanel.Controls.Add(algorithm);
                        Label digestType = new Label();
                        digestType.Text = record.digestType.ToString();
                        digestType.Location = new System.Drawing.Point(algorithm.Left + algorithm.Width + 10, 30);
                        digestType.AutoSize = true;
                        DNSPanel.Controls.Add(digestType);
                        Label digest = new Label();
                        digest.Text = record.digest;
                        digest.Location = new System.Drawing.Point(digestType.Left + digestType.Width + 10, 30);
                        digest.AutoSize = true;
                        DNSPanel.Controls.Add(digest);
                        break;
                    case "TXT":

                        int j = 0;
                        foreach (string txt in record.TXT)
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
                Button DeleteButton = new Button();
                DeleteButton.Text = "X";
                DeleteButton.Width = 30;
                DeleteButton.Height = 30;
                DeleteButton.Left = DNSPanel.Width - 40;
                // On click remove record from DNS[] and refresh display
                DeleteButton.TextAlign = ContentAlignment.MiddleCenter;

                DNS current = record;
                DeleteButton.Click += (sender, e) =>
                {
                    DNS[] newRecords = new DNS[DNSrecords.Length - 1];
                    int a = 0;
                    foreach (DNS r in DNSrecords)
                    {
                        if (r.type == current.type && r.ns == current.ns &&
                            r.TXT == current.TXT && r.address == current.address &&
                            r.digest == current.digest && r.digestType == current.digestType &&
                            r.algorithm == current.algorithm && r.keyTag == current.keyTag)
                        {
                            // Deleted
                        }
                        else
                        {
                            newRecords[a] = r;
                            a++;
                        }
                    }
                    DNSrecords = newRecords;
                    UpdateDNSGroup();
                };

                DNSPanel.Controls.Add(DeleteButton);
                DeleteButton.BringToFront();

                panelRecords.Controls.Add(DNSPanel);
                i++;

            }
        }

        #region Logging
        public void AddLog(string message)
        {
            StreamWriter sw = new StreamWriter(dir + "log.txt", true);
            sw.WriteLine(DateTime.Now.ToString() + ": " + message);
            sw.Dispose();
        }
        #endregion
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
                AddLog("Theme file is missing key");
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
                || c.GetType() == typeof(ComboBox) || c.GetType() == typeof(StatusStrip) || c.GetType() == typeof(ToolStrip) ||
                c.GetType() == typeof(ListBox))
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
                        else
                        {
                            AddLog("No transparency-key found in theme file");
                        }
                        break;
                    case "percent":
                        if (theme.ContainsKey("transparency-percent"))
                        {
                            Opacity = Convert.ToDouble(theme["transparency-percent"]) / 100;
                        }
                        else
                        {
                            AddLog("No transparency-percent found in theme file");
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
            sw.WriteLine("selected-bg: #000000");
            sw.WriteLine("selected-fg: #ffffff");
            sw.WriteLine("error: #ff0000");

            sw.Dispose();
            AddLog("Created theme file");
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
        #region API


        private async void GetDNS()
        {

            // Get DNS records
            string contentDNS = "{\"method\": \"getnameresource\", \"params\": [\"" + domain + "\"]}";
            string responseDNS = await APIPost("", false, contentDNS);
            JObject jObjectDNS = JObject.Parse(responseDNS);

            if (jObjectDNS["result"].ToString() == "")
            {
                return;
            }

            JObject result = (JObject)jObjectDNS["result"];
            JArray records = (JArray)result["records"];
            // For each record
            int i = 0;
            DNSrecords = new DNS[records.Count()];
            foreach (JObject record in records)
            {
                Panel DNSPanel = new Panel();
                // Count for scroll width
                DNSPanel.Width = panelRecords.Width - SystemInformation.VerticalScrollBarWidth - 2;
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
                        DNSrecords[i] = new DNS(record["type"].ToString(), record["ns"].ToString());
                        break;
                    case "GLUE4":
                    case "GLUE6":
                        DNSrecords[i] = new DNS(record["type"].ToString(), record["ns"].ToString(), record["address"].ToString());
                        break;
                    case "DS":
                        DNSrecords[i] = new DNS(record["type"].ToString(), int.Parse(record["keyTag"].ToString()),
                            int.Parse(record["algorithm"].ToString()), int.Parse(record["digestType"].ToString()),
                            record["digest"].ToString());
                        break;
                    case "TXT":
                        JArray txts = (JArray)record["txt"];
                        int j = 0;
                        string[] TXTs = new string[txts.Count];
                        foreach (string txt in txts)
                        {
                            TXTs[j] = txt;
                            j++;
                        }
                        DNSrecords[i] = new DNS("TXT", TXTs);
                        break;
                }
                i++;
            }

            UpdateDNSGroup();
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
            if (mainForm.HSDNetwork == 1)
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
            if (mainForm.HSDNetwork == 1)
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
        #endregion

        private void buttonAddRecord_Click(object sender, EventArgs e)
        {
            DNS[] updatedDNS = new DNS[DNSrecords.Length + 1];
            DNSrecords.CopyTo(updatedDNS, 0);
            if (textBoxMain.Text == "")
            {
                AddLog("No Content");
                return;
            }
            switch (comboBoxType.Text)
            {
                case "NS":
                    updatedDNS[updatedDNS.Length - 1] = new DNS("NS", textBoxMain.Text.ToLower());
                    break;
                case "TXT":
                    string[] TXT = new string[1];
                    TXT[0] = textBoxMain.Text;
                    updatedDNS[updatedDNS.Length - 1] = new DNS("TXT", TXT);
                    break;
                case "DS":
                    try
                    {
                        string[] parts = textBoxMain.Text.Trim().Split(' ');
                        if (parts.Length != 4) return;
                        int keyTag = int.Parse(parts[0]);
                        int algorithm = int.Parse(parts[1]);
                        int digestType = int.Parse(parts[2]);
                        string digest = parts[3];
                        updatedDNS[updatedDNS.Length - 1] = new DNS("DS", keyTag, algorithm, digestType, digest);
                    }
                    catch
                    {
                        NotifyForm notifyForm = new NotifyForm("Incorrect format for DS");
                        notifyForm.ShowDialog();
                        notifyForm.Dispose();
                    }
                    break;
                case "GLUE4":
                case "GLUE6":
                    if (textBoxAlt.Text == "") return;
                    updatedDNS[updatedDNS.Length - 1] = new DNS(comboBoxType.Text, textBoxMain.Text, textBoxAlt.Text);
                    break;
            }
            textBoxMain.Text = "";
            textBoxAlt.Text = "";
            DNSrecords = updatedDNS;
            UpdateDNSGroup();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.cancel = true;
            this.Close();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            this.cancel = false;
            this.Close();
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxType.Text)
            {
                case "NS":
                    textBoxAlt.Hide();
                    labelRecordAlt.Hide();
                    labelRecordMain.Text = "NS";
                    textBoxMain.PlaceholderText = "ns1.woodburn.";
                    break;
                case "DS":
                    textBoxAlt.Hide();
                    labelRecordAlt.Hide();
                    labelRecordMain.Text = "DS";
                    textBoxMain.PlaceholderText = "57355 8 2 95a57c3bab7849dbcddf7c72ada71a88146b141110318ca5be672057e865c3e2";
                    break;
                case "TXT":
                    textBoxAlt.Hide();
                    labelRecordAlt.Hide();
                    labelRecordMain.Text = "TXT";
                    break;
                case "GLUE4":
                    textBoxAlt.Show();
                    labelRecordAlt.Show();
                    labelRecordMain.Text = "NS";
                    labelRecordAlt.Text = "Address";
                    textBoxMain.PlaceholderText = "ns1.woodburn.";
                    textBoxAlt.PlaceholderText = "127.0.0.1";
                    break;
                case "GLUE6":
                    textBoxAlt.Show();
                    labelRecordAlt.Show();
                    labelRecordMain.Text = "NS";
                    labelRecordAlt.Text = "Address";
                    textBoxMain.PlaceholderText = "ns1.woodburn.";
                    textBoxAlt.PlaceholderText = "::1";
                    break;
            }
        }

        private void textBoxMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                e.SuppressKeyPress = true;
                buttonAddRecord.PerformClick();

            }
        }

        private void comboBoxType_DropDownClosed(object sender, EventArgs e)
        {
            textBoxMain.Focus();
        }
    }
}
