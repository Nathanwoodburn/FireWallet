using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FireWallet
{
    public partial class NodeForm : Form
    {
        string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FireWallet\\";
        public NodeForm()
        {
            InitializeComponent();
        }
        private void CreateForm_Load(object sender, EventArgs e)
        {
            AddLog("Starting Setup");
            UpdateTheme();
            SizeForm();
        }

        #region API
        async void TestAPI(object sender, EventArgs e)
        {
            // This will curl the below URL and return the result
            //curl http://x:api-key@127.0.0.1:12039/wallet/$id/account

            string key = textBoxNodeKey.Text;
            string ip = textBoxNodeIP.Text;
            int network = comboBoxNodeNetwork.SelectedIndex;
            string port = "1203";
            if (network == 1)
            {
                port = "1303";
            }



            // Create HTTP client
            HttpClient httpClient = new HttpClient();

            AddLog("Testing: http://x:" + key + "@" + ip + " Network:" + comboBoxNodeNetwork.Text);
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://" + ip + ":" + port + "7");
                // Add API key to header
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes("x:" + key)));
                // Send request and log response
                HttpResponseMessage response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                AddLog("Node Connected");
                labelNodeStatus.Text = "Node Connected";
            }
            // Log errors to log textbox
            catch (Exception ex)
            {
                AddLog("Node Failed: " + ex.Message);
                labelNodeStatus.Text = "Node Connection failed";
            }
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

            AddLog("Reading theme file");
            // Read file
            StreamReader sr = new StreamReader(dir + "theme.txt");
            Dictionary<string, string> theme = new Dictionary<string, string>();
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
                if (c.GetType() == typeof(GroupBox))
                {
                    c.ForeColor = ColorTranslator.FromHtml(theme["foreground"]);
                    foreach (Control sub in c.Controls)
                    {
                        if (sub.GetType() == typeof(TextBox) || sub.GetType() == typeof(Button)
                            || sub.GetType() == typeof(ComboBox))
                        {
                            sub.ForeColor = ColorTranslator.FromHtml(theme["foreground-alt"]);
                            sub.BackColor = ColorTranslator.FromHtml(theme["background-alt"]);
                        }

                    }
                }
                if (c.GetType() == typeof(TextBox) || c.GetType() == typeof(Button)
                    || c.GetType() == typeof(ComboBox))
                {
                    c.ForeColor = ColorTranslator.FromHtml(theme["foreground-alt"]);
                    c.BackColor = ColorTranslator.FromHtml(theme["background-alt"]);
                }
            }




            // Transparancy

            AddLog("Finished applying theme");
            AddLog("Applying transparency");
            applyTransparency(theme);


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
        #endregion
        #region Sizing
        private void CreateForm_Resize(object sender, EventArgs e)
        {
            SizeForm();
        }
        private void SizeForm()
        {
            labelWelcome.Left = (this.ClientSize.Width - labelWelcome.Size.Width) / 2;
        }
        #endregion


        private void SaveSettings(object sender, EventArgs e)
        {
            if (textBoxNodeKey.Text == "")
            {
                NotifyForm notifyForm = new NotifyForm("Please enter a key");
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                return;
            }
            if (checkBoxRunHSD.Checked)
            {
                if (!Regex.IsMatch(textBoxNodeKey.Text, @"^[a-zA-Z0-9]+$"))
                {
                    NotifyForm notifyForm = new NotifyForm("Please enter valid API key\nDo not use spaces or weird stuff");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                    return;
                }

                StreamWriter sw = new StreamWriter(dir + "node.txt");
                sw.WriteLine("IP: " + textBoxNodeIP.Text);
                sw.WriteLine("Network: " + comboBoxNodeNetwork.SelectedIndex);
                sw.WriteLine("Key: " + textBoxNodeKey.Text);
                sw.WriteLine("HSD: True");
                sw.Dispose();
                this.Close();
            }
            else
            {

                buttonNodeTest.PerformClick();

                if (labelNodeStatus.Text != "Node Connected")
                {
                    return;
                }
                StreamWriter sw = new StreamWriter(dir + "node.txt");
                sw.WriteLine("IP: " + textBoxNodeIP.Text);
                sw.WriteLine("Network: " + comboBoxNodeNetwork.SelectedIndex);
                sw.WriteLine("Key: " + textBoxNodeKey.Text);
                sw.Dispose();
                this.Close();
            }
        }
    }
}
