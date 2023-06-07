using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using IronBarCode;

namespace FireWallet
{
    public partial class MainForm : Form
    {
        #region Variables
        string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FireWallet\\";
        Dictionary<string, string> nodeSettings;
        Dictionary<string, string> userSettings;
        Dictionary<string, string> theme;
        int Network;
        string account;
        string password;
        decimal balance;
        decimal balanceLocked;
        int screen; // 0 = login, 1 = portfolio
        int height;
        double syncProgress;
        int pendingTransactions;

        #endregion
        #region Application
        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateTheme();
            LoadNode();
            LoadSettings();

            // Edit the theme of the navigation panel
            panelNav.BackColor = ColorTranslator.FromHtml(theme["background-alt"]);
            panelNav.ForeColor = ColorTranslator.FromHtml(theme["foreground-alt"]);
            foreach (Control c in panelNav.Controls)
            {
                c.BackColor = ColorTranslator.FromHtml(theme["background"]);
                c.ForeColor = ColorTranslator.FromHtml(theme["foreground"]);
            }
            panelNav.Dock = DockStyle.Left;

            ResizeForm();
            panelNav.Visible = false;
            screen = 0;

            // Prompt for login
            GetAccounts();

            AddLog("Loaded");
        }
        private void MainForm_Closing(object sender, FormClosingEventArgs e)
        {
            AddLog("Closing");
        }
        #endregion




        #region Settings
        private void LoadNode()
        {
            if (!File.Exists(dir + "node.txt"))
            {
                CreateForm cf = new CreateForm();
                cf.ShowDialog();
                // Initial run
            }
            if (!File.Exists(dir + "node.txt"))
            {
                AddLog("Node setup failed");
                this.Close();
                return;
            }

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
                AddLog("Node Settings file is missing key");
                this.Close();
                return;
            }
            Network = Convert.ToInt32(nodeSettings["Network"]);
            switch (Network)
            {
                case 0:
                    toolStripStatusLabelNetwork.Text = "Network: Mainnet";
                    break;
                case 1:
                    toolStripStatusLabelNetwork.Text = "Network: Regtest";
                    break;
                case 2:
                    toolStripStatusLabelNetwork.Text = "Network: Testnet";
                    break;
            }
            NodeStatus();


        }
        private void LoadSettings()
        {
            if (!File.Exists(dir + "settings.txt"))
            {
                AddLog("Creating settings file");
                StreamWriter sw = new StreamWriter(dir + "settings.txt");
                sw.WriteLine("explorer-tx: https://niami.io/tx/");
                sw.WriteLine("explorer-addr: https://niami.io/address/");
                sw.WriteLine("explorer-block: https://niami.io/block/");
                sw.Dispose();
            }


            StreamReader sr = new StreamReader(dir + "settings.txt");
            userSettings = new Dictionary<string, string>();
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                userSettings.Add(line.Substring(0, line.IndexOf(":")).Trim(), line.Substring(line.IndexOf(":") + 1).Trim());
            }
            sr.Dispose();
        }


        #endregion
        #region Logging
        private void AddLog(string message)
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




            this.Width = Screen.PrimaryScreen.Bounds.Width / 5 * 3;
            this.Height = Screen.PrimaryScreen.Bounds.Height / 5 * 3;
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
                || c.GetType() == typeof(ComboBox) || c.GetType() == typeof(StatusStrip) || c.GetType() == typeof(ToolStrip))
            {
                c.ForeColor = ColorTranslator.FromHtml(theme["foreground-alt"]);
                c.BackColor = ColorTranslator.FromHtml(theme["background-alt"]);
            }
            if (c.GetType() == typeof(Panel)) c.Dock = DockStyle.Fill;
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
        private void Form1_Resize(object sender, EventArgs e)
        {
            ResizeForm();
        }
        private void ResizeForm()
        {
            groupBoxaccount.Left = (this.ClientSize.Width - groupBoxaccount.Width) / 2;
            groupBoxaccount.Top = (this.ClientSize.Height - groupBoxaccount.Height) / 2;
        }
        #endregion
        #region Accounts

        private async void GetAccounts()
        {
            string APIresponse = await APIGet("wallet", true);
            comboBoxaccount.Items.Clear();
            if (APIresponse != "Error")
            {
                JArray jArray = JArray.Parse(APIresponse);
                foreach (string account in jArray)
                {
                    comboBoxaccount.Items.Add(account);
                }
                if (comboBoxaccount.Items.Count > 0)
                {
                    comboBoxaccount.SelectedIndex = 0;
                }
                else
                {
                    comboBoxaccount.Items.Add("No accounts found");
                    comboBoxaccount.Enabled = false;
                }
            }
            else
            {
                comboBoxaccount.Items.Add("No accounts found");
                comboBoxaccount.Enabled = false;
            }
            textBoxaccountpassword.Focus();
        }
        private async Task<bool> Login()
        {
            string path = "wallet/" + account + "/unlock";
            string content = "{\"passphrase\": \"" + password + "\",\"timeout\": 60}";

            string APIresponse = await APIPost(path, true, content);

            if (!APIresponse.Contains("true"))
            {
                AddLog("Login failed");
                NotifyForm notifyForm = new NotifyForm("Login Failed\nMake sure your password is correct");
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                return false;
            }

            path = "";
            content = "{\"method\": \"selectwallet\",\"params\":[ \"" + account + "\"]}";

            APIresponse = await APIPost(path, true, content);
            if (!APIresponse.Contains("\"error\":null"))
            {
                AddLog("Wallet selection failed");
                NotifyForm notifyForm = new NotifyForm("Wallet selection failed\n" + APIresponse);
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                return false;
            }
            UpdateBalance();
            return true;
        }

        private async void LoginClick(object sender, EventArgs e)
        {
            account = comboBoxaccount.Text;
            password = textBoxaccountpassword.Text;
            bool loggedin = await Login();
            if (loggedin)
            {
                toolStripStatusLabelaccount.Text = "Account: " + account;
                textBoxaccountpassword.Text = "";
                panelaccount.Visible = false;
                toolStripSplitButtonlogout.Visible = true;
                panelNav.Visible = true;
                screen = 1;
                buttonNavPortfolio.PerformClick();
            }
        }

        private void PasswordEntered(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                LoginClick(sender, e);
            }
        }

        private void AccountChoose(object sender, EventArgs e)
        {
            textBoxaccountpassword.Focus();
        }

        private async void Logout(object sender, EventArgs e)
        {
            toolStripSplitButtonlogout.Visible = false;
            string path = "wallet/" + account + "/lock";
            string content = "";
            string APIresponse = await APIPost(path, true, content);
            if (!APIresponse.Contains("true"))
            {
                AddLog("Logout failed");
                NotifyForm notifyForm = new NotifyForm("Logout Failed\n" + APIresponse);
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                panelaccount.Visible = true;
                return;
            }
            panelaccount.Visible = true;
            panelNav.Visible = false;
            panelSend.Visible = false;
            panelRecieve.Visible = false;
            toolStripStatusLabelaccount.Text = "Account: Not Logged In";
            screen = 0;

        }
        #endregion
        #region API
        HttpClient httpClient = new HttpClient();
        private async void NodeStatus()
        {

            if (await APIGet("", false) == "Error")
            {
                toolStripStatusLabelstatus.Text = "Status: Node Not Connected";
            }
            else
            {
                toolStripStatusLabelstatus.Text = "Status: Node Connected";
            }

        }
        private async Task UpdateBalance()
        {
            string response = await APIGet("wallet/" + account + "/balance?account=default", true);
            if (response == "Error") return;

            JObject resp = JObject.Parse(response);

            decimal available = Convert.ToDecimal(resp["unconfirmed"].ToString()) - Convert.ToDecimal(resp["lockedUnconfirmed"].ToString()) / 1000000;
            decimal locked = Convert.ToDecimal(resp["lockedUnconfirmed"].ToString()) / 1000000;
            available = available - locked;
            available = decimal.Round(available, 2);
            locked = decimal.Round(locked, 2);
            balance = available;
            balanceLocked = locked;
        }
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
            if (Network == 1)
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
            if (Network == 1)
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
        private async Task<string> GetAddress()
        {
            string content = "{\"account\":\"default\"}";
            string path = "wallet/" + account + "/address";
            string APIresponse = await APIPost(path, true, content);
            if (APIresponse == "Error")
            {
                AddLog("GetAddress Error");
                return "Error";
            }
            JObject resp = JObject.Parse(APIresponse);
            return resp["address"].ToString();
        }

        private async void GetTXHistory()
        {
            // Get height and progress
            String APIresponse = await APIGet("", false);
            JObject resp = JObject.Parse(APIresponse);
            JObject chain = JObject.Parse(resp["chain"].ToString());
            labelHeight.Text = "Height: " + chain["height"].ToString();
            decimal progress = Convert.ToDecimal(chain["progress"].ToString());
            labelSyncPercent.Text = "Sync: " + decimal.Round(progress * 100, 2) + "%";




            // Get Unconfirmed TX
            string path = "wallet/" + account + "/tx/unconfirmed";
            APIresponse = await APIGet(path, true);
            if (APIresponse == "Error")
            {
                AddLog("GetInfo Error");
                return;
            }
            JArray pendingTxs = JArray.Parse(APIresponse);
            labelPendingCount.Text = "Unconfirmed TX: " + pendingTxs.Count.ToString();


            // Get TX's
            APIresponse = await APIGet("wallet/" + account + "/tx/history", true);
            if (APIresponse == "Error")
            {
                AddLog("GetInfo Error");
                return;
            }
            JArray txs = JArray.Parse(APIresponse);
            int txCount = txs.Count;
            if (txCount > groupBoxTransactions.Height / 55) txCount = (int)Math.Floor(groupBoxTransactions.Height / 55.0);
            Control[] tmpControls = new Control[txCount];
            for (int i = 0; i < txCount; i++)
            {
                // Get last tx
                JObject tx = JObject.Parse(txs[txs.Count - 1 - i].ToString());
                string hash = tx["hash"].ToString();
                string date = tx["mdate"].ToString();

                Panel tmpPanel = new Panel();
                tmpPanel.Width = groupBoxTransactions.Width - 20;
                tmpPanel.Height = 50;
                tmpPanel.Location = new Point(10, 20 + (i * 55));
                tmpPanel.BorderStyle = BorderStyle.FixedSingle;
                tmpPanel.BackColor = ColorTranslator.FromHtml(theme["background-alt"]);
                tmpPanel.ForeColor = ColorTranslator.FromHtml(theme["foreground-alt"]);
                tmpPanel.Controls.Add(
                    new Label()
                    {
                        Text = "Date: " + date,
                        Location = new Point(10, 5)
                    }
                    );

                Label labelHash = new Label()
                {
                    Text = "Hash: " + hash.Substring(0, 10) + "..." + hash.Substring(hash.Length - 10),
                    AutoSize = true,
                    Location = new Point(10, 25),
                    Font = new Font(this.Font, FontStyle.Underline)
                };
                labelHash.Click += (sender, e) =>
                {
                    string url = userSettings["explorer-tx"] + hash;
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    };
                    Process.Start(psi);

                };
                tmpPanel.Controls.Add(labelHash);

                // Count inputs and outputs
                JArray inputs = JArray.Parse(tx["inputs"].ToString());
                JArray outputs = JArray.Parse(tx["outputs"].ToString());
                int inputCount = inputs.Count;
                int outputCount = outputs.Count;

                Label labelInputOutput = new Label()
                {
                    Text = "Inputs: " + inputCount + " Outputs: " + outputCount,
                    AutoSize = true,
                    Location = new Point(300, 20)
                };
                tmpPanel.Controls.Add(labelInputOutput);


                tmpControls[i] = tmpPanel;


            }
            groupBoxTransactions.Controls.Clear();
            groupBoxTransactions.Controls.AddRange(tmpControls);


        }
        private async Task<string> GetFee()
        {
            try
            {
                string response = await APIGet("fee", false);
                JObject resp = JObject.Parse(response);
                decimal fee = Convert.ToDecimal(resp["rate"].ToString());
                fee = fee / 1000000;
                if (fee < 0.0001m) fee = 1;

                return fee.ToString();
                //return resp["rate"].ToString();
            }
            catch
            {
                return "1";
            }
        }
        private async Task<bool> ValidAddress(string address)
        {
            string output = await APIPost("", false, "{\"method\": \"validateaddress\",\"params\": [ \"" + address + "\" ]}");
            JObject APIresp = JObject.Parse(output);
            JObject result = JObject.Parse(APIresp["result"].ToString());
            if (result["isvalid"].ToString() == "True") return true;
            else return false;
        }

        #endregion
        #region Timers
        private void timerNodeStatus_Tick(object sender, EventArgs e)
        {
            NodeStatus();
            // If logged in, update info
            if (panelaccount.Visible == false)
            {
                GetTXHistory();
            }
        }
        #endregion
        #region Nav
        private async void PortfolioPanel_Click(object sender, EventArgs e)
        {
            panelSend.Hide();
            panelPortfolio.Show();
            panelRecieve.Hide();
            buttonNavSend.BackColor = ColorTranslator.FromHtml(theme["background"]);
            buttonNavSend.ForeColor = ColorTranslator.FromHtml(theme["foreground"]);
            buttonNavReceive.BackColor = ColorTranslator.FromHtml(theme["background"]);
            buttonNavReceive.ForeColor = ColorTranslator.FromHtml(theme["foreground"]);
            await UpdateBalance();
            GetTXHistory();
            labelBalance.Text = "Available: " + balance.ToString() + " HNS";
            labelLocked.Text = "Locked: " + balanceLocked.ToString() + " HNS";
            labelBalanceTotal.Text = "Total: " + (balance + balanceLocked).ToString() + " HNS";
            if (theme.ContainsKey("selected-bg") && theme.ContainsKey("selected-fg"))
            {
                buttonNavPortfolio.BackColor = ColorTranslator.FromHtml(theme["selected-bg"]);
                buttonNavPortfolio.ForeColor = ColorTranslator.FromHtml(theme["selected-fg"]);
            }
        }

        private async void SendPanel_Click(object sender, EventArgs e)
        {
            panelPortfolio.Hide();
            panelSend.Show();
            panelRecieve.Hide();
            buttonNavPortfolio.BackColor = ColorTranslator.FromHtml(theme["background"]);
            buttonNavPortfolio.ForeColor = ColorTranslator.FromHtml(theme["foreground"]);
            buttonNavReceive.BackColor = ColorTranslator.FromHtml(theme["background"]);
            buttonNavReceive.ForeColor = ColorTranslator.FromHtml(theme["foreground"]);
            if (theme.ContainsKey("selected-bg") && theme.ContainsKey("selected-fg"))
            {
                buttonNavSend.BackColor = ColorTranslator.FromHtml(theme["selected-bg"]);
                buttonNavSend.ForeColor = ColorTranslator.FromHtml(theme["selected-fg"]);
            }
            if (theme.ContainsKey("error"))
            {
                labelSendingError.ForeColor = ColorTranslator.FromHtml(theme["error"]);
            }

            labelSendPrompt.Left = (panelSend.Width - labelSendPrompt.Width) / 2;
            buttonSendHNS.Left = (panelSend.Width - buttonSendHNS.Width) / 2;
            labelSendingTo.Left = (panelSend.Width - labelSendingTo.Width - textBoxSendingTo.Width) / 2;
            labelSendingAmount.Left = labelSendingTo.Left;
            textBoxSendingTo.Left = labelSendingTo.Left + labelSendingTo.Width + 10;
            textBoxSendingAmount.Left = textBoxSendingTo.Left;
            labelSendingMax.Left = labelSendingTo.Left;
            labelSendingError.Left = textBoxSendingTo.Left + textBoxSendingTo.Width + 10;
            labelSendingFee.Left = labelSendingTo.Left;
            buttonSendMax.Left = textBoxSendingAmount.Left + textBoxSendingAmount.Width - buttonSendMax.Width;
            checkBoxSendSubFee.Left = labelSendingTo.Left;

            labelSendingMax.Text = "Max: " + balance.ToString() + " HNS";
            textBoxSendingTo.Focus();
            string fee = await GetFee();
            labelSendingFee.Text = "Est. Fee: " + fee + " HNS";
            labelSendingError.Hide();

        }
        private async void ReceivePanel_Click(object sender, EventArgs e)
        {
            panelSend.Hide();
            panelPortfolio.Hide();
            panelRecieve.Show();
            buttonNavSend.BackColor = ColorTranslator.FromHtml(theme["background"]);
            buttonNavSend.ForeColor = ColorTranslator.FromHtml(theme["foreground"]);
            buttonNavPortfolio.BackColor = ColorTranslator.FromHtml(theme["background"]);
            buttonNavPortfolio.ForeColor = ColorTranslator.FromHtml(theme["foreground"]);


            if (theme.ContainsKey("selected-bg") && theme.ContainsKey("selected-fg"))
            {
                buttonNavReceive.BackColor = ColorTranslator.FromHtml(theme["selected-bg"]);
                buttonNavReceive.ForeColor = ColorTranslator.FromHtml(theme["selected-fg"]);
            }
            labelReceive1.Left = (panelRecieve.Width - labelReceive1.Width) / 2;
            labelReceive2.Left = (panelRecieve.Width - labelReceive2.Width) / 2;
            textBoxReceiveAddress.Left = (panelRecieve.Width - textBoxReceiveAddress.Width) / 2;


            string address = await GetAddress();
            textBoxReceiveAddress.Text = address;
            textBoxReceiveAddress.TextAlign = HorizontalAlignment.Center;
            Size size = TextRenderer.MeasureText(textBoxReceiveAddress.Text, textBoxReceiveAddress.Font);
            textBoxReceiveAddress.Width = size.Width + 10;
            textBoxReceiveAddress.Left = (panelRecieve.Width - textBoxReceiveAddress.Width) / 2;

            GeneratedBarcode Qrcode = QRCodeWriter.CreateQrCode(textBoxReceiveAddress.Text);
            pictureBoxReceiveQR.Image = Qrcode.Image;
            pictureBoxReceiveQR.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxReceiveQR.Width = panelRecieve.Width / 3;
            pictureBoxReceiveQR.Left = (panelRecieve.Width - pictureBoxReceiveQR.Width) / 2;



        }
        #endregion
        #region Send
        private async void textBoxSendingTo_Leave(object sender, EventArgs e)
        {
            if (textBoxSendingTo.Text == "") return;
            if (textBoxSendingTo.Text.Substring(0, 1) == "@")
            {
                labelSendingError.Show();
                labelSendingError.Text = "HIP-02 Not supported yet";
                return;
                /*
                string domain = textBoxSendingTo.Text.Substring(1);
                try
                {
                    string address = "";

                    bool valid = await ValidAddress(address);
                    if (valid)
                    {
                        labelSendingError.Hide();
                        labelSendingError.Text = "";
                    }
                    else
                    {
                        labelSendingError.Show();
                        labelSendingError.Text = "Invalid Address";
                    }
                }
                catch (Exception ex)
                {
                    labelSendingError.Show();
                    labelSendingError.Text = ex.Message;
                }*/
            }
            else
            {
                try
                {
                    bool valid = await ValidAddress(textBoxSendingTo.Text);
                    if (valid)
                    {
                        labelSendingError.Hide();
                        labelSendingError.Text = "";
                    }
                    else
                    {
                        labelSendingError.Show();
                        labelSendingError.Text = "Invalid Address";
                    }
                }
                catch (Exception ex)
                {
                    labelSendingError.Show();
                    labelSendingError.Text = ex.Message;
                }
            }
        }

        private void textBoxSendingAmount_Leave(object sender, EventArgs e)
        {
            decimal amount = 0;
            if (decimal.TryParse(textBoxSendingAmount.Text, out amount))
            {
                labelSendingError.Hide();
                labelSendingError.Text = "";
            }
            else
            {
                labelSendingError.Show();
                labelSendingError.Text = "Invalid Amount";
            }
        }

        private async void buttonSendMax_Click(object sender, EventArgs e)
        {
            string fee = await GetFee();
            decimal feeDecimal = decimal.Parse(fee);
            textBoxSendingAmount.Text = (balance - feeDecimal).ToString();
        }

        private async void buttonSendHNS_Click(object sender, EventArgs e)
        {
            try
            {
                string address = textBoxSendingTo.Text;
                bool valid = await ValidAddress(address);
                if (!valid)
                {
                    labelSendingError.Show();
                    labelSendingError.Text = "Invalid Address";
                    return;
                }
                decimal amount = 0;
                if (!decimal.TryParse(textBoxSendingAmount.Text, out amount))
                {
                    labelSendingError.Show();
                    labelSendingError.Text += " Invalid Amount";
                    return;
                }
                string feeString = await GetFee();
                decimal fee = decimal.Parse(feeString);
                string subtractFee = "false";
                if (checkBoxSendSubFee.Checked) subtractFee = "true";
                else if (amount > (balance - fee))
                {
                    labelSendingError.Show();
                    labelSendingError.Text += " Insufficient Funds";
                    return;
                }

                AddLog("Sending " + amount.ToString() + " HNS to " + address);
                string content = "{\"method\": \"sendtoaddress\",\"params\": [ \"" + address + "\", " +
                    amount.ToString() + ", \"\", \"\", " + subtractFee + " ]}";
                string output = await APIPost("", true, content);
                JObject APIresp = JObject.Parse(output);
                if (APIresp["error"].ToString() == "null")
                {
                    NotifyForm notify = new NotifyForm("Error Transaction Failed");
                    notify.ShowDialog();
                    return;
                }
                string hash = APIresp["result"].ToString();
                string link = userSettings["explorer-tx"] + hash;
                NotifyForm notifySuccess = new NotifyForm("Transaction Sent\nThis transaction could take up to 20 minutes to mine",
                    "Explorer", link);
                notifySuccess.ShowDialog();
                textBoxSendingTo.Text = "";
                textBoxSendingAmount.Text = "";
                labelSendingError.Hide();
                labelSendingError.Text = "";
                buttonNavPortfolio.PerformClick();

            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
                labelSendingError.Show();
                labelSendingError.Text = ex.Message;
            }
        }
        #endregion

        private void textBoxRecieveAddress_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBoxReceiveAddress.Text);
            labelReceive2.Text = "Copied to clipboard";
            labelReceive2.Left = (panelRecieve.Width - labelReceive2.Width) / 2;
        }
    }
}