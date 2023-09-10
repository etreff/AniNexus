using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.JSInterop;

/// <summary>
/// <see cref="IJSRuntime"/> extensions.
/// </summary>
public static class IJsRuntimeExtensions
{
    internal const string StreamImageMethodName = "setImage";

    /// <summary>
    /// Opens a URL in a new tab.
    /// </summary>
    /// <param name="jsRuntime">The Javascript runtime.</param>
    /// <param name="uri">The URI to navigate to.</param>
    public static ValueTask OpenUrlInNewTabAsync(this IJSRuntime jsRuntime, string uri)
    {
        return jsRuntime.InvokeVoidAsync("open", uri, "_blank");
    }

    /// <summary>
    /// Opens a URL in a new tab.
    /// </summary>
    /// <param name="jsRuntime">The Javascript runtime.</param>
    /// <param name="uri">The URI to navigate to.</param>
    public static ValueTask OpenUrlInNewTabAsync(this IJSRuntime jsRuntime, Uri uri)
    {
        return jsRuntime.InvokeVoidAsync("open", uri.ToString(), "_blank");
    }

    /// <summary>
    /// Opens a URL in a popup window.
    /// </summary>
    /// <param name="jsRuntime">The Javascript runtime.</param>
    /// <param name="uri">The URI to navigate to.</param>
    public static ValueTask OpenUrlInPopupWindowAsync(this IJSRuntime jsRuntime, string uri)
    {
        return jsRuntime.InvokeVoidAsync("open", uri, "popup", "popup=true");
    }

    /// <summary>
    /// Opens a URL in a popup window.
    /// </summary>
    /// <param name="jsRuntime">The Javascript runtime.</param>
    /// <param name="uri">The URI to navigate to.</param>
    public static ValueTask OpenUrlInPopupWindowAsync(this IJSRuntime jsRuntime, Uri uri)
    {
        return jsRuntime.InvokeVoidAsync("open", uri.ToString(), "popup", "popup=true");
    }

    /// <summary>
    /// Streams the contents of <paramref name="stream"/> to the image element with an id of <paramref name="elementId"/>.
    /// </summary>
    /// <param name="jsRuntime">The JavaScript runtime.</param>
    /// <param name="stream">The image data.</param>
    /// <param name="elementId">The id of the element to stream the content to.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Whether the data was streamed successfully.</returns>
    public static async Task<bool> StreamImageAsync(this IJSRuntime jsRuntime, Stream stream, string elementId, CancellationToken cancellationToken = default)
    {
        if (stream is not null)
        {
            try
            {
                var dotnetImageStream = new DotNetStreamReference(stream);
                await jsRuntime.InvokeVoidAsync(StreamImageMethodName, cancellationToken, elementId, dotnetImageStream);

                return true;
            }
            catch
            {
                // Suppress
            }
        }

        return false;
    }
}
