using System.Net.Http.Headers;

namespace LCS_Management_Platform.Helpers
{
    public static class HttpRequestHelper
    {
        public static HttpRequestMessage CreateGetRequest(string url, string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Add("x-ms-version", "2017-09-15");
            return request;
        }

        public static HttpRequestMessage CreatePostRequest(string url, string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Add("x-ms-version", "2017-09-15");
            return request;
        }
    }
}
