using LCS_Management_Platform.Services.Interfaces;

namespace LCS_Management_Platform.Services.Implementations
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _httpClient;

        public HttpClientWrapper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return await _httpClient.SendAsync(request);
        }
    }
}
