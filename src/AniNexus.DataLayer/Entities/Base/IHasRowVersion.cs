namespace AniNexus.Data.Entities;

/// <summary>
/// When added to a class, a RowVersion field will be wired up.
/// </summary>
public interface IHasRowVersion
{
    /// <summary>
    /// The row version.
    /// </summary>
    public byte[] RowVersion { get; set; }
}
