namespace AniNexus.DataAccess.Entities
{
    /// <summary>
    /// Models a category that an anime falls under.
    /// </summary>
    /// <seealso cref="EAnimeCategory"/>
    public sealed class AnimeCategoryTypeEntity : EnumEntity<AnimeCategoryTypeEntity, EAnimeCategory> { }

    /// <summary>
    /// Models a status that an anime list entry can be under.
    /// </summary>
    /// <seealso cref="EAnimeListStatus"/>
    public sealed class AnimeListStatusTypeEntity : EnumEntity<AnimeListStatusTypeEntity, EAnimeListStatus> { }

    /// <summary>
    /// Models an anime season.
    /// </summary>
    /// <seealso cref="EAnimeSeason"/>
    public sealed class AnimeSeasonTypeEntity : EnumEntity<AnimeSeasonTypeEntity, EAnimeSeason> { }

    /// <summary>
    /// Models a role that a character plays in a piece of media.
    /// </summary>
    /// <seealso cref="ECharacterRole"/>
    public sealed class CharacterRoleTypeEntity : EnumEntity<CharacterRoleTypeEntity, ECharacterRole> { }

    /// <summary>
    /// Models a role a company plays in the production of a piece of media.
    /// </summary>
    /// <seealso cref="ECompanyRole"/>
    public sealed class CompanyRoleTypeEntity : EnumEntity<CompanyRoleTypeEntity, ECompanyRole> { }

    /// <summary>
    /// Models a company type.
    /// </summary>
    /// <seealso cref="ECompanyType"/>
    public sealed class CompanyTypeEntity : EnumEntity<CompanyTypeEntity, ECompanyType> { }

    /// <summary>
    /// Models a category that a manga falls under.
    /// </summary>
    /// <seealso cref="EMangaCategory"/>
    public sealed class MangaCategoryTypeEntity : EnumEntity<MangaCategoryTypeEntity, EMangaCategory> { }

    /// <summary>
    /// Models a status that a manga list entry can be under.
    /// </summary>
    /// <seealso cref="EMangaListStatus"/>
    public sealed class MangaListStatusTypeEntity : EnumEntity<MangaListStatusTypeEntity, EMangaListStatus> { }

    /// <summary>
    /// Models how two pieces of media can be related to each other.
    /// </summary>
    /// <seealso cref="EMediaRelationType"/>
    public sealed class MediaRelationTypeEntity : EnumEntity<MediaRelationTypeEntity, EMediaRelationType> { }

    /// <summary>
    /// Models the production status of a piece of media.
    /// </summary>
    /// <seealso cref="EMediaStatus"/>
    public sealed class MediaStatusTypeEntity : EnumEntity<MediaStatusTypeEntity, EMediaStatus> { }

    /// <summary>
    /// Models a role that a person plays in the production of a piece of media.
    /// </summary>
    /// <seealso cref="EPersonRole"/>
    public sealed class PersonRoleTypeEntity : EnumEntity<PersonRoleTypeEntity, EPersonRole> { }
}

namespace AniNexus.DataAccess
{
    using AniNexus.DataAccess.Entities;
    using Microsoft.EntityFrameworkCore;

    public partial class AniNexusDbContext
    {
        /// <summary>
        /// A collection of categories that an anime can fall under.
        /// </summary>
        public DbSet<AnimeCategoryTypeEntity> AnimeCategoryTypes { get; set; }

        /// <summary>
        /// A collection of categories an anime list entry can fall under.
        /// </summary>
        public DbSet<AnimeListStatusTypeEntity> AnimeListStatusTypes { get; set; }

        /// <summary>
        /// A collection of anime seasons.
        /// </summary>
        public DbSet<AnimeSeasonTypeEntity> AnimeSeasonTypes { get; set; }

        /// <summary>
        /// A collection of roles a character can play in a piece of media.
        /// </summary>
        public DbSet<CharacterRoleTypeEntity> CharacterRoleTypes { get; set; }

        /// <summary>
        /// A collection of roles a company can play in the production of a piece of media.
        /// </summary>
        public DbSet<CompanyRoleTypeEntity> CompanyRoleTypes { get; set; }

        /// <summary>
        /// A collection of company types.
        /// </summary>
        public DbSet<CompanyTypeEntity> CompanyTypes { get; set; }

        /// <summary>
        /// A collection of categories that a manga can fall under.
        /// </summary>
        public DbSet<MangaCategoryTypeEntity> MangaCategoryTypes { get; set; }

        /// <summary>
        /// A collection of categories a manga list entry can fall under.
        /// </summary>
        public DbSet<MangaListStatusTypeEntity> MangaListStatusTypes { get; set; }

        /// <summary>
        /// A collection of relationship types between two pieces of media.
        /// </summary>
        public DbSet<MediaRelationTypeEntity> MediaRelationTypes { get; set; }

        /// <summary>
        /// A collection of statuses that a piece of media can be in with regards to production.
        /// </summary>
        public DbSet<MediaStatusTypeEntity> MediaStatusTypes { get; set; }

        /// <summary>
        /// A collection of roles that a person can play in the production of a piece of media.
        /// </summary>
        public DbSet<PersonRoleTypeEntity> PersonRoleTypes { get; set; }
    }
}
