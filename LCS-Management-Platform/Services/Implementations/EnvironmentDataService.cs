using LCS_Management_Platform.Data;
using LCS_Management_Platform.Helpers;
using LCS_Management_Platform.Models;
using LCS_Management_Platform.Services.Interfaces;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Web.Http;

namespace LCS_Management_Platform.Services.Implementations
{
    public class EnvironmentDataService : IEnvironmentDataService
    {
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly SettingsService _settingsService;

        public EnvironmentDataService(IHttpClientWrapper httpClientWrapper, SettingsService settingsService)
        {
            _httpClientWrapper = httpClientWrapper;
            _settingsService = settingsService;
        }

        public async Task<EnvironmentData> GetEnvironmentDataAsync(string projectId, string environmentId, string accessToken)
        {
            try
            {
                var url = $"{_settingsService.Settings.LCSAPI}/environmentinfo/v1/detail/project/{projectId}/?environmentId={environmentId}";

                var request = HttpRequestHelper.CreateGetRequest(url, accessToken);

                var response = await _httpClientWrapper.SendAsync(request);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<EnvironmentData>(responseContent);
                }
                else if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    ExceptionHelper.HandleTooManyRequests();
                }
                else
                {
                    throw new Exception($"{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"API call failed. {ex.Message}");
            }

            return null;
        }

        public async Task<EnvironmentHistoryData> GetEnvironmentHistoryDataAsync(string projectId, string environmentId, string accessToken)
        {
            try
            {
                var url = $"{_settingsService.Settings.LCSAPI}/environmentinfo/v1/history/project/{projectId}/environment/{environmentId}/?page=1";

                var request = HttpRequestHelper.CreateGetRequest(url, accessToken);

                var response = await _httpClientWrapper.SendAsync(request);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<EnvironmentHistoryData>(responseContent);
                }
                else if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    ExceptionHelper.HandleTooManyRequests();
                }
                else
                {
                    throw new Exception($"{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"API call failed. {ex.Message}");
            }

            return null;
        }
    }
}
