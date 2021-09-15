using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a language or locale.
/// </summary>
public class LocaleModel : IHasAudit, IHasRowVersion, IEntityTypeConfiguration<LocaleModel>
{
    /// <summary>
    /// The Id of the locale.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The friendly name of the locale.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// The i18n language code of the locale.
    /// </summary>
    public string LanguageCode { get; set; } = default!;

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
    #endregion

    public void Configure(EntityTypeBuilder<LocaleModel> builder)
    {
        builder.ToTable("Locale");

        builder.HasKey(m => m.Id);
        builder.HasIndex(m => m.LanguageCode).IsUnique();

        builder.HasData(
            new LocaleModel { Id = 1, Name = "Japanese", LanguageCode = "ja_JP" },
            new LocaleModel { Id = 2, Name = "English", LanguageCode = "en_US" });

        builder.Property(m => m.Name).HasComment("The name of the locale.").HasMaxLength(64);
        builder.Property(m => m.LanguageCode).HasComment("The i18n language code of the locale.")
            // https://www.w3.org/Protocols/rfc2616/rfc2616-sec3.html#sec3.10
            .HasMaxLength(17).IsFixedLength();
    }
}
