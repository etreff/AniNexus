namespace AniNexus.Data.Entities;

/// <summary>
/// Models an audio album.
/// </summary>
public sealed class AlbumEntity : AuditableEntity<AlbumEntity>, IHasPublicId
{
    /// <summary>
    /// The name of the album.
    /// </summary>
    public NameEntity Name { get; set; } = default!;

    /// <summary>
    /// The public Id of this album.
    /// </summary>
    public int PublicId { get; set; }

    /// <summary>
    /// The release date of the album.
    /// </summary>
    public Date? ReleaseDate { get; set; }

    /// <summary>
    /// The Id of the publisher who has rights to this album.
    /// </summary>
    public Guid? PublisherId { get; set; }

    /// <summary>
    /// The Id of the anime this album is associated with.
    /// </summary>
    public Guid? AnimeId { get; set; }

    /// <summary>
    /// The publisher who has rights to this album.
    /// </summary>
    public CompanyEntity? Publisher { get; set; }

    /// <summary>
    /// The anime this album is associated with.
    /// </summary>
    public AnimeEntity? Anime { get; set; }

    /// <summary>
    /// A link to the places where this album can be legally purchased in various regions.
    /// </summary>
    public IList<AlbumRegionMapEntity>? PurchaseUrls { get; set; }

    /// <summary>
    /// The groups or bands associated with this album.
    /// </summary>
    public IList<SongArtistAlbumMapEntity> Artists { get; set; } = default!;

    /// <summary>
    /// The individuals associated with this album.
    /// </summary>
    public IList<PersonAlbumMapEntity> People { get; set; } = default!;

    /// <summary>
    /// The songs in this album.
    /// </summary>
    public IList<SongEntity> Songs { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AlbumEntity> builder)
    {
        base.ConfigureEntity(builder);

        // 1. Index specification
        builder.HasIndex(m => m.PublisherId).HasNotNullFilter();
        builder.HasIndex(m => m.AnimeId).HasNotNullFilter();
        // 2. Navigation properties
        builder.HasOne(m => m.Publisher).WithMany(m => m.Albums).HasForeignKey(m => m.PublisherId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(m => m.Anime).WithMany(m => m.Albums).HasForeignKey(m => m.AnimeId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
        builder.OwnsOne(m => m.Name, static owned => owned.ConfigureOwnedEntity());
        // 3. Propery specification
        builder.Property(m => m.ReleaseDate).HasComment("The release date of the album.");
        // 4. Other
    }
}
