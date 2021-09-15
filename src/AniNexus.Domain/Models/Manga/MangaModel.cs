using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using AniNexus.Domain.Validation;
using AniNexus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a manga.
/// </summary>
/// <seealso cref="EMangaCategory"/>
public class MangaModel : IHasAudit, IHasRowVersion, IHasSoftDelete, IEntityTypeConfiguration<MangaModel>, IValidatableObject
{
    /// <summary>
    /// The Id of the manga.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The category/type of this manga.
    /// </summary>
    /// <seealso cref="EMangaCategory"/>
    /// <seealso cref="MangaCategoryTypeModel"/>
    public int CategoryId { get; set; }

    /// <summary>
    /// The official website of this manga.
    /// </summary>
    public string? WebsiteUrl { get; set; }

    /// <summary>
    /// A synopsis or description of the manga.
    /// </summary>
    public string? Synopsis { get; set; }

    /// <summary>
    /// The average user rating for this manga, between 0 and 100.
    /// The rating only takes into account ratings from users who have
    /// given the manga a rating and have completed the manga.
    /// </summary>
    /// <remarks>
    /// This value will be calculated by the system periodically.
    /// </remarks>
    public byte? Rating { get; set; }

    /// <summary>
    /// The average user rating for this manga, between 0 and 100.
    /// The rating only takes into account ratings from users who have given
    /// the manga a rating and are actively reading the manga.
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
    /// The category of the manga.
    /// </summary>
    public MangaCategoryTypeModel Category { get; set; } = default!;

    /// <summary>
    /// The releases of this manga.
    /// </summary>
    /// <remarks>
    /// <para>
    /// As an example, a manga may be officially released in both Japanese and
    /// English. These will count as different releases.
    /// </para>
    /// <para>
    /// An manga must have at least one release defined, even if nothing is known about it.
    /// </para>
    /// </remarks>
    public IList<MangaReleaseModel> Releases { get; set; } = default!;

    /// <summary>
    /// The user reviews of this manga.
    /// </summary>
    public IList<MangaReviewModel> Reviews { get; set; } = default!;

    /// <summary>
    /// The Id of this manga in different third party trackers.
    /// </summary>
    public IList<MangaThirdPartyMapModel> ExternalIds { get; set; } = default!;

    /// <summary>
    /// Twitter hashtags associated with this manga.
    /// </summary>
    public IList<MangaTwitterHashTagMapModel> TwitterHashtags { get; set; } = default!;

    /// <summary>
    /// Genres that this manga belongs to.
    /// </summary>
    public IList<MangaGenreMapModel> Genres { get; set; } = default!;

    /// <summary>
    /// Tags that have been assigned to this manga.
    /// </summary>
    public IList<MangaTagMapModel> Tags { get; set; } = default!;

    /// <summary>
    /// Other manga entries that are related to this one.
    /// </summary>
    public IList<MangaRelatedMapModel> Related { get; set; } = default!;

    /// <summary>
    /// The root parent series of this manga.
    /// </summary>
    /// <remarks>
    /// This generally should only have a single entry, but in the case of collaborations
    /// or crossovers this may contain more than one element.
    /// </remarks>
    public IList<MediaSeriesMangaMapModel> Series { get; set; } = default!;

    /// <summary>
    /// The companies associated with this manga.
    /// </summary>
    public IList<MediaCompanyMangaMapModel> Companies { get; set; } = default!;

    /// <summary>
    /// The people associated with this manga.
    /// </summary>
    public IList<MangaPersonRoleMapModel> People { get; set; } = default!;

    /// <summary>
    /// The characters involved in this manga.
    /// </summary>
    public IList<MangaCharacterMapModel> Characters { get; set; } = default!;

    /// <summary>
    /// The users who have favorited this piece of media.
    /// </summary>
    public IList<MangaFavoriteMapModel> Favorites { get; set; } = default!;
    #endregion

    #region Helper Properties
    /// <summary>
    /// Returns the name of the manga as defined by the manga's primary release.
    /// </summary>
    public MangaReleaseModel.NameModel Name => GetPrimaryRelease().GetPrimaryName();

    /// <summary>
    /// The companies that have a license to distribute or stream the manga.
    /// </summary>
    public IEnumerable<MediaCompanyMangaMapModel> Licensees
        => GetCompaniesWithRole(ECompanyRole.Publisher);
    #endregion

    public void Configure(EntityTypeBuilder<MangaModel> builder)
    {
        builder.ToTable("Manga");

        builder.HasKey(m => m.Id);
        builder.HasIndex(m => m.CategoryId);

        builder.HasOne(m => m.Category).WithMany().HasForeignKey(m => m.CategoryId).IsRequired().OnDelete(DeleteBehavior.Restrict);

        builder.Property(m => m.WebsiteUrl).HasComment("The URL to the manga's official website.");
        builder.Property(m => m.Synopsis).HasComment("A synopsis or description of the manga.");
        builder.Property(m => m.Rating).HasComment("The user rating of the manga (Completed Only), from 0 to 100. Calculated by the system periodically.");
        builder.Property(m => m.ActiveRating).HasComment("The user rating of the manga (Readonly Only), from 0 to 100. Calculated by the system periodically.");
        builder.Property(m => m.Votes).HasComment("The number of votes that contributed to the rating. Calculated by the system periodically.").HasDefaultValue(0);
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
    public MangaReleaseModel GetPrimaryRelease()
    {
        return GetReleases().Single(static r => r.IsPrimary);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private IList<MangaReleaseModel> GetReleases()
    {
        if (Releases is null)
        {
            throw new InvalidOperationException("Releases must be loaded via Include() before this property can be used.");
        }

        return Releases;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private IEnumerable<MediaCompanyMangaMapModel> GetCompaniesWithRole(ECompanyRole role)
    {
        if (Companies is null)
        {
            throw new InvalidOperationException("Companies must be loaded via Include() before this property can be used.");
        }

        return Companies.Where(r => r.Role.Id == (int)role);
    }

    public static IQueryable<MangaModel> IncludeAll(DbSet<MangaModel> dbSet, bool splitQuery = true)
    {
        IQueryable<MangaModel> result = dbSet
            .Include(m => m.Category)
            .Include(m => m.Releases).ThenInclude(m => m.Names)
            .Include(m => m.Releases).ThenInclude(m => m.Locale)
            .Include(m => m.Releases).ThenInclude(m => m.AgeRating)
            .Include(m => m.Releases).ThenInclude(m => m.Status)
            .Include(m => m.Releases).ThenInclude(m => m.Volumes)
            .Include(m => m.Releases).ThenInclude(m => m.Chapters)
            .Include(m => m.Reviews).ThenInclude(m => m.User)
            .Include(m => m.Reviews).ThenInclude(m => m.Votes)
            .Include(m => m.ExternalIds)
            .Include(m => m.TwitterHashtags)
            .Include(m => m.Genres).ThenInclude(m => m.Genre)
            .Include(m => m.Tags).ThenInclude(m => m.Tag)
            .Include(m => m.Related)
            // Only include the parent series names in the include results.
            .Include(m => m.Series).ThenInclude(m => m.Series).ThenInclude(m => m.Names)
            // Don't care about company aliases.
            .Include(m => m.Companies).ThenInclude(m => m.Company).ThenInclude(m => m.Name)
            .Include(m => m.Companies).ThenInclude(m => m.Role)
            .Include(m => m.People).ThenInclude(m => m.Person)
            .Include(m => m.Characters).ThenInclude(m => m.Character)
            // Favorites is *not* included for performance reasons. The count is inline in the record.
            //.Include(m => m.Favorites)
            ;

        if (splitQuery)
        {
            result = result.AsSplitQuery();
        }

        return result;
    }
}
