using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a user's review of an anime.
/// </summary>
public sealed class AnimeUserReviewEntity : AuditableEntity<AnimeUserReviewEntity>, IHasSoftDelete
{
    /// <summary>
    /// The public key of this recommendation.
    /// </summary>
    public Guid PublicId { get; set; }

    /// <summary>
    /// The Id of the user who wrote the review.
    /// </summary>
    public Guid UserId { get; set; } = default!;

    /// <summary>
    /// The Id of the anime being reviewed.
    /// </summary>
    public Guid AnimeId { get; set; }

    /// <summary>
    /// Whether the user recommends the anime.
    /// </summary>
    public bool Recommend { get; set; }

    /// <summary>
    /// The contents of the review.
    /// </summary>
    public string Review { get; set; } = default!;

    /// <summary>
    /// Whether this entry is soft-deleted. It will not be included in queries unless
    /// <see cref="M:Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.IgnoreQueryFilters``1(System.Linq.IQueryable{``0})" />
    /// is invoked.
    /// </summary>
    public bool IsSoftDeleted { get; set; }

    /// <summary>
    /// The anime being reviewed.
    /// </summary>
    public AnimeEntity Anime { get; set; } = default!;

    /// <summary>
    /// The user who wrote the review.
    /// </summary>
    public UserEntity User { get; set; } = default!;

    /// <summary>
    /// A collection of votes that affirm or deny the review.
    /// </summary>
    public IList<AnimeUserReviewVoteEntity> Votes { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AnimeUserReviewEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => new[] { m.UserId, m.AnimeId }).IsUnique();
        builder.HasIndex(m => m.AnimeId);
        builder.HasIndex(m => m.PublicId).IsUnique();
        // 2. Navigation properties
        builder.HasOne(m => m.User).WithMany(m => m.AnimeReviews).HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Anime).WithMany(m => m.UserReviews).HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 3. Propery specification
        builder.Property(m => m.PublicId).HasValueGenerator<SequentialGuidValueGenerator>();
        builder.Property(m => m.Recommend).HasComment("Whether the user recommends the anime.");
        builder.Property(m => m.Review).HasComment("The review content.").HasMaxLength(2500).UseCollation(Collation.Japanese);
        // 4. Other
    }
}

/// <summary>
/// Models a <see cref="AnimeUserReviewEntity"/> that has been reported.
/// </summary>
public sealed class AnimeUserReviewReportedEntity : AuditableEntity<AnimeUserReviewReportedEntity>
{
    /// <summary>
    /// The Id of the review that was reported.
    /// </summary>
    public Guid ReviewId { get; set; }

    /// <summary>
    /// The Id of the user that reported the review.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// The reason the review was reported.
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// The review that was reported.
    /// </summary>
    public AnimeUserReviewEntity Review { get; set; } = default!;

    /// <summary>
    /// The user that reported the review.
    /// </summary>
    public UserEntity User { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AnimeUserReviewReportedEntity> builder)
    {
        base.ConfigureEntity(builder);

        // 1. Index specification
        builder.HasIndex(m => new[] { m.UserId, m.ReviewId }).IsUnique();
        builder.HasIndex(m => m.ReviewId);
        // 2. Navigation properties
        builder.HasOne(m => m.User).WithMany().HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Review).WithMany().HasForeignKey(m => m.ReviewId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 3. Propery specification
        builder.Property(m => m.Reason).HasMaxLength(500).HasComment("The reason the review was reported.");
        // 4. Other
    }
}

/// <summary>
/// Models a <see cref="AnimeUserReviewEntity"/> that has been deleted.
/// </summary>
public sealed class AnimeUserReviewDeletedEntity : AuditableEntity<AnimeUserReviewDeletedEntity>
{
    /// <summary>
    /// The Id of the user that deleted the review.
    /// </summary>
    public Guid? DeletedByUserId { get; set; }

    /// <summary>
    /// The Id of the user that wrote the review.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// The reason the review was deleted.
    /// </summary>
    public string Reason { get; set; } = default!;

    /// <summary>
    /// The contents of the review.
    /// </summary>
    public string Review { get; set; } = default!;

    /// <summary>
    /// The user that deleted the review.
    /// </summary>
    public UserEntity DeletedByUser { get; set; } = default!;

    /// <summary>
    /// The user that wrote the review.
    /// </summary>
    public UserEntity User { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AnimeUserReviewDeletedEntity> builder)
    {
        base.ConfigureEntity(builder);

        // 1. Index specification
        builder.HasIndex(m => m.DeletedByUserId).HasNotNullFilter();
        builder.HasIndex(m => m.UserId);
        // 2. Navigation properties
        // Handled by EF trigger - SqlServer will falsely flag multiple cascade paths since the mapping
        // points to two entities in the same table.
        builder.HasOne(m => m.DeletedByUser).WithMany().HasForeignKey(m => m.DeletedByUserId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(m => m.User).WithMany().HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        // 3. Propery specification
        builder.Property(m => m.Reason).HasMaxLength(500).HasComment("The reason the review was reported.");
        // 4. Other
    }
}
