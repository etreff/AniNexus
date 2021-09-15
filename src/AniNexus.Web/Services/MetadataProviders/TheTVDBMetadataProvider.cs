using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using AniNexus.Domain;
using AniNexus.Models.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AniNexus.Services.MetadataProviders;

public sealed class TheTVDBMetadataProvider : AnimeMetadataProvider
{
    public override string Name { get; } = "TheTVDB";

    private readonly IHttpClientFactory HttpClientFactory;
    private readonly IOptions<TheTVDBOptions> Options;

    public TheTVDBMetadataProvider(
        IDbContextFactory<ApplicationDbContext> dbContextFactory,
        IHttpClientFactory httpClientFactory,
        IOptions<TheTVDBOptions> options,
        ILogger<AniDbMetadataProvider> logger)
        : base(dbContextFactory, logger)
    {
        HttpClientFactory = httpClientFactory;
        Options = options;
    }

    protected override string GetAnimeByIdEndpoint(string thirdPartyAnimeId)
    {
        return $"https://api4.thetvdb.com/v4/series/{thirdPartyAnimeId}/extended";
    }

    protected override string GetAnimeByNameEndpoint(string animeName)
    {
        throw new NotSupportedException();
    }

    protected override async Task<object?> GetMetadataByIdAsyncCore(string endpoint, CancellationToken cancellationToken)
    {
        var httpClient = HttpClientFactory.CreateClient();
        var options = Options.Value;

        string accessToken;
        using (var response = await httpClient.PostAsJsonAsync($"https://api4.thetvdb.com/v4/login", new { apiKey = options.ApiKey, pin = options.Pin }, cancellationToken))
        {
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseModel = await response.Content.ReadFromJsonAsync<TheTVDBAuthResult>((JsonSerializerOptions?)null, cancellationToken);
            if (!string.Equals(responseModel?.Status, "", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(responseModel?.Data?.Token))
            {
                return null;
            }

            accessToken = responseModel.Data.Token;
        }

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
        using (var response = await httpClient.GetAsync(endpoint, cancellationToken))
        {
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseModel = await response.Content.ReadFromJsonAsync<TheTVDBMetadataResult>((JsonSerializerOptions?)null, cancellationToken);

            // TODO: Transform

            return responseModel;
        }

    }

    protected override Task<object?> GetMetadataByNameAsyncCore(string endpoint, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private class TheTVDBAuthResult
    {
        [JsonPropertyName("data")]
        public TheTVDBAuthData? Data { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = default!;
    }

    private class TheTVDBAuthData
    {
        [JsonPropertyName("token")]
        public string? Token { get; set; }
    }

    private class TheTVDBMetadataResult
    {
        // https://thetvdb.github.io/v4-api/#/Series/getSeriesExtended
    }
}
