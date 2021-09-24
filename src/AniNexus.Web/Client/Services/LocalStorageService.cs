using System.Text.Json;
using Microsoft.JSInterop;

namespace AniNexus.Web.Client.Services
{
    /// <summary>
    /// A service for interacting with the browser's local storage.
    /// </summary>
    public interface ILocalStorageService
    {
        /// <summary>
        /// Gets a value from the persisted local storage.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="key">The key of the object.</param>
        ValueTask<T?> GetAsync<T>(string key);

        /// <summary>
        /// Sets a value in the persisted local storage.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="key">The key of the object.</param>
        /// <param name="value">The object to persist.</param>
        ValueTask SetAsync<T>(string key, T? value);

        /// <summary>
        /// Removes an item from the persisted local storage.
        /// </summary>
        /// <param name="key">The key of the item to remove.</param>
        ValueTask RemoveAsync(string key);

        /// <summary>
        /// Returns the user's access token.
        /// </summary>
        ValueTask<string?> GetAccessTokenAsync();

        /// <summary>
        /// Sets the user's access token.
        /// </summary>
        /// <param name="token">The access token.</param>
        ValueTask SetAccessTokenAsync(string? token);
    }

    internal class LocalStorageService : ILocalStorageService
    {
        private const string AccessTokenKey = "token";
        private readonly IJSRuntime JSRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            JSRuntime = jsRuntime;
        }

        public ValueTask<string?> GetAccessTokenAsync()
            => GetAsync<string>(AccessTokenKey);

        public ValueTask SetAccessTokenAsync(string? token)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                return SetAsync(AccessTokenKey, token);
            }
            else
            {
                return RemoveAsync(AccessTokenKey);
            }
        }

        public async ValueTask<T?> GetAsync<T>(string key)
        {
            string? json = await JSRuntime.InvokeAsync<string?>("localStorage.getItem", key);
            if (json is null)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T?>(json);
        }

        public ValueTask RemoveAsync(string key)
        {
            return JSRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }

        public ValueTask SetAsync<T>(string key, T? value)
        {
            return JSRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));
        }
    }
}
