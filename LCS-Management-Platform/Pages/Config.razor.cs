namespace LCS_Management_Platform.Pages
{
    public partial class Config
    {
        public int TimerSeconds
        {
            get
            {
                // Convert milliseconds to seconds for display
                return SettingsService.Settings.APICooldown / 1000;
            }
            set
            {
                // Convert seconds back to milliseconds when the value changes
                SettingsService.Settings.APICooldown = value * 1000;
            }
        }

        private string newEnvironmentId = string.Empty;

        // Method to add a new environment ID to the list
        private void AddEnvironmentId()
        {
            if (!string.IsNullOrEmpty(newEnvironmentId) &&
                !SettingsService.Settings.EnvironmentIds.Contains(newEnvironmentId))
            {
                SettingsService.Settings.EnvironmentIds.Add(newEnvironmentId);
                newEnvironmentId = string.Empty; // Clear the input field
            }
        }

        // Method to remove an environment ID from the list
        private void RemoveEnvironmentId(string id)
        {
            SettingsService.Settings.EnvironmentIds.Remove(id);
        }

        // Save changes to file on button click - not tab depedent
        private void SaveChanges()
        {
            SettingsService.SaveSettings(SettingsService.Settings);
        }
    }
}