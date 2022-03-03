namespace AniNexus.Data.Entities;

/// <summary>
/// Models a real person.
/// </summary>
public class PersonEntity : AuditableEntity<PersonEntity>, IHasRowVersion, IHasSoftDelete, IHasPublicId
{
    /// <summary>
    /// The name of the person.
    /// </summary>
    public NameEntity Name { get; set; } = default!;

    /// <summary>
    /// The public Id of the person.
    /// </summary>
    public int PublicId { get; set; }

    /// <summary>
    /// The date of birth of the person.
    /// </summary>
    public DateOnly? DateOfBirth { get; set; }

    /// <summary>
    /// The city the person was born.
    /// </summary>
    public string? BirthCity { get; set; }

    /// <summary>
    /// The state the person was born.
    /// </summary>
    public string? BirthState { get; set; }

    /// <summary>
    /// The city the person was born.
    /// </summary>
    public string? BirthCountry { get; set; }

    /// <summary>
    /// The height of the character, in centimeters.
    /// </summary>
    public int? Height { get; set; }

    /// <summary>
    /// A description of the person.
    /// </summary>
    public string? Description { get; set; }

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

    /// <summary>
    /// The names this person has.
    /// </summary>
    public IList<NameEntity> Aliases { get; set; } = default!;

    /// <summary>
    /// The roles this person has played in the creation of an anime release.
    /// </summary>
    public IList<AnimeReleasePersonMapEntity> AnimeRoles { get; set; } = default!;

    /// <summary>
    /// The characters this person has voice acted.
    /// </summary>
    public IList<PersonVoiceActorMapEntity> VoiceActedCharacters { get; set; } = default!;

    /// <summary>
    /// The characters this person has played in a live action adaptation.
    /// </summary>
    public IList<PersonLiveActorMapEntity> LiveActedCharacters { get; set; } = default!;

    /// <summary>
    /// Albums that this person is associated with.
    /// </summary>
    public IList<PersonAlbumMapEntity> Albums { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<PersonEntity> builder)
    {
        base.ConfigureEntity(builder);

        // 1. Index specification
        // 2. Navigation properties
        builder.OwnsOne(m => m.Name, static owned => owned.ConfigureOwnedEntity());
        builder.OwnsMany(m => m.Aliases, static owned => owned.ConfigureOwnedEntity());
        // 3. Propery specification
        builder.Property(m => m.DateOfBirth).HasColumnName("DOB").HasComment("The date of birth.");
        builder.Property(m => m.BirthCity).HasComment("The romanized name of the city the person was born in.");
        builder.Property(m => m.BirthState).HasComment("The romanized name of the state the person was born in.");
        builder.Property(m => m.BirthCountry).HasComment("The romanized name of the country the person was born in.");
        builder.Property(m => m.Description).HasMaxLength(2500).HasComment("A description of the person.");
        builder.Property(m => m.Height).HasComment("The height of the person, in centimeters.");
        // 4. Other
    }
}
