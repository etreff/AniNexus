using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using AniNexus.Domain.Validation;
using AniNexus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models an entity that contains information about an anime and its releases.
/// </summary>
/// <seealso cref="EAnimeCategory"/>
public class AnimeModel : IHasAudit, IHasRowVersion, IHasSoftDelete, IEntityTypeConfiguration<AnimeModel>, IValidatableObject
{
    /// <summary>
    /// The Id of the anime.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The category/type of this anime.
    /// </summary>
    /// <seealso cref="EAnimeCategory"/>
    /// <seealso cref="AnimeCategoryTypeModel"/>
    public int CategoryId { get; set; }

    /// <summary>
    /// The Id of the season the anime will be released in.
    /// </summary>
    /// <remarks>
    /// The season refers to the Japanese anime schedule.
    /// </remarks>
    /// <seealso cref="EAnimeSeason"/>
    /// <seealso cref="AnimeSeasonTypeModel"/>
    public int? SeasonId { get; set; }

    /// <summary>
    /// The official website of this anime.
    /// </summary>
    public string? WebsiteUrl { get; set; }

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

    #region Interface Properties
    /// <summary>
    /// The UTC date and time this entry was added to the table.
    /// </summary>
    public DateTime DateAdded { get; set; }

    /// <summary>
    /// The UTC date and time this entry was last updated.
    /// </summary>
    public DateTime DateUpdated { get; set; }

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
    #endregion

    #region Navigation Properties
    /// <summary>
    /// The category of the anime.
    /// </summary>
    public AnimeCategoryTypeModel Category { get; set; } = default!;

    /// <summary>
    /// The season the anime was or will be released in.
    /// </summary>
    /// <remarks>
    /// The season generally refers to the Japanese airing schedule.
    /// </remarks>
    public AnimeSeasonTypeModel? Season { get; set; }

    /// <summary>
    /// The releases of this anime.
    /// </summary>
    /// <remarks>
    /// <para>
    /// As an example, an anime will release in Japanese and have an English dub release
    /// published by Funimation. These will count as different releases.
    /// </para>
    /// <para>
    /// An anime must have at least one release defined, even if nothing is known about it.
    /// </para>
    /// </remarks>
    public IList<AnimeReleaseModel> Releases { get; set; } = default!;

    /// <summary>
    /// The user reviews of this anime.
    /// </summary>
    public IList<AnimeReviewModel> Reviews { get; set; } = default!;

    /// <summary>
    /// The Id of this anime in different third party trackers.
    /// </summary>
    public IList<AnimeThirdPartyMapModel> ExternalIds { get; set; } = default!;

    /// <summary>
    /// Twitter hashtags associated with this anime.
    /// </summary>
    public IList<AnimeTwitterHashTagMapModel> TwitterHashtags { get; set; } = default!;

    /// <summary>
    /// Genres that describe this anime.
    /// </summary>
    public IList<AnimeGenreMapModel> Genres { get; set; } = default!;

    /// <summary>
    /// Tags that have been assigned to this anime.
    /// </summary>
    public IList<AnimeTagMapModel> Tags { get; set; } = default!;

    /// <summary>
    /// Other anime entries that are related to this one.
    /// </summary>
    public IList<AnimeRelatedMapModel> Related { get; set; } = default!;

    /// <summary>
    /// The root parent series of this anime.
    /// </summary>
    /// <remarks>
    /// This generally should only have a single entry, but in the case of collaborations
    /// or crossovers this may contain more than one element.
    /// </remarks>
    public IList<MediaSeriesAnimeMapModel> Series { get; set; } = default!;

    /// <summary>
    /// The companies associated with this anime.
    /// </summary>
    public IList<MediaCompanyAnimeMapModel> Companies { get; set; } = default!;

    /// <summary>
    /// The people associated with this anime.
    /// </summary>
    public IList<AnimePersonRoleMapModel> People { get; set; } = default!;

    /// <summary>
    /// The characters involved in this anime.
    /// </summary>
    public IList<AnimeCharacterMapModel> Characters { get; set; } = default!;

    /// <summary>
    /// The users who have favorited this anime.
    /// </summary>
    public IList<AnimeFavoriteMapModel> Favorites { get; set; } = default!;
    #endregion

    #region Helper Properties
    /// <summary>
    /// Returns the name of the anime as defined by the anime's primary release.
    /// </summary>
    public AnimeReleaseModel.NameModel Name => GetPrimaryRelease().Name;

    /// <summary>
    /// A synopsis or description of the anime.
    /// </summary>
    public string? Synopsis => GetPrimaryRelease().Synopsis;

    /// <summary>
    /// The producers of the anime.
    /// </summary>
    public IEnumerable<MediaCompanyAnimeMapModel> Producers
        => GetCompaniesWithRole(ECompanyRole.Producer);

    /// <summary>
    /// The studios who created the anime.
    /// </summary>
    public IEnumerable<MediaCompanyAnimeMapModel> Studios
        => GetCompaniesWithRole(ECompanyRole.Studio);

    /// <summary>
    /// The companies that have a license to distribute or stream the anime.
    /// </summary>
    public IEnumerable<MediaCompanyAnimeMapModel> Licensees
        => GetCompaniesWithRole(ECompanyRole.Publisher);
    #endregion

    public void Configure(EntityTypeBuilder<AnimeModel> builder)
    {
        builder.ToTable("Anime");

        builder.HasKey(m => m.Id);
        builder.HasIndex(m => m.CategoryId);
        builder.HasIndex(m => m.SeasonId).HasFilter("[SeasonId] IS NOT NULL");

        builder.HasOne(m => m.Category).WithMany().HasForeignKey(m => m.CategoryId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.Season).WithMany().HasForeignKey(m => m.SeasonId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);

        builder.Property(m => m.WebsiteUrl).HasComment("The URL to the anime's official website.");
        builder.Property(m => m.Rating).HasComment("The user rating of the anime (Completed Only), from 0 to 100. Calculated by the system periodically.");
        builder.Property(m => m.ActiveRating).HasComment("The user rating of the anime (Watching Only), from 0 to 100. Calculated by the system periodically.");
        builder.Property(m => m.Votes).HasComment("The number of votes that contributed to the rating. Calculated by the system periodically.").HasDefaultValue(0);

        //builder.Ignore(m => m.Producers);
        //builder.Ignore(m => m.Studios);
        //builder.Ignore(m => m.Licensees);
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Rating.HasValue)
        {
            if (Rating > 100)
            {
                yield return new ValidationResult("Rating cannot be above 100", new[] { nameof(Rating) });
            }
            if (Votes == 0)
            {
                yield return new ValidationResult("Votes may not be 0 if a rating is specified.", new[] { nameof(Votes) });
            }
        }

        if (ActiveRating.HasValue && ActiveRating > 100)
        {
            yield return new ValidationResult("Active rating cannot be above 100", new[] { nameof(ActiveRating) });
        }

        if (Votes < 0)
        {
            yield return new ValidationResult("Votes must be greater than or equal to 0.", new[] { nameof(Votes) });
        }

        if (!UriValidator.Validate(WebsiteUrl, nameof(WebsiteUrl), out var urlValidationResult))
        {
            yield return urlValidationResult;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AnimeReleaseModel GetPrimaryRelease()
    {
        return GetReleases().Single(static r => r.IsPrimary);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private IList<AnimeReleaseModel> GetReleases()
    {
        if (Releases is null)
        {
            throw new InvalidOperationException("Releases must be loaded via Include() before this property can be used.");
        }

        return Releases;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private IEnumerable<MediaCompanyAnimeMapModel> GetCompaniesWithRole(ECompanyRole role)
    {
        if (Companies is null)
        {
            throw new InvalidOperationException("Companies must be loaded via Include() before this property can be used.");
        }

        return Companies.Where(r => r.Role.Id == (int)role);
    }
}
