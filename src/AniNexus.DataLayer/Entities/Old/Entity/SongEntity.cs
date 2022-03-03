namespace AniNexus.Data.Entities;

public sealed class SongEntity : AuditableEntity<SongEntity>
{
    /// <summary>
    /// The public Id of this song.
    /// </summary>
    public int PublicId { get; set; }

    /// <summary>
    /// The Id of the primary genre.
    /// </summary>
    public byte GenreId { get; set; }

    /// <summary>
    /// The Id of the album.
    /// </summary>
    public Guid? AlbumId { get; set; }

    /// <summary>
    /// The primary genre.
    /// </summary>
    public AudioGenreEntity Genre { get; set; } = default!;

    /// <summary>
    /// The artists that worked on this song.
    /// </summary>
    public IList<SongArtistSongMapEntity> Artists { get; set; } = default!;

    /// <summary>
    /// Episodes that this song was used as the OP for.
    /// </summary>
    public IList<AnimeEpisodeEntity> EpisodesOP { get; set; } = default!;
    /// <summary>
    /// Episodes that this song was used as the ED for.
    /// </summary>
    public IList<AnimeEpisodeEntity> EpisodesED { get; set; } = default!;
}
