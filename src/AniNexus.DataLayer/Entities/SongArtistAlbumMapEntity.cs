namespace AniNexus.Data.Entities;

/// <summary>
/// Models a mapping between a band/group and the albums they created.
/// </summary>
public sealed class SongArtistAlbumMapEntity : Entity<SongArtistAlbumMapEntity>
{
    /// <summary>
    /// The Id of the artist/group.
    /// </summary>
    public Guid ArtistId { get; set; }

    /// <summary>
    /// The Id of the album.
    /// </summary>
    public Guid AlbumId { get; set; }

    /// <summary>
    /// The artist/group.
    /// </summary>
    public SongArtistEntity Artist { get; set; } = default!;

    /// <summary>
    /// The album.
    /// </summary>
    public AlbumEntity Album { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<SongArtistAlbumMapEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => new { m.ArtistId, m.AlbumId }).IsUnique();
        builder.HasIndex(m => m.AlbumId);
        // 2. Navigation properties
        builder.HasOne(m => m.Artist).WithMany(m => m.Albums).HasForeignKey(m => m.ArtistId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Album).WithMany(m => m.Artists).HasForeignKey(m => m.AlbumId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 3. Propery specification
        // 4. Other
    }
}
