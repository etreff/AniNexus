using System.ComponentModel.DataAnnotations;
using AniNexus.Data.Validation;

namespace AniNexus.Data.Entities;

/// <summary>
/// Models a mapping between an anime and a third party tracker.
/// </summary>
public class AnimeThirdPartyMapEntity : Entity<AnimeThirdPartyMapEntity>
{
    /// <summary>
    /// The AniNexus anime Id.
    /// </summary>
    public Guid AnimeId { get; set; }

    /// <summary>
    /// The Id of the third party tracker.
    /// </summary>
    /// <seealso cref="ThirdPartyTrackerEntity"/>
    public Guid ThirdPartyId { get; set; }

    /// <summary>
    /// The Id the third party tracker has assigned this anime.
    /// </summary>
    public string ExternalMediaId { get; set; } = default!;

    /// <summary>
    /// The anime.
    /// </summary>
    public AnimeEntity Anime { get; set; } = default!;

    /// <summary>
    /// The third party tracker.
    /// </summary>
    public ThirdPartyTrackerEntity ThirdParty { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AnimeThirdPartyMapEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => new { m.AnimeId, m.ThirdPartyId });
        builder.HasIndex(m => m.ThirdPartyId);
        // 2. Navigation properties
        builder.HasOne(m => m.Anime).WithMany(m => m.ExternalIds).HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.ThirdParty).WithMany().HasForeignKey(m => m.ThirdPartyId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // Justification - the target entity is small and we will almost always need all of its properties.
        builder.Navigation(m => m.ThirdParty).AutoInclude();
        // 3. Propery specification
        builder.Property(m => m.ExternalMediaId).HasComment("The Id that the third party tracker has assigned to the anime entry.");
        // 4. Other
        builder.HasQueryFilter(m => !m.Anime.IsSoftDeleted && !m.ThirdParty.IsSoftDeleted);
    }

    /// <inheritdoc/>
    protected override void Validate(ValidationContext validationContext, ValidationBuilder<AnimeThirdPartyMapEntity> validator)
    {
        base.Validate(validationContext, validator);

        validator.Property(m => m.ExternalMediaId).IsNotNullOrWhiteSpace();
    }
}
