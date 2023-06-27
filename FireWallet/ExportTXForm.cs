using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;

namespace FireWallet
{
    public partial class ExportTXForm : Form
    {
        MainForm mainForm;
        List<Transaction> TXs;
        public ExportTXForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.BackColor = ColorTranslator.FromHtml(mainForm.Theme["background"]);
            this.ForeColor = ColorTranslator.FromHtml(mainForm.Theme["foreground"]);
            foreach (Control c in Controls)
            {
                mainForm.ThemeControl(c);
            }
            GetTXs();
        }

        private async void ExportTXForm_Load(object sender, EventArgs e)
        {


        }
        private async Task GetTXs()
        {
            // Get TXs
            TXs = new List<Transaction>();

            // Check how many TX there are
            string APIresponse = await mainForm.APIGet("wallet/" + mainForm.Account, true);
            JObject wallet = JObject.Parse(APIresponse);
            if (!wallet.ContainsKey("balance"))
            {
                mainForm.AddLog("GetInfo Error");
                mainForm.AddLog(APIresponse);
                return;
            }
            JObject balance = JObject.Parse(wallet["balance"].ToString());
            int TotalTX = Convert.ToInt32(balance["tx"].ToString());
            int toGet = (int)numericUpDownLimit.Value;

            if (toGet > TotalTX) toGet = TotalTX;
            int toSkip = TotalTX - toGet;

            // GET TXs
            if (mainForm.WatchOnly)
            {
                APIresponse = await mainForm.APIPost("", true, "{\"method\": \"listtransactions\",\"params\": [\"default\"," + toGet + "," + toSkip + ", true]}");
            }
            else
            {
                APIresponse = await mainForm.APIPost("", true, "{\"method\": \"listtransactions\",\"params\": [\"default\"," + toGet + "," + toSkip + "]}");
            }

            if (APIresponse == "Error")
            {
                mainForm.AddLog("GetInfo Error");
                return;
            }
            JObject TXGET = JObject.Parse(APIresponse);

            // Check for error
            if (TXGET["error"].ToString() != "")
            {
                mainForm.AddLog("GetInfo Error");
                mainForm.AddLog(APIresponse);
                return;
            }

            JArray txs = JArray.Parse(TXGET["result"].ToString());
            if (toGet > txs.Count) toGet = txs.Count; // In case there are less TXs than expected (usually happens when the get TX's fails)
            Control[] tmpControls = new Control[toGet];
            for (int i = 0; i < toGet; i++)
            {

                // Get last tx
                JObject tx = JObject.Parse(await mainForm.APIGet("wallet/" + mainForm.Account + "/tx/" + txs[toGet - i - 1]["txid"].ToString(), true));
                Transaction transaction = new Transaction();
                transaction.Hash = tx["hash"].ToString();
                transaction.Date = DateTime.Parse(tx["mdate"].ToString());
                transaction.Confirmations = Convert.ToInt32(tx["confirmations"].ToString());

                string hash = transaction.Hash;
                string date = transaction.Date.ToShortDateString();




                Panel tmpPanel = new Panel();
                tmpPanel.Width = groupBoxTXs.Width - SystemInformation.VerticalScrollBarWidth - 20;
                tmpPanel.Height = 50;
                tmpPanel.Location = new Point(5, (i * 55));
                tmpPanel.BorderStyle = BorderStyle.FixedSingle;
                tmpPanel.BackColor = ColorTranslator.FromHtml(mainForm.Theme["background-alt"]);
                tmpPanel.ForeColor = ColorTranslator.FromHtml(mainForm.Theme["foreground-alt"]);
                tmpPanel.Controls.Add(
                    new Label()
                    {
                        Text = "Date: " + date,
                        Location = new Point(10, 5)
                    }
                    );
                int confirmations = Convert.ToInt32(tx["confirmations"].ToString());
                if (mainForm.UserSettings.ContainsKey("confirmations"))
                {
                    if (confirmations < Convert.ToInt32(mainForm.UserSettings["confirmations"]))
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
                transaction.Amount = costHNS;
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
                    TXForm txForm = new TXForm(mainForm, hash);
                    txForm.Show();
                };
                foreach (Control c in tmpPanel.Controls)
                {
                    c.Click += (sender, e) =>
                    {
                        TXForm txForm = new TXForm(mainForm, hash);
                        txForm.Show();
                    };
                }
                tmpControls[i] = tmpPanel;


                // Outputs
                foreach (JObject output in outputs)
                {
                    Output tmpOutput = new Output();
                    tmpOutput.Address = output["address"].ToString();
                    tmpOutput.Amount = decimal.Parse(output["value"].ToString());
                    // Convert amount to HNS
                    tmpOutput.Amount = tmpOutput.Amount / 1000000;
                    JObject covenant = (JObject)output["covenant"];

                    if (output["path"].ToString() != "")
                    {
                        tmpOutput.Own = true;
                    }
                    if (covenant.ContainsKey("action"))
                    {
                        tmpOutput.Covenant = covenant["action"].ToString();
                        tmpOutput.Name = "";
                        if (covenant["action"].ToString() != "NONE")
                        {

                            JArray items = (JArray)covenant["items"];
                            string namehash = items[0].ToString();
                            string content = "{\"method\": \"getnamebyhash\", \"params\": [\"" + namehash + "\"]}";
                            JObject name = JObject.Parse(await mainForm.APIPost("", false, content));
                            tmpOutput.Name = name["result"].ToString();
                        }
                    }
                    else
                    {
                        tmpOutput.Covenant = "NONE";
                        tmpOutput.Name = "";
                    }
                    transaction.Outputs.Add(tmpOutput);
                }

                TXs.Add(transaction);
            }
            groupBoxTXs.Controls.Clear();
            Panel txPanel = new Panel();
            txPanel.Width = groupBoxTXs.Width - SystemInformation.VerticalScrollBarWidth;
            txPanel.Controls.AddRange(tmpControls);
            txPanel.AutoScroll = true;
            txPanel.Dock = DockStyle.Fill;
            groupBoxTXs.Controls.Add(txPanel);
        }

        private async void buttonRefresh_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            await GetTXs();
            this.Enabled = true;
        }

        private async void buttonExport_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            await GetTXs();
            string filter = comboBoxFilter.Text;
            if (filter == "ALL")
            {
                filter = "";
            }

            //List<Transaction> filteredTXs = TXs.Where(tx => tx.Filter(filter)).ToList();
            List<Transaction> filteredTXs = TXs.FindAll(tx => tx.Filter(filter));

            JArray outputJson = new JArray();
            foreach (Transaction tx in filteredTXs)
            {
                //outputJson += tx.ToString(filter) + "\n";
                outputJson.Add(tx.ToJson(filter));
            }
            this.Enabled = true;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON File|*.json";
            saveFileDialog.Title = "Save JSON File";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, outputJson.ToString());
                } catch
                {
                    NotifyForm notifyForm = new NotifyForm("Error saving JSON file");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                }
            }

        }
    }
    public class Transaction
    {
        public string Hash { get; set; }
        public DateTime Date { get; set; }
        public int Confirmations { get; set; }
        public decimal Amount { get; set; }
        public List<Output> Outputs { get; set; }
        public Transaction()
        {
            Hash = "";
            Date = new DateTime();
            Confirmations = 0;
            Amount = 0;
            Outputs = new List<Output>();
        }
        public string ToString(string filter)
        {
            string output = "";
            output += Date.ToShortDateString() + "," + Hash + "," + Amount + "," + Confirmations + ","+ filter + ",";
            if (filter == "")
            {
                string names = "";
                string addresses = "";
                names = string.Join(";", Outputs.Select(o => o.Name));
                addresses = string.Join(";", Outputs.Select(o => o.Address));
                output += names + "," + addresses;
            } else if (filter == "NONE")
            {
                output += "," + string.Join(";", Outputs.Select(o => o.Address));
            } else
            {
                string names = "";
                string addresses = "";
                names = string.Join(";", Outputs.Select(o => o.Name));
                addresses = string.Join(";", Outputs.Select(o => o.Address));
                output += names + "," + addresses;
            }
            return output;
        }
        public JObject ToJson(string filter)
        {
            JObject output = new JObject();
            output.Add("date", Date.ToShortDateString());
            output.Add("hash", Hash);
            output.Add("amount", Amount);
            output.Add("confirmations", Confirmations);
            JArray outputArray = new JArray();
            foreach (Output output1 in Outputs)
            {
                outputArray.Add(output1.ToJson());
            }

            output.Add("outputs", outputArray);
            return output;
        }

        public bool Filter(string filter)
        {
            if (filter == "")
            {
                return true;
            }
            else if (filter == "NONE")
            {
                foreach (Output output in Outputs)
                {
                    if (output.Covenant != filter) // EXPORT NONE ONLY TXs
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                foreach (Output output in Outputs)
                {
                    if (output.Covenant == filter)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
    public class Output
    {
        public string Address { get; set; }
        public decimal Amount { get; set; }
        public string Covenant { get; set; }
        public string Name { get; set; }
        public bool Own { get; set; }
        public Output()
        {
            Address = "";
            Amount = 0;
            Covenant = "";
            Name = "";
            Own = false;
        }
        public override string ToString()
        {
            if (Covenant == "NONE")
            {
                return "Address: " + Address + " Amount: " + Amount + " HNS";
            }
            else
            {
                return "Address: " + Address + " Amount: " + Amount + " HNS Covenant: " + Covenant + " Name: " + Name;
            }
        }
        public JObject ToJson()
        {
            JObject output = new JObject();
            output.Add("address", Address);
            output.Add("amount", Amount);
            output.Add("covenant", Covenant);
            output.Add("name", Name);
            output.Add("own-address", Own);
            return output;
        }
    }
}
