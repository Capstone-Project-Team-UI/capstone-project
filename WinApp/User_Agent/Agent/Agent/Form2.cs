using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Windows.Forms;

namespace DeviceInfoApp
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        // 🔹 Open Folder Browser Dialog
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                txtDirectory.Text = folderDialog.SelectedPath;
            }
        }

        

        // 🔹 Helper Method: Check if AIMTManageabilityService is Running
        private bool IsServiceRunning(string serviceName)
        {
            try
            {
                ServiceController sc = new ServiceController(serviceName);
                return sc.Status == ServiceControllerStatus.Running;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking service: {ex.Message}", "Service Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // 🔹 Helper Method: Run CMD as Administrator and Execute Command
        private void RunCommandAsAdmin(string workingDirectory, string command)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {command}",
                    WorkingDirectory = workingDirectory,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    Verb = "runas"  // Ensure it runs as Administrator
                };

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit(); // Wait for execution to complete
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error running AIM-TProvisioningApp.exe: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnRunProvisioning_Click(object sender, EventArgs e)
        {
            txtCommandOutput.Text = ""; // Clear previous output
            string selectedDirectory = txtDirectory.Text;
            if (string.IsNullOrEmpty(selectedDirectory) || !Directory.Exists(selectedDirectory))
            {
                MessageBox.Show("Please select a valid directory!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 🔹 Ensure AIMTManageabilityService is running
            if (!IsServiceRunning("AIMTManageabilityService"))
            {
                MessageBox.Show("AIM-T Manageability Service is not running. Ensure AIM-T is enabled in BIOS.", "Service Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 🔹 Construct full paths
            string provisioningAppPath = Path.Combine(selectedDirectory, "AIM-TProvisioningApp.exe");
            string logFilePath = Path.Combine(selectedDirectory, "provision_log.txt");

            if (!File.Exists(provisioningAppPath))
            {
                MessageBox.Show("AIM-TProvisioningApp.exe not found in the selected directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 🔹 Ensure path is enclosed in double quotes (for spaces in paths)
            string workingDir = $"\"{selectedDirectory}\"";

            // 🔹 **Step 1: Ensure Log File is Created with Proper Permissions**
            try
            {
                string createLogFileCommand = $"echo. > \"{logFilePath}\" && icacls \"{logFilePath}\" /grant Everyone:F";
                RunCommandAsAdmin(selectedDirectory, createLogFileCommand);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error creating log file: {ex.Message}", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 🔹 Run the `_oMt` provisioning command and save output to log file
            txtCommandOutput.AppendText("🔹 Running AIM-T Provisioning (_oMt)...\r\n");

            string provisionCommand = $"cd /d {workingDir} && \"{provisioningAppPath}\" -i AIM-T_CRYPTO_Package_01_oMt >> \"{logFilePath}\" 2>&1";
            RunCommandAsAdmin(selectedDirectory, provisionCommand);

            // 🔹 Ensure the log file exists before reading
            if (!File.Exists(logFilePath))
            {
                MessageBox.Show("❌ Error: Log file was not created. Ensure the application has write permissions.", "Log Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 🔹 Read the log file and check for "System owned error"
            bool alreadyProvisioned = false;
            string[] logLines = File.ReadAllLines(logFilePath);
            txtCommandOutput.AppendText("🔹 Log File Output:\r\n" + string.Join("\r\n", logLines) + "\r\n\r\n");

            foreach (string line in logLines)
            {
                if (line.Contains("System owned error"))
                {
                    alreadyProvisioned = true;
                    break;
                }
            }

            // 🔹 If "System owned error" is detected, run `_M`
            if (alreadyProvisioned)
            {
                txtCommandOutput.AppendText("⚠ AIM-T is already provisioned. Running re-provisioning (_M)...\r\n");
                string reprovisionCommand = $"cd /d {workingDir} && \"{provisioningAppPath}\" -i AIM-T_CRYPTO_Package_01_M >> \"{logFilePath}\" 2>&1";
                RunCommandAsAdmin(selectedDirectory, reprovisionCommand);
                txtCommandOutput.AppendText("✅ Already provisioned, please restart for re-provisioning.\r\n");
            }
            else
            {
                txtCommandOutput.AppendText("✅ Successfully Provisioned, please restart.\r\n");
            }

            // 🔹 **Step 2: Delete Log File After Completion**
            try
            {
                File.Delete(logFilePath);
                txtCommandOutput.AppendText("🗑 Log file deleted after successful provisioning.\r\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"⚠ Warning: Failed to delete log file. You may delete it manually.\n{ex.Message}", "Cleanup Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }









    }
}
