using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Policy;
using Newtonsoft.Json.Linq;

namespace FireWallet
{
    public partial class MainForm : Form
    {
        #region Variables
        string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FireWallet\\";
        Dictionary<string, string> nodeSettings;
        Dictionary<string, string> theme;
        int Network;
        string account;
        string password;
        double balance;
        double balanceLocked;
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
                buttonPortfolio.PerformClick();
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

            double available = Convert.ToDouble(resp["unconfirmed"].ToString()) / 1000000;
            double locked = Convert.ToDouble(resp["lockedUnconfirmed"].ToString()) / 1000000;
            available = available - locked;
            available = Math.Round(available, 2);
            locked = Math.Round(locked, 2);
            balance = available;
            balanceLocked = locked;
        }

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

        private async void GetInfo()
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
            JArray txs = JArray.Parse(APIresponse);
            labelPendingCount.Text = "Unconfirmed TX: " + resp.Count.ToString();


        }

        #endregion
        #region Timers
        private void timerNodeStatus_Tick(object sender, EventArgs e)
        {
            NodeStatus();
            // If logged in, update info
            if (panelaccount.Visible == false)
            {
                GetInfo();
            }
        }
        #endregion

        #region Nav
        private async void buttonPortfolio_Click(object sender, EventArgs e)
        {
            panelPortfolio.Show();
            await UpdateBalance();
            GetInfo();
            labelBalance.Text = "Available: " + balance.ToString() + " HNS";
            labelLocked.Text = "Locked: " + balanceLocked.ToString() + " HNS";
            labelBalanceTotal.Text = "Total: " + (balance + balanceLocked).ToString() + " HNS";
        }
        #endregion
    }
}