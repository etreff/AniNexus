using AniNexus.Models;

namespace AniNexus.Domain.Entities;

/// <summary>
/// Models an entity that contains information about an anime and its releases.
/// </summary>
/// <seealso cref="EAnimeCategory"/>
public sealed class AnimeEntity : FranchiseMediaEntity<AnimeEntity>
{
    /// <summary>
    /// The category/type of this anime.
    /// </summary>
    /// <seealso cref="EAnimeCategory"/>
    /// <seealso cref="AnimeCategoryTypeEntity"/>
    public byte CategoryId { get; set; }

    /// <summary>
    /// The Id of the season the anime will be released in.
    /// </summary>
    /// <remarks>
    /// The season refers to the Japanese anime schedule.
    /// </remarks>
    /// <seealso cref="EAnimeSeason"/>
    /// <seealso cref="AnimeSeasonTypeEntity"/>
    public byte? SeasonId { get; set; }

    /// <summary>
    /// The category of the anime.
    /// </summary>
    public AnimeCategoryTypeEntity Category { get; set; } = default!;

    /// <summary>
    /// The season the anime was or will be released in.
    /// </summary>
    /// <remarks>
    /// The season generally refers to the Japanese airing schedule.
    /// </remarks>
    public AnimeSeasonTypeEntity? Season { get; set; }

    /// <summary>
    /// A collection of releases for this anime.
    /// </summary>
    public IList<AnimeReleaseEntity> Releases { get; set; } = default!;

    /// <summary>
    /// A mapping of third party trackers and the identifiers they have assigned this anime.
    /// </summary>
    public IList<AnimeThirdPartyMapEntity> ExternalIds { get; set; } = default!;

    /// <summary>
    /// A mapping of users who have favorited this anime.
    /// </summary>
    public IList<AnimeFavoriteMapEntity> Favorites { get; set; } = default!;

    /// <summary>
    /// Genres that define this anime.
    /// </summary>
    public IList<AnimeGenreMapEntity> Genres { get; set; } = default!;

    /// <summary>
    /// Tags that have been applied to this anime.
    /// </summary>
    public IList<AnimeTagMapEntity> Tags { get; set; } = default!;

    /// <summary>
    /// Anime that are related to this anime.
    /// </summary>
    public IList<AnimeRelatedMapEntity> RelatedAnime { get; set; } = default!;

    /// <summary>
    /// User reviews for the anime.
    /// </summary>
    public IList<AnimeUserReviewEntity> UserReviews { get; set; } = default!;

    /// <summary>
    /// The characters in this anime.
    /// </summary>
    public IList<AnimeCharacterMapEntity> Characters { get; set; } = default!;

    /// <summary>
    /// Companies that were involved in the creation of the native release of the anime.
    /// </summary>
    public IList<CompanyAnimeMapEntity> Companies { get; set; } = default!;

    /// <summary>
    /// Albums that are associated with this anime.
    /// </summary>
    public IList<AlbumEntity> Albums { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AnimeEntity> builder)
    {
        base.ConfigureEntity(builder);

        // 1. Index specification
        builder.HasIndex(m => m.CategoryId);
        builder.HasIndex(m => m.SeasonId).HasNotNullFilter();
        // 2. Navigation properties
        builder.HasOne(m => m.Category).WithMany().HasForeignKey(m => m.CategoryId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.Season).WithMany().HasForeignKey(m => m.SeasonId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
        // 3. Propery specification
        // 4. Other
    }
}
