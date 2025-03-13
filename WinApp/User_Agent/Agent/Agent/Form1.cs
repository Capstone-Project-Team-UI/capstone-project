using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace DeviceInfoApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string GenerateSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        //  Fetch Serial Number & IP
        private async void btnFetch_Click(object sender, EventArgs e)
        {
            txtApiOutput.Text = ""; // Clear previous logs
            txtSerial.Text = GetSerialNumber();
            txtIP.Text = GetLocalIPAddress();

            string userID = Environment.UserName; // Current user

            string organization = "Company A"; 
            string serialNumber = "Device123";

            //string serialNumber = txtSerial.Text.Trim(); (This is for later)

            string uniqueID = GenerateSHA256Hash(serialNumber); // Hash Serial Number
            string emailAddress = $"support@companya.com"; // Placeholder email

            // 🔹 Step 2: Check if user is already registered
            bool userExists = await ApiHelper.CheckUserExists(userID, organization, serialNumber, uniqueID, emailAddress, txtApiOutput);

            if (!userExists)
            {
                // 🔹 Step 3: Register user if not found
                bool registrationSuccess = await ApiHelper.RegisterUser(userID, organization, serialNumber, uniqueID, emailAddress, txtApiOutput);
                if (!registrationSuccess)
                {
                    MessageBox.Show("❌ Device registration failed. Provisioning cannot continue.", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            MessageBox.Show("✅ Device verified & registered successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }




        // 🔹 Save to Local File
        private void btnSave_Click(object sender, EventArgs e)
        {
            string data = $"Serial Number: {txtSerial.Text}\nIP Address: {txtIP.Text}";
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "device_info.txt");

            File.WriteAllText(filePath, data);
            MessageBox.Show($"Device info saved at:\n{filePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 🔹 Get System Serial Number
        private string GetSerialNumber()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BIOS");
                foreach (ManagementObject obj in searcher.Get())
                {
                    return obj["SerialNumber"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching serial number: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return "Unknown";
        }

        // 🔹 Get Local IP Address
        private string GetLocalIPAddress()
        {
            try
            {
                string hostName = Dns.GetHostName();
                IPAddress[] addresses = Dns.GetHostAddresses(hostName);
                foreach (IPAddress addr in addresses)
                {
                    if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return addr.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching IP Address: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return "Unknown";
        }

    }
}
