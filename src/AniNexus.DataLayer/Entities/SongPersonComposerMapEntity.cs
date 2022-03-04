namespace AniNexus.Data.Entities;

/// <summary>
/// Models a mapping between a song and its composers.
/// </summary>
public sealed class SongPersonComposerMapEntity : Entity<SongPersonComposerMapEntity>
{
    /// <summary>
    /// The Id of the song.
    /// </summary>
    public Guid SongId { get; set; }

    /// <summary>
    /// The Id of the composer.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// The song.
    /// </summary>
    public SongEntity Song { get; set; } = default!;

    /// <summary>
    /// The composer.
    /// </summary>
    public PersonEntity Person { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<SongPersonComposerMapEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => new { m.SongId, m.PersonId }).IsUnique();
        builder.HasIndex(m => m.PersonId);
        // 2. Navigation properties
        builder.HasOne(m => m.Song).WithMany(m => m.Composers).HasForeignKey(m => m.SongId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Person).WithMany(m => m.ComposedSongs).HasForeignKey(m => m.PersonId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 3. Propery specification
        // 4. Other
    }
}
