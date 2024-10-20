using LCS_Management_Platform.Data;
using System.Text.Json;

namespace LCS_Management_Platform.Services
{
    public class SettingsService
    {
        private readonly string _filePath;

        public Settings Settings { get; private set; }

        public SettingsService()
        {
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Settings.json");
            LoadSettings();
        }

        public void LoadSettings()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                Settings = JsonSerializer.Deserialize<Settings>(json);
            }
            else
            {
                // Default settings if the file does not exist
                Settings = new Settings
                {
                    Project = "",
                    LCSAPI = "https://lcsapi.lcs.dynamics.com",
                    EnvironmentIds = new List<string>(),
                    APICooldown = 60000,
                    AdminUser = "",
                    ClientId = "",
                    AzureTenant = "",
                    LCSScope = "https://lcsapi.lcs.dynamics.com//.default"
                };
            }
        }

        public void SaveSettings(Settings newPreferences)
        {
            Settings = newPreferences;
            var json = JsonSerializer.Serialize(Settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}
