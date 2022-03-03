using AniNexus.Data.Validation;

namespace AniNexus.Data.Entities;

/// <summary>
/// Models a name of a person.
/// </summary>
public sealed class PersonNameEntity : OwnedEntity<PersonNameEntity>
{
    /// <summary>
    /// The first name in the person's native language.
    /// </summary>
    public string NativeFirstName { get; set; } = default!;

    /// <summary>
    /// The middle name in the person's native language.
    /// </summary>
    public string? NativeMiddleName { get; set; }

    /// <summary>
    /// The last name in the person's native language.
    /// </summary>
    public string? NativeLastName { get; set; }

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

    /// <inheritdoc/>
    protected override void ConfigureEntity<TOwnerEntity>(OwnedNavigationBuilder<TOwnerEntity, PersonNameEntity> builder)
    {
        builder.Property(m => m.NativeFirstName).HasComment("The native first name.").HasColumnName(nameof(NativeFirstName));
        builder.Property(m => m.NativeMiddleName).HasComment("The native middle name.").HasColumnName(nameof(NativeMiddleName));
        builder.Property(m => m.NativeLastName).HasComment("The name last name.").HasColumnName(nameof(NativeLastName));
        builder.Property(m => m.RomajiFirstName).HasComment("The romanized first name.").HasColumnName(nameof(RomajiFirstName));
        builder.Property(m => m.RomajiMiddleName).HasComment("The romanized middle name.").HasColumnName(nameof(RomajiMiddleName));
        builder.Property(m => m.RomajiLastName).HasComment("The romanized last name.").HasColumnName(nameof(RomajiLastName));
    }

    /// <inheritdoc/>
    public override void Validate(ValidationBuilder<PersonNameEntity> validator)
    {
        validator.Property(m => m.NativeFirstName).IsNotNullOrWhiteSpace();
        validator.Property(m => m.NativeMiddleName).IsNotNullOrWhiteSpace();
        validator.Property(m => m.NativeLastName).IsNotNullOrWhiteSpace();
        validator.Property(m => m.RomajiFirstName).IsNotNullOrWhiteSpace();
        validator.Property(m => m.RomajiMiddleName).IsNotNullOrWhiteSpace();
        validator.Property(m => m.RomajiLastName).IsNotNullOrWhiteSpace();
    }
}
