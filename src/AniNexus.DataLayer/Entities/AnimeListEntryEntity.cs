using System.ComponentModel.DataAnnotations;
using AniNexus.Data.Validation;
using AniNexus.Models;

namespace AniNexus.Data.Entities;

/// <summary>
/// Models an entry in a user's anime list.
/// </summary>
public class AnimeListEntryEntity : AuditableEntity<AnimeListEntryEntity>
{
    /// <summary>
    /// The user that owns this list entry.
    /// </summary>
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
    /// dub did not alter the episode count, and things like DBZ Kai are
    /// their own entities.
    /// </remarks>
    public Guid AnimeId { get; set; }

    /// <summary>
    /// The Id of the status that describes the state the user has put this anime in.
    /// </summary>
    /// <seealso cref="EAnimeListStatus"/>
    /// <seealso cref="AnimeListStatusTypeEntity"/>
    public byte StatusId { get; set; }

    /// <summary>
    /// The number of episodes of this anime the user has watched.
    /// </summary>
    /// <remarks>
    /// This number is capped at the shows native release episode count, not the release
    /// available in the user's locale.
    /// </remarks>
    public short EpisodeCount { get; set; }

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
    public short RewatchCount { get; set; }

    /// <summary>
    /// The date on which the user first started watching the anime.
    /// </summary>
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// The date on which the user finished watching the anime.
    /// </summary>
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// The anime this list entry refers to.
    /// </summary>
    public AnimeEntity Anime { get; set; } = default!;

    /// <summary>
    /// The status this user has put this anime in.
    /// </summary>
    public AnimeListStatusTypeEntity Status { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AnimeListEntryEntity> builder)
    {
        base.ConfigureEntity(builder);

        // 1. Index specification
        builder.HasIndex(m => new { m.UserId, m.AnimeId }).IsUnique();
        builder.HasIndex(m => m.AnimeId);
        builder.HasIndex(m => m.StatusId);
        // 2. Navigation properties
        builder.HasOne(m => m.Anime).WithMany().HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<UserEntity>().WithMany().HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Status).WithMany().HasForeignKey(m => m.StatusId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 3. Propery specification
        builder.Property(m => m.EpisodeCount).HasComment("The number of episodes the user has seen.");
        builder.Property(m => m.Rating).HasComment("The rating this user gives the anime, from 0 to 100.");
        builder.Property(m => m.Comment).HasComment("Comments the user has about the anime.").HasMaxLength(250);
        builder.Property(m => m.RewatchCount).HasComment("The number of times the user has rewatched the anime.");
        builder.Property(m => m.StartDate).HasComment("The date the user first started watching the anime.");
        builder.Property(m => m.EndDate).HasComment("The date the user watched the final episode of the anime.");
        // 4. Other
    }

    /// <inheritdoc/>
    protected override void Validate(ValidationContext validationContext, ValidationBuilder<AnimeListEntryEntity> validator)
    {
        base.Validate(validationContext, validator);

        validator.Property(m => m.EpisodeCount).IsGreaterThanOrEqualTo((short)0);
        validator.Property(m => m.RewatchCount).IsGreaterThanOrEqualTo((short)0);
        validator.Property(m => m.Rating).IsBetween(0, 100);
        // If the end-date is not set, compare it to the start date which will always be true since we are comparing
        // the start date to itself.
        validator.Property(m => m.StartDate).IsLessThanOrEqualTo(m => m.EndDate ?? m.StartDate!.Value);

        //TODO: When fetching the episode count from the list, Max() it with the anime's primary release.
    }
}
