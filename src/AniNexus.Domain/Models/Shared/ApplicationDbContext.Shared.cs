using AniNexus.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AniNexus.Domain;

public partial class ApplicationDbContext
{
    /// <summary>
    /// A collection of application resource strings.
    /// </summary>
    public DbSet<ApplicationResourceModel> ApplicationResources => Set<ApplicationResourceModel>();

    /// <summary>
    /// A collection of audits.
    /// </summary>
    public DbSet<AuditModel> Audits => Set<AuditModel>();

    /// <summary>
    /// A collection of known locales.
    /// </summary>
    public DbSet<LocaleModel> Locales => Set<LocaleModel>();

    /// <summary>
    /// A collection of known media genres.
    /// </summary>
    public DbSet<MediaGenreModel> Genres => Set<MediaGenreModel>();

    /// <summary>
    /// A collection of tags that describe a piece of media.
    /// </summary>
    public DbSet<MediaTagModel> Tags => Set<MediaTagModel>();

    /// <summary>
    /// A collection of third party trackers.
    /// </summary>
    public DbSet<ThirdPartyModel> ThirdParties => Set<ThirdPartyModel>();

    /// <summary>
    /// A collection of twitter hashtags.
    /// </summary>
    public DbSet<TwitterHashTagModel> TwitterHashtags => Set<TwitterHashTagModel>();

    /// <summary>
    /// Gets an application resource.
    /// </summary>
    /// <param name="key">The key of the application resource. Expects a value in <see cref="ApplicationResource"/>.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async ValueTask<string?> GetApplicationResourceAsync(string key, CancellationToken cancellationToken = default)
    {
        var resource = await ApplicationResources.FindAsync(new object[] { key }, cancellationToken);
        return resource?.Value;
    }

    /// <summary>
    /// Returns the Japanese locale.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async ValueTask<LocaleModel> GetJapaneseLocaleAsync(CancellationToken cancellationToken = default)
    {
        var locale = await Locales.FindAsync(new object[] { 1 }, cancellationToken);
        if (locale?.LanguageCode != "ja_JP")
        {
            throw new InvalidOperationException($"Expected to find Japanese locale in index 1, found {locale?.LanguageCode ?? "Null"}");
        }
        return locale;
    }

    /// <summary>
    /// Returns the English locale.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async ValueTask<LocaleModel> GetEnglishLocaleAsync(CancellationToken cancellationToken = default)
    {
        var locale = await Locales.FindAsync(new object[] { 2 }, cancellationToken);
        if (locale?.LanguageCode != "en_US")
        {
            throw new InvalidOperationException($"Expected to find English locale in index 2, found {locale?.LanguageCode ?? "Null"}");
        }
        return locale;
    }
}
