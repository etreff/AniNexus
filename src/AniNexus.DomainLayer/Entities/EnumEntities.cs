namespace AniNexus.Domain.Entities
{
    using AniNexus.Models;

    /// <summary>
    /// Models an anime age rating.
    /// </summary>
    /// <seealso cref="EAnimeAgeRating"/>
    public sealed class AnimeAgeRatingTypeEntity : EnumEntity<EAnimeAgeRating, AnimeAgeRatingTypeEntity, byte>
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
    public sealed class AnimeCategoryTypeEntity : EnumEntity<EAnimeCategory, AnimeCategoryTypeEntity, byte> { }

    /// <summary>
    /// Models a status that an anime list entry can be under.
    /// </summary>
    /// <seealso cref="EAnimeListStatus"/>
    public sealed class AnimeListStatusTypeEntity : EnumEntity<EAnimeListStatus, AnimeListStatusTypeEntity, byte> { }

    /// <summary>
    /// Models an anime season.
    /// </summary>
    /// <seealso cref="EAnimeSeason"/>
    public sealed class AnimeSeasonTypeEntity : EnumEntity<EAnimeSeason, AnimeSeasonTypeEntity, byte> { }

    /// <summary>
    /// Models a role that a character plays in a piece of media.
    /// </summary>
    /// <seealso cref="ECharacterRole"/>
    public sealed class CharacterRoleTypeEntity : EnumEntity<ECharacterRole, CharacterRoleTypeEntity, byte> { }

    /// <summary>
    /// Models a role a company plays in the production of a piece of media.
    /// </summary>
    /// <seealso cref="ECompanyRole"/>
    public sealed class CompanyRoleTypeEntity : EnumEntity<ECompanyRole, CompanyRoleTypeEntity, byte> { }

    /// <summary>
    /// Models a company type.
    /// </summary>
    /// <seealso cref="ECompanyType"/>
    public sealed class CompanyTypeEntity : EnumEntity<ECompanyType, CompanyTypeEntity, byte> { }

    /// <summary>
    /// Models a category that a game falls under.
    /// </summary>
    /// <seealso cref="EGameCategory"/>
    public sealed class GameCategoryTypeEntity : EnumEntity<EGameCategory, GameCategoryTypeEntity, byte> { }

    /// <summary>
    /// Models a status that a game list entry can be under.
    /// </summary>
    /// <seealso cref="EGameListStatus"/>
    public sealed class GameListStatusTypeEntity : EnumEntity<EGameListStatus, GameListStatusTypeEntity, byte> { }

    /// <summary>
    /// Models a manga age rating.
    /// </summary>
    /// <seealso cref="EMangaAgeRating"/>
    public sealed class MangaAgeRatingTypeEntity : EnumEntity<EMangaAgeRating, MangaAgeRatingTypeEntity, byte>
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
    public sealed class MangaCategoryTypeEntity : EnumEntity<EMangaCategory, MangaCategoryTypeEntity, byte> { }

    /// <summary>
    /// Models a status that a manga list entry can be under.
    /// </summary>
    /// <seealso cref="EMangaListStatus"/>
    public sealed class MangaListStatusTypeEntity : EnumEntity<EMangaListStatus, MangaListStatusTypeEntity, byte> { }

    /// <summary>
    /// Models how two pieces of media can be related to each other.
    /// </summary>
    /// <seealso cref="EMediaRelationType"/>
    public sealed class MediaRelationTypeEntity : EnumEntity<EMediaRelationType, MediaRelationTypeEntity, byte> { }

    /// <summary>
    /// Models the production status of a piece of media.
    /// </summary>
    /// <seealso cref="EMediaStatus"/>
    public sealed class MediaStatusTypeEntity : EnumEntity<EMediaStatus, MediaStatusTypeEntity, byte> { }

    /// <summary>
    /// Models a role that a person plays in the production of a piece of media.
    /// </summary>
    /// <seealso cref="EPersonRole"/>
    public sealed class PersonRoleTypeEntity : EnumEntity<EPersonRole, PersonRoleTypeEntity, byte> { }
}

namespace AniNexus.Domain
{
    using AniNexus.Domain.Entities;

    public partial class ApplicationDbContext
    {
        /// <summary>
        /// A collection of known age ratings that can be applied to anime.
        /// </summary>
        public DbSet<AnimeAgeRatingTypeEntity> AnimeAgeRatings => Set<AnimeAgeRatingTypeEntity>();

        /// <summary>
        /// A collection of categories that an anime can fall under.
        /// </summary>
        public DbSet<AnimeCategoryTypeEntity> AnimeCategoryTypes => Set<AnimeCategoryTypeEntity>();

        /// <summary>
        /// A collection of categories an anime list entry can fall under.
        /// </summary>
        public DbSet<AnimeListStatusTypeEntity> AnimeListStatusTypes => Set<AnimeListStatusTypeEntity>();

        /// <summary>
        /// A collection of anime seasons.
        /// </summary>
        public DbSet<AnimeSeasonTypeEntity> AnimeSeasonTypes => Set<AnimeSeasonTypeEntity>();

        /// <summary>
        /// A collection of roles a character can play in a piece of media.
        /// </summary>
        public DbSet<CharacterRoleTypeEntity> CharacterRoleTypes => Set<CharacterRoleTypeEntity>();

        /// <summary>
        /// A collection of roles a company can play in the production of a piece of media.
        /// </summary>
        public DbSet<CompanyRoleTypeEntity> CompanyRoleTypes => Set<CompanyRoleTypeEntity>();

        /// <summary>
        /// A collection of company types.
        /// </summary>
        public DbSet<CompanyTypeEntity> CompanyTypes => Set<CompanyTypeEntity>();

        /// <summary>
        /// A collection of categories that a game can fall under.
        /// </summary>
        public DbSet<GameCategoryTypeEntity> GameCategoryTypes => Set<GameCategoryTypeEntity>();

        /// <summary>
        /// A collection of categories a game list entry can fall under.
        /// </summary>
        public DbSet<GameListStatusTypeEntity> GameListStatusTypes => Set<GameListStatusTypeEntity>();

        /// <summary>
        /// A collection of known age ratings that can be applied to manga.
        /// </summary>
        public DbSet<MangaAgeRatingTypeEntity> MangaAgeRatings => Set<MangaAgeRatingTypeEntity>();

        /// <summary>
        /// A collection of categories that a manga can fall under.
        /// </summary>
        public DbSet<MangaCategoryTypeEntity> MangaCategoryTypes => Set<MangaCategoryTypeEntity>();

        /// <summary>
        /// A collection of categories a manga list entry can fall under.
        /// </summary>
        public DbSet<MangaListStatusTypeEntity> MangaListStatusTypes => Set<MangaListStatusTypeEntity>();

        /// <summary>
        /// A collection of relationship types between two pieces of media.
        /// </summary>
        public DbSet<MediaRelationTypeEntity> MediaRelationTypes => Set<MediaRelationTypeEntity>();

        /// <summary>
        /// A collection of statuses that a piece of media can be in with regards to production.
        /// </summary>
        public DbSet<MediaStatusTypeEntity> MediaStatusTypes => Set<MediaStatusTypeEntity>();

        /// <summary>
        /// A collection of roles that a person can play in the production of a piece of media.
        /// </summary>
        public DbSet<PersonRoleTypeEntity> PersonRoleTypes => Set<PersonRoleTypeEntity>();
    }
}
