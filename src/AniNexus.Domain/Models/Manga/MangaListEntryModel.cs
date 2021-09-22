using System.ComponentModel.DataAnnotations;
using AniNexus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a entry in a user's manga list.
/// </summary>
/// <remarks>
/// This is used to track whether a manga can be added to a user's manga list.
/// </remarks>
public class MangaListEntryModel : IEntityTypeConfiguration<MangaListEntryModel>, IValidatableObject
{
    /// <summary>
    /// The user that owns this list entry.
    /// </summary>
    /// <seealso cref="UserModel"/>
    public Guid UserId { get; set; } = default!;

    /// <summary>
    /// The Id of the manga this entry refers to.
    /// </summary>
    /// <seealso cref="MangaModel"/>
    public int MangaId { get; set; }

    /// <summary>
    /// The Id of the status that describes the state the user has put this manga in.
    /// </summary>
    /// <seealso cref="EMangaListStatus"/>
    /// <seealso cref="MangaListStatusTypeModel"/>
    public int StatusId { get; set; }

    /// <summary>
    /// The number of volumes of this manga the user has read.
    /// </summary>
    /// <remarks>
    /// This number is capped at the shows native release volume count, not the release
    /// available in the user's locale.
    /// </remarks>
    public int VolumeCount { get; set; }

    /// <summary>
    /// The number of chapters of this manga the user has read.
    /// </summary>
    /// <remarks>
    /// This number is capped at the shows native release chapter count, not the release
    /// available in the user's locale.
    /// </remarks>
    public int ChapterCount { get; set; }

    /// <summary>
    /// The rating the user gives the manga, from 0 to 100.
    /// </summary>
    public byte? Rating { get; set; }

    /// <summary>
    /// A comment the user has left about this manga.
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// The number of times the user has reread the manga.
    /// </summary>
    public int RereadCount { get; set; }

    /// <summary>
    /// The date on which the user first started reading the manga.
    /// </summary>
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// The date on which the user finished reading the manga.
    /// </summary>
    public DateOnly? EndDate { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The manga this list entry refers to.
    /// </summary>
    public MangaModel Manga { get; set; } = default!;

    /// <summary>
    /// The status this user has put this manga in.
    /// </summary>
    public MangaListStatusTypeModel Status { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MangaListEntryModel> builder)
    {
        builder.ToTable("MangaListEntry");

        builder.HasKey(m => new { m.UserId, m.MangaId });
        builder.HasIndex(m => m.MangaId);
        builder.HasIndex(m => m.StatusId);

        builder.HasOne(m => m.Manga).WithMany().HasForeignKey(m => m.MangaId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<UserModel>().WithMany().HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Status).WithMany().HasForeignKey(m => m.StatusId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - when polling for a manga list entry, the name of the manga and the status name
        // will almost always need to come with it.
        builder.Navigation(m => m.Manga).AutoInclude();
        builder.Navigation(m => m.Status).AutoInclude();

        builder.Property(m => m.VolumeCount).HasComment("The number of volumes the user has read.").HasDefaultValue(0);
        builder.Property(m => m.ChapterCount).HasComment("The number of chapters the user has read.").HasDefaultValue(0);
        builder.Property(m => m.Rating).HasComment("The rating this user gives the manga, from 0 to 100.");
        builder.Property(m => m.Comment).HasComment("Comments the user has about the manga.").HasMaxLength(250);
        builder.Property(m => m.RereadCount).HasComment("The number of times the user has reread the manga.").HasDefaultValue(0);
        builder.Property(m => m.StartDate).HasComment("The date the user first started reading the manga.");
        builder.Property(m => m.EndDate).HasComment("The date the user finished reading the manga.");
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (VolumeCount < 0)
        {
            yield return new ValidationResult("The number of volumes a user has read must be greater than " +
                                              "or equal to 0.", new[] { nameof(VolumeCount) });
        }
        if (ChapterCount < 0)
        {
            yield return new ValidationResult("The number of chapters a user has read must be greater than " +
                                              "or equal to 0.", new[] { nameof(ChapterCount) });
        }
        if (RereadCount < 0)
        {
            yield return new ValidationResult("The reread count must be greater than or equal to 0.", new[] { nameof(RereadCount) });
        }

        var mangaPrimaryRelease = Manga?.GetPrimaryRelease();
        if (mangaPrimaryRelease is not null)
        {
            if (mangaPrimaryRelease.VolumeCount is not null && VolumeCount > mangaPrimaryRelease.VolumeCount)
            {
                yield return new ValidationResult("The number of volumes a user has read cannot exceed " +
                                                  "the number of volumes available in the primary release.",
                                                  new[] { nameof(VolumeCount) });
            }
            if (mangaPrimaryRelease.ChapterCount is not null && ChapterCount > mangaPrimaryRelease.ChapterCount)
            {
                yield return new ValidationResult("The number of chapters a user has read cannot exceed " +
                                                  "the number of chapters available in the primary release.",
                                                  new[] { nameof(ChapterCount) });
            }
        }

        if (Rating is not null && (Rating < 0 || Rating > 100))
        {
            yield return new ValidationResult("The rating must be between 0 and 100, or null.", new[] { nameof(Rating) });
        }

        if (StartDate.HasValue && EndDate.HasValue && StartDate > EndDate)
        {
            yield return new ValidationResult("The start date must be after the end date.", new[] { nameof(StartDate), nameof(EndDate) });
        }
    }
}
