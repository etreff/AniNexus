namespace AniNexus.Domain;

/// <summary>
/// Keys to pass into <see cref="ApplicationDbContext.GetApplicationResourceAsync(string, CancellationToken)"/>.
/// </summary>
public static class ApplicationResource
{
    /// <summary>
    /// The key for the resource that contains the content CDN hostname.
    /// </summary>
    public const string ContentHostKey = "ContentHostKey";

    /// <summary>
    /// The key for the resource that contains the path format for anime cover art.
    /// </summary>
    public const string AnimeCoverArtPathKey = "AnimeCoverArtPath";

    /// <summary>
    /// The key for the resource that contains the path format for media series cover art.
    /// </summary>
    public const string MediaSeriesCoverArtPathKey = "MediaSeriesCoverArtPath";

    /// <summary>
    /// The key for the resource that contains the path format for OST album art.
    /// </summary>
    public const string SoundTrackAlbumArtPathKey = "SoundTrackAlbumArtPath";

    /// <summary>
    /// The key for the resource that contains the path format for a character's picture.
    /// </summary>
    public const string CharacterPicturePathkey = "CharacterPicturePath";

    /// <summary>
    /// The key for the resource that contains the path format for a person's profile picture.
    /// </summary>
    public const string PersonPicturePathkey = "PersonPicturePath";
}
