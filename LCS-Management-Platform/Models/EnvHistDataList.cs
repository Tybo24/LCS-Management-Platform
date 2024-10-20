namespace LCS_Management_Platform.Models
{
    public class EnvHistDataList
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string TypeDisplay { get; set; }
        public DateTime StartDateTimeUTC { get; set; }
        public DateTime EndDateTimeUTC { get; set; }
        public string Status { get; set; }
        public string ActivityId { get; set; }
        public string EnvironmentId { get; set; }
        public int ProjectId { get; set; }
    }
}
