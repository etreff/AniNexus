namespace AniNexus.Domain.Entities;

/// <summary>
/// Models an episode of an anime.
/// </summary>
/// <remarks>
/// An episode may also refer to a feature length film.
/// </remarks>
public sealed class AnimeEpisodeEntity : InstallmentEntity<AnimeEpisodeEntity, AnimeReleaseEntity>
{
    /// <summary>
    /// Whether the episode is a ".5" episode.
    /// </summary>
    public bool IsEpisodeNumberPoint5 { get; set; }

    /// <summary>
    /// Whether the episode is a recap episode.
    /// </summary>
    public bool IsRecap { get; set; }

    /// <summary>
    /// The length of the episode.
    /// </summary>
    public TimeSpan? Duration { get; set; }

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AnimeEpisodeEntity> builder)
    {
        base.ConfigureEntity(builder);

        // 1. Index specification
        builder.HasIndex(m => new object[] { m.Number, m.ReleaseId, m.IsEpisodeNumberPoint5 }).IsUnique();
        // 2. Navigation properties
        // 3. Propery specification
        builder.Property(m => m.IsEpisodeNumberPoint5).HasComment("Whether the episode is a .5 release.");
        builder.Property(m => m.IsRecap).HasComment("Whether the episode is a recap episode.");
        builder.Property(m => m.Duration).HasComment("The length of the episode.");
        // 4. Other
    }
}
