using AniNexus.Models;
using Microsoft.EntityFrameworkCore;

namespace AniNexus.Domain.Models
{
    /// <summary>
    /// Models an anime age rating.
    /// </summary>
    /// <seealso cref="EAnimeAgeRating"/>
    public class AnimeAgeRatingTypeModel : EnumModelBase<EAnimeAgeRating, AnimeAgeRatingTypeModel>
    {
        /// <summary>
        /// Gets the minimum age as defined by the age rating.
        /// </summary>
        public int GetMinAge()
        {
            var rating = (EAnimeAgeRating)Id;
            var metadata = rating.GetMember().GetAttributes<EnumMetadataAttribute>();
            string? age = metadata.SingleOrDefault(static m => m.Key.Equals("MinAge", StringComparison.OrdinalIgnoreCase))?.Value;
            return !string.IsNullOrEmpty(age) ? int.Parse(age) : 0;
        }

        /// <summary>
        /// Gets the abbreviation of the age rating.
        /// </summary>
        public string? GetAbbreviation()
        {
            var rating = (EAnimeAgeRating)Id;
            var metadata = rating.GetMember().GetAttributes<EnumMetadataAttribute>();
            return metadata.SingleOrDefault(static m => m.Key.Equals("Abbreviation", StringComparison.OrdinalIgnoreCase))?.Value;
        }

        /// <summary>
        /// Gets a description of the age rating.
        /// </summary>
        public string? GetDescription()
        {
            var rating = (EAnimeAgeRating)Id;
            var metadata = rating.GetMember().GetAttributes<EnumMetadataAttribute>();
            return metadata.SingleOrDefault(static m => m.Key.Equals("Description", StringComparison.OrdinalIgnoreCase))?.Value;
        }
    }

    /// <summary>
    /// Models a category that an anime falls under.
    /// </summary>
    /// <seealso cref="EAnimeCategory"/>
    public class AnimeCategoryTypeModel : EnumModelBase<EAnimeCategory, AnimeCategoryTypeModel> { }

    /// <summary>
    /// Models a status that an anime list entry can be under.
    /// </summary>
    /// <seealso cref="EAnimeListStatus"/>
    public class AnimeListStatusTypeModel : EnumModelBase<EAnimeListStatus, AnimeListStatusTypeModel> { }

    /// <summary>
    /// Models an anime season.
    /// </summary>
    /// <seealso cref="EAnimeSeason"/>
    public class AnimeSeasonTypeModel : EnumModelBase<EAnimeSeason, AnimeSeasonTypeModel> { }

    /// <summary>
    /// Models a role that a character plays in a piece of media.
    /// </summary>
    /// <seealso cref="ECharacterRole"/>
    public class CharacterRoleTypeModel : EnumModelBase<ECharacterRole, CharacterRoleTypeModel> { }

    /// <summary>
    /// Models a role a company plays in the production of a piece of media.
    /// </summary>
    /// <seealso cref="ECompanyRole"/>
    public class CompanyRoleTypeModel : EnumModelBase<ECompanyRole, CompanyRoleTypeModel> { }

    /// <summary>
    /// Models a category that a game falls under.
    /// </summary>
    /// <seealso cref="EGameCategory"/>
    public class GameCategoryTypeModel : EnumModelBase<EGameCategory, GameCategoryTypeModel> { }

    /// <summary>
    /// Models a status that a game list entry can be under.
    /// </summary>
    /// <seealso cref="EGameListStatus"/>
    public class GameListStatusTypeModel : EnumModelBase<EGameListStatus, GameListStatusTypeModel> { }

    /// <summary>
    /// Models a manga age rating.
    /// </summary>
    /// <seealso cref="EMangaAgeRating"/>
    public class MangaAgeRatingTypeModel : EnumModelBase<EMangaAgeRating, MangaAgeRatingTypeModel>
    {
        /// <summary>
        /// Gets the minimum age as defined by the age rating.
        /// </summary>
        public int GetMinAge()
        {
            var rating = (EMangaAgeRating)Id;
            var metadata = rating.GetMember().GetAttributes<EnumMetadataAttribute>();
            string? age = metadata.SingleOrDefault(static m => m.Key.Equals("MinAge", StringComparison.OrdinalIgnoreCase))?.Value;
            return !string.IsNullOrEmpty(age)
                ? int.Parse(age)
                : 0;
        }

        /// <summary>
        /// Gets the abbreviation of the age rating.
        /// </summary>
        public string? GetAbbreviation()
        {
            var rating = (EMangaAgeRating)Id;
            var metadata = rating.GetMember().GetAttributes<EnumMetadataAttribute>();
            return metadata.SingleOrDefault(static m => m.Key.Equals("Abbreviation", StringComparison.OrdinalIgnoreCase))?.Value;
        }

        /// <summary>
        /// Gets a description of the age rating.
        /// </summary>
        public string? GetDescription()
        {
            var rating = (EMangaAgeRating)Id;
            var metadata = rating.GetMember().GetAttributes<EnumMetadataAttribute>();
            return metadata.SingleOrDefault(static m => m.Key.Equals("Description", StringComparison.OrdinalIgnoreCase))?.Value;
        }
    }

    /// <summary>
    /// Models a category that a manga falls under.
    /// </summary>
    /// <seealso cref="EMangaCategory"/>
    public class MangaCategoryTypeModel : EnumModelBase<EMangaCategory, MangaCategoryTypeModel> { }

    /// <summary>
    /// Models a status that a manga list entry can be under.
    /// </summary>
    /// <seealso cref="EMangaListStatus"/>
    public class MangaListStatusTypeModel : EnumModelBase<EMangaListStatus, MangaListStatusTypeModel> { }

    /// <summary>
    /// Models how two pieces of media can be related to each other.
    /// </summary>
    /// <seealso cref="EMediaRelationType"/>
    public class MediaRelationTypeModel : EnumModelBase<EMediaRelationType, MediaRelationTypeModel> { }

    /// <summary>
    /// Models the production status of a piece of media.
    /// </summary>
    /// <seealso cref="EMediaStatus"/>
    public class MediaStatusTypeModel : EnumModelBase<EMediaStatus, MediaStatusTypeModel> { }

    /// <summary>
    /// Models a role that a person plays in the production of a piece of media.
    /// </summary>
    /// <seealso cref="EPersonRole"/>
    public class PersonRoleTypeModel : EnumModelBase<EPersonRole, PersonRoleTypeModel> { }
}

namespace AniNexus.Domain
{
    using AniNexus.Domain.Models;

    public partial class ApplicationDbContext
    {
        /// <summary>
        /// A collection of known age ratings that can be applied to anime.
        /// </summary>
        public DbSet<AnimeAgeRatingTypeModel> AnimeAgeRatings => Set<AnimeAgeRatingTypeModel>();

        /// <summary>
        /// A collection of categories that an anime can fall under.
        /// </summary>
        public DbSet<AnimeCategoryTypeModel> AnimeCategoryTypes => Set<AnimeCategoryTypeModel>();

        /// <summary>
        /// A collection of categories an anime list entry can fall under.
        /// </summary>
        public DbSet<AnimeListStatusTypeModel> AnimeListStatusTypes => Set<AnimeListStatusTypeModel>();

        /// <summary>
        /// A collection of anime seasons.
        /// </summary>
        public DbSet<AnimeSeasonTypeModel> AnimeSeasonTypes => Set<AnimeSeasonTypeModel>();

        /// <summary>
        /// A collection of roles a character can play in a piece of media.
        /// </summary>
        public DbSet<CharacterRoleTypeModel> CharacterRoleTypes => Set<CharacterRoleTypeModel>();

        /// <summary>
        /// A collection of roles a company can play in the production of a piece of media.
        /// </summary>
        public DbSet<CompanyRoleTypeModel> CompanyRoleTypes => Set<CompanyRoleTypeModel>();

        /// <summary>
        /// A collection of categories that a game can fall under.
        /// </summary>
        public DbSet<GameCategoryTypeModel> GameCategoryTypes => Set<GameCategoryTypeModel>();

        /// <summary>
        /// A collection of categories a game list entry can fall under.
        /// </summary>
        public DbSet<GameListStatusTypeModel> GameListStatusTypes => Set<GameListStatusTypeModel>();

        /// <summary>
        /// A collection of known age ratings that can be applied to manga.
        /// </summary>
        public DbSet<MangaAgeRatingTypeModel> MangaAgeRatings => Set<MangaAgeRatingTypeModel>();

        /// <summary>
        /// A collection of categories that a manga can fall under.
        /// </summary>
        public DbSet<MangaCategoryTypeModel> MangaCategoryTypes => Set<MangaCategoryTypeModel>();

        /// <summary>
        /// A collection of categories a manga list entry can fall under.
        /// </summary>
        public DbSet<MangaListStatusTypeModel> MangaListStatusTypes => Set<MangaListStatusTypeModel>();

        /// <summary>
        /// A collection of relationship types between two pieces of media.
        /// </summary>
        public DbSet<MediaRelationTypeModel> MediaRelationTypes => Set<MediaRelationTypeModel>();

        /// <summary>
        /// A collection of statuses that a piece of media can be in with regards to production.
        /// </summary>
        public DbSet<MediaStatusTypeModel> MediaStatusTypes => Set<MediaStatusTypeModel>();

        /// <summary>
        /// A collection of roles that a person can play in the production of a piece of media.
        /// </summary>
        public DbSet<PersonRoleTypeModel> PersonRoleTypes => Set<PersonRoleTypeModel>();
    }
}
