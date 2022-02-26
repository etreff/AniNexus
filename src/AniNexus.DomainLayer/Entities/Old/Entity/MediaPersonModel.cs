namespace AniNexus.Domain.Models;

/// <summary>
/// Models a real person.
/// </summary>
public class MediaPersonModel : IHasAudit, IHasRowVersion, IHasSoftDelete, IEntityTypeConfiguration<MediaPersonModel>
{
    /// <summary>
    /// The Id of the person.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The date of birth of the person.
    /// </summary>
    public DateOnly? DateOfBirth { get; set; }

    /// <summary>
    /// A description of the person.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The place the person was born.
    /// </summary>
    public string? BirthPlace { get; set; }

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
    /// The names this person has.
    /// </summary>
    public IList<NameModel> Names { get; set; } = default!;

    /// <summary>
    /// Models a name of a person.
    /// </summary>
    public class NameModel
    {
        /// <summary>
        /// The full name in the native language.
        /// </summary>
        public string? NativeName { get; set; }

        /// <summary>
        /// The romanization of the native name.
        /// </summary>
        public string? RomajiName { get; set; }

        /// <summary>
        /// The romanized first name.
        /// </summary>
        public string? RomajiFirstName { get; set; }

        /// <summary>
        /// The romanized middle name.
        /// </summary>
        public string? RomajiMiddleName { get; set; }

        /// <summary>
        /// The romanized last name.
        /// </summary>
        public string? RomajiLastName { get; set; }

        /// <summary>
        /// Whether the name is the primary name.
        /// </summary>
        public bool IsPrimary { get; set; }
    }

    /// <summary>
    /// Roles this person has played in the creation of an anime.
    /// </summary>
    public IList<AnimePersonRoleMapModel> AnimeRoles { get; set; } = default!;

    /// <summary>
    /// Roles this person has played in the creation of a game.
    /// </summary>
    public IList<GamePersonRoleMapModel> GameRoles { get; set; } = default!;

    /// <summary>
    /// Roles this person has played in the creation of an anime.
    /// </summary>
    public IList<MangaPersonRoleMapModel> MangaRoles { get; set; } = default!;

#if SONGMODEL
    /// <summary>
    /// Songs that this person was involved in.
    /// </summary>
    public IList<PersonSongMapModel> Songs { get; set; } = default!;
#endif

    /// <summary>
    /// Characters that this person has voice acted.
    /// </summary>
    public IList<MediaCharacterVoiceActorMapModel> VoiceActedCharacters { get; set; } = default!;

    /// <summary>
    /// Characters that this person has live-acted.
    /// </summary>
    public IList<MediaCharacterActorMapModel> ActedCharacters { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MediaPersonModel> builder)
    {
        builder.ToTable("Person");

        builder.HasKey(m => m.Id);

        builder.OwnsMany(m => m.Names, name =>
        {
            name.ToTable("PersonName");

            name.Property(m => m.NativeName).HasComment("The native name.").HasColumnName(nameof(NameModel.NativeName));
            name.Property(m => m.RomajiName).HasComment("The romanization of the native name.").HasColumnName(nameof(NameModel.RomajiName));
            name.Property(m => m.RomajiFirstName).HasComment("The romanization of the first name.").HasColumnName(nameof(NameModel.RomajiFirstName));
            name.Property(m => m.RomajiMiddleName).HasComment("The romanization of the middle name.").HasColumnName(nameof(NameModel.RomajiMiddleName));
            name.Property(m => m.RomajiLastName).HasComment("The romanization of the last name.").HasColumnName(nameof(NameModel.RomajiLastName));
            name.Property(m => m.IsPrimary).HasComment("Whether this name is the primary name of the release.").HasColumnName(nameof(NameModel.IsPrimary));
        });

        builder.Property(m => m.DateOfBirth).HasComment("The person's date of birth.").HasColumnName("DOB");
        builder.Property(m => m.Description).HasComment("A description of the person").HasMaxLength(1000);
        builder.Property(m => m.BirthPlace).HasComment("The place this person was born.").HasMaxLength(128);
    }
}
