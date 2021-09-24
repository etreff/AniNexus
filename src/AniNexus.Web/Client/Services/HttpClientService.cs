using System.Net.Http;
using System.Net.Http.Headers;

namespace AniNexus.Web.Client.Services
{
    /// <summary>
    /// A service for getting <see cref="HttpClient"/> instances.
    /// </summary>
    public interface IHttpClientService
    {
        /// <summary>
        /// Gets an <see cref="HttpClient"/> configured with the user's access token if they have one.
        /// </summary>
        ValueTask<HttpClient> GetHttpClientAsync();
    }

    internal class HttpClientService : IHttpClientService
    {
        private readonly ILocalStorageService LocalStorage;
        private readonly IHttpClientFactory HttpClientFactory;

        public HttpClientService(ILocalStorageService localStorage, IHttpClientFactory httpClientFactory)
        {
            LocalStorage = localStorage;
            HttpClientFactory = httpClientFactory;
        }

        public async ValueTask<HttpClient> GetHttpClientAsync()
        {
            var client = HttpClientFactory.CreateClient("AniNexus");
            string? token = await LocalStorage.GetAccessTokenAsync();
            if (token is not null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }
    }
}
