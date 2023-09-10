namespace AniNexus;

/// <summary>
/// The status of a piece of media.
/// </summary>
public enum EMediaStatus
{
    /// <summary>
    /// The media has been completed and is no longer being worked on.
    /// </summary>
    Complete = 1,

    /// <summary>
    /// At least one part of the media has been released and it is still being produced.
    /// </summary>
    Releasing = 2,

    /// <summary>
    /// The media has been officially announced, has not yet been released, and is in production.
    /// </summary>
    InProduction = 3,

    /// <summary>
    /// The media has been officially announced but no information is available in regard to production.
    /// </summary>
    NotReleased = 4,

    /// <summary>
    /// The media has been officially announced, is not complete, and is no longer in active production.
    /// </summary>
    Canceled = 5,

    /// <summary>
    /// The media has been officially announced, production has at one point started, the media is not complete,
    /// the producers/studios plan on completing the media, and the media is not in active production.
    /// </summary>
    OnHaitus = 6,

    /// <summary>
    /// The status of the media is unkown, either because the media has not been announced or because the data
    /// has not been entered into the database.
    /// </summary>
    Unknown = 7
}
