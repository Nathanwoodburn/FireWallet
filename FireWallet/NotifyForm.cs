using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FireWallet
{
    public partial class NotifyForm : Form
    {
        string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FireWallet\\";
        Dictionary<string, string> theme;
        string altLink;
        bool Linkcopy;
        bool allowClose = true;
        public NotifyForm(string Message)
        {
            InitializeComponent();
            labelmessage.Text = Message;
            altLink = "";
        }
        public void CloseNotification()
        {
            this.Close();
        }
        public NotifyForm(string Message, string altText, string altLink)
        {
            InitializeComponent();
            labelmessage.Text = Message;
            buttonALT.Text = altText;
            buttonALT.Visible = true;
            this.altLink = altLink;
            buttonOK.Focus();
            Linkcopy = false;
        }
        public NotifyForm(string Message, bool allowClose)
        {
            InitializeComponent();
            labelmessage.Text = Message;
            buttonOK.Focus();
            Linkcopy = false;
            buttonOK.Visible = allowClose;
            allowClose = allowClose;
        }

        public NotifyForm(string Message, string altText, string altLink, bool Linkcopy)
        {
            InitializeComponent();
            labelmessage.Text = Message;
            buttonALT.Text = altText;
            buttonALT.Visible = true;
            this.altLink = altLink;
            buttonOK.Focus();
            this.Linkcopy = Linkcopy;

            if (Linkcopy)
            {
                // Small font to fix more data
                labelmessage.Font = new Font(labelmessage.Font.FontFamily, 10);
            }
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

        private void NotifyForm_Load(object sender, EventArgs e)
        {
            UpdateTheme();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            allowClose = true;
            this.Close();
        }

        private void buttonALT_Click(object sender, EventArgs e)
        {
            if (Linkcopy)
            {
                // Copy link to clipboard
                Clipboard.SetText(altLink);
                return;
            }
            // Open link
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = altLink,
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void NotifyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!allowClose) e.Cancel = true;
        }
    }
}
