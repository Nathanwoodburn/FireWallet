using System.Data;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using Point = System.Drawing.Point;

namespace FireWallet
{
    public partial class BatchForm : Form
    {
        string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FireWallet\\";
        Dictionary<string, string> theme;
        MainForm mainForm;
        Batch[] batches;


        public BatchForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            batches = new Batch[0];
        }
        public void AddBatch(string domain, string operation)
        {
            if (operation == "BID") return;
            batches = batches.Concat(new Batch[] { new Batch(domain, operation) }).ToArray();
            Panel tx = new Panel();
            tx.Left = 0;
            tx.Top = panelTXs.Controls.Count * 52 + 2;
            tx.Width = panelTXs.Width - SystemInformation.VerticalScrollBarWidth;
            tx.Height = 50;
            tx.BorderStyle = BorderStyle.FixedSingle;

            Label action = new Label();
            action.Text = operation;
            action.Location = new Point(10, 10);
            action.AutoSize = true;
            tx.Controls.Add(action);
            Label target = new Label();
            target.Text = domain;
            target.Location = new Point(10, 30);
            target.AutoSize = true;
            tx.Controls.Add(target);

            Button deleteTX = new Button();
            deleteTX.Text = "X";
            deleteTX.Location = new Point(tx.Width - 40, 10);
            deleteTX.Width = 25;
            deleteTX.Height = 25;
            deleteTX.TextAlign = ContentAlignment.MiddleCenter;
            deleteTX.Click += (sender, e) => { 
                panelTXs.Controls.Remove(tx);
                FixSpacing();
                List<Batch> temp = new List<Batch>();
                foreach (Batch batch in batches)
                {
                    if (batch.domain != domain && batch.method != operation)
                    {
                        temp.Add(batch);
                    }
                }
                batches = temp.ToArray();
            };
            deleteTX.FlatStyle = FlatStyle.Flat;
            deleteTX.Font = new Font(deleteTX.Font.FontFamily, 9F, FontStyle.Bold);
            tx.Controls.Add(deleteTX);

            panelTXs.Controls.Add(tx);
            UpdateTheme();
        }
        public void AddBatch(string domain, string operation, decimal bid, decimal lockup)
        {
            if (operation != "BID")
            {
                AddBatch(domain, operation);
                return;
            }
            batches = batches.Concat(new Batch[] { new Batch(domain, operation, bid, lockup) }).ToArray();
            Panel tx = new Panel();
            tx.Left = 0;
            tx.Top = panelTXs.Controls.Count * 52 + 2;
            tx.Width = panelTXs.Width - SystemInformation.VerticalScrollBarWidth;
            tx.Height = 50;
            tx.BorderStyle = BorderStyle.FixedSingle;

            Label action = new Label();
            action.Text = operation;
            action.Location = new Point(10, 10);
            action.AutoSize = true;
            tx.Controls.Add(action);
            Label target = new Label();
            target.Text = domain;
            target.Location = new Point(10, 30);
            target.AutoSize = true;
            tx.Controls.Add(target);

            Label bidLabel = new Label();
            bidLabel.Text = "Bid: " + bid.ToString();
            bidLabel.Location = new Point(200, 10);
            bidLabel.AutoSize = true;
            tx.Controls.Add(bidLabel);
            Label lockupLabel = new Label();
            lockupLabel.Text = "Lockup: " + lockup.ToString();
            lockupLabel.Location = new Point(200, 30);
            lockupLabel.AutoSize = true;
            tx.Controls.Add(lockupLabel);

            Button deleteTX = new Button();
            deleteTX.Text = "X";
            deleteTX.Location = new Point(tx.Width - 40, 10);
            deleteTX.Width = 25;
            deleteTX.Height = 25;
            deleteTX.TextAlign = ContentAlignment.MiddleCenter;
            deleteTX.Click += (sender, e) => { 
                panelTXs.Controls.Remove(tx);
                FixSpacing();
                List<Batch> temp = new List<Batch>();
                foreach (Batch batch in batches)
                {
                    if (batch.domain != domain && batch.method != operation)
                    {
                        temp.Add(batch);
                    }
                }
                batches = temp.ToArray();
            };
            deleteTX.FlatStyle = FlatStyle.Flat;
            deleteTX.Font = new Font(deleteTX.Font.FontFamily, 9F, FontStyle.Bold);
            tx.Controls.Add(deleteTX);

            panelTXs.Controls.Add(tx);
            UpdateTheme();
        }

        private void FixSpacing()
        {
            // Fix spacing from deleting a tx in the middle
            for (int i = 0; i < panelTXs.Controls.Count; i++)
            {
                panelTXs.Controls[i].Top = i * 52 + 2;
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
                || c.GetType() == typeof(ComboBox) || c.GetType() == typeof(StatusStrip) || c.GetType() == typeof(ToolStrip))
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

        private void BatchForm_Load(object sender, EventArgs e)
        {
            UpdateTheme();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            mainForm.FinishBatch();
            AddLog("Batch Cancelled");
            this.Close();
        }
        HttpClient httpClient = new HttpClient();
        private async void buttonSend_Click(object sender, EventArgs e)
        {
            string batchTX = "[" +string.Join(", ", batches.Select(batch => batch.ToString())) + "]";
            string content = "{\"method\": \"sendbatch\",\"params\":[ " + batchTX + "]}";
            string responce = await APIPost("",true,content);
            
            if (responce == "Error")
            {
                AddLog("Error sending batch");
                NotifyForm notifyForm = new NotifyForm("Error sending batch");
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                return;
            }
            
            JObject jObject = JObject.Parse(responce);
            if (jObject["error"].ToString() != "")
            {
                AddLog("Error: ");
                AddLog(jObject["error"].ToString());
                NotifyForm notifyForm = new NotifyForm("Error: \n" + jObject["error"].ToString());
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                return;
            }
            
            JObject result = JObject.Parse(jObject["result"].ToString());
            string hash = result["hash"].ToString();
            AddLog("Batch sent with hash: " + hash);
            NotifyForm notifyForm2 = new NotifyForm("Batch sent\nThis might take a while to mine.", "Explorer", mainForm.userSettings["explorer-tx"]+hash);
            notifyForm2.ShowDialog();
            notifyForm2.Dispose();
            this.Close();
        }

        private void BatchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainForm.FinishBatch();
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV File|*.csv";
            saveFileDialog.Title = "Save Batch";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
                foreach (Batch b in batches)
                {
                    sw.WriteLine(b.domain + "," + b.method + "," + b.bid + "," + b.lockup);
                }
                sw.Dispose();
            }
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV File|*.csv|TXT File|*.txt";
            openFileDialog.Title = "Open Batch";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog.FileName);
                string line;
                string[] domains = new string[0];
                while ((line = sr.ReadLine()) != null)
                {
                    string[] split = line.Split(',');
                    try
                    {
                        if (split.Length == 2)
                        {
                            AddBatch(split[0], split[1]);
                        }
                        else if (split.Length == 4)
                        {
                            AddBatch(split[0], split[1], Convert.ToDecimal(split[2]), Convert.ToDecimal(split[3]));
                        }
                        else
                        {
                            // Select operation and import domains
                            string[] newDomains = new string[domains.Length + 1];
                            for (int i = 0; i < domains.Length; i++)
                            {
                                newDomains[i] = domains[i];
                            }
                            newDomains[domains.Length] = line.Trim();
                            domains = newDomains;
                        }
                    }
                    catch (Exception ex)
                    {
                        AddLog("Error importing batch: " + ex.Message);
                        NotifyForm notifyForm = new NotifyForm("Error importing batch");
                        notifyForm.ShowDialog();
                        notifyForm.Dispose();
                    }
                }
                if (domains.Length > 0)
                {
                    BatchImportForm batchImportForm = new BatchImportForm(domains);
                    batchImportForm.ShowDialog();
                    if (batchImportForm.batches != null)
                    {
                        foreach (Batch b in batchImportForm.batches)
                        {
                            AddBatch(b.domain, b.method, b.bid, b.lockup);
                        }
                    }
                }
                sr.Dispose();
            }
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
            string key = mainForm.nodeSettings["Key"];
            string ip = mainForm.nodeSettings["IP"];
            string port = "1203";
            if (mainForm.network == 1)
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
    }
    public class Batch
    {
        public string domain { get; }
        public string method { get; }
        public decimal bid { get; }
        public decimal lockup { get; }
        public Batch(string domain, string operation)
        {
            this.domain = domain;
            this.method = operation;
            bid = 0;
            lockup = 0;
        }
        public Batch(string domain, string operation, decimal bid, decimal lockup)
        {
            this.domain = domain;
            this.method = operation;
            this.bid = bid;
            this.lockup = lockup;
        }
        public override string ToString()
        {
            if (method == "BID")
            {
                return "[\"BID\", \"" + domain + "\", " + bid + ", " + lockup + "]";
            }
            return "[\"" + method + "\", \"" + domain + "\"]";
        }
    }
}
