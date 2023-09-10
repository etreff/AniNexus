namespace AniNexus.MediaProviders.Anilist;

/// <summary>
/// Anilist configuration options.
/// </summary>
public sealed class AnilistOptions
{
    /// <summary>
    /// The Anilist Client Id.
    /// </summary>
    public string ClientId { get; set; } = default!;

    /// <summary>
    /// The Anilist Client secret.
    /// </summary>
    public string ClientSecret { get; set; } = default!;

    /// <summary>
    /// The redirect URI.
    /// </summary>
    public string RedirectUri { get; set; } = default!;
}
