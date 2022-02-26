using System.Collections.Immutable;
using System.Reflection;

namespace AniNexus.Domain;

/// <summary>
/// Application resource keys.
/// </summary>
public sealed class AppResource
{
    internal static IImmutableDictionary<string, string> Resources { get; }

    /// <summary>
    /// The key for the resource that contains the content CDN hostname.
    /// </summary>
    public static AppResource ContentHostKey { get; } = new AppResource("ContentHostKey", "https://localhost:5001");

    /// <summary>
    /// The key for the resource that contains the path format for anime cover art.
    /// </summary>
    public static AppResource AnimeCoverArtPathKey { get; } = new AppResource("AnimeCoverArtPath", "assets/coverart/anime/{0}.jpg");

    /// <summary>
    /// The key for the resource that contains the path format for manga cover art.
    /// </summary>
    public static AppResource MangaCoverArtPathKey { get; } = new AppResource("MangaCoverArtPath", "assets/coverart/manga/{0}.jpg");

    /// <summary>
    /// The key for the resource that contains the path format for game cover art.
    /// </summary>
    public static AppResource GameCoverArtPathKey { get; } = new AppResource("GameCoverArtPath", "assets/coverart/game/{0}.jpg");

    /// <summary>
    /// The key for the resource that contains the path format for media series cover art.
    /// </summary>
    public static AppResource MediaSeriesCoverArtPathKey { get; } = new AppResource("MediaSeriesCoverArtPath", "assets/coverart/franchise/{0}.jpg");

    /// <summary>
    /// The key for the resource that contains the path format for OST album art.
    /// </summary>
    public static AppResource SoundTrackAlbumArtPathKey { get; } = new AppResource("SoundTrackAlbumArtPath", "assets/coverart/ost/{0}.jpg");

    /// <summary>
    /// The key for the resource that contains the path format for a character's picture.
    /// </summary>
    public static AppResource CharacterPicturePathkey { get; } = new AppResource("CharacterPicturePath", "assets/coverart/character/{0}.jpg");

    /// <summary>
    /// The key for the resource that contains the path format for a person's profile picture.
    /// </summary>
    public static AppResource PersonPicturePathkey { get; } = new AppResource("PersonPicturePath", "assets/coverart/person/{0}.jpg");

    /// <summary>
    /// The key of the app resource.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// The default value of the app resource.
    /// </summary>
    internal string DefaultValue { get; }

    private AppResource(string key, string value)
    {
        Key = key;
        DefaultValue = value;
    }

    static AppResource()
    {
        Resources = typeof(AppResource)
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Where(p => p.PropertyType == typeof(AppResource))
            .Select(p => (AppResource)p.GetValue(null)!)
            .ToImmutableDictionary(p => p.Key, p => p.DefaultValue);
    }
}
