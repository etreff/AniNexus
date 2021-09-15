using AniNexus.Domain.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a chapter or section of a piece of literature.
/// </summary>
public class MangaChapterModel : IHasAudit, IHasRowVersion, IEntityTypeConfiguration<MangaChapterModel>, IValidatableObject
{
    /// <summary>
    /// The Id of the chapter.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// The release the chapter is associated with.
    /// </summary>
    /// <seealso cref="MangaReleaseModel"/>
    public int ReleaseId { get; set; }

    /// <summary>
    /// The volume the chapter is associated with.
    /// </summary>
    /// <seealso cref="MangaVolumeModel"/>
    public long? VolumeId { get; set; }

    /// <summary>
    /// The chapter number.
    /// </summary>
    public int ChapterNumber { get; set; }

    /// <summary>
    /// The date the chapter was published.
    /// </summary>
    public Date? ReleaseDate { get; set; }

    /// <summary>
    /// The number of pages in this chapter.
    /// </summary>
    public int? PageCount { get; set; }

    /// <summary>
    /// A synopsis of the chapter.
    /// </summary>
    public string? Synopsis { get; set; }

    /// <summary>
    /// A URL to a place where the user can legally read the chapter.
    /// </summary>
    public string? ReadUrl { get; set; }

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

    #region Navigation Properties
    /// <summary>
    /// The release this chapter belongs to.
    /// </summary>
    public MangaReleaseModel Release { get; set; } = default!;
    /// <summary>
    /// The release this chapter belongs to.
    /// </summary>
    public MangaVolumeModel? Volume { get; set; } = default!;

    /// <summary>
    /// The name of the chapter.
    /// </summary>
    public NameModel? Name { get; set; } = default!;

    /// <summary>
    /// Models a name of a manga chapter.
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
        /// The name in English.
        /// </summary>
        public string? EnglishName { get; set; }
    }
    #endregion

    public void Configure(EntityTypeBuilder<MangaChapterModel> builder)
    {
        builder.ToTable("MangaChapter");

        builder.HasKey(m => m.Id);
        builder.HasIndex(m => m.ReleaseId);
        builder.HasIndex(m => m.VolumeId).HasFilter("[VolumeId] IS NOT NULL");
        builder.HasIndex(m => new { m.ReleaseId, m.ChapterNumber }).IsUnique();

        builder.HasOne(m => m.Volume).WithMany(m => m.Chapters).HasForeignKey(m => m.VolumeId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(m => m.Release).WithMany(m => m.Chapters).HasForeignKey(m => m.ReleaseId).IsRequired().OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(m => m.Name, name =>
        {
            name.ToTable("MangaChapterName");

            name.Property(m => m.NativeName).HasComment("The native name.").HasColumnName(nameof(NameModel.NativeName));
            name.Property(m => m.RomajiName).HasComment("The romanization of the native name.").HasColumnName(nameof(NameModel.RomajiName));
            name.Property(m => m.EnglishName).HasComment("The name in English.").HasColumnName(nameof(NameModel.EnglishName));
        });

        builder.Property(m => m.ChapterNumber).HasComment("The chapter number in the release.");
        builder.Property(m => m.PageCount).HasComment("The number of pages the chapter has.");
        builder.Property(m => m.ReleaseDate).HasComment("The date this chapter released.");
        builder.Property(m => m.Synopsis).HasComment("The chapter synopsis.");
        builder.Property(m => m.ReadUrl).HasComment("A URL to a place where the user can legally read the chapter.");
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ChapterNumber < 0)
        {
            yield return new ValidationResult("The chapter number must be greater than or equal to 0.", new[] { nameof(ChapterNumber) });
        }

        if (PageCount.HasValue && PageCount < 1)
        {
            yield return new ValidationResult("The page count must be greater than or equal to 1.", new[] { nameof(PageCount) });
        }

        if (!UriValidator.Validate(ReadUrl, nameof(ReadUrl), out var uriValidationResult))
        {
            yield return uriValidationResult;
        }
    }
}
