using System.ComponentModel.DataAnnotations;
using AniNexus.Domain.Validation;

namespace AniNexus.Domain.Entities;

/// <summary>
/// Models an anime recommendation made by a user.
/// </summary>
public sealed class AnimeUserRecommendationEntity : AuditableEntity<AnimeUserRecommendationEntity>, IHasSoftDelete, IHasPublicId<Guid>
{
    /// <summary>
    /// The public key of this recommendation.
    /// </summary>
    public Guid PublicId { get; set; }

    /// <summary>
    /// The Id of the anime the recommendation is based on.
    /// </summary>
    public Guid AnimeId { get; set; }

    /// <summary>
    /// The Id of the anime being recommended.
    /// </summary>
    public Guid AnimeRecommendationId { get; set; }

    /// <summary>
    /// The Id of the user who made the recommendation.
    /// </summary>
    public Guid UserId { get; set; } = default!;

    /// <summary>
    /// The reason they recommend the anime.
    /// </summary>
    public string Reason { get; set; } = default!;

    /// <summary>
    /// Whether this entry is soft-deleted. It will not be included in queries unless
    /// <see cref="M:Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.IgnoreQueryFilters``1(System.Linq.IQueryable{``0})" />
    /// is invoked.
    /// </summary>
    public bool IsSoftDeleted { get; set; }

    /// <summary>
    /// The user who recommended the anime.
    /// </summary>
    public UserEntity User { get; set; } = default!;

    /// <summary>
    /// The anime the recommendation is based on.
    /// </summary>
    public AnimeEntity Anime { get; set; } = default!;

    /// <summary>
    /// The anime being recommended as similar to <see cref="Anime"/>.
    /// </summary>
    public AnimeEntity Recommendation { get; set; } = default!;

    /// <summary>
    /// The votes of users who agree or disagree with the recommendation.
    /// </summary>
    public IList<AnimeUserRecommendationVoteEntity> Votes { get; set; } = default!;

    /// <inheritdoc/>
    protected override string GetTableName()
    {
        return "AnimeRecUser";
    }

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AnimeUserRecommendationEntity> builder)
    {
        base.ConfigureEntity(builder);

        // 1. Index specification
        builder.HasIndex(m => new { m.AnimeId, m.AnimeRecommendationId, m.UserId }).IsUnique();
        builder.HasIndex(m => m.UserId);
        // 2. Navigation properties
        // Many third party trackers will keep user reviews when the user has been deleted,
        // assigning "Deleted" or the like as the reviewer's name. We will not be doing this.
        // We will try and respect the right to be forgotten and remove their recommendation.
        builder.HasOne(m => m.User).WithMany().HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // Handled by EF trigger - SqlServer will falsely flag multiple cascade paths since the mapping
        // points to two entities in the same table.
        builder.HasOne(m => m.Anime).WithMany().HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(m => m.Recommendation).WithMany().HasForeignKey(m => m.AnimeRecommendationId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        // 3. Propery specification
        builder.Property(m => m.AnimeRecommendationId).HasColumnName("AnimeRecId");
        builder.Property(m => m.Reason).HasComment("The reason why this user recommends the anime.").HasMaxLength(2500).IsUnicode();
        // 4. Other
    }

    /// <inheritdoc/>
    protected override void Validate(ValidationContext validationContext, ValidationBuilder<AnimeUserRecommendationEntity> validator)
    {
        base.Validate(validationContext, validator);

        validator.Property(m => m.AnimeId)
            .IsNotEqualTo(m => m.AnimeRecommendationId, "An anime cannot be a recommendation for itself.")
            .IsLessThan(m => m.AnimeRecommendationId, "The anime with the lower PK must come first in the relationship.");
    }
}
