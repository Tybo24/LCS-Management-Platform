namespace LCS_Management_Platform.Services.Interfaces
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }
}
