namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a release of an anime in a specific locale.
/// </summary>
public sealed class AnimeReleaseEntity : ReleaseEntity<AnimeReleaseEntity, AnimeEpisodeEntity>
{
    /// <summary>
    /// The age rating for this release.
    /// </summary>
    public AnimeAgeRatingTypeEntity AgeRating { get; set; } = default!;

    /// <summary>
    /// A list of people who worked on this release, excluding VAs.
    /// </summary>
    public IList<AnimeReleasePersonMapEntity> People { get; set; } = default!;

    /// <summary>
    /// The voice actors who voice this character.
    /// </summary>
    public IList<PersonVoiceActorMapEntity> VoiceActors { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AnimeReleaseEntity> builder)
    {
        base.ConfigureEntity(builder);

        // 1. Index specification
        // 2. Navigation properties
        builder.HasOne<AnimeEntity>().WithMany(m => m.Releases).HasForeignKey(m => m.OwnerId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.AgeRating).WithMany().HasForeignKey(m => m.AgeRatingId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
        // 3. Propery specification
        // 4. Other
    }
}
