using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json.Linq;

namespace FireWallet
{
    public partial class ImportTXForm : Form
    {
        MainForm mainForm;
        JObject tx;
        int totalSigs;
        int reqSigs;
        int sigs;
        string signedTX;
        string[] domains;
        public ImportTXForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            tx = null;
        }
        public ImportTXForm(MainForm mainForm, string tx)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            JObject txObj = JObject.Parse(tx);
            this.tx = txObj;
        }

        private void ImportTXForm_Load(object sender, EventArgs e)
        {
            // Default variables
            domains = new string[0];
            signedTX = "";
            totalSigs = 3;
            reqSigs = 2;
            sigs = 0;

            // Theme
            this.BackColor = ColorTranslator.FromHtml(mainForm.Theme["background"]);
            this.ForeColor = ColorTranslator.FromHtml(mainForm.Theme["foreground"]);
            foreach (Control c in Controls)
            {
                mainForm.ThemeControl(c);
            }
            if (tx == null)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Transaction files (*.json)|*.json";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string tx = System.IO.File.ReadAllText(openFileDialog.FileName);
                    try
                    {
                        JObject txObj = JObject.Parse(tx);
                        this.tx = txObj;
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
            ParseTX();
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

            // Get multisig info
            JObject firstIn = (JObject)inputs[0];
            JArray witnesses = (JArray)firstIn["txinwitness"];

            string scriptSig = witnesses[witnesses.Count - 1].ToString();
            // decode script
            string sigGetContent = "{\"method\":\"decodescript\",\"params\":[\"" + scriptSig + "\"]}";
            string sigGetResponse = await mainForm.APIPost("", false, sigGetContent);
            if (sigGetResponse == null)
            {
                NotifyForm notifyForm = new NotifyForm("Error decoding transaction");
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                return;
            }
            JObject sigGetJson = JObject.Parse(sigGetResponse);
            if (sigGetJson["error"].ToString() != "")
            {
                NotifyForm notifyForm = new NotifyForm("Error decoding transaction");
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                return;
            }
            JObject sigGetResult = (JObject)sigGetJson["result"];
            string[] asm = sigGetResult["asm"].ToString().Split(" ");
            string totalSigsStr = asm[asm.Length - 2];
            totalSigs = int.Parse(totalSigsStr.Replace("OP_", ""));
            reqSigs = int.Parse(sigGetResult["reqSigs"].ToString());
            sigs = -1;
            for (int i = 0; i < witnesses.Count; i++)
            {
                string witness = witnesses[i].ToString();
                if (witness != "")
                {
                    sigs++;
                }
            }





            // Set sig label sizes
            labelSigsReq.Width = (labelSigsTotal.Width / totalSigs) * reqSigs;
            labelSigsSigned.Width = (labelSigsTotal.Width / totalSigs) * sigs;
            labelSigInfo.Text = "Signed: " + sigs + "\nReq: " + reqSigs + " of " + totalSigs;





            for (int i = 0; i < inputs.Count; i++)
            {
                JObject input = (JObject)inputs[i];
                JObject metaInput = (JObject)metaInputs[i];

                Panel PanelInput = new Panel();
                PanelInput.Size = new Size(panelIn.Width - SystemInformation.VerticalScrollBarWidth - 10, 50);
                PanelInput.Location = new Point(5, panelIn.Controls.Count * 50);
                PanelInput.BorderStyle = BorderStyle.FixedSingle;


                if (metaInput.ContainsKey("sighashType"))
                {
                    Label sighashType = new Label();
                    sighashType.Text = "Sighash Type: " + metaInput["sighashType"].ToString();
                    sighashType.Location = new Point(5, 25);
                    sighashType.AutoSize = true;
                    PanelInput.Controls.Add(sighashType);
                }

                string txid = input["txid"].ToString();
                int vout = int.Parse(input["vout"].ToString());
                string txInfo = await mainForm.APIGet("tx/" + txid, false);
                if (txInfo == "Error" || txInfo == "")
                {
                    NotifyForm notifyForm = new NotifyForm("Error getting transaction info");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                    return;
                }
                else
                {
                    JObject txInfoJson = JObject.Parse(txInfo);
                    JArray txoutputs = (JArray)txInfoJson["outputs"];
                    JObject txoutput = (JObject)txoutputs[vout];
                    string txoutputAddress = txoutput["address"].ToString();

                    Label address = new Label();
                    string addressString = txoutputAddress.Substring(0, 5) + "..." + txoutputAddress.Substring(txoutputAddress.Length - 5, 5);
                    address.Text = "Address: " + addressString;
                    address.Location = new Point(5, 5);
                    address.AutoSize = true;
                    PanelInput.Controls.Add(address);

                    Label amount = new Label();
                    Decimal value = Decimal.Parse(txoutput["value"].ToString()) / 1000000;
                    amount.Text = "Amount: " + value.ToString();
                    amount.Location = new Point(5, 25);
                    amount.AutoSize = true;
                    PanelInput.Controls.Add(amount);

                }



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

                            string[] domainsNew = new string[domains.Length + 1];
                            for (int j = 0; j < domains.Length; j++)
                            {
                                domainsNew[j] = domains[j];
                            }
                            domainsNew[domainsNew.Length - 1] = name;
                            domains = domainsNew;
                        }
                    }
                }

                bool own = false;
                string addressResp = await mainForm.APIGet("wallet/" + mainForm.Account + "/key/" + addressRaw["string"].ToString(), true);
                if (addressResp != "Error") own = true;

                if (own)
                {
                    Label ownAddress = new Label();
                    ownAddress.Text = "Own Address";
                    ownAddress.Location = new Point(PanelOutput.Width - 150, 5);
                    ownAddress.AutoSize = true;
                    PanelOutput.Controls.Add(ownAddress);
                }

                Label amount = new Label();
                Decimal value = Decimal.Parse(output["value"].ToString());
                amount.Text = "Amount: " + value.ToString();
                amount.Location = new Point(PanelOutput.Width - 150, 25);
                amount.AutoSize = true;
                PanelOutput.Controls.Add(amount);
                panelOut.Controls.Add(PanelOutput);
            }
        }

        private async void buttonSign_Click(object sender, EventArgs e)
        {
            if (!mainForm.WatchOnly)
            {
                string content = "{\"tx\":\"" + tx["tx"].ToString() + "\", \"passphrase\":\"" + mainForm.Password + "\"}";
                string response = await mainForm.APIPost("wallet/" + mainForm.Account + "/sign", true, content);
                if (response == "Error" || response == "")
                {
                    NotifyForm notifyForm = new NotifyForm("Error signing transaction");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                    return;
                }
                buttonSign.Enabled = false;
                buttonExport.Enabled = true;
                sigs++;
                // Set sig label sizes
                labelSigsReq.Width = (labelSigsTotal.Width / totalSigs) * reqSigs;
                labelSigsSigned.Width = (labelSigsTotal.Width / totalSigs) * sigs;
                labelSigInfo.Text = "Signed: " + sigs + "\nReq: " + reqSigs + " of " + totalSigs;
                signedTX = response;
            } else {

            }
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            mainForm.ExportTransaction(signedTX,domains);
        }

        private async void buttonSend_Click(object sender, EventArgs e)
        {
            string content = "";
            if (signedTX != "")
            {
                JObject signed = JObject.Parse(signedTX);
                content = "{\"method\":\"sendrawtransaction\", \"params\":[\"" + signed["hex"].ToString() + "\"]}";
            }
            else
            {
                content = "{\"method\":\"sendrawtransaction\", \"params\":[\"" + tx["tx"].ToString() + "\"]}";
            }
            string response = await mainForm.APIPost("", false, content);
            if (response == "Error" || response == "")
            {
                mainForm.AddLog(response);
                NotifyForm notifyError = new NotifyForm("Error sending transaction");
                notifyError.ShowDialog();
                notifyError.Dispose();
                return;
            }
            JObject responseJson = JObject.Parse(response);
            if (responseJson["error"].ToString() != "")
            {
                mainForm.AddLog(response);
                JObject error = (JObject)responseJson["error"];
                NotifyForm notifyError = new NotifyForm("Error sending transaction\n" + error["message"].ToString());
                notifyError.ShowDialog();
                notifyError.Dispose();
                return;
            }
            string txHash = responseJson["result"].ToString();
            NotifyForm notifyForm = new NotifyForm("Transaction sent\nIf the transaction hasn't been signed it might not be mined", "Explorer", mainForm.UserSettings["explorer-tx"] + txHash);
            notifyForm.ShowDialog();
            notifyForm.Dispose();
            this.Close();
            
        }
    }
}
