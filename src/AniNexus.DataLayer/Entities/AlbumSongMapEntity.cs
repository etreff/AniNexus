namespace AniNexus.Data.Entities;

/// <summary>
/// Models a mapping of albums and the songs they contain.
/// </summary>
public sealed class AlbumSongMapEntity : Entity<AlbumSongMapEntity>
{
    /// <summary>
    /// The Id of the album.
    /// </summary>
    public Guid AlbumId { get; set; }

    /// <summary>
    /// The Id of the song.
    /// </summary>
    public Guid SongId { get; set; }

    /// <summary>
    /// The album.
    /// </summary>
    public AlbumEntity Album { get; set; } = default!;

    /// <summary>
    /// The song.
    /// </summary>
    public SongEntity Song { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AlbumSongMapEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => new { m.AlbumId, m.SongId }).IsUnique();
        builder.HasIndex(m => m.SongId);
        // 2. Navigation properties
        builder.HasOne(m => m.Album).WithMany(m => m.Songs).HasForeignKey(m => m.AlbumId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Song).WithMany(m => m.Albums).HasForeignKey(m => m.SongId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 3. Propery specification
        // 4. Other
    }
}
