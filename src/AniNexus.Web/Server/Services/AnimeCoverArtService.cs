//using System.Collections.Concurrent;
//using System.IO;
//using System.IO.Compression;
//using System.Net.Http;
//using System.Xml.Linq;
//using AniNexus.Domain;
//using Microsoft.EntityFrameworkCore;

//namespace AniNexus.Services;

///// <summary>
///// A service for downloading Anime cover art to the asset server.
///// </summary>
//public interface IAnimeCoverArtService : IDisposable, IAsyncDisposable
//{
//    /// <summary>
//    /// Asynchronously fetches the url to an Anime's cover art on the asset server.
//    /// If the covert art does not exist for this anime, it will be downloaded to the
//    /// server.
//    /// </summary>
//    /// <param name="animeId">The AniNexus anime Id.</param>
//    /// <param name="cancellationToken">The cancellation token.</param>
//    public ValueTask<Uri?> GetCoverArtUrlAsync(int animeId, CancellationToken cancellationToken = default);
//}

//internal class AnimeCoverArtService : IAnimeCoverArtService
//{
//    private readonly IDbContextFactory<ApplicationDbContext> DbContextFactory;
//    private readonly IContentPathProvider ContentPathProvider;
//    private readonly ILogger Logger;
//    private readonly HttpClient HttpClient;

//    private readonly ConcurrentDictionary<int, TaskCompletionSource> DownloadSources;

//    public AnimeCoverArtService(
//        IContentPathProvider contentPathProvider,
//        IDbContextFactory<ApplicationDbContext> dbContextFactory,
//        ILogger<AnimeCoverArtService> logger)
//    {
//        DbContextFactory = dbContextFactory;
//        ContentPathProvider = contentPathProvider;
//        Logger = logger;

//        HttpClient = new HttpClient();
//        DownloadSources = new ConcurrentDictionary<int, TaskCompletionSource>();
//    }

//    public void Dispose()
//    {
//        HttpClient.Dispose();
//        foreach (var element in DownloadSources)
//        {
//            element.Value.TrySetException(new ObjectDisposedException(nameof(AnimeCoverArtService)));
//        }
//        DownloadSources.Clear();
//    }

//    public ValueTask DisposeAsync()
//    {
//        Dispose();

//        return default;
//    }

//    public async ValueTask<Uri?> GetCoverArtUrlAsync(int animeId, CancellationToken cancellationToken = default)
//    {
//        var webResourceLocation = await ContentPathProvider.GetCdnResourceAsync(ApplicationResource.AnimeCoverArtPathKey);
//        var physicalResourceLocation = new FileInfo(Path.Combine(ContentPathProvider.Root.FullName, string.Format(webResourceLocation.Resource, animeId)));

//        if (physicalResourceLocation.Exists)
//        {
//            return new Uri(webResourceLocation.ToString(animeId));
//        }

//        var tcs = new TaskCompletionSource();
//        var downloadSource = DownloadSources.GetOrAdd(animeId, tcs);

//        bool hasPrimaryToken = ReferenceEquals(tcs, downloadSource);
//        if (!hasPrimaryToken)
//        {
//            await tcs.Task;
//            return new Uri(webResourceLocation.ToString(animeId));
//        }

//        try
//        {
//            await DownloadCoverArtAsync(animeId, physicalResourceLocation.FullName, cancellationToken);
//            DownloadSources.TryRemove(animeId, out _);
//        }
//        catch (Exception e)
//        {
//            Logger.LogError(e, "An error has occurred while downloading anime cover art.");
//            try
//            {
//                File.Delete(physicalResourceLocation.FullName);
//            }
//            catch (Exception e2)
//            {
//                // Suppress
//                Logger.LogError(e2, "An error has occurred while deleting the local anime cover art file.");
//            }

//            return null;
//        }
//        finally
//        {
//            tcs.TrySetResult();
//        }

//        return new Uri(webResourceLocation.ToString(animeId));
//    }

//    private async Task DownloadCoverArtAsync(int animeId, string destination, CancellationToken cancellationToken = default)
//    {
//        using var context = DbContextFactory.CreateDbContext();

//        // TODO: No idea where to grab these.
//        // For now we will download them from AniDB. We need to replace this ASAP since leeching off of AniDB's work
//        // is a big no-no. We risk marking the AMSA client as a leech.
//        var anime = await context.Anime
//            .Include(a => a.ExternalIds)
//            .FirstOrDefaultAsync(a => a.Id == animeId, cancellationToken);

//        if (anime is null)
//        {
//            throw new InvalidOperationException($"No anime with Id {animeId} exists.");
//        }

//        var thirdPartySource = anime.ExternalIds?.FirstOrDefault(e => e.ThirdParty.Name == "AniDB");
//        if (thirdPartySource is null)
//        {
//            throw new InvalidOperationException($"Unable to find the third party association for the anime with Id {animeId}");
//        }

//        string thirdPartyId = thirdPartySource.ExternalMediaId;

//        using var scope = Logger.BeginScope(animeId);

//        // The image name is not the same as the anime Id. MAL works the same way for some reason, presumably to
//        // prevent people from doing what I'm doing here. We need to grab the anime information directly from
//        // AniDB and get the image path from there.

//        string thirdPartyResourceUrl = $"http://api.anidb.net:9001/httpapi?request=anime&client=amsa&clientver=1&protover=1&aid={thirdPartyId}";
//        Logger.LogInformation($"Downloading anime {animeId} information from AniDB (ExternalId = {thirdPartyId}) from {thirdPartyResourceUrl}");

//        string thirdPartyArtResource;
//        using (var response = await HttpClient.GetAsync(thirdPartyResourceUrl, cancellationToken))
//        {
//            response.EnsureSuccessStatusCode();

//            await using var ms = await response.Content.ReadAsStreamAsync(cancellationToken);
//            await using var gz = new GZipStream(ms, CompressionMode.Decompress, true);

//            var doc = await XDocument.LoadAsync(gz, LoadOptions.None, cancellationToken);
//            var imageNode = doc.Root!.Element("picture");
//            thirdPartyArtResource = imageNode!.Value;
//        }

//        thirdPartyResourceUrl = $"https://cdn-us.anidb.net/images/main/{thirdPartyArtResource}";

//        Logger.LogInformation($"Downloading cover art for anime {animeId} (ExternalId = {thirdPartyId}) from {thirdPartyResourceUrl}");
//        using (var response = await HttpClient.GetAsync(thirdPartyResourceUrl, cancellationToken))
//        {
//            response.EnsureSuccessStatusCode();

//            await using var ms = await response.Content.ReadAsStreamAsync(cancellationToken);
//            await using var fs = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);

//            if (ms.CanSeek)
//            {
//                ms.Seek(0, SeekOrigin.Begin);
//            }

//            Logger.LogInformation($"Writing image to {destination}");
//            await ms.CopyToAsync(fs, cancellationToken);
//            await fs.FlushAsync(cancellationToken);
//            Logger.LogInformation("Image written succesfully.");
//        }
//    }
//}
