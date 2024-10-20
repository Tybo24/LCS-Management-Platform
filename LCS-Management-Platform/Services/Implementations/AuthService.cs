using LCS_Management_Platform.Data;
using LCS_Management_Platform.Models;
using LCS_Management_Platform.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace LCS_Management_Platform.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly SettingsService _settingsService;

        public AuthService(IHttpClientWrapper httpClientWrapper, SettingsService settingsService)
        {
            _httpClientWrapper = httpClientWrapper;
            _settingsService = settingsService;
        }

        public async Task<bool> GetAuthTokenAsync(string username, string password)
        {
            try
            {
                var url = $"https://login.microsoftonline.com/{_settingsService.Settings.AzureTenant}/oauth2/v2.0/token";

                var keyValuePairs = new[]
                {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("client_id", _settingsService.Settings.ClientId),
                new KeyValuePair<string, string>("scope", _settingsService.Settings.LCSScope),

            };

                var content = new FormUrlEncodedContent(keyValuePairs);

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                var response = await _httpClientWrapper.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    StoreAuthToken(responseContent);

                    return response.IsSuccessStatusCode;
                }
                else
                {
                    throw new HttpRequestException($"Authentication request failed with status code {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;

            }
        }

        private async void StoreAuthToken(string responseContent)
        {
            if (!string.IsNullOrEmpty(responseContent))
            {
                AuthToken authToken = JsonConvert.DeserializeObject<AuthToken>(responseContent);
                AppState.accessToken = authToken.access_token;
                AppState.tokenExpiresAt = DateTime.UtcNow.AddSeconds(authToken.expires_in);
            }
        }
    }

}
