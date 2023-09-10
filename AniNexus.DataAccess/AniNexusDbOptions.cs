namespace AniNexus.DataAccess;

/// <summary>
/// AniNexus database options.
/// </summary>
public sealed class AniNexusDbOptions
{
    /// <summary>
    /// The database connection string.
    /// </summary>
    public string DbConnection { get; set; } = default!;
}
