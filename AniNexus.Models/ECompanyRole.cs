namespace AniNexus;

/// <summary>
/// Defines the role a company plays in a media release.
/// </summary>
public enum ECompanyRole
{
    /// <summary>
    /// The company was a producer for the media.
    /// </summary>
    Producer = 1,

    /// <summary>
    /// The company was a studio that helped create the media.
    /// </summary>
    Studio = 2,

    /// <summary>
    /// The company was a publisher or licensee for the media.
    /// </summary>
    Publisher = 3
}
