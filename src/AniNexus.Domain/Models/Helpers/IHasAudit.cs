namespace AniNexus.Domain.Models;

/// <summary>
/// When added to a class, the model will be audited.
/// </summary>
public interface IHasAudit
{
    /// <summary>
    /// The UTC date and time this entry was added to the table.
    /// </summary>
    public DateTime DateAdded { get; set; }

    /// <summary>
    /// The UTC date and time this entry was last updated.
    /// </summary>
    public DateTime DateUpdated { get; set; }
}
