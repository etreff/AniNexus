namespace AniNexus.Data.Entities;

/// <summary>
/// Models a mapping between a band/group and the people in it.
/// </summary>
public sealed class SongArtistPersonMapEntity : Entity<SongArtistPersonMapEntity>
{
    /// <summary>
    /// The Id of the artist/group.
    /// </summary>
    public Guid ArtistId { get; set; }

    /// <summary>
    /// The Id of the person.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// The artist/group.
    /// </summary>
    public SongArtistEntity Artist { get; set; } = default!;

    /// <summary>
    /// The person.
    /// </summary>
    public PersonEntity Person { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<SongArtistPersonMapEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => new { m.ArtistId, m.PersonId }).IsUnique();
        builder.HasIndex(m => m.PersonId);
        // 2. Navigation properties
        builder.HasOne(m => m.Artist).WithMany(m => m.Members).HasForeignKey(m => m.ArtistId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Person).WithMany().HasForeignKey(m => m.PersonId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 3. Propery specification
        // 4. Other
    }
}
