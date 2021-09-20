using System.Net.Http;

namespace AniNexus.Web.Client
{
    /// <summary>
    /// <see cref="HttpClient"/> names.
    /// </summary>
    public static class HttpClientName
    {
        /// <summary>
        /// The name of a <see cref="HttpClient"/> that does not require or include access
        /// tokens when making server requests.
        /// </summary>
        public const string Anon = "AniNexus.AnonAPI";

        /// <summary>
        /// The name of a <see cref="HttpClient"/> that requires and includes addess tokens
        /// when making server requests. Unauthorized/unauthenticated users will not be able
        /// to make calls with this client.
        /// </summary>
        public const string Auth = "AniNexus.AuthAPI";

        /// <summary>
        /// Returns a <see cref="HttpClient"/> that allows anonymous HTTP requests to the server.
        /// </summary>
        /// <param name="factory">The client factory.</param>
        public static HttpClient GetAnonClient(this IHttpClientFactory factory)
        {
            return factory.CreateClient(Anon);
        }

        /// <summary>
        /// Returns a <see cref="HttpClient"/> that requires authentication before sending
        /// HTTP requests to the server.
        /// </summary>
        /// <param name="factory">The client factory.</param>
        public static HttpClient GetAuthClient(this IHttpClientFactory factory)
        {
            return factory.CreateClient(Auth);
        }
    }
}
