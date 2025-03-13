namespace DeviceInfoApp
{
    partial class Form2
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblDirectory;
        private TextBox txtDirectory;
        private Button btnBrowse;
        private Button btnRunProvisioning;
        private TextBox txtCommandOutput; // ✅ Multi-line TextBox for output
        private FolderBrowserDialog folderDialog;

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
            this.lblDirectory = new Label();
            this.txtDirectory = new TextBox();
            this.btnBrowse = new Button();
            this.btnRunProvisioning = new Button();
            this.txtCommandOutput = new TextBox();
            this.folderDialog = new FolderBrowserDialog();

            this.SuspendLayout();

            // 🔹 Label for Directory Path
            this.lblDirectory.AutoSize = true;
            this.lblDirectory.Location = new System.Drawing.Point(20, 20);
            this.lblDirectory.Text = "AIM-T Directory:";

            // 🔹 TextBox for Directory Path
            this.txtDirectory.Location = new System.Drawing.Point(150, 20);
            this.txtDirectory.Size = new System.Drawing.Size(250, 27);
            this.txtDirectory.ReadOnly = true;

            // 🔹 Browse Button
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.Location = new System.Drawing.Point(410, 20);
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);

            // 🔹 Run Provisioning Button (Replaces Check Provisioning)
            this.btnRunProvisioning.Text = "Run Provisioning Command";
            this.btnRunProvisioning.Location = new System.Drawing.Point(20, 60);
            this.btnRunProvisioning.Size = new System.Drawing.Size(250, 30);
            this.btnRunProvisioning.Click += new System.EventHandler(this.btnRunProvisioning_Click);

            // 🔹 Multi-line TextBox for Command Output
            this.txtCommandOutput.Location = new System.Drawing.Point(20, 100);
            this.txtCommandOutput.Size = new System.Drawing.Size(450, 200);
            this.txtCommandOutput.Multiline = true;
            this.txtCommandOutput.ScrollBars = ScrollBars.Vertical;
            this.txtCommandOutput.ReadOnly = true;

            // 🔹 Add Controls to Form
            this.Controls.Add(this.lblDirectory);
            this.Controls.Add(this.txtDirectory);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.btnRunProvisioning);
            this.Controls.Add(this.txtCommandOutput);

            // 🔹 Form Configurations
            this.ClientSize = new System.Drawing.Size(500, 330);
            this.Text = "Select AIM-T Directory";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
