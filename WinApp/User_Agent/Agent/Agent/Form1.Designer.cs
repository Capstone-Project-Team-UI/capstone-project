namespace DeviceInfoApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblSerial;
        private TextBox txtSerial;
        private Label lblIP;
        private TextBox txtIP;
        private Button btnFetch;
        private Button btnSave;
        private Button btnOpenAIMTDirectory; // ✅ Keep only for opening Form2
        private TextBox txtApiOutput;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {

            this.txtApiOutput = new TextBox();

            // 🔹 API Output TextBox
            this.txtApiOutput.Location = new System.Drawing.Point(20, 220);
            this.txtApiOutput.Size = new System.Drawing.Size(400, 100);
            this.txtApiOutput.Multiline = true;
            this.txtApiOutput.ScrollBars = ScrollBars.Vertical;
            this.txtApiOutput.ReadOnly = true;

            // 🔹 Add to Form
            this.Controls.Add(this.txtApiOutput);

            this.lblSerial = new Label();
            this.txtSerial = new TextBox();
            this.lblIP = new Label();
            this.txtIP = new TextBox();
            this.btnFetch = new Button();
            this.btnSave = new Button();
            this.btnOpenAIMTDirectory = new Button();

            this.SuspendLayout();

            // 🔹 Serial Number Label
            this.lblSerial.AutoSize = true;
            this.lblSerial.Location = new System.Drawing.Point(20, 20);
            this.lblSerial.Text = "Serial Number:";

            // 🔹 Serial Number TextBox
            this.txtSerial.Location = new System.Drawing.Point(150, 20);
            this.txtSerial.Size = new System.Drawing.Size(250, 27);
            this.txtSerial.ReadOnly = true;

            // 🔹 IP Address Label
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(20, 60);
            this.lblIP.Text = "IP Address:";

            // 🔹 IP Address TextBox
            this.txtIP.Location = new System.Drawing.Point(150, 60);
            this.txtIP.Size = new System.Drawing.Size(250, 27);
            this.txtIP.ReadOnly = true;

            // 🔹 Fetch Button
            this.btnFetch.Location = new System.Drawing.Point(20, 100);
            this.btnFetch.Size = new System.Drawing.Size(120, 30);
            this.btnFetch.Text = "Fetch Info";
            this.btnFetch.Click += new System.EventHandler(this.btnFetch_Click);

            // 🔹 Save Button
            this.btnSave.Location = new System.Drawing.Point(150, 100);
            this.btnSave.Size = new System.Drawing.Size(120, 30);
            this.btnSave.Text = "Save Locally";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // 🔹 Open File Browser Button (Now for Form2)
            this.btnOpenAIMTDirectory.Text = "Select AIM-T Directory";
            this.btnOpenAIMTDirectory.Location = new System.Drawing.Point(20, 140);
            this.btnOpenAIMTDirectory.Size = new System.Drawing.Size(250, 30);
            this.btnOpenAIMTDirectory.Click += new System.EventHandler(this.btnOpenAIMTDirectory_Click);

            // 🔹 Add Components to Form
            this.Controls.Add(this.lblSerial);
            this.Controls.Add(this.txtSerial);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.btnFetch);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnOpenAIMTDirectory);

            // 🔹 Form Settings
            this.ClientSize = new System.Drawing.Size(450, 200);
            this.Name = "Form1";
            this.Text = "Device Info";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void btnOpenAIMTDirectory_Click(object sender, EventArgs e)
        {
            Form2 aimtDirectoryForm = new Form2();
            aimtDirectoryForm.Show();
        }
    }
}
