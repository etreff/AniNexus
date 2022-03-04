namespace AniNexus.Data.Entities;

/// <summary>
/// Models a music genre.
/// </summary>
public sealed class MusicGenreEntity : Entity<MusicGenreEntity, byte>, IHasTranslations<MusicGenreTranslationEntity, MusicGenreEntity, byte>
{
    /// <summary>
    /// The name of the genre.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// The valid subgenres of this entity.
    /// </summary>
    public IList<MusicSubGenreEntity> SubGenres { get; set; } = default!;

    /// <summary>
    /// Songs that are of this genre.
    /// </summary>
    public IList<SongEntity> Songs { get; set; } = default!;

    /// <inheritdoc/>
    public IList<MusicGenreTranslationEntity> Translations { get; set; } = default!;

    /// <inheritdoc/>
    protected override IEnumerable<MusicGenreEntity> GetSeedData()
    {
        return new[]
        {
            new MusicGenreEntity { Name = "Alternative" },
            new MusicGenreEntity { Name = "Blues" },
            new MusicGenreEntity { Name = "Childrens" },
            new MusicGenreEntity { Name = "Classical" },
            new MusicGenreEntity { Name = "Comedy" },
            new MusicGenreEntity { Name = "Commercial" },
            new MusicGenreEntity { Name = "Country" },
            new MusicGenreEntity { Name = "Dance" },
            new MusicGenreEntity { Name = "Disney" },
            new MusicGenreEntity { Name = "EasyListening" },
            new MusicGenreEntity { Name = "Electronic" },
            new MusicGenreEntity { Name = "Enka" },
            new MusicGenreEntity { Name = "FrenchPop" },
            new MusicGenreEntity { Name = "Folk" },
            new MusicGenreEntity { Name = "GermanFolk" },
            new MusicGenreEntity { Name = "GermanPop" },
            new MusicGenreEntity { Name = "Gospel" },
            new MusicGenreEntity { Name = "FitnessAndWorkout" },
            new MusicGenreEntity { Name = "HipHopOrRap" },
            new MusicGenreEntity { Name = "Holiday" },
            new MusicGenreEntity { Name = "IndiePop" },
            new MusicGenreEntity { Name = "Industrial" },
            new MusicGenreEntity { Name = "Instrumental" },
            new MusicGenreEntity { Name = "JPop" },
            new MusicGenreEntity { Name = "Jazz" },
            new MusicGenreEntity { Name = "KPop" },
            new MusicGenreEntity { Name = "Karaoke" },
            new MusicGenreEntity { Name = "Kayokyoku" },
            new MusicGenreEntity { Name = "Latin" },
            new MusicGenreEntity { Name = "Metal" },
            new MusicGenreEntity { Name = "NewAge" },
            new MusicGenreEntity { Name = "Opera" },
            new MusicGenreEntity { Name = "Pop" },
            new MusicGenreEntity { Name = "PostDisco" },
            new MusicGenreEntity { Name = "Progressive" },
            new MusicGenreEntity { Name = "Reggae" },
            new MusicGenreEntity { Name = "Rock" },
            new MusicGenreEntity { Name = "Singer" },
            new MusicGenreEntity { Name = "Soul" },
            new MusicGenreEntity { Name = "SpokenWord" },
            new MusicGenreEntity { Name = "Tejano" },
            new MusicGenreEntity { Name = "Vocal" },
            new MusicGenreEntity { Name = "World" }
        };
    }

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<MusicGenreEntity> builder)
    {
        // 1. Index specification
        // 2. Navigation properties
        // 3. Propery specification
        builder.Property(m => m.Name).HasComment("The name of the genre.");
        // 4. Other
    }
}

/// <summary>
/// Models a music genre translation.
/// </summary>
public sealed class MusicGenreTranslationEntity : TranslationEntity<MusicGenreTranslationEntity, MusicGenreEntity, byte>
{
}
