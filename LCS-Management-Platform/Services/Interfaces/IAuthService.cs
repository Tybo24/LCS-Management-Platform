using LCS_Management_Platform.Data;

namespace LCS_Management_Platform.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> GetAuthTokenAsync(string username, string password);
    }
}
