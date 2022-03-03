namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a mapping of an audio album and a region along with additional metadata.
/// </summary>
public sealed class AlbumRegionMapEntity : Entity<AlbumRegionMapEntity>
{
    /// <summary>
    /// The Id of the album.
    /// </summary>
    public Guid AlbumId { get; set; }

    /// <summary>
    /// The Id of the region.
    /// </summary>
    public short RegionId { get; set; }

    /// <summary>
    /// Urls to a website where this album can be purchased legally in this region.
    /// </summary>
    public IList<string> PurchaseUrl { get; set; } = default!;

    /// <summary>
    /// The album.
    /// </summary>
    public AlbumEntity Album { get; set; } = default!;

    /// <summary>
    /// The region.
    /// </summary>
    public RegionEntity Region { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AlbumRegionMapEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => new object[] { m.AlbumId, m.RegionId }).IsUnique();
        builder.HasIndex(m => m.RegionId);
        // 2. Navigation properties
        builder.HasOne(m => m.Album).WithMany().HasForeignKey(m => m.AlbumId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Region).WithMany().HasForeignKey(m => m.RegionId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 3. Propery specification
        builder.Property(m => m.PurchaseUrl).HasListConversion().HasComment("The urls to websites where the album can be legally purchased in the region.");
        // 4. Other
    }
}
