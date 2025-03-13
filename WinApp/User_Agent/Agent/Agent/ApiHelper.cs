using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.IO;

namespace DeviceInfoApp
{
    public class ApiHelper
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string baseUrl = "http://localhost:8090/users"; // Update if needed

        // 🔹 Logs API calls to UI and a file
        private static void LogApiResponse(string message, TextBox outputBox)
        {
            string logMessage = $"{DateTime.Now}: {message}\r\n";
            outputBox.AppendText(logMessage);

            // Save to log file
            string logFilePath = Path.Combine(Application.StartupPath, "api_log.txt");
            File.AppendAllText(logFilePath, logMessage);
        }

        //  Check if device exists in API
        public static async Task<bool> CheckUserExists(string userID, string organization, string serialNumber, string uniqueID, string emailAddress, TextBox outputBox)
        {
            string requestUrl = $"{baseUrl}/checkUser?userID={userID}&organization={organization}&serialNumber={serialNumber}&uniqueID={uniqueID}&emailAddress={emailAddress}";

            try
            {
                LogApiResponse($"🔍 Checking user: {requestUrl}", outputBox);
                HttpResponseMessage response = await client.GetAsync(requestUrl);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    LogApiResponse($"✅ User exists: {responseBody}", outputBox);
                    return true;
                }
                else
                {
                    LogApiResponse($"❌ User not found: {responseBody}", outputBox);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogApiResponse($"⚠ API Request Failed: {ex.Message}", outputBox);
                return false;
            }
        }

        // 🔹 Register new device in API
        public static async Task<bool> RegisterUser(string userID, string organization, string serialNumber, string uniqueID, string emailAddress, TextBox outputBox)
        {
            var requestBody = new
            {
                userID = userID,
                organization = organization,
                serialNumber = serialNumber,
                uniqueID = uniqueID,
                emailAddress = emailAddress
            };

            string json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                LogApiResponse($"📤 Registering user: {json}", outputBox);
                HttpResponseMessage response = await client.PostAsync(baseUrl, content);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    LogApiResponse($"✅ Registration successful: {responseBody}", outputBox);
                    return true;
                }
                else
                {
                    LogApiResponse($"❌ Registration failed: {responseBody}", outputBox);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogApiResponse($"⚠ API Request Failed: {ex.Message}", outputBox);
                return false;
            }
        }

    }
}
