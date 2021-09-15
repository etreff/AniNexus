using System.IO.Compression;
using System.Net.Http;
using System.Web;
using System.Xml.Linq;
using AniNexus.Domain;
using Microsoft.EntityFrameworkCore;

namespace AniNexus.Services.MetadataProviders;

public sealed class AniDbMetadataProvider : AnimeMetadataProvider
{
    public override string Name { get; } = "AniDB";

    private readonly IHttpClientFactory HttpClientFactory;

    public AniDbMetadataProvider(
        IDbContextFactory<ApplicationDbContext> dbContextFactory,
        IHttpClientFactory httpClientFactory,
        ILogger<AniDbMetadataProvider> logger)
        : base(dbContextFactory, logger)
    {
        HttpClientFactory = httpClientFactory;
    }

    protected override string GetAnimeByIdEndpoint(string thirdPartyAnimeId)
    {
        return $"http://api.anidb.net:9001/httpapi?request=anime&client=amsa&clientver=1&protover=1&aid={HttpUtility.UrlEncode(thirdPartyAnimeId)}";
    }

    protected override string GetAnimeByNameEndpoint(string animeName)
    {
        throw new NotImplementedException();
    }

    protected override async Task<object?> GetMetadataByIdAsyncCore(string endpoint, CancellationToken cancellationToken)
    {
        var httpClient = HttpClientFactory.CreateClient();

        using var response = await httpClient.GetAsync(endpoint, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        await using var ms = await response.Content.ReadAsStreamAsync(cancellationToken);
        await using var gz = new GZipStream(ms, CompressionMode.Decompress, true);

        var doc = await XDocument.LoadAsync(gz, LoadOptions.None, cancellationToken);

        //TODO: Map result to model
        object? model = null;

        return model;

    }

    protected override Task<object?> GetMetadataByNameAsyncCore(string endpoint, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
