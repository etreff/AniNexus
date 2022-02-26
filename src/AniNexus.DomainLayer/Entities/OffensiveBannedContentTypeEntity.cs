namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a mapping between an offensive content type and a region that bans that content.
/// </summary>
public sealed class OffensiveBannedContentTypeEntity : Entity<OffensiveBannedContentTypeEntity, int>
{
    /// <summary>
    /// The Id of the region.
    /// </summary>
    public short RegionId { get; set; }

    /// <summary>
    /// The Id of the trigger that is NSFW in this region.
    /// </summary>
    public byte ContentId { get; set; }

    /// <summary>
    /// The region in which this content is banned.
    /// </summary>
    public RegionEntity Region { get; set; } = default!;

    /// <summary>
    /// The content that is banned.
    /// </summary>
    public OffensiveContentTypeEntity ContentType { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<OffensiveBannedContentTypeEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => m.RegionId);
        builder.HasIndex(m => m.ContentId);
        // 2. Navigation properties
        builder.HasOne(m => m.Region).WithMany().HasForeignKey(m => m.RegionId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.ContentType).WithMany(m => m.BannedRegions).HasForeignKey(m => m.ContentId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 3. Propery specification
        // 4. Other
    }
}
