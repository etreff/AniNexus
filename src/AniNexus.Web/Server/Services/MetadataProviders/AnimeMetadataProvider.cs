using AniNexus.Domain;
using Microsoft.EntityFrameworkCore;

namespace AniNexus.Services.MetadataProviders;

/// <summary>
/// Defines a provider of anime metadata.
/// </summary>
public interface IAnimeMetadataProvider
{
    /// <summary>
    /// The name of the provider.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Downloads metadata from a third party tracker.
    /// </summary>
    /// <param name="animeId">The AniNexus anime Id to get the metadata for.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <remarks>
    /// This provider will fetch information about an anime from a third party tracker. This information
    /// can be used to quickly populate default values for an anime entry.
    /// </remarks>
    public Task<object?> GetMetadataAsync(int animeId, CancellationToken cancellationToken = default)
        => GetMetadataAsync(animeId, null, cancellationToken);

    /// <summary>
    /// Downloads metadata from a third party tracker.
    /// </summary>
    /// <param name="animeId">The AniNexus anime Id to get the metadata for.</param>
    /// <param name="thirdPartyAnimeId">The third party anime Id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <remarks>
    /// This provider will fetch information about an anime from a third party tracker. This information
    /// can be used to quickly populate default values for an anime entry.
    /// </remarks>
    public Task<object?> GetMetadataAsync(int animeId, string? thirdPartyAnimeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads metadata from a third party tracker.
    /// </summary>
    /// <param name="animeName">The name of the anime to get the metadata for.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <remarks>
    /// This provider will fetch information about an anime from a third party tracker. This information
    /// can be used to quickly populate default values for an anime entry.
    /// </remarks>
    public Task<object?> GetMetadataAsync(string animeName, CancellationToken cancellationToken = default);
}

public abstract class AnimeMetadataProvider : IAnimeMetadataProvider
{
    public abstract string Name { get; }
    protected IDbContextFactory<ApplicationDbContext> DbContextFactory { get; }
    protected ILogger Logger { get; }

    protected AnimeMetadataProvider(
        IDbContextFactory<ApplicationDbContext> dbContextFactory,
        ILogger logger)
    {
        DbContextFactory = dbContextFactory;
        Logger = logger;
    }

    public async Task<object?> GetMetadataAsync(int animeId, string? thirdPartyAnimeId, CancellationToken cancellationToken = default)
    {
        thirdPartyAnimeId ??= await MapAnimeIdToThirdPartyIdAsync(animeId, cancellationToken);
        if (string.IsNullOrWhiteSpace(thirdPartyAnimeId))
        {
            return null;
        }

        string thirdPartyApiEndpoint = GetAnimeByIdEndpoint(thirdPartyAnimeId);
        Logger.LogInformation($"Downloading anime {animeId} information (ExternalId = {thirdPartyAnimeId}) from {thirdPartyApiEndpoint}");

        return await GetMetadataByIdAsyncCore(thirdPartyApiEndpoint, cancellationToken);
    }

    public async Task<object?> GetMetadataAsync(string animeName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(animeName))
        {
            return null;
        }

        string thirdPartyApiEndpoint = GetAnimeByNameEndpoint(animeName);
        Logger.LogInformation($"Downloading anime {animeName} information from {thirdPartyApiEndpoint}");

        return await GetMetadataByNameAsyncCore(thirdPartyApiEndpoint, cancellationToken);
    }

    protected virtual async ValueTask<string?> MapAnimeIdToThirdPartyIdAsync(int animeId, CancellationToken cancellationToken)
    {
        using var dbContext = DbContextFactory.CreateDbContext();

        var anime = await dbContext.Anime.Include(a => a.ExternalIds).FirstOrDefaultAsync(a => a.Id == animeId, cancellationToken);
        if (anime is null)
        {
            return null;
        }

        string name = GetThirdPartyDatabaseKey();
        var thirdParty = anime.ExternalIds.FirstOrDefault(e => e.ThirdParty.Name == name);
        if (thirdParty is null)
        {
            return null;
        }

        return thirdParty.ExternalMediaId;
    }

    protected virtual string GetThirdPartyDatabaseKey()
    {
        return Name;
    }

    protected abstract string GetAnimeByIdEndpoint(string thirdPartyAnimeId);
    protected abstract string GetAnimeByNameEndpoint(string animeName);
    protected abstract Task<object?> GetMetadataByIdAsyncCore(string endpoint, CancellationToken cancellationToken);
    protected abstract Task<object?> GetMetadataByNameAsyncCore(string endpoint, CancellationToken cancellationToken);
}
