namespace LCS_Management_Platform.Services.Interfaces
{
    public interface IHttpClientWrapper
    {
        //Task<HttpResponseMessage> PostAsync(string url, HttpContent content);

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }
}
