using System.ComponentModel.DataAnnotations;
using AniNexus.Data.Validation;
using AniNexus.Models;

namespace AniNexus.Data.Entities;

/// <summary>
/// Models a mapping between related anime entries.
/// </summary>
public class AnimeRelatedMapEntity : Entity<AnimeRelatedMapEntity>
{
    /// <summary>
    /// The Id of the anime.
    /// </summary>
    public Guid AnimeId { get; set; }

    /// <summary>
    /// The Id of the related anime.
    /// </summary>
    public Guid RelatedAnimeId { get; set; }

    /// <summary>
    /// The Id of the relation type.
    /// </summary>
    /// <seealso cref="EMediaRelationType"/>
    /// <seealso cref="MediaRelationTypeEntity"/>
    public byte RelationTypeId { get; set; }

    /// <summary>
    /// The anime.
    /// </summary>
    public AnimeEntity Anime { get; set; } = default!;

    /// <summary>
    /// The anime that is related.
    /// </summary>
    public AnimeEntity Related { get; set; } = default!;

    /// <summary>
    /// How the anime is related.
    /// </summary>
    public MediaRelationTypeEntity RelationType { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AnimeRelatedMapEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => new { m.AnimeId, m.RelatedAnimeId }).IsUnique();
        builder.HasIndex(m => m.RelatedAnimeId);
        builder.HasIndex(m => m.RelationTypeId);
        // 2. Navigation properties
        // Handled by EF trigger - SqlServer will falsely flag multiple cascade paths since the mapping
        // points to two entities in the same table.
        builder.HasOne(m => m.Anime).WithMany(m => m.RelatedAnime).HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(m => m.Related).WithMany().HasForeignKey(m => m.RelatedAnimeId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(m => m.RelationType).WithMany().HasForeignKey(m => m.RelatedAnimeId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 3. Propery specification
        // 4. Other
        builder.HasQueryFilter(m => !m.Anime.IsSoftDeleted && !m.Related.IsSoftDeleted);
    }

    /// <inheritdoc/>
    protected override void Validate(ValidationContext validationContext, ValidationBuilder<AnimeRelatedMapEntity> validator)
    {
        base.Validate(validationContext, validator);

        validator.Property(m => m.AnimeId)
            .IsNotEqualTo(m => m.RelatedAnimeId, "An anime cannot be related to itself.")
            .IsLessThan(m => m.RelatedAnimeId, "The anime with the lower PK must come first in the relationship.");
    }
}
