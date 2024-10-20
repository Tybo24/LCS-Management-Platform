using System.Net.NetworkInformation;

namespace LCS_Management_Platform.Data
{
    public class Settings
    {
        public string Project {  get; set; }
		public string LCSAPI { get; set; }
		public List<string> EnvironmentIds { get; set; }
        public int APICooldown { get; set; }
        public string AdminUser { get; set; }
        public string ClientId { get; set; }
        public string AzureTenant {  get; set; }
        public string LCSScope { get; set; }
    }
}
