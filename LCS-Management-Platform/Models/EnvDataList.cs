namespace LCS_Management_Platform.Models
{
    public class EnvDataList
    {
        public string EnvironmentId { get; set; }
        public string EnvironmentName { get; set; }
        public int ProjectId { get; set; }
        public string EnvironmentInfrastructure { get; set; }
        public string EnvironmentType { get; set; }
        public string EnvironmentGroup { get; set; }
        public string EnvironmentProduct { get; set; }
        public string EnvironmentEndpointBaseUrl { get; set; }
        public string DeploymentState { get; set; }
        public string TopologyDisplayName { get; set; }
        public string CurrentApplicationBuildVersion { get; set; }
        public string CurrentApplicationReleaseName { get; set; }
        public string CurrentPlatformReleaseName { get; set; }
        public string CurrentPlatformVersion { get; set; }
        public string DeployedOnUTC { get; set; }
        public string CloudStorageLocation { get; set; }
        public string DisasterRecoveryLocation { get; set; }
        public string DeploymentStatusDisplay { get; set; }
        public bool CanStart { get; set; }
        public bool CanStop { get; set; }
        public int Number { get; set; }
        public bool ShowDetails { get; set; }
        public bool DataMissing { get; set; }
        public string CurrentCustomisation { get; set; }
        public IList<EnvHistDataList> EnvHistData { get; set; }
    }
}
