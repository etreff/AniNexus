namespace AniNexus.Domain.Models;

/// <summary>
/// Models a company. This includes producers, studios, circles, groups, and publishers.
/// </summary>
public class MediaCompanyModel : IHasAudit, IHasRowVersion, IHasSoftDelete, IEntityTypeConfiguration<MediaCompanyModel>
{
    /// <summary>
    /// The Id of the company.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The date this company was established or founded.
    /// </summary>
    public DateOnly? CreationDate { get; set; }

    /// <summary>
    /// The location this company was established or founded.
    /// </summary>
    public string? CreationLocation { get; set; }

    /// <summary>
    /// A description of this company.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Whether this company represents a circle.
    /// </summary>
    public bool IsCircle { get; set; }

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
    /// The name aliases of the company.
    /// </summary>
    public IList<NameModel> Names { get; set; } = default!;

    /// <summary>
    /// Models a name of a company.
    /// </summary>
    public class NameModel
    {
        /// <summary>
        /// The name in the native language.
        /// </summary>
        public string? NativeName { get; set; }

        /// <summary>
        /// The romanization of the native name.
        /// </summary>
        public string? RomajiName { get; set; }

        /// <summary>
        /// Whether the name is the primary name.
        /// </summary>
        public bool IsPrimary { get; set; }
    }

    /// <summary>
    /// Companies that are related to this company.
    /// </summary>
    public IList<MediaCompanyRelatedMapModel> RelatedCompanies { get; set; } = default!;

    /// <summary>
    /// Roles this company played in various anime.
    /// </summary>
    public IList<MediaCompanyAnimeMapModel> AnimeRoles { get; set; } = default!;

    /// <summary>
    /// Roles this company played in various games.
    /// </summary>
    public IList<MediaCompanyGameMapModel> GameRoles { get; set; } = default!;

    /// <summary>
    /// Roles this company played in various manga.
    /// </summary>
    public IList<MediaCompanyMangaMapModel> MangaRoles { get; set; } = default!;

#if SONGMODEL
    /// <summary>
    /// Roles this company played in various albums.
    /// </summary>
    public IList<MediaCompanyAlbumMapModel> AlbumRoles { get; set; } = default!;

    /// <summary>
    /// Roles this company played in various songs.
    /// </summary>
    public IList<MediaCompanySongMapModel> SongRoles { get; set; } = default!;
#endif
    #endregion

    #region Helper Properties
    /// <summary>
    /// The primary name of the company.
    /// </summary>
    public NameModel Name => Names.Single(static n => n.IsPrimary);
    #endregion

    public void Configure(EntityTypeBuilder<MediaCompanyModel> builder)
    {
        builder.ToTable("Company");

        builder.HasKey(m => m.Id);
        
        builder.OwnsMany(m => m.Names, name =>
        {
            name.ToTable("CompanyName");

            name.Property(m => m.NativeName).HasComment("The native name.").HasColumnName(nameof(NameModel.NativeName));
            name.Property(m => m.RomajiName).HasComment("The romanization of the native name.").HasColumnName(nameof(NameModel.RomajiName));
            name.Property(m => m.IsPrimary).HasComment("Whether this name is the primary name.").HasColumnName(nameof(NameModel.IsPrimary));
        });

        builder.Property(m => m.CreationDate).HasComment("The date this company was established or founded.").HasColumnName("DOB");
        builder.Property(m => m.CreationLocation).HasComment("The location the company was established or founded.");
        builder.Property(m => m.Description).HasComment("A description of the entity.").HasMaxLength(1250);
        builder.Property(m => m.IsCircle).HasComment("Whether the entity represents a circle.").HasDefaultValue(false);
    }
}
