using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace FireWallet
{
    public partial class TXForm : Form
    {
        MainForm mainForm;
        JObject tx;
        string txid;
        public TXForm(MainForm mainForm, string txid)
        {
            InitializeComponent();
            // Theme
            this.BackColor = ColorTranslator.FromHtml(mainForm.Theme["background"]);
            this.ForeColor = ColorTranslator.FromHtml(mainForm.Theme["foreground"]);
            foreach (Control c in Controls)
            {
                mainForm.ThemeControl(c);
            }

            this.mainForm = mainForm;
            this.txid = txid;
        }

        private async void TXForm_Load(object sender, EventArgs e)
        {
            tx = JObject.Parse(await mainForm.APIGet("wallet/"+mainForm.Account+"/tx/" + txid,true));

            this.Text = "TX: " + tx["hash"].ToString();
            labelHash.Text = "Hash: " + tx["hash"].ToString();
            mainForm.AddLog("Viewing TX: " + tx["hash"].ToString());
            
            // Disable scrolling on the panels until they are populated
            panelInputs.Visible = false;
            panelOutputs.Visible = false;

            // For each input
            JArray inputs = (JArray)tx["inputs"];
            foreach (JObject input in inputs)
            {
                Panel PanelInput = new Panel();
                PanelInput.Size = new Size(panelInputs.Width - SystemInformation.VerticalScrollBarWidth - 10, 50);
                PanelInput.Location = new Point(5, panelInputs.Controls.Count * 50);
                PanelInput.BorderStyle = BorderStyle.FixedSingle;

                Label address = new Label();
                string addressString = input["address"].ToString().Substring(0,5) + "..." + input["address"].ToString().Substring(input["address"].ToString().Length - 5, 5);
                address.Text = "Address: " + addressString;
                address.Location = new Point(5, 5);
                address.AutoSize = true;
                PanelInput.Controls.Add(address);

                Label amount = new Label();
                Decimal value = Decimal.Parse(input["value"].ToString())/1000000;
                amount.Text = "Amount: " + value.ToString();
                amount.Location = new Point(5, 25);
                amount.AutoSize = true;
                PanelInput.Controls.Add(amount);

                if (input["path"].ToString() != "")
                {
                    Label ownAddress = new Label();
                    ownAddress.Text = "Own Address";
                    ownAddress.Location = new Point(PanelInput.Width - 100, 5);
                    ownAddress.AutoSize = true;
                    PanelInput.Controls.Add(ownAddress);
                }

                panelInputs.Controls.Add(PanelInput);
            }
            panelInputs.Visible = true;
            // For each output
            JArray outputs = (JArray)tx["outputs"];
            foreach (JObject output in outputs)
            {
                Panel PanelOutput = new Panel();
                PanelOutput.Size = new Size(panelOutputs.Width - SystemInformation.VerticalScrollBarWidth - 10, 50);
                PanelOutput.Location = new Point(5, panelOutputs.Controls.Count * 50);
                PanelOutput.BorderStyle = BorderStyle.FixedSingle;

                Label address = new Label();
                string addressString = output["address"].ToString().Substring(0, 5) + "..." + output["address"].ToString().Substring(output["address"].ToString().Length - 5, 5);
                address.Text = "Address: " + addressString;
                address.Location = new Point(5, 5);
                address.AutoSize = true;
                PanelOutput.Controls.Add(address);

                JObject covenant = (JObject)output["covenant"];
                if (covenant.ContainsKey("action"))
                {
                    if (covenant["action"].ToString() != "NONE")
                    {
                        JArray items = (JArray)covenant["items"];


                        Label covenantLabel = new Label();
                        string namehash = items[0].ToString();

                        string content = "{\"method\": \"getnamebyhash\", \"params\": [\"" +namehash +"\"]}";
                        JObject name = JObject.Parse(await mainForm.APIPost("",false,content));


                        covenantLabel.Text = covenant["action"].ToString() + ": " +  name["result"].ToString();
                        covenantLabel.Location = new Point(5, 25);
                        covenantLabel.AutoSize = true;
                        PanelOutput.Controls.Add(covenantLabel);
                    }
                }

                Label amount = new Label();
                Decimal value = Decimal.Parse(output["value"].ToString()) / 1000000;
                amount.Text = "Amount: " + value.ToString();
                amount.Location = new Point(PanelOutput.Width - 100, 25);
                amount.AutoSize = true;
                PanelOutput.Controls.Add(amount);

                if (output["path"].ToString() != "")
                {
                    Label ownAddress = new Label();
                    ownAddress.Text = "Own Address";
                    ownAddress.Location = new Point(PanelOutput.Width - 100, 5);
                    ownAddress.AutoSize = true;
                    PanelOutput.Controls.Add(ownAddress);
                }

                panelOutputs.Controls.Add(PanelOutput);
            }
            panelOutputs.Visible = true;
        }

        private void Explorer_Click(object sender, EventArgs e)
        {
            // Open the transaction in a browser
            string url = mainForm.UserSettings["explorer-tx"] + tx["hash"].ToString();
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}
