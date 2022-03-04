namespace AniNexus.Data.Entities;

/// <summary>
/// Models a music sub-genre.
/// </summary>
public sealed class MusicSubGenreEntity : Entity<MusicSubGenreEntity, byte>, IHasTranslations<MusicSubGenreTranslationEntity, MusicSubGenreEntity, byte>
{
    /// <summary>
    /// The Id of the genre this sub-genre belongs to.
    /// </summary>
    public byte GenreId { get; set; }

    /// <summary>
    /// The name of the genre.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// The genre this sub-genre belongs to.
    /// </summary>
    public MusicGenreEntity Genre { get; set; } = default!;

    /// <summary>
    /// Songs that are of this sub-genre.
    /// </summary>
    public IList<SongEntity> Songs { get; set; } = default!;

    /// <inheritdoc/>
    public IList<MusicSubGenreTranslationEntity> Translations { get; set; } = default!;

    /// <inheritdoc/>
    protected override IEnumerable<MusicSubGenreEntity> GetSeedData()
    {
        // Unfortunately we cannot seed these mappings even if they are known at compile-time due to
        // the GenreId requirement.
        // We can revisit this later and have some other mechanism to automatically seed the data.
        // https://www.musicgenreslist.com/
        return Array.Empty<MusicSubGenreEntity>();
    }

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<MusicSubGenreEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => m.GenreId);
        // 2. Navigation properties
        builder.HasOne(m => m.Genre).WithMany(m => m.SubGenres).HasForeignKey(m => m.GenreId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 3. Propery specification
        builder.Property(m => m.Name).HasComment("The name of the sub-genre.");
        // 4. Other
    }
}

/// <summary>
/// Models a music genre translation.
/// </summary>
public sealed class MusicSubGenreTranslationEntity : TranslationEntity<MusicSubGenreTranslationEntity, MusicSubGenreEntity, byte>
{
}
