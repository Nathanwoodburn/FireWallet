using Newtonsoft.Json.Linq;

namespace FireWallet
{
    public partial class ImportTXForm : Form
    {
        MainForm mainForm;
        JObject tx;
        public ImportTXForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void ImportTXForm_Load(object sender, EventArgs e)
        {
            // Theme
            this.BackColor = ColorTranslator.FromHtml(mainForm.Theme["background"]);
            this.ForeColor = ColorTranslator.FromHtml(mainForm.Theme["foreground"]);
            foreach (Control c in Controls)
            {
                mainForm.ThemeControl(c);
            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Transaction files (*.json)|*.json";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string tx = System.IO.File.ReadAllText(openFileDialog.FileName);
                try
                {
                    JObject txObj = JObject.Parse(tx);
                    this.tx = txObj;
                    ParseTX();
                }
                catch
                {
                    NotifyForm notifyForm = new NotifyForm("Invalid transaction file.");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                    this.Close();
                }
            }
        }

        private void Cancelbutton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private async void ParseTX()
        {
            if (tx == null) this.Close();

            string hex = tx["tx"].ToString();
            string content = "{\"method\":\"decoderawtransaction\",\"params\":[\"" + hex + "\"]}";
            string response = await mainForm.APIPost("", false, content);
            if (response == null)
            {
                NotifyForm notifyForm = new NotifyForm("Error decoding transaction");
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                return;
            }
            JObject json = JObject.Parse(response);
            if (json["error"].ToString() != "")
            {
                NotifyForm notifyForm = new NotifyForm("Error decoding transaction");
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                return;
            }
            JObject metadata = (JObject)tx["metadata"];
            JArray metaInputs = (JArray)metadata["inputs"];
            JArray metaOutputs = (JArray)metadata["outputs"];

            JArray inputs = (JArray)json["result"]["vin"];
            JArray outputs = (JArray)json["result"]["vout"];
            for (int i = 0; i < inputs.Count; i++)
            {
                JObject input = (JObject)inputs[i];
                JObject metaInput = (JObject)metaInputs[i];

                Panel PanelInput = new Panel();
                PanelInput.Size = new Size(panelIn.Width - SystemInformation.VerticalScrollBarWidth - 10, 50);
                PanelInput.Location = new Point(5, panelIn.Controls.Count * 50);
                PanelInput.BorderStyle = BorderStyle.FixedSingle;

                Label txid = new Label();
                txid.Text = "TXID: " + input["txid"].ToString();
                txid.Location = new Point(5, 5);
                txid.AutoSize = true;
                PanelInput.Controls.Add(txid);

                if (metaInput.ContainsKey("sighashType"))
                {
                    Label sighashType = new Label();
                    sighashType.Text = "Sighash Type: " + metaInput["sighashType"].ToString();
                    sighashType.Location = new Point(5, 25);
                    sighashType.AutoSize = true;
                    PanelInput.Controls.Add(sighashType);
                }

                //Label address = new Label();
                //string addressString = input["address"].ToString().Substring(0, 5) + "..." + input["address"].ToString().Substring(input["address"].ToString().Length - 5, 5);
                //address.Text = "Address: " + addressString;
                //address.Location = new Point(5, 5);
                //address.AutoSize = true;
                //PanelInput.Controls.Add(address);

                //Label amount = new Label();
                //Decimal value = Decimal.Parse(input["value"].ToString()) / 1000000;
                //amount.Text = "Amount: " + value.ToString();
                //amount.Location = new Point(5, 25);
                //amount.AutoSize = true;
                //PanelInput.Controls.Add(amount);

                //if (input["path"].ToString() != "")
                //{
                //    Label ownAddress = new Label();
                //    ownAddress.Text = "Own Address";
                //    ownAddress.Location = new Point(PanelInput.Width - 100, 5);
                //    ownAddress.AutoSize = true;
                //    PanelInput.Controls.Add(ownAddress);
                //}

                panelIn.Controls.Add(PanelInput);
            }

            for (int i = 0; i < outputs.Count; i++)
            {
                JObject output = (JObject)outputs[i];
                JObject metaOutput = (JObject)metaOutputs[i];
                
                Panel PanelOutput = new Panel();
                PanelOutput.Size = new Size(panelOut.Width - SystemInformation.VerticalScrollBarWidth - 10, 50);
                PanelOutput.Location = new Point(5, panelOut.Controls.Count * 50);
                PanelOutput.BorderStyle = BorderStyle.FixedSingle;

                Label address = new Label();

                JObject addressRaw = (JObject)output["address"];

                string addressString = addressRaw["string"].ToString().Substring(0, 5) + "..." + addressRaw["string"].ToString().Substring(addressRaw["string"].ToString().Length - 5, 5);
                address.Text = "Address: " + addressString;
                address.Location = new Point(5, 5);
                address.AutoSize = true;
                PanelOutput.Controls.Add(address);

                JObject covenant = (JObject)output["covenant"];
                if (covenant.ContainsKey("action"))
                {
                    if (covenant["action"].ToString() != "NONE")
                    {
                        if (metaOutput.ContainsKey("name"))
                        {



                            Label covenantLabel = new Label();
                            string name = metaOutput["name"].ToString();

                            covenantLabel.Text = covenant["action"].ToString() + ": " + name;
                            covenantLabel.Location = new Point(5, 25);
                            covenantLabel.AutoSize = true;
                            PanelOutput.Controls.Add(covenantLabel);
                        }
                    }
                }

                bool own = false;
                string addressResp = await mainForm.APIGet("wallet/" + mainForm.Account + "/key/" + addressRaw["string"].ToString(),true);
                if (addressResp != "Error") own = true;

                if (own)
                {
                    Label ownAddress = new Label();
                    ownAddress.Text = "Own Address";
                    ownAddress.Location = new Point(PanelOutput.Width - 100, 5);
                    ownAddress.AutoSize = true;
                    PanelOutput.Controls.Add(ownAddress);
                }

                Label amount = new Label();
                Decimal value = Decimal.Parse(output["value"].ToString());
                amount.Text = "Amount: " + value.ToString();
                amount.Location = new Point(PanelOutput.Width - 100, 25);
                amount.AutoSize = true;
                PanelOutput.Controls.Add(amount);
                panelOut.Controls.Add(PanelOutput);
            }
        }
    }
}
