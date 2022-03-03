namespace AniNexus.Data.Entities;

/// <summary>
/// Models a mapping of individual contributors to an audio album.
/// </summary>
public sealed class PersonAlbumMapEntity : Entity<PersonAlbumMapEntity>
{
    /// <summary>
    /// The Id of the person.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// The Id of the album.
    /// </summary>
    public Guid AlbumId { get; set; }

    /// <summary>
    /// The person.
    /// </summary>
    public PersonEntity Person { get; set; } = default!;

    /// <summary>
    /// The album.
    /// </summary>
    public AlbumEntity Album { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<PersonAlbumMapEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => new[] { m.PersonId, m.AlbumId }).IsUnique();
        builder.HasIndex(m => m.AlbumId);
        // 2. Navigation properties
        builder.HasOne(m => m.Person).WithMany(m => m.Albums).HasForeignKey(m => m.PersonId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Album).WithMany(m => m.People).HasForeignKey(m => m.AlbumId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 3. Propery specification
        // 4. Other
    }
}
