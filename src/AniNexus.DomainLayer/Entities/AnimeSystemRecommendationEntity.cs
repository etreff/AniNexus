using System.ComponentModel.DataAnnotations;
using AniNexus.Domain.Validation;

namespace AniNexus.Domain.Entities;

/// <summary>
/// Models an anime recommendation made by AniNexus.
/// </summary>
/// <remarks>
/// System recommendations intentionally do not have the ability to specify a reason.
/// </remarks>
public class AnimeSystemRecommendationEntity : Entity<AnimeSystemRecommendationEntity>
{
    /// <summary>
    /// The Id of the anime the recommendation is based off of.
    /// </summary>
    public Guid AnimeId { get; set; }

    /// <summary>
    /// The Id of the anime being recommended.
    /// </summary>
    public Guid AnimeRecommendationId { get; set; }

    /// <summary>
    /// The order in which the recommendation is listed among other AniNexus recommendations for
    /// this anime.
    /// </summary>
    /// <remarks>
    /// A lower order will be listed first.
    /// </remarks>
    public byte Order { get; set; }

    /// <summary>
    /// The anime the recommendation is based on.
    /// </summary>
    public AnimeEntity Anime { get; set; } = default!;

    /// <summary>
    /// The anime being recommended as similar to <see cref="Anime"/>.
    /// </summary>
    public AnimeEntity Recommendation { get; set; } = default!;

    /// <inheritdoc/>
    protected override string GetTableName()
    {
        return "AnimeRecSys";
    }

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AnimeSystemRecommendationEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => new { m.AnimeId, m.AnimeRecommendationId }).IsUnique();
        // 2. Navigation properties
        // Handled by EF trigger - SqlServer will falsely flag multiple cascade paths since the mapping
        // points to two entities in the same table.
        builder.HasOne(m => m.Anime).WithMany().HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(m => m.Recommendation).WithMany().HasForeignKey(m => m.AnimeRecommendationId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        // 3. Propery specification
        builder.Property(m => m.AnimeRecommendationId).HasColumnName("AnimeRecId");
        // 4. Other
        builder.Property(m => m.Order).HasComment("The order in which the recommendation will be listed. Lower order will be listed first.").HasDefaultValue(10);
    }

    /// <inheritdoc/>
    protected override void Validate(ValidationContext validationContext, ValidationBuilder<AnimeSystemRecommendationEntity> validator)
    {
        base.Validate(validationContext, validator);

        validator.Property(m => m.AnimeId)
            .IsNotEqualTo(m => m.AnimeRecommendationId, "An anime cannot be a recommendation for itself.")
            .IsLessThan(m => m.AnimeRecommendationId, "The anime with the lower PK must come first in the relationship.");
    }
}
