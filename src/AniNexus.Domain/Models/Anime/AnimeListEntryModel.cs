using System.ComponentModel.DataAnnotations;
using AniNexus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models an entry in a user's anime list.
/// </summary>
public class AnimeListEntryModel : IEntityTypeConfiguration<AnimeListEntryModel>, IValidatableObject
{
    /// <summary>
    /// The user that owns this list entry.
    /// </summary>
    /// <seealso cref="UserModel"/>
    public Guid UserId { get; set; } = default!;

    /// <summary>
    /// The Id of the anime this entry refers to.
    /// </summary>
    /// <remarks>
    /// While we may want this to target a release instead of the anime,
    /// for now we will force entries to use a primary release instead of
    /// their locale release. This will cover 99.9% of the use cases since
    /// the primary locales are English and Japanese, and English dubs rarely,
    /// if ever, differ from the native source other than the dub language.
    /// To my knowledge even the excessive censorship of the 4Kids Yu-Gi-Oh
    /// dub did not alter the episode count.
    /// </remarks>
    /// <seealso cref="AnimeModel"/>
    public int AnimeId { get; set; }

    /// <summary>
    /// The Id of the status that describes the state the user has put this anime in.
    /// </summary>
    /// <seealso cref="EAnimeListStatus"/>
    /// <seealso cref="AnimeListStatusTypeModel"/>
    public int StatusId { get; set; }

    /// <summary>
    /// The number of episodes of this anime the user has watched.
    /// </summary>
    /// <remarks>
    /// This number is capped at the shows native release episode count, not the release
    /// available in the user's locale.
    /// </remarks>
    public int EpisodeCount { get; set; }

    /// <summary>
    /// The rating the user gives the anime, from 0 to 100.
    /// </summary>
    public byte? Rating { get; set; }

    /// <summary>
    /// A comment the user has left about this anime.
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// The number of times the user has rewatched the anime.
    /// </summary>
    public int RewatchCount { get; set; }

    /// <summary>
    /// The date on which the user first started watching the anime.
    /// </summary>
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// The date on which the user finished watching the anime.
    /// </summary>
    public DateOnly? EndDate { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The anime this list entry refers to.
    /// </summary>
    public AnimeModel Anime { get; set; } = default!;

    /// <summary>
    /// The status this user has put this anime in.
    /// </summary>
    public AnimeListStatusTypeModel Status { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<AnimeListEntryModel> builder)
    {
        builder.ToTable("AnimeListEntry");

        builder.HasKey(m => new { m.UserId, m.AnimeId });
        builder.HasIndex(m => m.AnimeId);
        builder.HasIndex(m => m.StatusId);

        builder.HasOne(m => m.Anime).WithMany().HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<UserModel>().WithMany().HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Status).WithMany().HasForeignKey(m => m.StatusId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - when polling for an anime list entry, the name of the anime and the status name
        // will almost always need to come with it.
        builder.Navigation(m => m.Anime).AutoInclude();
        builder.Navigation(m => m.Status).AutoInclude();

        builder.Property(m => m.EpisodeCount).HasComment("The number of episodes the user has seen.").HasDefaultValue(0);
        builder.Property(m => m.Rating).HasComment("The rating this user gives the anime, from 0 to 100.");
        builder.Property(m => m.Comment).HasComment("Comments the user has about the anime.").HasMaxLength(250);
        builder.Property(m => m.RewatchCount).HasComment("The number of times the user has rewatched the anime.").HasDefaultValue(0);
        builder.Property(m => m.StartDate).HasComment("The date the user first started watching the anime.");
        builder.Property(m => m.EndDate).HasComment("The date the user watched the final episode of the anime.");
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EpisodeCount < 0)
        {
            yield return new ValidationResult("The number of episodes a user has seen must be greater than " +
                                              "or equal to 0.", new[] { nameof(EpisodeCount) });
        }
        if (RewatchCount < 0)
        {
            yield return new ValidationResult("The rewatch count must be greater than or equal to 0.", new[] { nameof(RewatchCount) });
        }

        var animePrimaryRelease = Anime?.GetPrimaryRelease();
        if (animePrimaryRelease is not null && EpisodeCount > animePrimaryRelease.EpisodeCount)
        {
            yield return new ValidationResult("The number of episodes a user has seen cannot exceed " +
                                              "the number of episodes available in the primary release.",
                                              new[] { nameof(EpisodeCount) });
        }

        if (Rating.HasValue && (Rating < 0 || Rating > 100))
        {
            yield return new ValidationResult("The rating must be between 0 and 100, or null.", new[] { nameof(Rating) });
        }

        if (StartDate.HasValue && EndDate.HasValue && StartDate > EndDate)
        {
            yield return new ValidationResult("The start date must be after the end date.", new[] { nameof(StartDate), nameof(EndDate) });
        }
    }
}
