using System.ComponentModel.DataAnnotations;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a character that appears in a piece of media. These characters are usually
/// fictional.
/// </summary>
public class MediaCharacterModel : IHasAudit, IHasRowVersion, IHasSoftDelete, IEntityTypeConfiguration<MediaCharacterModel>, IValidatableObject
{
    /// <summary>
    /// The Id of the entry.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The date of birth of this character.
    /// </summary>
    public DateOnly? DateOfBirth { get; set; }

    /// <summary>
    /// The height of the character, in centimeters.
    /// </summary>
    public int? Height { get; set; }

    /// <summary>
    /// The age of the character, in months.
    /// </summary>
    public int? Age { get; set; }

    /// <summary>
    /// A description of the character.
    /// </summary>
    public string? Description { get; set; }

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
    /// The names of the character.
    /// </summary>
    public IList<NameModel> Names { get; set; } = default!;

    /// <summary>
    /// Models a character name.
    /// </summary>
    public class NameModel
    {
        /// <summary>
        /// The full name in the native language.
        /// </summary>
        public string NativeName { get; set; } = default!;

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
        /// Whether the name is a spoiler.
        /// </summary>
        public bool IsSpoiler { get; set; }

        /// <summary>
        /// Whether the name is the primary name.
        /// </summary>
        public bool IsPrimary { get; set; }
    }

    /// <summary>
    /// The anime that the character appears in.
    /// </summary>
    public IList<AnimeCharacterMapModel> Anime { get; set; } = default!;

    /// <summary>
    /// The games that the character appears in.
    /// </summary>
    public IList<GameCharacterMapModel> Games { get; set; } = default!;

    /// <summary>
    /// The manga that the character appears in.
    /// </summary>
    public IList<MangaCharacterMapModel> Manga { get; set; } = default!;

    /// <summary>
    /// The voice actors who voice this character.
    /// </summary>
    public IList<MediaCharacterVoiceActorMapModel> VoiceActors { get; set; } = default!;

    /// <summary>
    /// The live-action actors who portray this character.
    /// </summary>
    public IList<MediaCharacterActorMapModel> LiveActors { get; set; } = default!;
    #endregion

    #region Helper Properties
    /// <summary>
    /// The age of the character, in years.
    /// </summary>
    public int? AgeYears => Age != null ? Age / 12 : null;
    #endregion

    public void Configure(EntityTypeBuilder<MediaCharacterModel> builder)
    {
        builder.ToTable("Character");

        builder.HasKey(m => m.Id);
        builder.HasIndex(m => m.DateOfBirth).HasFilter("[DOB] IS NOT NULL");
        builder.HasIndex(m => m.Age).HasFilter("[Age] IS NOT NULL");

        builder.OwnsMany(m => m.Names, name =>
        {
            name.ToTable("CharacterName");

            name.Property(m => m.NativeName).HasComment("The native name of the character.");
            name.Property(m => m.RomajiName).HasComment("The romanization of the native name.");
            name.Property(m => m.RomajiFirstName).HasComment("The romanization of the first name.");
            name.Property(m => m.RomajiMiddleName).HasComment("The romanization of the middle name.");
            name.Property(m => m.RomajiLastName).HasComment("The romanization of the last name.");
            name.Property(m => m.IsSpoiler).HasComment("Whether this name is a spoiler.").HasDefaultValue(false);
        });

        builder.Property(m => m.DateOfBirth)
            .HasComment("The character's date of birth")
            .HasColumnName("DOB")
            .IsRequired(false);
        builder.Property(m => m.Height)
            .HasComment("The height of the character, in centimeters.")
            .IsRequired(false);
        builder.Property(m => m.Age)
            .HasComment("The age of the character, in months.")
            .IsRequired(false);
        builder.Property(m => m.Description)
            .HasComment("A description of the character.")
            .IsRequired(false)
            .UseCollation(Collation.Japanese);
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Height.HasValue && Height < 0)
        {
            yield return new ValidationResult("Height must be greater than or equal to 0.", new[] { nameof(Height) });
        }

        if (Age.HasValue && Age < 0)
        {
            yield return new ValidationResult("Age must be greater than or equal to 0.", new[] { nameof(Age) });
        }
    }
}
