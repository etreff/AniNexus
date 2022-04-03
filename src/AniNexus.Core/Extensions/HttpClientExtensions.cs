using System.IO;
using System.Net.Http;

namespace AniNexus;

/// <summary>
/// Stores the progress of a download.
/// </summary>
public readonly struct DownloadProgress
{
    /// <summary>
    /// The number of bytes currently downloaded.
    /// </summary>
    public readonly long CurrentBytes { get; }

    /// <summary>
    /// The total number of bytes being downloaded.
    /// </summary>
    public readonly long TotalBytes { get; }

    /// <summary>
    /// Creates a new <see cref="DownloadProgress"/> instance.
    /// </summary>
    /// <param name="currentBytes">The current number of bytes downloaded.</param>
    /// <param name="totalBytes">The total number of bytes to download.</param>
    public DownloadProgress(long currentBytes, long totalBytes)
    {
        CurrentBytes = currentBytes;
        TotalBytes = totalBytes;
    }

    /// <summary>
    /// Returns a string representation of this <see cref="DownloadProgress"/> instance.
    /// </summary>
    public override readonly string ToString()
    {
        return $"{CurrentBytes}/{TotalBytes}";
    }
}

/// <summary>
/// <see cref="HttpClient"/> extensions.
/// </summary>
public static class HttpClientExtensions
{
    /// <summary>
    /// Downloads a resource into the provided destination stream.
    /// </summary>
    /// <param name="client">The <see cref="HttpClient"/>.</param>
    /// <param name="requestUri">The URI of the requested resource.</param>
    /// <param name="destination">The stream in which to write the resource bytes.</param>
    /// <param name="progress">The progress provider.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <remarks>https://stackoverflow.com/a/46497896</remarks>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="client"/> is <see langword="null"/></exception>
    /// <exception cref="T:System.ArgumentException"><paramref name="requestUri"/> is null, empty, or whitespace.</exception>
    public static async Task DownloadFileAsync(this HttpClient client, string requestUri, Stream destination, IProgress<DownloadProgress>? progress = null, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(client, nameof(client));
        Guard.IsNotNull(requestUri, nameof(requestUri));
        Guard.IsNotNull(destination, nameof(destination));

        using var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        long? contentLength = response.Content.Headers.ContentLength;

        using var download = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

        if (progress is null || !contentLength.HasValue)
        {
            await download.CopyToAsync(destination, cancellationToken).ConfigureAwait(false);
            return;
        }

        var relativeProgress = new Progress<long>(bytesDownloaded => progress.Report(new DownloadProgress(bytesDownloaded, contentLength.Value)));
        await download.CopyToAsync(destination, 1024 * 32 /* KB */, relativeProgress, cancellationToken).ConfigureAwait(false);
        progress.Report(new DownloadProgress(contentLength.Value, contentLength.Value));
    }

    /// <summary>
    /// Downloads a resource to the provided file location.
    /// </summary>
    /// <param name="client">The <see cref="HttpClient"/>.</param>
    /// <param name="requestUri">The URI of the requested resource.</param>
    /// <param name="destination">The file path in which to write the resource bytes.</param>
    /// <param name="overwrite">Whether to overwrite <paramref name="destination"/> if it already exists.</param>
    /// <param name="progress">The progress provider.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="FileInfo"/> object referring to the downloaded file.</returns>
    /// <remarks>https://stackoverflow.com/a/46497896</remarks>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="client"/> is <see langword="null"/></exception>
    /// <exception cref="ArgumentException"><paramref name="requestUri"/> is null, empty, or whitespace.</exception>
    /// <exception cref="IOException"><paramref name="destination"/> already exists and <paramref name="overwrite"/> is <see langword="false"/>.</exception>
    public static async Task<FileInfo> DownloadFileAsync(this HttpClient client, string requestUri, string destination, bool overwrite = true, IProgress<DownloadProgress>? progress = null, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(client, nameof(client));
        Guard.IsNotNull(requestUri, nameof(requestUri));
        Guard.IsNotNullOrWhiteSpace(destination, nameof(destination));

        var file = new FileInfo(destination);
        if (file.Exists)
        {
            if (!overwrite)
            {
                throw new IOException($"The file {destination} already exists.");
            }

            file.Delete();
        }

        using var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        long? contentLength = response.Content.Headers.ContentLength;

        using var download = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

        using var fs = file.OpenWrite();

        if (progress is null || !contentLength.HasValue)
        {
            await download.CopyToAsync(fs, cancellationToken).ConfigureAwait(false);
            await fs.FlushAsync(cancellationToken);
            file.Refresh();
            return file;
        }

        var relativeProgress = new Progress<long>(bytesDownloaded => progress.Report(new DownloadProgress(bytesDownloaded, contentLength.Value)));

        await download.CopyToAsync(fs, 1024 * 32 /* KB */, relativeProgress, cancellationToken).ConfigureAwait(false);
        await fs.FlushAsync(cancellationToken);
        progress.Report(new DownloadProgress(contentLength.Value, contentLength.Value));

        file.Refresh();
        return file;
    }

    /// <summary>
    /// Send a GET request to the specified Uri and return the response body as a stream in an asynchronous operation.
    /// </summary>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    public static async Task<Stream> GetStreamAsync(this HttpClient client, string requestUri, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(client, nameof(client));
        Guard.IsNotNull(requestUri, nameof(requestUri));

        var response = await client.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Send a GET request to the specified Uri and return the response body as a stream in an asynchronous operation.
    /// </summary>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    public static async Task<Stream> GetStreamAsync(this HttpClient client, Uri requestUri, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(client, nameof(client));
        Guard.IsNotNull(requestUri, nameof(requestUri));

        var response = await client.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
    }
}
