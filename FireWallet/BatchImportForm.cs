using System.Runtime.InteropServices;

namespace FireWallet
{
    public partial class BatchImportForm : Form
    {
        string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FireWallet\\";
        Dictionary<string, string> theme;
        string[] domains;
        public Batch[] batches { get; set; }
        MainForm mainForm;

        public BatchImportForm(string[] domains, MainForm mainForm)
        {
            InitializeComponent();
            this.domains = domains;
            comboBoxMode.SelectedIndex = 1;
            this.mainForm = mainForm;
        }

        private void BatchImportForm_Load(object sender, EventArgs e)
        {
            UpdateTheme();
            foreach (string domain in domains)
            {
                listBoxDomains.Items.Add(domain);
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void buttonImport_Click(object sender, EventArgs e)
        {
            if (comboBoxMode.Text == "BID")
            {
                batches = new Batch[0];
                foreach (string domain in listBoxDomains.Items)
                {
                    if (domain != "")
                    {
                        try
                        {
                            decimal bid = Convert.ToDecimal(textBoxBid.Text);
                            decimal lockup = Convert.ToDecimal(textBoxBlind.Text) + bid;

                            Batch[] newBatch = new Batch[batches.Length + 1];
                            Array.Copy(batches, newBatch, batches.Length);
                            newBatch[newBatch.Length - 1] = new Batch(domain, "BID", bid, lockup);
                            batches = newBatch;
                        }
                        catch (Exception ex)
                        {
                            NotifyForm notify = new NotifyForm("Import error: \n" + ex.Message);
                            notify.ShowDialog();
                            notify.Dispose();
                        }
                    }
                }
                this.Close();
            }
            else if (comboBoxMode.Text == "OPEN" || comboBoxMode.Text == "REVEAL" || comboBoxMode.Text == "REDEEM"
                || comboBoxMode.Text == "RENEW" || comboBoxMode.Text == "FINALIZE" || comboBoxMode.Text == "CANCEL")
            {
                batches = new Batch[0];
                foreach (string domain in listBoxDomains.Items)
                {
                    if (domain != "")
                    {
                        Batch[] newBatch = new Batch[batches.Length + 1];
                        Array.Copy(batches, newBatch, batches.Length);
                        newBatch[newBatch.Length - 1] = new Batch(domain, comboBoxMode.Text);
                        batches = newBatch;
                    }
                }
                this.Close();
            }
            else if (comboBoxMode.Text == "REGISTER")
            {
                batches = new Batch[0];
                foreach (string domain in listBoxDomains.Items)
                {
                    if (domain != "")
                    {
                        Batch[] newBatch = new Batch[batches.Length + 1];
                        Array.Copy(batches, newBatch, batches.Length);
                        newBatch[newBatch.Length - 1] = new Batch(domain,"UPDATE",new DNS[0]);
                        batches = newBatch;
                    }
                }
                this.Close();
            }
            else if (comboBoxMode.Text == "TRANSFER")
            {
                if (textBoxToAddress.Text == "")
                {
                    MessageBox.Show("Please enter a to address");
                    return;
                }
                string address = textBoxToAddress.Text;
                if (address.Substring(0,1) == "@")
                {
                    address = await mainForm.HIP02Lookup(address.Substring(1));
                    if (address == "ERROR")
                    {
                        NotifyForm notify = new NotifyForm("Invalid HIP-02 address");
                        notify.ShowDialog();
                        notify.Dispose();
                        return;
                    }
                }

                if (!(await mainForm.ValidAddress(address)))
                {
                    NotifyForm notify = new NotifyForm("Invalid address");
                    notify.ShowDialog();
                    notify.Dispose();
                    return;
                }

                batches = new Batch[0];
                foreach (string domain in listBoxDomains.Items)
                {
                    if (domain != "")
                    {
                        Batch[] newBatch = new Batch[batches.Length + 1];
                        Array.Copy(batches, newBatch, batches.Length);
                        newBatch[newBatch.Length - 1] = new Batch(domain, comboBoxMode.Text, address);
                        batches = newBatch;
                    }
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a mode");
            }
        }

        private void comboBoxMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxMode.Text == "BID")
            {
                groupBoxBid.Visible = true;
                groupBoxtransfer.Visible = false;
            }
            else if (comboBoxMode.Text == "TRANSFER")
            {
                groupBoxBid.Visible = false;
                groupBoxtransfer.Visible = true;
            }
            else
            {
                groupBoxBid.Visible = false;
                groupBoxtransfer.Visible = false;
            }
        }
    }
}
public class Batch
{
    public string domain { get; }
    public string operation { get; }
    public decimal bid { get; }
    public decimal lockup { get; }
    public string toAddress { get; }
    public Batch(string domain, string operation)
    {
        this.domain = domain;
        this.operation = operation;
        bid = 0;
        lockup = 0;
        toAddress = "";
    }
    public Batch(string domain, string operation, string toAddress)
    {
        this.domain = domain;
        this.operation = operation;
        this.toAddress = toAddress;
        bid = 0;
        lockup = 0;
    }
    public Batch(string domain, string operation, decimal bid, decimal lockup)
    {
        this.domain = domain;
        this.operation = operation;
        this.bid = bid;
        this.lockup = lockup;
        toAddress = "";
    }
}
