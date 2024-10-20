using LCS_Management_Platform.Models;

namespace LCS_Management_Platform.Services.Interfaces
{
    public interface IEnvironmentDataService
    {
        Task<EnvironmentData> GetEnvironmentDataAsync(string projectId, string environmentId, string accessToken);
        Task<EnvironmentHistoryData> GetEnvironmentHistoryDataAsync(string projectId, string environmentId, string accessToken);
    }
}
