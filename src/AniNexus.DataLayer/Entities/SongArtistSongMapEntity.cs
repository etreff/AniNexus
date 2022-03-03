namespace AniNexus.Data.Entities;

/// <summary>
/// Models a mapping between a band/group and the songs they created.
/// </summary>
public sealed class SongArtistSongMapEntity : Entity<SongArtistSongMapEntity>
{
    /// <summary>
    /// The Id of the artist/group.
    /// </summary>
    public Guid ArtistId { get; set; }

    /// <summary>
    /// The Id of the song.
    /// </summary>
    public Guid SongId { get; set; }

    /// <summary>
    /// The artist/group.
    /// </summary>
    public SongArtistEntity Artist { get; set; } = default!;

    /// <summary>
    /// The album.
    /// </summary>
    public SongEntity Song { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<SongArtistSongMapEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => new { m.ArtistId, m.SongId }).IsUnique();
        builder.HasIndex(m => m.SongId);
        // 2. Navigation properties
        builder.HasOne(m => m.Artist).WithMany(m => m.Songs).HasForeignKey(m => m.ArtistId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Song).WithMany(m => m.Artists).HasForeignKey(m => m.SongId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 3. Propery specification
        // 4. Other
    }
}
