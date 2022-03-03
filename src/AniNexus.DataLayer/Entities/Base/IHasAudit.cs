namespace AniNexus.Data.Entities;

/// <summary>
/// When added to a class, the model will be audited.
/// </summary>
public interface IHasAudit : IEntity
{
    /// <summary>
    /// The UTC date and time this entity was added to the table.
    /// </summary>
    public DateTime DateAdded { get; set; }

    /// <summary>
    /// The UTC date and time this entity was last updated.
    /// </summary>
    public DateTime DateUpdated { get; set; }
}
