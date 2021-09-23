namespace AniNexus.Models.Configuration;

/// <summary>
/// TheTVDB settings.
/// </summary>
public class TheTVDBSettings
{
    /// <summary>
    /// The API key.
    /// </summary>
    public string ApiKey { get; set; } = default!;

    /// <summary>
    /// The pin.
    /// </summary>
    public string Pin { get; set; } = default!;
}
