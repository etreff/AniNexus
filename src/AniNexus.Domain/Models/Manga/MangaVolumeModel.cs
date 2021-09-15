using System.ComponentModel.DataAnnotations;
using AniNexus.Domain.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a manga volume, book, or collection of sectioned pieces of literature.
/// </summary>
public class MangaVolumeModel : IHasAudit, IHasRowVersion, IHasSoftDelete, IEntityTypeConfiguration<MangaVolumeModel>, IValidatableObject
{
    /// <summary>
    /// The Id of the volume.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// The release the volume is associated with.
    /// </summary>
    /// <seealso cref="MangaReleaseModel"/>
    public int ReleaseId { get; set; }

    /// <summary>
    /// The volume number.
    /// </summary>
    public int VolumeNumber { get; set; }

    /// <summary>
    /// The date the volume was published.
    /// </summary>
    public Date? ReleaseDate { get; set; }

    /// <summary>
    /// The number of pages in this volume.
    /// </summary>
    public int? PageCount { get; set; }

    /// <summary>
    /// A synopsis of the volume.
    /// </summary>
    public string? Synopsis { get; set; }

    /// <summary>
    /// A URL to a place where the user can legally read the volume.
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

    /// <summary>
    /// Whether this entry is soft-deleted. It will not be included in queries unless
    /// <see cref="M:Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.IgnoreQueryFilters``1(System.Linq.IQueryable{``0})" />
    /// is invoked.
    /// </summary>
    public bool IsSoftDeleted { get; set; }
    #endregion

    #region Navigation Properties
    /// <summary>
    /// The release this volume belongs to.
    /// </summary>
    public MangaReleaseModel Release { get; set; } = default!;

    /// <summary>
    /// The name of the volume.
    /// </summary>
    public NameModel? Name { get; set; } = default!;

    /// <summary>
    /// Models a name of a manga volume.
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

        /// <summary>
        /// Whether the name is the primary name.
        /// </summary>
        public bool IsPrimary { get; set; }
    }

    /// <summary>
    /// The chapters contained in this volume.
    /// </summary>
    public IList<MangaChapterModel> Chapters { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MangaVolumeModel> builder)
    {
        builder.ToTable("MangaVolume");

        builder.HasKey(m => m.Id);
        builder.HasIndex(m => m.ReleaseId);
        builder.HasIndex(m => new { m.ReleaseId, m.VolumeNumber }).IsUnique();

        builder.HasOne(m => m.Release).WithMany(m => m.Volumes).HasForeignKey(m => m.ReleaseId).IsRequired().OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(m => m.Name, name =>
        {
            name.ToTable("MangaVolumeName");

            name.Property(m => m.NativeName).HasComment("The native name.").HasColumnName(nameof(NameModel.NativeName));
            name.Property(m => m.RomajiName).HasComment("The romanization of the native name.").HasColumnName(nameof(NameModel.RomajiName));
            name.Property(m => m.EnglishName).HasComment("The name in English.").HasColumnName(nameof(NameModel.EnglishName));
            name.Property(m => m.IsPrimary).HasComment("Whether this name is the primary name of the release.").HasColumnName(nameof(NameModel.IsPrimary));
        });

        builder.Property(m => m.VolumeNumber).HasComment("The volume number in the release.");
        builder.Property(m => m.PageCount).HasComment("The number of pages the volume has.");
        builder.Property(m => m.ReleaseDate).HasComment("The date this volume released.");
        builder.Property(m => m.Synopsis).HasComment("The volume synopsis.");
        builder.Property(m => m.ReadUrl).HasComment("A URL to a place where the user can legally read the volume.");
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (VolumeNumber < 0)
        {
            yield return new ValidationResult("The volume number must be greater than or equal to 0.", new[] { nameof(VolumeNumber) });
        }

        if (PageCount.HasValue && PageCount > 0)
        {
            yield return new ValidationResult("The page count must be greater than or equal to 1.", new[] { nameof(PageCount) });
        }

        if (!UriValidator.Validate(ReadUrl, nameof(ReadUrl), out var urlValidationResult))
        {
            yield return urlValidationResult;
        }
    }
}
