﻿using System.ComponentModel.DataAnnotations;
using AniNexus.Domain.Validation;

namespace AniNexus.Domain.Entities;

/// <summary>
/// The base class for a piece of media belonging to a <see cref="FranchiseEntity"/>.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public abstract class FranchiseMediaEntity<TEntity> : AuditableEntity<TEntity>, IHasRowVersion, IHasSoftDelete, IHasPublicId
    where TEntity : FranchiseMediaEntity<TEntity>
{
    /// <summary>
    /// The public Id of the entity, used for navigation URLs.
    /// </summary>
    public int PublicId { get; set; }

    /// <summary>
    /// The official website of this media entity.
    /// </summary>
    public string? WebsiteUrl { get; set; }

    /// <summary>
    /// A synopsis or description of the media entity.
    /// </summary>
    public string? Synopsis { get; set; }

    /// <summary>
    /// The average user rating for this anime, between 0 and 100.
    /// The rating only takes into account ratings from users who have
    /// given the anime a rating and have completed the anime.
    /// </summary>
    /// <remarks>
    /// This value will be calculated by the system periodically.
    /// </remarks>
    public byte? Rating { get; set; }

    /// <summary>
    /// The average user rating for this anime, between 0 and 100.
    /// The rating only takes into account ratings from users who have given
    /// the anime a rating and are actively watching the anime.
    /// </summary>
    /// <remarks>
    /// This value will be calculated by the system periodically.
    /// </remarks>
    public byte? ActiveRating { get; set; }

    /// <summary>
    /// The number of user votes that contributed to <see cref="Rating"/>.
    /// </summary>
    /// <remarks>
    /// This value will be calculated by the system periodically.
    /// </remarks>
    public int Votes { get; set; }

    /// <summary>
    /// The row version.
    /// </summary>
    public byte[] RowVersion { get; set; } = default!;

    /// <summary>
    /// Whether this entry is soft-deleted. It will not be included in queries unless
    /// <see cref="M:Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.IgnoreQueryFilters``1(System.Linq.IQueryable{``0})" />
    /// is invoked.
    /// </summary>
    public bool IsSoftDeleted { get; set; }

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<TEntity> builder)
    {
        base.ConfigureEntity(builder);

        // 1. Primary key specification (if not Entity<>)
        // 2. Index specification
        // 3. Navigation properties
        // 4. Propery specification
        builder.Property(m => m.ActiveRating).HasComment("The user rating of the anime (Watching Only), from 0 to 100. Calculated by the system periodically.");
        builder.Property(m => m.Rating).HasComment("The user rating of the anime (Completed Only), from 0 to 100. Calculated by the system periodically.");
        builder.Property(m => m.Synopsis).HasMaxLength(2000).HasComment("A synopsis or description of the anime.");
        builder.Property(m => m.Votes).HasComment("The number of votes that contributed to the rating. Calculated by the system periodically.").HasDefaultValue(0);
        builder.Property(m => m.WebsiteUrl).HasComment("The URL to the media's official website.");
    }

    /// <inheritdoc/>
    protected override void Validate(ValidationContext validationContext, ValidationBuilder<TEntity> validator)
    {
        base.Validate(validationContext, validator);

        validator.Property(m => m.ActiveRating).IsBetween(0, 100);
        validator.Property(m => m.Rating).IsBetween(0, 100);
        validator.Property(m => m.Votes).IsGreaterThan(0);
        validator.Property(m => m.WebsiteUrl).IsValidUrl();
    }
}
