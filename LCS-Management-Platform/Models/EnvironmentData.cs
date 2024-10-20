namespace LCS_Management_Platform.Models
{
    public class EnvironmentData
    {
        public int ResultPageCurrent { get; set; }
        public bool ResultHasMorePages { get; set; }
        public List<EnvDataList> Data { get; set; }
        public bool IsSuccess { get; set; }
        public string OperationActivityId { get; set; }
        public object ErrorMessage { get; set; }
        public DateTime VersionEOL { get; set; }

    }
}
