namespace AniNexus.Data.Entities;

/// <summary>
/// Models a record that identifies an <see cref="AnimeReleaseEntity"/> that is currently airing.
/// </summary>
public class AnimeReleaseAiringEntity : Entity<AnimeReleaseAiringEntity, short>
{
    /// <summary>
    /// The Id of the anime release that is airing.
    /// </summary>
    public Guid ReleaseId { get; set; }

    /// <summary>
    /// The anime release that is airing.
    /// </summary>
    public AnimeReleaseEntity Release { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AnimeReleaseAiringEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => m.ReleaseId).IsUnique();
        // 2. Navigation properties
        builder.HasOne(m => m.Release).WithOne().HasForeignKey<AnimeReleaseAiringEntity>(m => m.ReleaseId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 3. Propery specification
        // 4. Other
    }
}
