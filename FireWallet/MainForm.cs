using System.Diagnostics;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using QRCoder;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Security.Policy;
using System.Windows.Forms;
using System.Net;

namespace FireWallet
{
    public partial class MainForm : Form
    {
        #region Variables
        public string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FireWallet\\";
        public Dictionary<string, string> nodeSettings { get; set; }
        public Dictionary<string, string> userSettings { get; set; }
        public Dictionary<string, string> theme { get; set; }
        public int network { get; set; }
        public string account { get; set; }
        public string password { get; set; }
        public decimal balance { get; set; }
        public decimal balanceLocked { get; set; }
        public int height { get; set; }
        public double syncProgress { get; set; }
        public int pendingTransactions { get; set; }
        public bool batchMode { get; set; }
        public BatchForm batchForm { get; set; }
        public bool watchOnly { get; set; }
        public bool HSD { get; set; }

        public Process hsdProcess { get; set; }
        #endregion
        #region Application
        public MainForm()
        {
            InitializeComponent();
            panelaccount.Visible = true;
        }
        private async void MainForm_Load(object sender, EventArgs e)
        {
            watchOnly = false;
            account = "";
            timerNodeStatus.Stop();
            LoadSettings();
            UpdateTheme();
            if (await LoadNode() != true) this.Close();

            if (userSettings.ContainsKey("hide-splash"))
            {
                if (userSettings["hide-splash"] == "false")
                {
                    // Show splash screen
                    SplashScreen ss = new SplashScreen();
                    ss.ShowDialog();
                    ss.Dispose();
                }
            }
            else
            {
                // Show splash screen
                SplashScreen ss = new SplashScreen();
                ss.ShowDialog();
                ss.Dispose();
            }



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

            // Prompt for login
            GetAccounts();

            AddLog("Loaded");
            Opacity = 1;
            batchMode = false;
            textBoxaccountpassword.Focus();
        }
        private void MainForm_Closing(object sender, FormClosingEventArgs e)
        {
            AddLog("Closing");
            if (hsdProcess != null)
            {
                this.Opacity = 0;
                hsdProcess.Kill();
                AddLog("HSD Closed");
                Thread.Sleep(1000);

                try
                {
                    hsdProcess.Dispose();
                }
                catch
                {
                    AddLog("Dispose failed");
                }
            }
        }
        #endregion




        #region Settings
        private async Task<bool> LoadNode()
        {
            HSD = false;
            if (!File.Exists(dir + "node.txt"))
            {
                NodeForm cf = new NodeForm();
                timerNodeStatus.Stop();
                cf.ShowDialog();
                timerNodeStatus.Start();
            }
            if (!File.Exists(dir + "node.txt"))
            {
                AddLog("Node setup failed");
                this.Close();
                await Task.Delay(1000);
                AddLog("Close Failed");
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
                await Task.Delay(1000);
                AddLog("Close Failed");
            }
            network = Convert.ToInt32(nodeSettings["Network"]);
            switch (network)
            {
                case 0:
                    toolStripStatusLabelNetwork.Text = "Network: Mainnet";
                    break;
                case 1:
                    toolStripStatusLabelNetwork.Text = "Network: Regtest (Not Fully Tested)";
                    break;
                case 2:
                    toolStripStatusLabelNetwork.Text = "Network: Testnet (Not Implemented)";
                    break;
            }

            if (nodeSettings.ContainsKey("HSD"))
            {
                if (nodeSettings["HSD"].ToLower() == "true")
                {
                    HSD = true;
                    AddLog("Starting HSD");
                    toolStripStatusLabelstatus.Text = "Status: HSD Starting";

                    if (!Directory.Exists(dir + "hsd"))
                    {
                        NotifyForm Notifyinstall = new NotifyForm("Installing hsd\nThis may take a few minutes\nDo not close FireWallet", false);
                        Notifyinstall.Show();
                        // Wait for the notification to show
                        await Task.Delay(1000);

                        string repositoryUrl = "https://github.com/handshake-org/hsd.git";
                        string destinationPath = dir + "hsd";
                        CloneRepository(repositoryUrl, destinationPath);

                        Notifyinstall.CloseNotification();
                        Notifyinstall.Dispose();
                    }
                    if (!Directory.Exists(dir + "hsd\\node_modules"))
                    {
                        AddLog("HSD install failed");
                        this.Close();
                        return false;
                    }

                    hsdProcess = new Process();

                    bool hideScreen = true;
                    if (nodeSettings.ContainsKey("HideScreen"))
                    {
                        if (nodeSettings["HideScreen"].ToLower() == "false")
                        {
                            hideScreen = false;
                        }
                    }
                    try
                    {
                        hsdProcess.StartInfo.CreateNoWindow = hideScreen;

                        if (hideScreen)
                        {
                            hsdProcess.StartInfo.RedirectStandardError = true;
                        } else
                        {
                            hsdProcess.StartInfo.RedirectStandardError = false;
                        }

                        hsdProcess.StartInfo.RedirectStandardInput = true;
                        hsdProcess.StartInfo.RedirectStandardOutput = false;
                        hsdProcess.StartInfo.UseShellExecute = false;
                        hsdProcess.StartInfo.FileName = "node.exe";
                        hsdProcess.StartInfo.Arguments = dir + "hsd/bin/hsd --agent=FireWallet --index-tx --index-address --api-key " + nodeSettings["Key"];

                        string bobPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Bob\\hsd_data";
                        if (Directory.Exists(bobPath))
                        {
                            hsdProcess.StartInfo.Arguments = hsdProcess.StartInfo.Arguments + " --prefix " + bobPath;
                        }
                        hsdProcess.Start();
                        // Wait for HSD to start
                        await Task.Delay(2000);
                        
                        // Check if HSD is running
                        if (hsdProcess.HasExited)
                        {
                            AddLog("HSD Failed to start");
                            AddLog(hsdProcess.StandardError.ReadToEnd());
                            NotifyForm Notifyinstall = new NotifyForm("HSD Failed to start\nPlease check the logs");
                            Notifyinstall.ShowDialog();
                            Notifyinstall.Dispose();

                            // Wait for the notification to show
                            await Task.Delay(1000);
                            this.Close();

                            await Task.Delay(1000);
                            return false;
                        }

                    }
                    catch (Exception ex)
                    {
                        AddLog("HSD Failed to start");
                        AddLog(ex.Message);
                        this.Close();
                        await Task.Delay(1000);
                    }
                }
            }
            timerNodeStatus.Start();
            NodeStatus();
            return true;

        }
        private void LoadSettings()
        {
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            if (!File.Exists(dir + "settings.txt"))
            {
                AddLog("Creating settings file");
                StreamWriter sw = new StreamWriter(dir + "settings.txt");
                sw.WriteLine("explorer-tx: https://niami.io/tx/");
                sw.WriteLine("explorer-addr: https://niami.io/address/");
                sw.WriteLine("explorer-block: https://niami.io/block/");
                sw.WriteLine("explorer-domain: https://niami.io/domain/");
                sw.WriteLine("confirmations: 1");
                sw.WriteLine("portfolio-tx: 20");
                sw.WriteLine("hide-splash: false");
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




            this.Width = Screen.PrimaryScreen.Bounds.Width / 5 * 3;
            this.Height = Screen.PrimaryScreen.Bounds.Height / 5 * 3;
            applyTransparency(theme);

            ResizeForm();
        }
        public void ThemeControl(Control c)
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
                || c.GetType() == typeof(ComboBox) || c.GetType() == typeof(StatusStrip) || c.GetType() == typeof(ToolStrip)
                || c.GetType() == typeof(NumericUpDown))
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
            groupBoxDomains.Width = panelDomains.Width - 20;
            groupBoxDomains.Left = 10;
            groupBoxDomains.Height = panelDomains.Height - groupBoxDomains.Top - 10;

            buttonNavSettings.Top = panelNav.Height - buttonNavSettings.Height - 10;
            buttonSettingsSave.Top = panelSettings.Height - buttonSettingsSave.Height - 10;
        }
        #endregion
        #region Accounts
        private void buttonaccountnew_Click(object sender, EventArgs e)
        {
            NewAccountForm newAccount = new NewAccountForm(this);
            newAccount.ShowDialog();
            newAccount.Dispose();
            GetAccounts();
        }
        private async void GetAccounts()
        {
            try
            {
                string APIresponse = await APIGet("wallet", true);
                comboBoxaccount.Items.Clear();
                if (APIresponse != "Error")
                {
                    comboBoxaccount.Enabled = true;
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
            }
            catch
            {
                AddLog("Error getting accounts");
            }
        }
        private async Task<bool> Login()
        {
            string path = "wallet/" + account + "/unlock";

            string content = "{\"passphrase\": \"" + password + "\",\"timeout\": 60}";
            if (password == "")
            {
                // For some reason, the API doesn't like an empty password, so we'll just use a default one
                content = "{\"passphrase\": \"password\" ,\"timeout\": 60}";
            }

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

            path = "wallet/" + account + "";
            APIresponse = await APIGet(path, true);
            JObject jObject = JObject.Parse(APIresponse);
            if (jObject["watchOnly"].ToString() == "True")
            {
                watchOnly = true;
                toolStripStatusLabelLedger.Text = "Cold Wallet";
                toolStripStatusLabelLedger.Visible = true;
            }
            else
            {
                watchOnly = false;
                toolStripStatusLabelLedger.Visible = false;
            }


            if (watchOnly)
            {
                buttonAddressVerify.Visible = true;
            }
            else
            {
                buttonAddressVerify.Visible = false;
            }
            return true;
        }

        private async void LoginClick(object sender, EventArgs e)
        {
            // If the node isn't connected show a message
            try
            {
                if (await APIGet("", false) == "Error")
                {
                    NotifyForm notifyForm = new NotifyForm("Node not connected");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                    return;
                }

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
                    buttonNavPortfolio.PerformClick();
                }
            }
            catch (Exception ex)
            {
                NotifyForm notifyForm = new NotifyForm("Node not connected\n" + ex.Message);
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                return;
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
            password = ""; // Clear password from memory as soon as possible
            toolStripSplitButtonlogout.Visible = false;
            toolStripStatusLabelLedger.Visible = false;
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
            hidePages();
            toolStripStatusLabelaccount.Text = "Account: Not Logged In";
            textBoxaccountpassword.Focus();
        }
        #endregion
        #region API
        HttpClient httpClient = new HttpClient();
        private async void NodeStatus()
        {

            if (await APIGet("", false) == "Error")
            {
                if (toolStripStatusLabelstatus.Text != "Status: HSD Starting")
                {
                    toolStripStatusLabelstatus.Text = "Status: Node Not Connected";
                }
                return;
            }
            else
            {
                if (toolStripStatusLabelstatus.Text != "Status: Node Connected") GetAccounts(); // Get accounts if node was not connected before
                toolStripStatusLabelstatus.Text = "Status: Node Connected";
            }
            if (account == "") return; // Don't update balance if not logged in

            // Try to keep wallet unlocked
            string path = "wallet/" + account + "/unlock";
            string content = "{\"passphrase\": \"" + password + "\",\"timeout\": 60}";

            await APIPost(path, true, content);

            path = "";
            content = "{\"method\": \"selectwallet\",\"params\":[ \"" + account + "\"]}";

            await APIPost(path, true, content);

        }
        private async Task UpdateBalance()
        {
            string response = await APIGet("wallet/" + account + "/balance?account=default", true);
            if (response == "Error") return;

            JObject resp = JObject.Parse(response);

            decimal available = (Convert.ToDecimal(resp["unconfirmed"].ToString()) - Convert.ToDecimal(resp["lockedUnconfirmed"].ToString())) / 1000000;
            decimal locked = Convert.ToDecimal(resp["lockedUnconfirmed"].ToString()) / 1000000;
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
        public async Task<string> APIPost(string path, bool wallet, string content)
        {
            if (content == "{\"passphrase\": \"\" ,\"timeout\": 60}")
            {
                // For some reason, the API doesn't like an empty password, so we'll just use a default one
                content = "{\"passphrase\": \"password\" ,\"timeout\": 60}";
            }
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
        public async Task<string> APIGet(string path, bool wallet)
        {
            if (nodeSettings == null) return "Error";
            if (!nodeSettings.ContainsKey("Key") || !nodeSettings.ContainsKey("IP")) return "Error";
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

            // Exit if set to 0 TXs
            if (userSettings.ContainsKey("portfolio-tx"))
            {
                if (userSettings["portfolio-tx"] == "0") return;
            }


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
            if (userSettings.ContainsKey("portfolio-tx")) txCount = Convert.ToInt32(userSettings["portfolio-tx"]);
            if (txCount > txs.Count) txCount = txs.Count;
            Control[] tmpControls = new Control[txCount];
            for (int i = 0; i < txCount; i++)
            {

                // Get last tx
                JObject tx = JObject.Parse(txs[txs.Count - 1 - i].ToString());

                string hash = tx["hash"].ToString();
                string date = tx["mdate"].ToString();

                Panel tmpPanel = new Panel();
                tmpPanel.Width = groupBoxTransactions.Width - SystemInformation.VerticalScrollBarWidth - 20;
                tmpPanel.Height = 50;
                tmpPanel.Location = new Point(5, (i * 55));
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
                int confirmations = Convert.ToInt32(tx["confirmations"].ToString());
                if (userSettings.ContainsKey("confirmations"))
                {
                    if (confirmations < Convert.ToInt32(userSettings["confirmations"]))
                    {
                        Label txPending = new Label()
                        {
                            Text = "Pending",
                            Location = new Point(100, 5)
                        };
                        tmpPanel.Controls.Add(txPending);
                        txPending.BringToFront();
                    }
                }
                Label labelHash = new Label()
                {
                    Text = "Hash: " + hash.Substring(0, 10) + "..." + hash.Substring(hash.Length - 10),
                    AutoSize = true,
                    Location = new Point(10, 25)
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
                tmpPanel.Click += (sender, e) =>
                {
                    TXForm txForm = new TXForm(this, tx);
                    txForm.Show();
                };
                tmpControls[i] = tmpPanel;


            }
            groupBoxTransactions.Controls.Clear();
            Panel txPanel = new Panel();
            txPanel.Width = groupBoxTransactions.Width - SystemInformation.VerticalScrollBarWidth;
            txPanel.Controls.AddRange(tmpControls);
            txPanel.AutoScroll = true;
            txPanel.Dock = DockStyle.Fill;
            groupBoxTransactions.Controls.Add(txPanel);


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
        public async Task<bool> ValidAddress(string address)
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
        }
        #endregion
        #region Nav
        private async void PortfolioPanel_Click(object sender, EventArgs e)
        {
            hidePages();
            panelPortfolio.Show();
            await UpdateBalance();
            GetTXHistory();
            labelBalance.Text = "Available: " + balance.ToString() + " HNS";
            labelLocked.Text = "Locked: " + balanceLocked.ToString() + " HNS*";
            labelBalanceTotal.Text = "Total: " + (balance + balanceLocked).ToString() + " HNS";
            if (theme.ContainsKey("selected-bg") && theme.ContainsKey("selected-fg"))
            {
                buttonNavPortfolio.BackColor = ColorTranslator.FromHtml(theme["selected-bg"]);
                buttonNavPortfolio.ForeColor = ColorTranslator.FromHtml(theme["selected-fg"]);
            }
        }

        private async void SendPanel_Click(object sender, EventArgs e)
        {
            hidePages();
            panelSend.Show();
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
            hidePages();
            panelRecieve.Show();

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

            QRCodeGenerator qrcode = new QRCodeGenerator();
            QRCodeData qrData = qrcode.CreateQrCode(textBoxReceiveAddress.Text, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrData);
            pictureBoxReceiveQR.Image = qrCode.GetGraphic(20, theme["foreground"], theme["background"]);
            pictureBoxReceiveQR.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxReceiveQR.Width = panelRecieve.Width / 3;
            pictureBoxReceiveQR.Left = (panelRecieve.Width - pictureBoxReceiveQR.Width) / 2;



        }
        private void buttonNavDomains_Click(object sender, EventArgs e)
        {
            hidePages();
            panelDomains.Show();

            if (theme.ContainsKey("selected-bg") && theme.ContainsKey("selected-fg"))
            {
                buttonNavDomains.BackColor = ColorTranslator.FromHtml(theme["selected-bg"]);
                buttonNavDomains.ForeColor = ColorTranslator.FromHtml(theme["selected-fg"]);
            }
            textBoxDomainSearch.Focus();
            groupBoxDomains.Width = panelDomains.Width - 20;
            groupBoxDomains.Left = 10;
            groupBoxDomains.Height = panelDomains.Height - groupBoxDomains.Top - 10;
            UpdateDomains();

        }
        private void hidePages()
        {
            panelSend.Hide();
            panelPortfolio.Hide();
            panelRecieve.Hide();
            panelDomains.Hide();
            panelSettings.Hide();
            buttonNavPortfolio.BackColor = ColorTranslator.FromHtml(theme["background"]);
            buttonNavPortfolio.ForeColor = ColorTranslator.FromHtml(theme["foreground"]);
            buttonNavSend.BackColor = ColorTranslator.FromHtml(theme["background"]);
            buttonNavSend.ForeColor = ColorTranslator.FromHtml(theme["foreground"]);
            buttonNavReceive.BackColor = ColorTranslator.FromHtml(theme["background"]);
            buttonNavReceive.ForeColor = ColorTranslator.FromHtml(theme["foreground"]);
            buttonNavDomains.BackColor = ColorTranslator.FromHtml(theme["background"]);
            buttonNavDomains.ForeColor = ColorTranslator.FromHtml(theme["foreground"]);
            buttonNavSettings.BackColor = ColorTranslator.FromHtml(theme["background"]);
            buttonNavSettings.ForeColor = ColorTranslator.FromHtml(theme["foreground"]);
        }
        private void buttonNavSettings_Click(object sender, EventArgs e)
        {
            hidePages();
            if (theme.ContainsKey("selected-bg") && theme.ContainsKey("selected-fg"))
            {
                buttonNavSettings.BackColor = ColorTranslator.FromHtml(theme["selected-bg"]);
                buttonNavSettings.ForeColor = ColorTranslator.FromHtml(theme["selected-fg"]);
            }

            panelSettings.Show();
            buttonSettingsSave.Top = panelSettings.Height - buttonSettingsSave.Height - 10;
            labelSettingsSaved.Top = buttonSettingsSave.Top + 10;
            textBoxExTX.Text = userSettings["explorer-tx"];
            textBoxExAddr.Text = userSettings["explorer-addr"];
            textBoxExBlock.Text = userSettings["explorer-block"];
            textBoxExName.Text = userSettings["explorer-domain"];
            numericUpDownConfirmations.Value = int.Parse(userSettings["confirmations"]);
            numericUpDownTXCount.Value = int.Parse(userSettings["portfolio-tx"]);
            labelSettingsSaved.Hide();
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

                if (!watchOnly)
                {

                    AddLog("Sending " + amount.ToString() + " HNS to " + address);
                    string content = "{\"method\": \"sendtoaddress\",\"params\": [ \"" + address + "\", " +
                        amount.ToString() + ", \"\", \"\", " + subtractFee + " ]}";
                    string output = await APIPost("", true, content);
                    JObject APIresp = JObject.Parse(output);
                    if (APIresp["error"].ToString() != "")
                    {
                        AddLog("Failed:");
                        AddLog(APIresp.ToString());
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
                else // Cold wallet signing
                {
                    AddLog("Sending CW " + amount.ToString() + " HNS to " + address);

                    if (!Directory.Exists(dir + "hsd-ledger"))
                    {
                        if (CheckNodeInstalled() == false)
                        {
                            AddLog("Node not installed");
                            NotifyForm notify1 = new NotifyForm("Node not installed\nPlease install it to use Ledger");
                            notify1.ShowDialog();
                            notify1.Dispose();
                            return;
                        }
                        AddLog("Installing hsd-ledger");

                        // Try to install hsd-ledger
                        try
                        {
                            NotifyForm Notifyinstall = new NotifyForm("Installing hsd-ledger\nThis may take a few minutes\nDo not close FireWallet", false);
                            Notifyinstall.Show();
                            // Wait for the notification to show
                            await Task.Delay(1000);

                            string repositoryUrl = "https://github.com/handshake-org/hsd-ledger.git";
                            string destinationPath = dir + "hsd-ledger";
                            CloneRepository(repositoryUrl, destinationPath);

                            Notifyinstall.CloseNotification();
                            Notifyinstall.Dispose();
                        }
                        catch (Exception ex)
                        {
                            NotifyForm notifyError = new NotifyForm("Error installing hsd-ledger\n" + ex.Message);
                            AddLog(ex.Message);
                            notifyError.ShowDialog();
                            notifyError.Dispose();
                            return;
                        }

                    }

                    NotifyForm notify = new NotifyForm("Please confirm the transaction on your Ledger device", false);
                    notify.Show();

                    var proc = new Process();
                    proc.StartInfo.CreateNoWindow = true;
                    proc.StartInfo.RedirectStandardInput = true;
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.RedirectStandardError = true;
                    proc.StartInfo.FileName = "node.exe";
                    proc.StartInfo.Arguments = dir + "hsd-ledger/bin/hsd-ledger sendtoaddress " + textBoxSendingTo.Text
                        + " " + textBoxSendingAmount.Text + " --api-key " + nodeSettings["Key"] + " -w " + account;
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

                    notify.CloseNotification();
                    notify.Dispose();

                    string output = outputBuilder.ToString();
                    AddLog(output);
                    if (output.Contains("Submitted TXID"))
                    {
                        string hash = output.Substring(output.IndexOf("Submitted TXID") + 16, 64);
                        string link = userSettings["explorer-tx"] + hash;
                        NotifyForm notifySuccess = new NotifyForm("Transaction Sent\nThis transaction could take up to 20 minutes to mine",
                                                       "Explorer", link);
                        notifySuccess.ShowDialog();
                        textBoxSendingTo.Text = "";
                        textBoxSendingAmount.Text = "";
                        buttonNavPortfolio.PerformClick();
                    }
                    else
                    {
                        NotifyForm notifyError = new NotifyForm("Error Transaction Failed\nCheck logs for more details");
                        notifyError.ShowDialog();
                        notifyError.Dispose();
                    }

                }

            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
                labelSendingError.Show();
                labelSendingError.Text = ex.Message;
            }
        }
        public void CloneRepository(string repositoryUrl, string destinationPath)
        {
            try
            {
                bool hideScreen = true;
                if (nodeSettings.ContainsKey("HideScreen"))
                {
                    if (nodeSettings["HideScreen"].ToLower() == "false")
                    {
                        hideScreen = false;
                    }
                }

                // Check if git is installed
                Process testInstalled = new Process();
                testInstalled.StartInfo.FileName = "git";
                testInstalled.StartInfo.Arguments = "-v";
                testInstalled.StartInfo.RedirectStandardOutput = true;
                testInstalled.StartInfo.UseShellExecute = false;
                testInstalled.StartInfo.CreateNoWindow = true;
                testInstalled.Start();
                string outputInstalled = testInstalled.StandardOutput.ReadToEnd();
                testInstalled.WaitForExit();

                if (!outputInstalled.Contains("git version"))
                {
                    AddLog("Git is not installed");
                    NotifyForm notifyForm = new NotifyForm("Git is not installed\nPlease install it to install HSD dependencies");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                    this.Close();
                    return;
                }

                // Check if node installed
                testInstalled = new Process();
                testInstalled.StartInfo.FileName = "node";
                testInstalled.StartInfo.Arguments = "-v";
                testInstalled.StartInfo.RedirectStandardOutput = true;
                testInstalled.StartInfo.UseShellExecute = false;
                testInstalled.StartInfo.CreateNoWindow = true;
                testInstalled.Start();
                outputInstalled = testInstalled.StandardOutput.ReadToEnd();
                testInstalled.WaitForExit();

                if (!outputInstalled.Contains("v"))
                {
                    AddLog("Node is not installed");
                    NotifyForm notifyForm = new NotifyForm("Node is not installed\nPlease install it to install HSD dependencies");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                    this.Close();
                    return;
                }


                // Check if npm installed
                testInstalled = new Process();
                testInstalled.StartInfo.FileName = "npm";
                testInstalled.StartInfo.Arguments = "-v";
                testInstalled.StartInfo.RedirectStandardOutput = true;
                testInstalled.StartInfo.UseShellExecute = false;
                testInstalled.StartInfo.CreateNoWindow = true;
                testInstalled.Start();
                outputInstalled = testInstalled.StandardOutput.ReadToEnd();
                testInstalled.WaitForExit();

                if (outputInstalled.Length < 3)
                {
                    AddLog("NPM is not installed");
                    NotifyForm notifyForm = new NotifyForm("NPM is not installed\nPlease install it to install HSD dependencies");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                    this.Close();
                    return;
                }



                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "git";
                startInfo.Arguments = $"clone {repositoryUrl} {destinationPath}";
                startInfo.RedirectStandardOutput = true;
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = hideScreen;

                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                while (!process.HasExited)
                {
                    output += process.StandardOutput.ReadToEnd();
                }
                var psiNpmRunDist = new ProcessStartInfo
                {
                    FileName = "cmd",
                    RedirectStandardInput = true,
                    WorkingDirectory = destinationPath,
                    CreateNoWindow = hideScreen
                };
                var pNpmRunDist = Process.Start(psiNpmRunDist);
                pNpmRunDist.StandardInput.WriteLine("npm install & exit");
                pNpmRunDist.WaitForExit();

                if (repositoryUrl == "https://github.com/handshake-org/hsd-ledger.git")
                {
                    // Replace /bin/hsd-ledger with /bin/hsd-ledger from
                    // https://raw.githubusercontent.com/Nathanwoodburn/FireWallet/master/hsd-ledger
                    // This version of hsd-ledger has the sendraw transaction function added which is needed for batching

                    string sourcePath = destinationPath + "\\bin\\hsd-ledger";
                    File.Delete(sourcePath);

                    // Download the new hsd-ledger
                    WebClient downloader = new WebClient();
                    downloader.DownloadFile("https://raw.githubusercontent.com/Nathanwoodburn/FireWallet/master/hsd-ledger", sourcePath);
                }
            }
            catch (Exception ex)
            {
                AddLog("Git/NPM Install FAILED");
                AddLog(ex.Message);
                if (ex.Message.Contains("to start process 'git'"))
                {
                    NotifyForm notifyForm = new NotifyForm("Git needs to be installed\nCheck logs for more details");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                }
                else if (ex.Message.Contains("to start process 'node'"))
                {
                    NotifyForm notifyForm = new NotifyForm("Node needs to be installed\nCheck logs for more details");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                }
                else if (ex.Message.Contains("to start process 'npm'"))
                {
                    NotifyForm notifyForm = new NotifyForm("NPM needs to be installed\nCheck logs for more details");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                }
                else
                {

                    NotifyForm notifyForm = new NotifyForm("Git/NPM Install FAILED\nCheck logs for more details");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                }
                this.Close();
            }
        }
        public bool CheckNodeInstalled()
        {
            try
            {
                // Create a new process to execute the 'node' command
                Process process = new Process();
                process.StartInfo.FileName = "node";
                process.StartInfo.Arguments = "--version";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                // Start the process and read the output
                process.Start();
                string output = process.StandardOutput.ReadToEnd();

                // Wait for the process to exit
                process.WaitForExit();

                // Check if the output contains a version number
                return !string.IsNullOrEmpty(output);
            }
            catch (Exception)
            {
                // An exception occurred, indicating that 'node' is not installed or accessible
                return false;
            }
        }
        #endregion
        #region Receive
        private void textBoxRecieveAddress_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBoxReceiveAddress.Text);
            labelReceive2.Text = "Copied to clipboard";
            labelReceive2.Left = (panelRecieve.Width - labelReceive2.Width) / 2;
        }
        private async void buttonAddressVerify_Click(object sender, EventArgs e)
        {

            var proc = new Process();
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.FileName = "node.exe";
            proc.StartInfo.Arguments = dir + "hsd-ledger/bin/hsd-ledger createaddress --api-key " + nodeSettings["Key"] + " -w " + account;
            proc.Start();

            // Wait 1 sec
            await Task.Delay(1000);

            // Get the output into a string
            string result = proc.StandardOutput.ReadLine();
            try
            {
                result = result.Replace("Verify address on Ledger device: ", "");
            }
            catch { }
            AddLog(result);
            textBoxReceiveAddress.Text = result;
            Size size = TextRenderer.MeasureText(textBoxReceiveAddress.Text, textBoxReceiveAddress.Font);
            textBoxReceiveAddress.Width = size.Width + 10;
            textBoxReceiveAddress.Left = (panelRecieve.Width - textBoxReceiveAddress.Width) / 2;


            NotifyForm notify = new NotifyForm("Please confirm the address on your Ledger device", false);
            notify.Show();

            // Handle events until process exits
            while (!proc.HasExited)
            {
                // Check for cancellation
                if (proc.WaitForExit(100))
                    break;
                Application.DoEvents();
            }

            notify.CloseNotification();
            notify.Dispose();

        }
        #endregion
        #region Domains
        public string[] Domains { get; set; }
        public string[] DomainsRenewable { get; set; }
        private async void UpdateDomains()
        {
            string response = await APIGet("wallet/" + account + "/name?own=true", true);
            JArray names = JArray.Parse(response);
            Domains = new string[names.Count];
            DomainsRenewable = new string[names.Count];
            int i = 0;
            int renewable = 0;
            panelDomainList.Controls.Clear();
            foreach (JObject name in names)
            {
                Domains[i] = name["name"].ToString();
                Panel domainTMP = new Panel();
                domainTMP.Width = panelDomainList.Width - 20 - SystemInformation.VerticalScrollBarWidth;
                domainTMP.Height = 30;
                domainTMP.Top = 30 * (i);
                domainTMP.Left = 10;
                domainTMP.BorderStyle = BorderStyle.FixedSingle;

                // On Click open domain
                domainTMP.Click += new EventHandler((sender, e) =>
                {
                    DomainForm domainForm = new DomainForm(this, name["name"].ToString(), userSettings["explorer-tx"], userSettings["explorer-domain"]);
                    domainForm.Show();
                });

                Label domainName = new Label();
                domainName.Text = Domains[i];
                domainName.Top = 5;
                domainName.Left = 5;
                domainName.AutoSize = true;


                domainTMP.Controls.Add(domainName);

                if (!name.ContainsKey("stats")) {
                    AddLog("Domain " + Domains[i] + " does not have stats");
                    continue;
                }
                Label expiry = new Label();
                JObject stats = JObject.Parse(name["stats"].ToString());
                if (stats.ContainsKey("daysUntilExpire"))
                {
                    expiry.Text = "Expires: " + stats["daysUntilExpire"].ToString() + " days";
                    expiry.Top = 5;
                    expiry.AutoSize = true;
                    expiry.Left = domainTMP.Width - expiry.Width - 100;
                    domainTMP.Controls.Add(expiry);

                    // Add to domains renewable
                    DomainsRenewable[renewable] = Domains[i];
                    renewable++;

                }
                else
                {
                    expiry.Text = "Expires: Not Registered yet";
                    expiry.Top = 5;
                    expiry.AutoSize = true;
                    expiry.Left = domainTMP.Width - expiry.Width - 100;
                    domainTMP.Controls.Add(expiry);
                }




                panelDomainList.Controls.Add(domainTMP);
                i++;
            }
        }
        private async void buttonRevealAll_Click(object sender, EventArgs e)
        {
            string content = "{\"method\": \"sendreveal\"}";
            string response = await APIPost("", true, content);
            AddLog(response);
            if (response == "Error")
            {
                AddLog("Error sending reveal");
                NotifyForm notifyForm = new NotifyForm("Error sending reveal");
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                return;
            }
            JObject resp = JObject.Parse(response);
            if (resp["error"] != null)
            {
                AddLog("Error sending reveal");
                AddLog(resp["error"].ToString());
                JObject error = JObject.Parse(resp["error"].ToString());
                NotifyForm notifyForm = new NotifyForm("Error sending reveal\n" + error["message"].ToString());
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                return;
            }
            if (resp.ContainsKey("result"))
            {
                JObject result = JObject.Parse(resp["result"].ToString());
                string hash = result["hash"].ToString();
                NotifyForm notifyForm = new NotifyForm("Reveal sent\n" + hash, "Explorer", userSettings["explorer-tx"] + hash);
                notifyForm.ShowDialog();
                notifyForm.Dispose();
            }
        }
        private void textBoxDomainSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBoxDomainSearch.Text = textBoxDomainSearch.Text.Trim().ToLower();
                e.SuppressKeyPress = true;
                DomainForm domainForm = new DomainForm(this, textBoxDomainSearch.Text, userSettings["explorer-tx"], userSettings["explorer-domain"]);

                domainForm.Show();

            }
        }
        private void textBoxDomainSearch_TextChanged(object sender, EventArgs e)
        {
            string domainSearch = textBoxDomainSearch.Text;
            domainSearch = Regex.Replace(textBoxDomainSearch.Text, "[^a-zA-Z0-9-_]", "");
            textBoxDomainSearch.Text = domainSearch;
        }

        #endregion
        #region Batching
        public void AddBatch(string domain, string operation)
        {
            if (operation == "BID") return;
            if (!batchMode)
            {
                batchForm = new BatchForm(this);
                batchForm.Show();
                batchMode = true;
            }
            batchForm.AddBatch(domain, operation);

        }
        public void AddBatch(string domain, string operation, decimal bid, decimal lockup)
        {
            if (operation != "BID") return;
            if (!batchMode)
            {
                batchForm = new BatchForm(this);
                batchForm.Show();
                batchMode = true;
            }
            batchForm.AddBatch(domain, operation, bid, lockup);

        }
        public void AddBatch(string domain, string operation, DNS[] updateRecords)
        {
            if (!batchMode)
            {
                batchForm = new BatchForm(this);
                batchForm.Show();
                batchMode = true;
            }
            batchForm.AddBatch(domain, operation, updateRecords);
        }
        public void AddBatch(string domain, string operation, string address)
        {
            if (!batchMode)
            {
                batchForm = new BatchForm(this);
                batchForm.Show();
                batchMode = true;
            }
            batchForm.AddBatch(domain, operation, address);
        }
        public void FinishBatch()
        {
            batchMode = false;
            batchForm.Dispose();
        }
        private void buttonBatch_Click(object sender, EventArgs e)
        {
            if (!batchMode)
            {
                batchForm = new BatchForm(this);
                batchForm.Show();
                batchMode = true;
            }
            else batchForm.Focus();
        }
        #endregion
        #region SettingsPage
        private void buttonSettingsSave_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter(dir + "settings.txt");
            sw.WriteLine("explorer-tx: " + textBoxExTX.Text);
            sw.WriteLine("explorer-addr: " + textBoxExAddr.Text);
            sw.WriteLine("explorer-block: " + textBoxExBlock.Text);
            sw.WriteLine("explorer-domain: " + textBoxExName.Text);
            sw.WriteLine("confirmations: " + numericUpDownConfirmations.Value);
            sw.WriteLine("portfolio-tx: " + numericUpDownTXCount.Value);
            sw.WriteLine("hide-splash: " + userSettings["hide-splash"]);
            sw.Dispose();
            LoadSettings();
            labelSettingsSaved.Show();
        }
        private async void buttonSeed_Click(object sender, EventArgs e)
        {
            string path = "wallet/" + account + "/master";
            string response = await APIGet(path, true);
            JObject resp = JObject.Parse(response);
            if (resp["encrypted"].ToString() == "False")
            {
                JObject mnemonic = JObject.Parse(resp["mnemonic"].ToString());
                string phrase = mnemonic["phrase"].ToString();
                NotifyForm notifyForm = new NotifyForm("Your seed phrase is:\n" + phrase, "Copy", phrase, true);
                notifyForm.ShowDialog();
                notifyForm.Dispose();
            }
            else
            {
                string algorithm = resp["algorithm"].ToString();
                if (algorithm != "pbkdf2")
                {
                    AddLog("Invalid algorithm");
                    AddLog(response);
                    NotifyForm notifyForm = new NotifyForm("Invalid algorithm");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                    return;
                }

                try
                {
                    string iv = resp["iv"].ToString();
                    string ciphertext = resp["ciphertext"].ToString();
                    string tmpn = resp["n"].ToString();
                    string tmpr = resp["r"].ToString();
                    string tmpp = resp["p"].ToString();

                    int n = int.Parse(tmpn);
                    int p = int.Parse(tmpp);

                    int iterations = n;


                }
                catch (Exception ex)
                {
                    AddLog("Error decrypting seed");
                    AddLog(ex.ToString());
                    NotifyForm notifyForm = new NotifyForm("Error decrypting seed");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                }


            }
        }

        private async void Rescan_Click(object sender, EventArgs e)
        {
            string content = "{\"height\": 0}";
            string response = await APIPost("rescan", true, content);
            if (!response.Contains("true"))
            {
                AddLog("Error starting rescan");
                AddLog(response);
                return;
            }
            AddLog("Starting rescan");

        }
        #endregion

        private void export_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV file (*.csv)|*.csv";
            saveFileDialog.Title = "Export";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
                foreach (string domain in DomainsRenewable)
                {
                    if (domain == null) break;
                    sw.WriteLine(domain);
                }
                sw.Dispose();
            }
        }

        private void buttonRenewAll_Click(object sender, EventArgs e)
        {
            if (DomainsRenewable == null)
            {
                NotifyForm notifyForm = new NotifyForm("No renewable domains found");
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                return;
            }
            foreach (string domain in DomainsRenewable)
            {
                if (domain == null) break;
                AddBatch(domain, "RENEW");
            }
        }
    }
}