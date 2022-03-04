using System.Globalization;

namespace AniNexus.Data.Entities;

/// <summary>
/// Models a language.
/// </summary>
public sealed class LanguageEntity : Entity<LanguageEntity, short>
{
    /// <summary>
    /// The English name of the language.
    /// </summary>
    public string EnglishName { get; set; } = default!;

    /// <summary>
    /// The languagecode2-country/regioncode2 code of the language.
    /// </summary>
    /// <remarks>
    /// Pass this value into <see cref="CultureInfo.GetCultureInfo(string)"/> to get a
    /// <see cref="CultureInfo"/> instance representing this language.
    /// </remarks>
    public string Code { get; set; } = default!;

    /// <summary>
    /// The three letter ISO code of the language.
    /// </summary>
    public string ThreeLetterISOCode { get; set; } = default!;

    /// <summary>
    /// The two letter ISO code of the language.
    /// </summary>
    public string TwoLetterISOCode { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<LanguageEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => m.ThreeLetterISOCode);
        builder.HasIndex(m => m.TwoLetterISOCode);
        // 2. Navigation properties
        // 3. Propery specification
        builder.Property(m => m.EnglishName).HasComment("The English name of the language.");
        builder.Property(m => m.Code).HasComment("The languagecode2-country/regioncode2 code of the language.");
        builder.Property(m => m.ThreeLetterISOCode).IsFixedLength(3).HasComment("The three letter ISO code of the language.");
        builder.Property(m => m.TwoLetterISOCode).IsFixedLength(2).HasComment("The two letter ISO code of the language.");
        // 4. Other
    }

    /// <inheritdoc/>
    protected override IEnumerable<LanguageEntity> GetSeedData()
    {
        return CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(c => new LanguageEntity
        {
            EnglishName = c.EnglishName,
            Code = c.Name,
            ThreeLetterISOCode = c.ThreeLetterISOLanguageName,
            TwoLetterISOCode = c.TwoLetterISOLanguageName
        }).OrderBy(c => c.ThreeLetterISOCode).ThenBy(c => c.EnglishName);
    }
}
