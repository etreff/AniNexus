using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using AniNexus.Domain.Validation;
using AniNexus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models an entity that contains information about a game.
/// </summary>
public class GameModel : IHasAudit, IHasRowVersion, IHasSoftDelete, IEntityTypeConfiguration<GameModel>, IValidatableObject
{
    /// <summary>
    /// The Id of the game.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The category/type of this game.
    /// </summary>
    /// <seealso cref="EGameCategory"/>
    /// <seealso cref="GameCategoryTypeModel"/>
    public int CategoryId { get; set; }

    /// <summary>
    /// The Id of the status that describes the production state of this game.
    /// </summary>
    /// <seealso cref="EMediaStatus"/>
    /// <seealso cref="MediaStatusTypeModel"/>
    public int StatusId { get; set; }

    /// <summary>
    /// The official website of this game.
    /// </summary>
    public string? WebsiteUrl { get; set; }

    /// <summary>
    /// The date this game was released.
    /// </summary>
    public Date? ReleaseDate { get; set; }

    /// <summary>
    /// A synopsis or description of the game.
    /// </summary>
    public string? Synopsis { get; set; }

    /// <summary>
    /// The average user rating for this piece of media, between 0 and 100.
    /// </summary>
    /// <remarks>
    /// This value will be calculated by the system periodically.
    /// </remarks>
    public byte? Rating { get; set; }

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
    /// The category of the game.
    /// </summary>
    public GameCategoryTypeModel Category { get; set; } = default!;

    /// <summary>
    /// The status that describes the production state of this release.
    /// </summary>
    public MediaStatusTypeModel Status { get; set; } = default!;

    /// <summary>
    /// The names of this game.
    /// </summary>
    public IList<NameModel> Names { get; set; } = default!;

    /// <summary>
    /// Models a name of a game.
    /// </summary>
    public class NameModel
    {
        /// <summary>
        /// The Id of the locale of the name.
        /// </summary>
        public int LocaleId { get; set; }

        /// <summary>
        /// The name in the native language.
        /// </summary>
        public string? NativeName { get; set; }

        /// <summary>
        /// The romanization of the native name.
        /// </summary>
        public string? RomajiName { get; set; }

        /// <summary>
        /// The name in English.
        /// </summary>
        public string? EnglishName { get; set; }

        /// <summary>
        /// Whether the name is the primary name.
        /// </summary>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// The locale of the name.
        /// </summary>
        public LocaleModel Locale { get; set; } = default!;
    }

    /// <summary>
    /// The user reviews of this game.
    /// </summary>
    public IList<GameReviewModel> Reviews { get; set; } = default!;

    /// <summary>
    /// The Id of this game in different third party trackers.
    /// </summary>
    public IList<GameThirdPartyMapModel> ExternalIds { get; set; } = default!;

    /// <summary>
    /// Twitter hashtags associated with this anime.
    /// </summary>
    public IList<GameTwitterHashTagMapModel> TwitterHashtags { get; set; } = default!;

    /// <summary>
    /// Genres that this game belongs to.
    /// </summary>
    public IList<GameGenreMapModel> Genres { get; set; } = default!;

    /// <summary>
    /// Tags that have been assigned to this game.
    /// </summary>
    public IList<GameTagMapModel> Tags { get; set; } = default!;

    /// <summary>
    /// Other game entries that are related to this one.
    /// </summary>
    public IList<GameRelatedMapModel> Related { get; set; } = default!;

    /// <summary>
    /// The root parent series of this game.
    /// </summary>
    /// <remarks>
    /// This generally should only have a single entry, but in the case of collaborations
    /// or crossovers this may contain more than one element.
    /// </remarks>
    public IList<MediaSeriesGameMapModel> Series { get; set; } = default!;

    /// <summary>
    /// The companies associated with this game.
    /// </summary>
    public IList<MediaCompanyGameMapModel> Companies { get; set; } = default!;

    /// <summary>
    /// The people associated with this game.
    /// </summary>
    public IList<GamePersonRoleMapModel> People { get; set; } = default!;

    /// <summary>
    /// The characters involved in this game.
    /// </summary>
    public IList<GameCharacterMapModel> Characters { get; set; } = default!;

    /// <summary>
    /// The users who have favorited this piece of media.
    /// </summary>
    public IList<GameFavoriteMapModel> Favorites { get; set; } = default!;

    /// <summary>
    /// Trailers for this game.
    /// </summary>
    public IList<TrailerModel> Trailers { get; set; } = default!;

    /// <summary>
    /// Models a game release trailer.
    /// </summary>
    public class TrailerModel : IValidatableObject
    {
        /// <summary>
        /// A link to the trailer.
        /// </summary>
        /// <remarks>
        /// This URL must be valid.
        /// </remarks>
        public string ResourceUrl { get; set; } = default!;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!UriValidator.Validate(ResourceUrl, nameof(ResourceUrl), out var urlValidationResult))
            {
                yield return urlValidationResult;
            }
        }
    }
    #endregion

    #region Helper Properties
    /// <summary>
    /// Returns the primary name of this game.
    /// </summary>
    public NameModel Name => Names.Single(static n => n.IsPrimary);

    /// <summary>
    /// Whether this game is considered NSFW.
    /// </summary>
    /// <remarks>
    /// Since gore is considered NSFW in America, this value will also return <see langword="true"/>
    /// if <see cref="IsGore"/> would return <see langword="true"/>.
    /// </remarks>
    public bool IsNSFW => Tags?.Select(static m => m.Tag).Any(static t => t.IsNSFW || t.IsGore) ?? false;

    /// <summary>
    /// Whether this game has what is considered to be gore.
    /// </summary>
    public bool IsGore => Tags?.Select(static m => m.Tag).Any(static t => t.IsGore) ?? false;

    /// <summary>
    /// The studios who created the game.
    /// </summary>
    public IEnumerable<MediaCompanyGameMapModel> Studios
        => GetCompaniesWithRole(ECompanyRole.Studio);

    /// <summary>
    /// The companies that have a license to distribute the game.
    /// </summary>
    public IEnumerable<MediaCompanyGameMapModel> Publishers
        => GetCompaniesWithRole(ECompanyRole.Publisher);
    #endregion

    public void Configure(EntityTypeBuilder<GameModel> builder)
    {
        builder.ToTable("Game");

        builder.HasKey(m => m.Id);
        builder.HasIndex(m => m.CategoryId);
        builder.HasIndex(m => m.StatusId);
        builder.HasIndex(m => m.ReleaseDate).HasFilter("[ReleaseDate] IS NOT NULL");

        builder.HasOne(m => m.Category).WithMany().HasForeignKey(m => m.CategoryId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.Status).WithMany().HasForeignKey(m => m.StatusId).IsRequired().OnDelete(DeleteBehavior.Restrict);

        builder.OwnsMany(m => m.Names, name =>
        {
            name.ToTable("GameName");

            name.HasOne(m => m.Locale).WithMany().HasForeignKey(m => m.LocaleId).IsRequired().OnDelete(DeleteBehavior.Restrict);

            // Justification - auto include locale information on an owned entity.
            name.Navigation(m => m.Locale).AutoInclude();

            name.Property(m => m.NativeName).HasComment("The native name.").HasColumnName(nameof(NameModel.NativeName));
            name.Property(m => m.RomajiName).HasComment("The romanization of the native name.").HasColumnName(nameof(NameModel.RomajiName));
            name.Property(m => m.EnglishName).HasComment("The name in English.").HasColumnName(nameof(NameModel.EnglishName));
            name.Property(m => m.IsPrimary).HasComment("Whether this name is the primary name of the release.").HasColumnName(nameof(NameModel.IsPrimary));
        });
        
        builder.OwnsMany(m => m.Trailers, trailer =>
        {
            trailer.ToTable("GameTrailer");

            trailer.Property(m => m.ResourceUrl).HasComment("The URL of the trailer or promotional video.").HasColumnName(nameof(TrailerModel.ResourceUrl));
        });

        builder.Property(m => m.ReleaseDate).HasComment("The date this game was released.");
        builder.Property(m => m.WebsiteUrl).HasComment("The URL to the media's official website.");
        builder.Property(m => m.Synopsis).HasComment("A synopsis or description of the media.");
        builder.Property(m => m.Rating).HasComment("The user rating of the media, from 0 to 100. Calculated by the system periodically.");
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

        if (Votes < 0)
        {
            yield return new ValidationResult("Votes must be greater than or equal to 0.", new[] { nameof(Votes) });
        }

        if (!UriValidator.Validate(WebsiteUrl, nameof(WebsiteUrl), out var urlValidationResult))
        {
            yield return urlValidationResult;
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private IEnumerable<MediaCompanyGameMapModel> GetCompaniesWithRole(ECompanyRole role)
    {
        if (Companies is null)
        {
            throw new InvalidOperationException("Companies must be loaded via Include() before this property can be used.");
        }

        return Companies.Where(r => r.Role.Id == (int)role);
    }

    public static IQueryable<GameModel> IncludeAll(DbSet<GameModel> dbSet, bool splitQuery = true)
    {
        IQueryable<GameModel> result = dbSet
            .Include(m => m.Category)
            .Include(m => m.Names)
            .Include(m => m.Reviews).ThenInclude(m => m.User)
            .Include(m => m.Reviews).ThenInclude(m => m.Votes)
            .Include(m => m.ExternalIds)
            .Include(m => m.TwitterHashtags)
            .Include(m => m.Genres).ThenInclude(m => m.Genre)
            .Include(m => m.Tags).ThenInclude(m => m.Tag)
            .Include(m => m.Related).ThenInclude(m => m.Related)
            .Include(m => m.Related).ThenInclude(m => m.RelationType)
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
