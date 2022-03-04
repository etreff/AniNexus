namespace AniNexus.Data.Entities;

/// <summary>
/// Models a song.
/// </summary>
public sealed class SongEntity : AuditableEntity<SongEntity>
{
    /// <summary>
    /// The name of the song.
    /// </summary>
    public NameEntity Name { get; set; } = default!;

    /// <summary>
    /// The public Id of this song.
    /// </summary>
    public int PublicId { get; set; }

    /// <summary>
    /// The Id of the primary genre.
    /// </summary>
    public byte GenreId { get; set; }

    /// <summary>
    /// The Id of the sub-genre.
    /// </summary>
    public short? SubGenreId { get; set; }

    /// <summary>
    /// The Id of the primary language of this song.
    /// </summary>
    public short? LanguageId { get; set; }

    /// <summary>
    /// The length of the song.
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// The primary genre.
    /// </summary>
    public MusicGenreEntity Genre { get; set; } = default!;

    /// <summary>
    /// The sub-genre.
    /// </summary>
    public MusicGenreEntity? SubGenre { get; set; }

    /// <summary>
    /// The albums this song is released in.
    /// </summary>
    public IList<AlbumSongMapEntity> Albums { get; set; } = default!;

    /// <summary>
    /// The artists that performed this song.
    /// </summary>
    public IList<SongArtistSongMapEntity> Artists { get; set; } = default!;

    /// <summary>
    /// The composers of this song.
    /// </summary>
    public IList<SongPersonComposerMapEntity> Composers { get; set; } = default!;

    /// <summary>
    /// Episodes that this song was used as the OP for.
    /// </summary>
    public IList<AnimeEpisodeEntity> EpisodesOP { get; set; } = default!;

    /// <summary>
    /// Episodes that this song was used as the ED for.
    /// </summary>
    public IList<AnimeEpisodeEntity> EpisodesED { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<SongEntity> builder)
    {
        base.ConfigureEntity(builder);

        // 1. Index specification
        builder.HasIndex(m => m.GenreId);
        builder.HasIndex(m => m.SubGenreId).HasNotNullFilter();
        builder.HasIndex(m => m.LanguageId).HasNotNullFilter();
        // 2. Navigation properties
        builder.HasOne(m => m.Genre).WithMany(m => m.Songs).HasForeignKey(m => m.GenreId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.SubGenre).WithMany(m => m.Songs).HasForeignKey(m => m.SubGenreId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
        builder.OwnsOne(m => m.Name, static owned => owned.ConfigureOwnedEntity());
        // 3. Propery specification
        builder.Property(m => m.Duration).HasComment("The length of the song.");
        // 4. Other
    }
}
