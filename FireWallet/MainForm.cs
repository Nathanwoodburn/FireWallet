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
using DnsClient;
using DnsClient.Protocol;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace FireWallet
{
    public partial class MainForm : Form
    {
        #region Variables
        // Main directory for the application files
        public string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FireWallet\\";
        // Settings and theme key value pairs
        public Dictionary<string, string> NodeSettings { get; set; }
        public Dictionary<string, string> UserSettings { get; set; }
        public Dictionary<string, string> Theme { get; set; }
        // HSD & Wallet settings
        public bool WatchOnly { get; set; }
        public bool HSD { get; set; }
        public Process HSDProcess { get; set; }
        public int HSDNetwork { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        // Wallet information
        public decimal Balance { get; set; }
        public decimal Balance_Locked { get; set; }
        // Batching variables
        public bool BatchMode { get; set; }
        public BatchForm BatchForm { get; set; }
        #endregion
        #region Application
        public MainForm()
        {
            InitializeComponent();
            panelaccount.Visible = true;
        }
        private async void MainForm_Load(object sender, EventArgs e)
        {
            // Set initial values
            WatchOnly = false;
            Account = "";

            // Stop timers and load settings
            timerNodeStatus.Stop();
            LoadSettings();

            // Show splash
            SplashScreen ss = new SplashScreen(false);
            bool splash = false;
            if (UserSettings.ContainsKey("hide-splash"))
            {
                if (UserSettings["hide-splash"] == "false")
                {
                    // Show splash screen
                    ss.Show();
                    splash = true;
                }
            }
            else
            {
                // Show splash screen
                ss.Show();           
                splash = true;
            }
            // Record time
            DateTime start = DateTime.Now;
            // Do form events
            Application.DoEvents();

            // Load theme
            UpdateTheme();
            // Theme drop down
            foreach (ToolStripItem item in toolStripDropDownButtonHelp.DropDownItems)
            {
                if (item is ToolStripDropDownItem dropDownItem)
                {
                    dropDownItem.ForeColor = ColorTranslator.FromHtml(Theme["foreground"]);
                    dropDownItem.BackColor = ColorTranslator.FromHtml(Theme["background"]);
                }
            }
            toolStripDropDownButtonHelp.DropDown.BackColor = ColorTranslator.FromHtml(Theme["background"]);

            // Load node
            if (await LoadNode() != true) this.Close();

            // If node load caused app to close, exit load function
            if (this.Disposing || this.IsDisposed) return;

            // Edit the theme of the navigation panel
            panelNav.BackColor = ColorTranslator.FromHtml(Theme["background-alt"]);
            panelNav.ForeColor = ColorTranslator.FromHtml(Theme["foreground-alt"]);
            foreach (Control c in panelNav.Controls)
            {
                c.BackColor = ColorTranslator.FromHtml(Theme["background"]);
                c.ForeColor = ColorTranslator.FromHtml(Theme["foreground"]);
            }
            panelNav.Dock = DockStyle.Left;

            ResizeForm();
            panelNav.Visible = false;

            // Prompt for login
            GetAccounts();
            BatchMode = false;

            if (splash)
            {
                // If not internal node
                if (!HSD)
                {
                    // Wait until the splash has been visible for 4 seconds
                    if ((DateTime.Now - start).TotalSeconds > 4) ss.CloseSplash();
                    else Thread.Sleep(4000 - (int)(DateTime.Now - start).TotalMilliseconds);
                    ss.CloseSplash();
                }
                else
                {
                    // Wait until the Node is connected before closing the splash
                    while (true)
                    {
                        string status = await APIGet("",false);
                        if (status != "Error")
                        {
                            ss.CloseSplash();
                            GetAccounts();
                            break;
                        }
                        else
                        {
                            Thread.Sleep(100);
                        }
                    }
                }
                Application.DoEvents();
            }
            AddLog("Loaded");

            // Pull form to front
            Opacity = 1;
            this.WindowState = FormWindowState.Minimized;
            this.Show();
            this.WindowState = FormWindowState.Normal;
            textBoxaccountpassword.Focus();
        }
        private void MainForm_Closing(object sender, FormClosingEventArgs e)
        {
            // Close HSD before closing FireWallet
            AddLog("Closing");
            if (HSDProcess != null)
            {
                this.Opacity = 0;
                HSDProcess.Kill();
                AddLog("HSD Closed");
                Thread.Sleep(1000);
                try
                {
                    HSDProcess.Dispose();
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
            NodeSettings = new Dictionary<string, string>();
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] split = line.Split(':');
                NodeSettings.Add(split[0].Trim(), split[1].Trim());

            }
            sr.Dispose();

            if (!NodeSettings.ContainsKey("Network") || !NodeSettings.ContainsKey("Key") || !NodeSettings.ContainsKey("IP"))
            {
                AddLog("Node Settings file is missing key");
                this.Close();
                await Task.Delay(1000);
                AddLog("Close Failed");
            }
            HSDNetwork = Convert.ToInt32(NodeSettings["Network"]);
            switch (HSDNetwork)
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

            if (NodeSettings.ContainsKey("HSD"))
            {
                if (NodeSettings["HSD"].ToLower() == "true")
                {
                    HSD = true;
                    AddLog("Starting HSD");
                    toolStripStatusLabelstatus.Text = "Status: HSD Starting";

                    string hsdPath = dir + "hsd\\bin\\hsd.exe";
                    if (NodeSettings.ContainsKey("HSD-command"))
                    {
                        if (NodeSettings["HSD-command"].Contains("{default-dir}"))
                        {
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
                        }
                    }
                    else
                    {
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
                    }



                    HSDProcess = new Process();

                    bool hideScreen = true;
                    if (NodeSettings.ContainsKey("HideScreen"))
                    {
                        if (NodeSettings["HideScreen"].ToLower() == "false")
                        {
                            hideScreen = false;
                        }
                    }
                    try
                    {
                        HSDProcess.StartInfo.CreateNoWindow = hideScreen;

                        if (hideScreen)
                        {
                            HSDProcess.StartInfo.RedirectStandardError = true;
                        }
                        else
                        {
                            HSDProcess.StartInfo.RedirectStandardError = false;
                        }

                        HSDProcess.StartInfo.RedirectStandardInput = true;
                        HSDProcess.StartInfo.RedirectStandardOutput = false;
                        HSDProcess.StartInfo.UseShellExecute = false;
                        HSDProcess.StartInfo.FileName = "node.exe";

                        if (NodeSettings.ContainsKey("HSD-command"))
                        {
                            AddLog("Using custom HSD command");
                            string command = NodeSettings["HSD-command"];
                            command = command.Replace("{default-dir}", dir + "hsd\\bin\\hsd");

                            string bobPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Bob\\hsd_data";
                            if (Directory.Exists(bobPath))
                            {
                                command = command.Replace("{Bob}", bobPath);
                            }
                            else if (command.Contains("{Bob}"))
                            {
                                AddLog("Bob not found, using default HSD command");
                                command = dir + "hsd\\bin\\hsd --agent=FireWallet --index-tx --index-address --api-key " + NodeSettings["Key"];
                            }

                            command = command.Replace("{key}", NodeSettings["Key"]);
                            HSDProcess.StartInfo.Arguments = command;
                        }
                        else
                        {
                            AddLog("Using default HSD command");
                            HSDProcess.StartInfo.Arguments = dir + "hsd\\bin\\hsd --agent=FireWallet --index-tx --index-address --api-key " + NodeSettings["Key"];
                            string bobPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Bob\\hsd_data";
                            if (Directory.Exists(bobPath))
                            {
                                HSDProcess.StartInfo.Arguments = HSDProcess.StartInfo.Arguments + " --prefix " + bobPath;
                            }
                        }




                        HSDProcess.Start();
                        // Wait for HSD to start
                        await Task.Delay(2000);

                        // Check if HSD is running
                        if (HSDProcess.HasExited)
                        {
                            AddLog("HSD Failed to start");
                            AddLog(HSDProcess.StandardError.ReadToEnd());
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
            UserSettings = new Dictionary<string, string>();
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                UserSettings.Add(line.Substring(0, line.IndexOf(":")).Trim(), line.Substring(line.IndexOf(":") + 1).Trim());
            }
            sr.Dispose();
        }


        #endregion
        #region Logging
        public void AddLog(string message)
        {
            if (message.Contains("Get Error: No connection could be made because the target machine actively refused it")) return;

            // If file size is over 1MB, rename it to old.log.txt
            if (File.Exists(dir + "log.txt"))
            {
                FileInfo fi = new FileInfo(dir + "log.txt");
                if (fi.Length > 1000000)
                {
                    if (File.Exists(dir + "old.log.txt")) File.Delete(dir + "old.log.txt"); // Delete old log file as it is super old
                    File.Move(dir + "log.txt", dir + "old.log.txt");
                }
            }

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
            Theme = new Dictionary<string, string>();
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] split = line.Split(':');
                Theme.Add(split[0].Trim(), split[1].Trim());
            }
            sr.Dispose();

            if (!Theme.ContainsKey("background") || !Theme.ContainsKey("background-alt") || !Theme.ContainsKey("foreground") || !Theme.ContainsKey("foreground-alt"))
            {
                AddLog("Theme file is missing key");
                return;
            }

            // Apply theme
            this.BackColor = ColorTranslator.FromHtml(Theme["background"]);

            // Foreground
            this.ForeColor = ColorTranslator.FromHtml(Theme["foreground"]);


            // Need to specify this for each groupbox to override the black text
            foreach (Control c in Controls)
            {
                ThemeControl(c);
            }




            this.Width = Screen.PrimaryScreen.Bounds.Width / 5 * 3;
            this.Height = Screen.PrimaryScreen.Bounds.Height / 5 * 3;
            applyTransparency(Theme);

            ResizeForm();
        }
        public void ThemeControl(Control c)
        {
            if (c.GetType() == typeof(GroupBox) || c.GetType() == typeof(Panel))
            {
                c.ForeColor = ColorTranslator.FromHtml(Theme["foreground"]);
                foreach (Control sub in c.Controls)
                {
                    ThemeControl(sub);
                }
            }
            if (c.GetType() == typeof(TextBox) || c.GetType() == typeof(Button)
                || c.GetType() == typeof(ComboBox) || c.GetType() == typeof(StatusStrip) || c.GetType() == typeof(ToolStrip)
                || c.GetType() == typeof(NumericUpDown))
            {
                c.ForeColor = ColorTranslator.FromHtml(Theme["foreground-alt"]);
                c.BackColor = ColorTranslator.FromHtml(Theme["background-alt"]);
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
            string path = "wallet/" + Account + "/unlock";

            string content = "{\"passphrase\": \"" + Password + "\",\"timeout\": 60}";
            if (Password == "")
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
            content = "{\"method\": \"selectwallet\",\"params\":[ \"" + Account + "\"]}";

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

            path = "wallet/" + Account + "";
            APIresponse = await APIGet(path, true);
            JObject jObject = JObject.Parse(APIresponse);
            if (jObject["watchOnly"].ToString() == "True")
            {
                WatchOnly = true;
                toolStripStatusLabelLedger.Text = "Cold Wallet";
                toolStripStatusLabelLedger.Visible = true;
                buttonRevealAll.Visible = false;

            }
            else
            {
                WatchOnly = false;
                toolStripStatusLabelLedger.Visible = false;
                buttonRevealAll.Visible = true;

            }


            if (WatchOnly)
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

                Account = comboBoxaccount.Text;
                Password = textBoxaccountpassword.Text;
                bool loggedin = await Login();
                if (loggedin)
                {
                    toolStripStatusLabelaccount.Text = "Account: " + Account;
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
            Password = ""; // Clear password from memory as soon as possible
            toolStripSplitButtonlogout.Visible = false;
            toolStripStatusLabelLedger.Visible = false;
            string path = "wallet/" + Account + "/lock";
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
            if (Account == "") return; // Don't update balance if not logged in

            // Try to keep wallet unlocked
            string path = "wallet/" + Account + "/unlock";
            string content = "{\"passphrase\": \"" + Password + "\",\"timeout\": 60}";

            await APIPost(path, true, content);

            path = "";
            content = "{\"method\": \"selectwallet\",\"params\":[ \"" + Account + "\"]}";

            await APIPost(path, true, content);

        }
        private async Task UpdateBalance()
        {
            string response = await APIGet("wallet/" + Account + "/balance?account=default", true);
            if (response == "Error") return;

            JObject resp = JObject.Parse(response);

            decimal available = (Convert.ToDecimal(resp["unconfirmed"].ToString()) - Convert.ToDecimal(resp["lockedUnconfirmed"].ToString())) / 1000000;
            decimal locked = Convert.ToDecimal(resp["lockedUnconfirmed"].ToString()) / 1000000;
            available = decimal.Round(available, 2);
            locked = decimal.Round(locked, 2);
            Balance = available;
            Balance_Locked = locked;
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
            if (content == "{\"passphrase\": \"\",\"timeout\": 60}")
            {
                return "";
            }
            string key = NodeSettings["Key"];
            string ip = NodeSettings["IP"];
            string port = "1203";
            if (HSDNetwork == 1)
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
                AddLog(await resp.Content.ReadAsStringAsync());
                AddLog("Content: " + content);
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
            if (NodeSettings == null) return "Error";
            if (!NodeSettings.ContainsKey("Key") || !NodeSettings.ContainsKey("IP")) return "Error";
            string key = NodeSettings["Key"];
            string ip = NodeSettings["IP"];

            string port = "1203";
            if (HSDNetwork == 1)
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
            string path = "wallet/" + Account + "/address";
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
            if (UserSettings.ContainsKey("portfolio-tx"))
            {
                if (UserSettings["portfolio-tx"] == "0") return;
            }


            // Get Unconfirmed TX
            string path = "wallet/" + Account + "/tx/unconfirmed";
            APIresponse = await APIGet(path, true);
            if (APIresponse == "Error")
            {
                AddLog("GetInfo Error");
                return;
            }
            JArray pendingTxs = JArray.Parse(APIresponse);
            labelPendingCount.Text = "Unconfirmed TX: " + pendingTxs.Count.ToString();


            // Check how many TX there are
            APIresponse = await APIGet("wallet/" + Account, true);
            JObject wallet = JObject.Parse(APIresponse);
            if (!wallet.ContainsKey("balance"))
            {
                AddLog("GetInfo Error");
                AddLog(APIresponse);
                return;
            }
            JObject balance = JObject.Parse(wallet["balance"].ToString());
            int TotalTX = Convert.ToInt32(balance["tx"].ToString());
            int toGet = 10;
            if (UserSettings.ContainsKey("portfolio-tx")) toGet = Convert.ToInt32(UserSettings["portfolio-tx"]);

            if (toGet > TotalTX) toGet = TotalTX;
            int toSkip = TotalTX - toGet;

            // GET TXs
            if (WatchOnly)
            {
                APIresponse = await APIPost("", true, "{\"method\": \"listtransactions\",\"params\": [\"default\"," + toGet + "," + toSkip + ", true]}");
            }
            else
            {
                APIresponse = await APIPost("", true, "{\"method\": \"listtransactions\",\"params\": [\"default\"," + toGet + "," + toSkip + "]}");
            }

            if (APIresponse == "Error")
            {
                AddLog("GetInfo Error");
                return;
            }
            JObject TXGET = JObject.Parse(APIresponse);

            // Check for error
            if (TXGET["error"].ToString() != "")
            {
                AddLog("GetInfo Error");
                AddLog(APIresponse);
                return;
            }

            JArray txs = JArray.Parse(TXGET["result"].ToString());
            if (toGet > txs.Count) toGet = txs.Count; // In case there are less TXs than expected (usually happens when the get TX's fails)
            Control[] tmpControls = new Control[toGet];
            for (int i = 0; i < toGet; i++)
            {

                // Get last tx
                JObject tx = JObject.Parse(await APIGet("wallet/" + Account + "/tx/" + txs[toGet - i - 1]["txid"].ToString(), true));

                string hash = tx["hash"].ToString();
                string date = tx["mdate"].ToString();

                date = DateTime.Parse(date).ToShortDateString();




                Panel tmpPanel = new Panel();
                tmpPanel.Width = groupBoxTransactions.Width - SystemInformation.VerticalScrollBarWidth - 20;
                tmpPanel.Height = 50;
                tmpPanel.Location = new Point(5, (i * 55));
                tmpPanel.BorderStyle = BorderStyle.FixedSingle;
                tmpPanel.BackColor = ColorTranslator.FromHtml(Theme["background-alt"]);
                tmpPanel.ForeColor = ColorTranslator.FromHtml(Theme["foreground-alt"]);
                tmpPanel.Controls.Add(
                    new Label()
                    {
                        Text = "Date: " + date,
                        Location = new Point(10, 5)
                    }
                    );
                int confirmations = Convert.ToInt32(tx["confirmations"].ToString());
                if (UserSettings.ContainsKey("confirmations"))
                {
                    if (confirmations < Convert.ToInt32(UserSettings["confirmations"]))
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

                JArray inputs = JArray.Parse(tx["inputs"].ToString());
                JArray outputs = JArray.Parse(tx["outputs"].ToString());
                int inputCount = inputs.Count;
                int outputCount = outputs.Count;

                decimal costHNS = decimal.Parse(txs[toGet - i - 1]["amount"].ToString());
                string cost = "";
                if (costHNS < 0)
                {
                    cost = "Spent: " + (costHNS * -1).ToString() + " HNS";
                }
                else if (costHNS > 0)
                {
                    cost = "Received: " + costHNS.ToString() + " HNS";
                }





                Label labelInputOutput = new Label()
                {
                    Text = "Inputs: " + inputCount + " Outputs: " + outputCount + "\n" + cost,
                    AutoSize = true,
                    Location = new Point(300, 5)
                };
                tmpPanel.Controls.Add(labelInputOutput);


                tmpPanel.Click += (sender, e) =>
                {
                    TXForm txForm = new TXForm(this, hash);
                    txForm.Show();
                };
                foreach (Control c in tmpPanel.Controls)
                {
                    c.Click += (sender, e) =>
                    {
                        TXForm txForm = new TXForm(this, hash);
                        txForm.Show();
                    };
                }

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
                string response = await APIPost("", false, "{\"method\": \"estimatefee\",\"params\": [ 3 ]}");
                JObject resp = JObject.Parse(response);
                string result = resp["result"].ToString();
                decimal fee = decimal.Parse(result);
                if (fee < 0.001m) fee = 1;

                return fee.ToString();
            }
            catch
            {
                AddLog("GetFee Error");
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
            labelBalance.Text = "Available: " + Balance.ToString() + " HNS";
            labelLocked.Text = "Locked: " + Balance_Locked.ToString() + " HNS*";
            labelBalanceTotal.Text = "Total: " + (Balance + Balance_Locked).ToString() + " HNS";
            if (Theme.ContainsKey("selected-bg") && Theme.ContainsKey("selected-fg"))
            {
                buttonNavPortfolio.BackColor = ColorTranslator.FromHtml(Theme["selected-bg"]);
                buttonNavPortfolio.ForeColor = ColorTranslator.FromHtml(Theme["selected-fg"]);
            }
        }

        private async void SendPanel_Click(object sender, EventArgs e)
        {
            hidePages();
            panelSend.Show();
            if (Theme.ContainsKey("selected-bg") && Theme.ContainsKey("selected-fg"))
            {
                buttonNavSend.BackColor = ColorTranslator.FromHtml(Theme["selected-bg"]);
                buttonNavSend.ForeColor = ColorTranslator.FromHtml(Theme["selected-fg"]);
            }
            if (Theme.ContainsKey("error"))
            {
                labelSendingError.ForeColor = ColorTranslator.FromHtml(Theme["error"]);
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

            labelSendingMax.Text = "Max: " + Balance.ToString() + " HNS";
            textBoxSendingTo.Focus();
            string fee = await GetFee();

            labelSendingFee.Text = "Est. Fee: " + fee + " HNS";
            labelSendingError.Hide();

        }
        private async void ReceivePanel_Click(object sender, EventArgs e)
        {
            hidePages();
            panelRecieve.Show();

            if (Theme.ContainsKey("selected-bg") && Theme.ContainsKey("selected-fg"))
            {
                buttonNavReceive.BackColor = ColorTranslator.FromHtml(Theme["selected-bg"]);
                buttonNavReceive.ForeColor = ColorTranslator.FromHtml(Theme["selected-fg"]);
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
            pictureBoxReceiveQR.Image = qrCode.GetGraphic(20, Theme["foreground"], Theme["background"]);
            pictureBoxReceiveQR.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxReceiveQR.Width = panelRecieve.Width / 3;
            pictureBoxReceiveQR.Left = (panelRecieve.Width - pictureBoxReceiveQR.Width) / 2;



        }
        private void buttonNavDomains_Click(object sender, EventArgs e)
        {
            hidePages();
            panelDomains.Show();

            if (Theme.ContainsKey("selected-bg") && Theme.ContainsKey("selected-fg"))
            {
                buttonNavDomains.BackColor = ColorTranslator.FromHtml(Theme["selected-bg"]);
                buttonNavDomains.ForeColor = ColorTranslator.FromHtml(Theme["selected-fg"]);
            }
            textBoxDomainSearch.Focus();
            groupBoxDomains.Width = panelDomains.Width - 20;
            groupBoxDomains.Left = 10;
            groupBoxDomains.Height = panelDomains.Height - groupBoxDomains.Top - 10;
            comboBoxDomainSort.SelectedIndex = 0;

            UpdateDomains();

        }
        private void hidePages()
        {
            panelSend.Hide();
            panelPortfolio.Hide();
            panelRecieve.Hide();
            panelDomains.Hide();
            panelSettings.Hide();
            buttonNavPortfolio.BackColor = ColorTranslator.FromHtml(Theme["background"]);
            buttonNavPortfolio.ForeColor = ColorTranslator.FromHtml(Theme["foreground"]);
            buttonNavSend.BackColor = ColorTranslator.FromHtml(Theme["background"]);
            buttonNavSend.ForeColor = ColorTranslator.FromHtml(Theme["foreground"]);
            buttonNavReceive.BackColor = ColorTranslator.FromHtml(Theme["background"]);
            buttonNavReceive.ForeColor = ColorTranslator.FromHtml(Theme["foreground"]);
            buttonNavDomains.BackColor = ColorTranslator.FromHtml(Theme["background"]);
            buttonNavDomains.ForeColor = ColorTranslator.FromHtml(Theme["foreground"]);
            buttonNavSettings.BackColor = ColorTranslator.FromHtml(Theme["background"]);
            buttonNavSettings.ForeColor = ColorTranslator.FromHtml(Theme["foreground"]);
        }
        private void buttonNavSettings_Click(object sender, EventArgs e)
        {
            hidePages();
            if (Theme.ContainsKey("selected-bg") && Theme.ContainsKey("selected-fg"))
            {
                buttonNavSettings.BackColor = ColorTranslator.FromHtml(Theme["selected-bg"]);
                buttonNavSettings.ForeColor = ColorTranslator.FromHtml(Theme["selected-fg"]);
            }

            panelSettings.Show();
            panelSettings.Dock = DockStyle.Fill;
            buttonSettingsSave.Top = panelSettings.Height - buttonSettingsSave.Height - 10;
            labelSettingsSaved.Top = buttonSettingsSave.Top + 10;
            textBoxExTX.Text = UserSettings["explorer-tx"];
            textBoxExAddr.Text = UserSettings["explorer-addr"];
            textBoxExBlock.Text = UserSettings["explorer-block"];
            textBoxExName.Text = UserSettings["explorer-domain"];
            numericUpDownConfirmations.Value = int.Parse(UserSettings["confirmations"]);
            numericUpDownTXCount.Value = int.Parse(UserSettings["portfolio-tx"]);
            labelSettingsSaved.Hide();
        }
        #endregion
        #region Send

        // Store TLSA hash
        public string TLSA { get; set; }
        private async void textBoxSendingTo_Leave(object sender, EventArgs e)
        {
            
            labelSendingError.Hide();
            labelHIPArrow.Hide();
            labelSendingHIPAddress.Hide();
            if (textBoxSendingTo.Text == "") return;
            if (textBoxSendingTo.Text.Substring(0, 1) == "@")
            {
                string domain = textBoxSendingTo.Text.Substring(1);

                try
                {
                    IPAddress iPAddress = null;
                    TLSA = "";


                    // Create an instance of LookupClient using the custom options
                    string ip = "127.0.0.1";
                    int port = 5350;

                    if (UserSettings.ContainsKey("hip-02-ip"))
                    {
                        ip = UserSettings["hip-02-ip"];
                    }
                    if (UserSettings.ContainsKey("hip-02-port"))
                    {
                        port = int.Parse(UserSettings["hip-02-port"]);
                    } else if (!HSD)
                    {
                        string bobPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Bob\\hsd_data";
                        if (Directory.Exists(bobPath))
                        {
                            port = 9892;
                        }
                    }

                    NameServer nameServer = new NameServer(IPAddress.Parse(ip), port);
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
                        labelSendingError.Show();
                        labelSendingError.Text = "HIP-02 lookup failed";
                        AddLog("No IP found");
                        return;
                    }

                    // Get TLSA record
                    var resultTLSA = client.Query("_443._tcp." + domain, QueryType.TLSA);
                    foreach (var record in resultTLSA.Answers.OfType<TlsaRecord>())
                    {
                        TLSA = record.CertificateAssociationDataAsString;
                    }



                    string url = "https://" + iPAddress.ToString() + "/.well-known/wallets/HNS";
                    var handler = new HttpClientHandler();

                    handler.ServerCertificateCustomValidationCallback = ValidateServerCertificate;

                    // Create an instance of HttpClient with the custom handler
                    using (var httpclient = new HttpClient(handler))
                    {
                        httpclient.DefaultRequestHeaders.Add("Host", domain);
                        // Send a GET request to the specified URL
                        HttpResponseMessage response = httpclient.GetAsync(url).Result;

                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            labelSendingError.Show();
                            labelSendingError.Text = "HIP-02 lookup failed";
                            AddLog("HTTPS get failed");
                            AddLog(response.Content.ReadAsStringAsync().Result);
                            return;
                        }

                        // Response
                        string address = response.Content.ReadAsStringAsync().Result;
                        address = address.Trim();


                        if (await ValidAddress(address))
                        {
                            labelSendingHIPAddress.Text = address;
                            labelSendingHIPAddress.Show();
                            labelHIPArrow.Show();
                        }
                        else
                        {
                            labelSendingError.Show();
                            labelSendingError.Text = "Invalid Address";
                            AddLog("Invalid Address\n"+address);

                        }

                        
                    }

                }
                catch (Exception ex)
                {
                    AddLog("HIP-02 lookup error");
                    AddLog(ex.Message);
                    labelSendingError.Show();
                    labelSendingError.Text = "HIP-02 lookup failed";
                }
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
        /// <summary>
        /// DANE validation. Used verifying Handshake domains
        /// </summary>
        /// <param name="certificate">certificate to check</param>
        /// <returns>True if DANE passes, Else False</returns>
        public bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            X509Certificate2 cert2 = new X509Certificate2(certificate);
            var rsaPublicKey = (RSA)cert2.PublicKey.Key;
            using (var sha256 = SHA256.Create())
            {
                byte[] publicKeyBytes = rsaPublicKey.ExportSubjectPublicKeyInfo();
                byte[] publicKeyHash = sha256.ComputeHash(publicKeyBytes);
                string hexFingerprint = ByteArrayToHexString(publicKeyHash);
                if (hexFingerprint == TLSA) return true;
                else
                {
                    AddLog("TLSA mismatch");
                    return false;
                }
            }
        }
        /// <summary>
        /// Convert byte array to hex string
        /// </summary>
        /// <param name="bytes">Byte array to convert</param>
        /// <returns>String of converted byte array</returns>
        static string ByteArrayToHexString(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                hex.AppendFormat("{0:X2}", b);
            }
            return hex.ToString();
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
            textBoxSendingAmount.Text = (Balance - feeDecimal).ToString();
        }

        private async void buttonSendHNS_Click(object sender, EventArgs e)
        {
            try
            {
                string address = textBoxSendingTo.Text;
                if (labelHIPArrow.Visible)
                {
                    address = labelSendingHIPAddress.Text;
                }
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
                else if (amount > (Balance - fee))
                {
                    labelSendingError.Show();
                    labelSendingError.Text += " Insufficient Funds";
                    return;
                }

                if (!WatchOnly)
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
                    string link = UserSettings["explorer-tx"] + hash;
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
                        + " " + textBoxSendingAmount.Text + " --api-key " + NodeSettings["Key"] + " -w " + Account;
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
                        string link = UserSettings["explorer-tx"] + hash;
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
                if (NodeSettings.ContainsKey("HideScreen"))
                {
                    if (NodeSettings["HideScreen"].ToLower() == "false")
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
                    NotifyForm notifyForm = new NotifyForm("Git is not installed\nPlease install it to install HSD dependencies", "Install", "https://git-scm.com/download/win");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                    this.Close();
                    Thread.Sleep(1000);
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
                    NotifyForm notifyForm = new NotifyForm("Node is not installed\nPlease install it to install HSD dependencies", "Install", "https://nodejs.org/en/download");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                    this.Close();
                    Thread.Sleep(1000);
                    return;
                }


                // Check if npm installed
                testInstalled = new Process();
                testInstalled.StartInfo.FileName = "cmd.exe";
                testInstalled.StartInfo.Arguments = "npm -v";
                testInstalled.StartInfo.RedirectStandardOutput = true;
                testInstalled.StartInfo.UseShellExecute = false;
                testInstalled.StartInfo.CreateNoWindow = false;
                testInstalled.Start();
                // Wait 3 seconds and then kill
                Thread.Sleep(3000);
                testInstalled.Kill();
                outputInstalled = testInstalled.StandardOutput.ReadToEnd();
                testInstalled.WaitForExit();
                if (Regex.IsMatch(outputInstalled, @"^\d+\.\d+\.\d+$"))
                {
                    AddLog("NPM is not installed");
                    AddLog(outputInstalled);
                    NotifyForm notifyForm = new NotifyForm("NPM is not installed\nPlease install it to install HSD dependencies", "Install", "https://docs.npmjs.com/downloading-and-installing-node-js-and-npm");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                    this.Close();
                    Thread.Sleep(1000);
                    return;
                }

                AddLog("Prerequisites installed");

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "git";
                startInfo.Arguments = $"clone {repositoryUrl} {destinationPath}";

                if (repositoryUrl == "https://github.com/handshake-org/hsd.git")
                {
                    startInfo.Arguments = $"clone --depth 1 --branch latest {repositoryUrl} {destinationPath}";
                }

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
            proc.StartInfo.Arguments = dir + "hsd-ledger/bin/hsd-ledger createaddress --api-key " + NodeSettings["Key"] + " -w " + Account;
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
            string response = await APIGet("wallet/" + Account + "/name?own=true", true);
            JArray names = JArray.Parse(response);
            Domains = new string[names.Count];
            DomainsRenewable = new string[names.Count];
            int i = 0;
            int renewable = 0;
            panelDomainList.Controls.Clear();

            // Sort the domains
            switch (comboBoxDomainSort.Text)
            {
                case "Default":
                    break;
                case "Alphabetical":
                    names = new JArray(names.OrderBy(obj => (string)obj["name"]));
                    break;
                case "Expiring":
                    names = new JArray(names.OrderBy(obj =>
                    {
                        JToken daysUntilExpireToken = obj["stats"]?["daysUntilExpire"];
                        return (int)(daysUntilExpireToken ?? int.MaxValue);
                    }));
                    break;
                case "Value":
                    // Sort by most valuable first
                    names = new JArray(names.OrderByDescending(obj =>
                    {
                        JToken valueToken = obj?["value"];
                        return (int)(valueToken ?? 0);
                    }));
                    break;
            }


            foreach (JObject name in names)
            {
                Domains[i] = name["name"].ToString();
                Panel domainTMP = new Panel();
                domainTMP.Width = panelDomainList.Width - 20 - SystemInformation.VerticalScrollBarWidth;
                domainTMP.Height = 30;
                domainTMP.Top = 30 * (i);
                domainTMP.Left = 10;
                domainTMP.BorderStyle = BorderStyle.FixedSingle;

                Label domainName = new Label();
                domainName.Text = Domains[i];
                domainName.Top = 5;
                domainName.Left = 5;
                domainName.AutoSize = true;


                domainTMP.Controls.Add(domainName);

                if (!name.ContainsKey("stats"))
                {
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


                // On Click open domain
                domainTMP.Click += new EventHandler((sender, e) =>
                {
                    DomainForm domainForm = new DomainForm(this, name["name"].ToString(), UserSettings["explorer-tx"], UserSettings["explorer-domain"]);
                    domainForm.Show();
                });


                foreach (Control c in domainTMP.Controls)
                {
                    c.Click += new EventHandler((sender, e) =>
                    {
                        DomainForm domainForm = new DomainForm(this, name["name"].ToString(), UserSettings["explorer-tx"], UserSettings["explorer-domain"]);
                        domainForm.Show();
                    });
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
            if (resp["error"].ToString() != "")
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
                NotifyForm notifyForm = new NotifyForm("Reveal sent\n" + hash, "Explorer", UserSettings["explorer-tx"] + hash);
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
                DomainForm domainForm = new DomainForm(this, textBoxDomainSearch.Text, UserSettings["explorer-tx"], UserSettings["explorer-domain"]);

                domainForm.Show();

            }
        }
        private void textBoxDomainSearch_TextChanged(object sender, EventArgs e)
        {
            string domainSearch = textBoxDomainSearch.Text;
            domainSearch = Regex.Replace(textBoxDomainSearch.Text, "[^a-zA-Z0-9-_]", "");
            textBoxDomainSearch.Text = domainSearch;
        }
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
        private void comboBoxDomainSort_DropDownClosed(object sender, EventArgs e)
        {
            UpdateDomains();
        }


        #endregion
        #region Batching
        public void AddBatch(string domain, string operation)
        {
            if (operation == "BID") return;
            if (!BatchMode)
            {
                BatchForm = new BatchForm(this);
                BatchForm.Show();
                BatchMode = true;
            }
            BatchForm.AddBatch(domain, operation);

        }
        public void AddBatch(string domain, string operation, decimal bid, decimal lockup)
        {
            if (operation != "BID") return;
            if (!BatchMode)
            {
                BatchForm = new BatchForm(this);
                BatchForm.Show();
                BatchMode = true;
            }
            BatchForm.AddBatch(domain, operation, bid, lockup);

        }
        public void AddBatch(string domain, string operation, DNS[] updateRecords)
        {
            if (!BatchMode)
            {
                BatchForm = new BatchForm(this);
                BatchForm.Show();
                BatchMode = true;
            }
            BatchForm.AddBatch(domain, operation, updateRecords);
        }
        public void AddBatch(string domain, string operation, string address)
        {
            if (!BatchMode)
            {
                BatchForm = new BatchForm(this);
                BatchForm.Show();
                BatchMode = true;
            }
            BatchForm.AddBatch(domain, operation, address);
        }
        public void FinishBatch()
        {
            BatchMode = false;
            BatchForm.Dispose();
        }
        private void buttonBatch_Click(object sender, EventArgs e)
        {
            if (!BatchMode)
            {
                BatchForm = new BatchForm(this);
                BatchForm.Show();
                BatchMode = true;
            }
            else BatchForm.Focus();
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
            sw.WriteLine("hide-splash: " + UserSettings["hide-splash"]);
            sw.Dispose();
            LoadSettings();
            labelSettingsSaved.Show();
        }
        private async void buttonSeed_Click(object sender, EventArgs e)
        {
            string path = "wallet/" + Account + "/master";
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

        #region Help Menu
        private void githubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open the GitHub page
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "https://github.com/Nathanwoodburn/FireWallet/",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void websiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "https://firewallet",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void supportDiscordServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "https://l.woodburn.au/discord",
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        private void otherProjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "https://nathan.woodburn.au/projects",
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        #endregion
    }
}