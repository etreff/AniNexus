using System.ComponentModel.DataAnnotations;
using AniNexus.Domain.Validation;

namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a character that appears in a piece of media.
/// </summary>
public class CharacterEntity : AuditableEntity<CharacterEntity>, IHasRowVersion, IHasSoftDelete, IHasPublicId
{
    /// <summary>
    /// The public Id of the entity, used for navigation URLs.
    /// </summary>
    public int PublicId { get; set; }

    /// <summary>
    /// The name of the character.
    /// </summary>
    public NameEntity Name { get; set; } = default!;

    /// <summary>
    /// The date of birth of this character.
    /// </summary>
    public DateOnly? DateOfBirth { get; set; }

    /// <summary>
    /// The age of the character, in months.
    /// </summary>
    /// <remarks>
    /// The age of a character may change through the releases they appear in. The age provided
    /// here should either be the age they spend the most time at, the age they are primary
    /// associated with, or the first age they appear as. If this data varies wildly then
    /// this value should be left <see langword="null"/>.
    /// </remarks>
    public int? Age { get; set; }

    /// <summary>
    /// The height of the character, in centimeters.
    /// </summary>
    public int? Height { get; set; }

    /// <summary>
    /// A description of the character.
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
    /// The anime that the character appears in.
    /// </summary>
    public IList<AnimeCharacterMapEntity> Anime { get; set; } = default!;

    /// <summary>
    /// The voice actors who voice this character.
    /// </summary>
    public IList<PersonVoiceActorMapEntity> VoiceActors { get; set; } = default!;

    /// <summary>
    /// The live-action actors who portray this character.
    /// </summary>
    public IList<PersonLiveActorMapEntity> LiveActors { get; set; } = default!;

    /// <summary>
    /// The age of the character, in years.
    /// </summary>
    public int? AgeYears => Age != null ? Age / 12 : null;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<CharacterEntity> builder)
    {
        base.ConfigureEntity(builder);

        // 1. Index specification
        // 2. Navigation properties
        builder.OwnsOne(m => m.Name, static owned => owned.ConfigureOwnedEntity());
        // 3. Propery specification
        builder.Property(m => m.Age).HasComment("The age of the character, in months.");
        builder.Property(m => m.DateOfBirth).HasColumnName("DOB").HasComment("The date of birth.");
        builder.Property(m => m.Description).HasMaxLength(2500).HasComment("A description of the characters.");
        builder.Property(m => m.Height).HasComment("The height of the person, in centimeters.");
        // 4. Other
    }

    /// <inheritdoc/>
    protected override void Validate(ValidationContext validationContext, ValidationBuilder<CharacterEntity> validator)
    {
        base.Validate(validationContext, validator);

        validator.Property(m => m.Age).IsGreaterThanOrEqualTo(0);
        validator.Property(m => m.Height).IsGreaterThanOrEqualTo(0);
        validator.Property(m => m.Description).HasLengthLessThanOrEqualTo(1250);
    }
}
