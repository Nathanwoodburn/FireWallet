using System.Data;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json.Linq;
using ContentAlignment = System.Drawing.ContentAlignment;
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
        public void bringToFront()
        {
            // Minimize this form and bring it back up
            this.WindowState = FormWindowState.Minimized;
            this.Show();
            this.WindowState = FormWindowState.Normal;
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
            deleteTX.Click += (sender, e) =>
            {
                panelTXs.Controls.Remove(tx);
                FixSpacing();
                List<Batch> temp = new List<Batch>();
                foreach (Batch batch in batches)
                {
                    if (batch.domain != domain || batch.method != operation)
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
            deleteTX.Click += (sender, e) =>
            {
                panelTXs.Controls.Remove(tx);
                FixSpacing();
                List<Batch> temp = new List<Batch>();
                foreach (Batch batch in batches)
                {
                    if (batch.domain != domain || batch.method != operation)
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
        }
        public void AddBatch(string domain, string operation, string toAddress)
        {
            if (operation != "TRANSFER")
            {
                AddBatch(domain, operation);
                return;
            }
            batches = batches.Concat(new Batch[] { new Batch(domain, operation, toAddress) }).ToArray();
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

            Label toAddressLabel = new Label();
            toAddressLabel.Text = "-> " + toAddress;
            toAddressLabel.Location = new Point(200, 10);
            toAddressLabel.AutoSize = true;
            tx.Controls.Add(toAddressLabel);


            Button deleteTX = new Button();
            deleteTX.Text = "X";
            deleteTX.Location = new Point(tx.Width - 40, 10);
            deleteTX.Width = 25;
            deleteTX.Height = 25;
            deleteTX.TextAlign = ContentAlignment.MiddleCenter;
            deleteTX.Click += (sender, e) =>
            {
                panelTXs.Controls.Remove(tx);
                FixSpacing();
                List<Batch> temp = new List<Batch>();
                foreach (Batch batch in batches)
                {
                    if (batch.domain != domain || batch.method != operation)
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
        }

        public void AddBatch(string domain, string operation, DNS[] updateRecords)
        {
            batches = batches.Concat(new Batch[] { new Batch(domain, operation, updateRecords) }).ToArray();
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

            Label toAddressLabel = new Label();
            toAddressLabel.Text = "-> UPDATE RECORDS";
            toAddressLabel.Location = new Point(200, 10);
            toAddressLabel.AutoSize = true;
            tx.Controls.Add(toAddressLabel);


            Button deleteTX = new Button();
            deleteTX.Text = "X";
            deleteTX.Location = new Point(tx.Width - 40, 10);
            deleteTX.Width = 25;
            deleteTX.Height = 25;
            deleteTX.TextAlign = ContentAlignment.MiddleCenter;
            deleteTX.Click += (sender, e) =>
            {
                panelTXs.Controls.Remove(tx);
                FixSpacing();
                List<Batch> temp = new List<Batch>();
                foreach (Batch batch in batches)
                {
                    if (batch.domain != domain || batch.method != operation)
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
        public void UpdateTheme()
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

            bringToFront();
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
            if (!mainForm.WatchOnly && !mainForm.multiSig)
            {
                string batchTX = "[" + string.Join(", ", batches.Select(batch => batch.ToString())) + "]";
                string content = "{\"method\": \"sendbatch\",\"params\":[ " + batchTX + "]}";
                string responce = await APIPost("", true, content);

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
                    if (jObject["error"].ToString().Contains("Batch output addresses would exceed lookahead"))
                    {
                        NotifyForm notifyForm = new NotifyForm("Error: \nBatch output addresses would exceed lookahead\nYour batch might have too many TXs.");
                        notifyForm.ShowDialog();
                        notifyForm.Dispose();
                    } else if (jObject["error"].ToString().Contains("Name is not registered"))
                    {
                        NotifyForm notifyForm = new NotifyForm("Error: \nName is not registered\nRemember you can't renew domains in transfer");
                        notifyForm.ShowDialog();
                        notifyForm.Dispose();
                    }
                    else
                    {
                        NotifyForm notifyForm = new NotifyForm("Error: \n" + jObject["error"].ToString());
                        notifyForm.ShowDialog();
                        notifyForm.Dispose();
                    }
                    return;
                }

                JObject result = JObject.Parse(jObject["result"].ToString());
                string hash = result["hash"].ToString();
                AddLog("Batch sent with hash: " + hash);
                NotifyForm notifyForm2 = new NotifyForm("Batch sent\nThis might take a while to mine.", "Explorer", mainForm.UserSettings["explorer-tx"] + hash);
                notifyForm2.ShowDialog();
                notifyForm2.Dispose();
                this.Close();
            }
            else if (!mainForm.WatchOnly)
            {
                string batchTX = "[" + string.Join(", ", batches.Select(batch => batch.ToString())) + "]";
                string content = "{\"method\": \"createbatch\",\"params\":[ " + batchTX + "]}";
                string responce = await APIPost("", true, content);

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
                    if (jObject["error"].ToString().Contains("Batch output addresses would exceed lookahead"))
                    {
                        NotifyForm notifyForm = new NotifyForm("Error: \nBatch output addresses would exceed lookahead\nYour batch might have too many TXs.");
                        notifyForm.ShowDialog();
                        notifyForm.Dispose();
                    }
                    else if (jObject["error"].ToString().Contains("Name is not registered"))
                    {
                        NotifyForm notifyForm = new NotifyForm("Error: \nName is not registered\nRemember you can't renew domains in transfer");
                        notifyForm.ShowDialog();
                        notifyForm.Dispose();
                    }
                    else
                    {
                        NotifyForm notifyForm = new NotifyForm("Error: \n" + jObject["error"].ToString());
                        notifyForm.ShowDialog();
                        notifyForm.Dispose();
                    }
                    return;
                }

                string[] domains = batches.Select(batch => batch.domain).ToArray();
                string tx = await mainForm.ExportTransaction(jObject["result"].ToString(),domains);
                if (tx != "Error")
                {
                    ImportTXForm importTXForm = new ImportTXForm(mainForm, tx);
                    this.Hide();
                    importTXForm.ShowDialog();
                    importTXForm.Dispose();
                    this.Close();
                }
            }
            else // watch only
            {
                string batchTX = "[" + string.Join(", ", batches.Select(batch => batch.ToString())) + "]";
                string content = "{\"method\": \"createbatch\",\"params\":[ " + batchTX + "]}";
                string response = await APIPost("", true, content);

                if (response == "Error")
                {
                    AddLog("Error creating batch");
                    NotifyForm notifyForm = new NotifyForm("Error creating batch");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                    return;
                }

                JObject jObject = JObject.Parse(response);
                if (jObject["error"].ToString() != "")
                {
                    AddLog("Error: ");
                    AddLog(jObject["error"].ToString());
                    NotifyForm notifyForm = new NotifyForm("Error: \n" + jObject["error"].ToString());
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                    return;
                }

                string domainslist = string.Join(",", batches.Select(batch => "\\\"" + batch.domain + "\\\""));

                NotifyForm notify = new NotifyForm("Please confirm the transaction on your Ledger device", false);
                notify.Show();

                var proc = new Process();
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.FileName = "node.exe";
                proc.StartInfo.WorkingDirectory = dir + "hsd-ledger/bin/";
                string args = "hsd-ledger/bin/hsd-ledger sendraw \"\"" + response.Replace("\"","\\\"") + "\"\" [" + domainslist + "] --api-key " + mainForm.NodeSettings["Key"] + " -w " + mainForm.Account;

                proc.StartInfo.Arguments = dir + args;
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
                    string link = mainForm.UserSettings["explorer-tx"] + hash;
                    NotifyForm notifySuccess = new NotifyForm("Transaction Sent\nThis transaction could take up to 20 minutes to mine",
                                                   "Explorer", link);
                    notifySuccess.ShowDialog();
                    this.Close();
                }
                else
                {
                    AddLog(args);
                    AddLog(proc.StandardError.ReadToEnd());
                    NotifyForm notifyError = new NotifyForm("Error Transaction Failed\nCheck logs for more details");
                    notifyError.ShowDialog();
                    notifyError.Dispose();
                }
            }
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
                    if (b.method == "BID")
                    {
                        sw.WriteLine(b.domain + "," + b.method + "," + b.bid + "," + b.lockup, b.toAddress);
                    }
                    else if (b.method == "TRANSFER")
                    {
                        sw.WriteLine(b.domain + "," + b.method + "," + b.toAddress);
                    }
                    else if (b.method == "UPDATE")
                    {
                        sw.WriteLine(b.domain + "," + b.method + ",[" + string.Join(";", b.update.Select(record => record.ToString())) + "]");
                    }
                    else
                    {
                        sw.WriteLine(b.domain + "," + b.method);
                    }
                }
                sw.Dispose();
            }
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files|*.*";
            openFileDialog.Title = "Open Batch";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader sr = new StreamReader(openFileDialog.FileName);
                    string line;
                    string[] domains = new string[0];
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] split = line.Split(',');
                        try
                        {
                            if (split.Length > 2)
                            {
                                if (split[1] == "UPDATE")
                                {
                                    // Join the rest of the line
                                    string[] updateArray = new string[split.Length - 2];
                                    for (int i = 0; i < updateArray.Length; i++)
                                    {
                                        updateArray[i] = split[i + 2];
                                    }
                                    string updateString = string.Join(",", updateArray);
                                    updateString.TrimStart('[');
                                    updateString.TrimEnd(']');

                                    string[] updateSplit = updateString.Split(';');
                                    DNS[] UpdateRecords = new DNS[updateSplit.Length];
                                    int r = 0;
                                    foreach (string update in updateSplit)
                                    {
                                        string[] updateRecord = update.Split(',');
                                        string type = updateRecord[0];
                                        type = type.Split(':')[1].Replace("\"","").Trim();
                                        switch (type)
                                        {
                                            case "NS":
                                                string ns = updateRecord[1].Split(':')[1].Replace("\"","").Replace("}","").Trim();
                                                UpdateRecords[r] = new DNS(type, ns);
                                                break;
                                            case "DS":
                                                int keyTag = int.Parse(updateRecord[1].Split(':')[1]);
                                                int algorithm = int.Parse(updateRecord[2].Split(':')[1]);
                                                int digestType = int.Parse(updateRecord[3].Split(':')[1]);
                                                string digest = updateRecord[4].Split(':')[1].Replace("\"", "").Replace("}", "");

                                                UpdateRecords[r] = new DNS(type, keyTag, algorithm, digestType, digest);
                                                break;
                                            case "TXT":
                                                string txt = updateRecord[1].Split(':')[1].Replace("\"", "").Replace("}", "");
                                                txt = txt.Replace("[", "");
                                                txt = txt.Replace("]", "");
                                                UpdateRecords[r] = new DNS(type, new string[] { txt.Trim() });
                                                break;
                                            case "GLUE4":
                                            case "GLUE6":
                                                string nsGlue = updateRecord[1].Split(':')[1].Replace("\"", "").Trim();
                                                string address = updateRecord[2].Split(':')[1].Replace("\"", "").Replace("}", "").Trim();
                                                UpdateRecords[r] = new DNS(type, nsGlue, address);
                                                break;
                                        }
                                        r++;
                                    }
                                    AddBatch(split[0], split[1], UpdateRecords);
                                    continue;
                                }
                            }

                            if (split.Length == 2)
                            {
                                AddBatch(split[0], split[1]);
                            }
                            else if (split.Length == 3)
                            {
                                AddBatch(split[0], split[1], split[2]);
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
                        BatchImportForm batchImportForm = new BatchImportForm(domains,mainForm);
                        batchImportForm.ShowDialog();
                        if (batchImportForm.batches != null)
                        {
                            foreach (Batch b in batchImportForm.batches)
                            {
                                if (b.method == "BID")
                                {
                                    AddBatch(b.domain, b.method, b.bid, b.lockup);
                                }
                                else if (b.method == "TRANSFER")
                                {
                                    AddBatch(b.domain, b.method, b.toAddress);
                                }
                                else if (b.method == "UPDATE")
                                {
                                    AddBatch(b.domain, b.method, b.update);
                                }
                                else
                                {
                                    AddBatch(b.domain, b.method);
                                }
                            }
                        }
                    }
                    sr.Dispose();
                } catch (Exception ex)
                {
                    AddLog("Error importing batch: " + ex.Message);
                    NotifyForm notifyForm = new NotifyForm("Error importing batch\nMake sure the file is in not in use");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                }
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
            string key = mainForm.NodeSettings["Key"];
            string ip = mainForm.NodeSettings["IP"];
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

            try
            {
                HttpResponseMessage resp = await httpClient.SendAsync(req);
                if (resp.StatusCode != HttpStatusCode.OK)
                {
                    AddLog("Post Error: " + resp.StatusCode.ToString());
                    AddLog(await resp.Content.ReadAsStringAsync());
                    AddLog(content);
                    return "Error";
                }
                return await resp.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                AddLog("Post Error: " + ex.Message);
                return "Error";
            }

            
        }
    }
    public class Batch
    {
        public string domain { get; }
        public string method { get; }
        public decimal bid { get; }
        public decimal lockup { get; }
        public string toAddress { get; }
        public DNS[]? update { get; }
        public Batch(string domain, string method) // Normal TXs
        {
            this.domain = domain;
            this.method = method;
            bid = 0;
            lockup = 0;
            toAddress = "";
            update = null;
        }
        public Batch(string domain, string method, string toAddress) // Transfers
        {
            this.domain = domain;
            this.method = method;
            this.toAddress = toAddress;
            bid = 0;
            lockup = 0;
            update = null;
        }
        public Batch(string domain, string method, decimal bid, decimal lockup) // Bids
        {
            this.domain = domain;
            this.method = method;
            this.bid = bid;
            this.lockup = lockup;
            toAddress = "";
            update = null;
        }
        public Batch(string domain, string method, DNS[] update) // DNS Update
        {
            this.domain = domain;
            this.method = method;
            this.update = update;

        }
        public override string ToString()
        {
            if (method == "BID")
            {
                return "[\"BID\", \"" + domain + "\", " + bid + ", " + lockup + "]";
            }
            else if (method == "TRANSFER")
            {
                return "[\"TRANSFER\", \"" + domain + "\", \"" + toAddress + "\"]";
            }
            else if (method == "UPDATE" && update != null)
            {

                string records = "{\"records\":[" + string.Join(", ", update.Select(record => record.ToString())) + "]}";
                return "[\"UPDATE\", \"" + domain + "\", " + records + "]";

            }
            else if (method == "UPDATE")
            {
                return "[\"UPDATE\", \"" + domain + "\", {\"records\":[]}]";
            }
            return "[\"" + method + "\", \"" + domain + "\"]";
        }
    }
    public class DNS
    {
        public string? type { get; }
        public string? ns { get; }
        public string? address { get; }
        public string[]? TXT { get; }
        public int? keyTag { get; }
        public int? algorithm { get; }
        public int? digestType { get; }
        public string? digest { get; }

        public DNS(string type, string content) // NS, SYNTH4, SYNTH6
        {
            this.type = type;
            switch (type)
            {
                case "NS":
                    ns = content;
                    break;
                case "SYNTH4":
                    address = content;
                    break;
                case "SYNTH6":
                    address = content;
                    break;
            }
        }
        public DNS(string type, string[] content) // TXT
        {
            this.type = type;
            TXT = content;
        }
        public DNS(string type, string ns, string address) // GLUE4, GLUE6
        {
            this.type = type;
            this.ns = ns;
            this.address = address;
        }
        public DNS(string type, int keyTag, int algorithm, int digestType, string digest) // DS
        {
            this.type = type;
            this.keyTag = keyTag;
            this.algorithm = algorithm;
            this.digestType = digestType;
            this.digest = digest;
        }
        public override string ToString()
        {
            switch (type)
            {
                case "DS":
                    return "{\"type\": \"" + type + "\",\"keyTag\": " + keyTag + ",\"algorithm\": " + algorithm + ",\"digestType\": " + digestType + ",\"digest\":\"" + digest + "\"}";
                    break;
                case "NS":
                    return "{\"type\": \"" + type + "\",\"ns\": \"" + ns + "\"}";
                    break;
                case "GLUE4":
                case "GLUE6":
                    return "{\"type\": \"" + type + "\",\"ns\": \"" + ns + "\",\"address\": \"" + address + "\"}";
                    break;
                case "SYNTH4":
                case "SYNTH6":
                    return "{\"type\": \"" + type + "\",\"address\": \"" + address + "\"}";
                    break;
                case "TXT":
                    return "{\"type\": \"" + type + "\",\"txt\": [" + string.Join(", ", TXT.Select(record => ("\"" + record + "\""))) + "]}";
                    break;
            }
            return "";

        }
    }
}
