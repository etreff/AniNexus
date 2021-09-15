using System.ComponentModel.DataAnnotations;
using AniNexus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a entry in a user's game list.
/// </summary>
public class GameListEntryModel : IEntityTypeConfiguration<GameListEntryModel>, IValidatableObject
{
    /// <summary>
    /// The user that owns this list entry.
    /// </summary>
    /// <seealso cref="ApplicationUserModel"/>
    public string UserId { get; set; } = default!;

    /// <summary>
    /// The Id of the game this entry refers to.
    /// </summary>
    /// <seealso cref="GameModel"/>
    public int GameId { get; set; }

    /// <summary>
    /// The Id of the status that describes the state the user has put this game in.
    /// </summary>
    /// <seealso cref="EAnimeListStatus"/>
    /// <seealso cref="AnimeListStatusTypeModel"/>
    public int StatusId { get; set; }

    /// <summary>
    /// The rating the user gives the game, from 0 to 100.
    /// </summary>
    public byte? Rating { get; set; }

    /// <summary>
    /// A comment the user has left about this game.
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// The number of times the user has replayed the game.
    /// </summary>
    public int ReplayCount { get; set; }

    /// <summary>
    /// The date on which the user first started playing the game.
    /// </summary>
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// The date on which the user finished playing the game.
    /// </summary>
    public DateOnly? EndDate { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The game this list entry refers to.
    /// </summary>
    public GameModel Game { get; set; } = default!;

    /// <summary>
    /// The status this user has put this game in.
    /// </summary>
    public GameListStatusTypeModel Status { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<GameListEntryModel> builder)
    {
        builder.ToTable("GameListEntry");

        builder.HasKey(m => new { m.UserId, m.GameId });
        builder.HasIndex(m => m.UserId);
        builder.HasIndex(m => m.GameId);

        builder.HasOne(m => m.Game).WithMany().HasForeignKey(m => m.GameId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<ApplicationUserModel>().WithMany().HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Status).WithMany().HasForeignKey(m => m.StatusId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - when polling for a game list entry, the name of the game and the status name
        // will almost always need to come with it.
        builder.Navigation(m => m.Game).AutoInclude();
        builder.Navigation(m => m.Status).AutoInclude();

        builder.Property(m => m.Rating).HasComment("The rating this user gives the game, from 0 to 100.");
        builder.Property(m => m.Comment).HasComment("Comments the user has about the game.").HasMaxLength(250);
        builder.Property(m => m.ReplayCount).HasComment("The number of times the user has replayed the game.").HasDefaultValue(0);
        builder.Property(m => m.StartDate).HasComment("The date the user first started playing the game.");
        builder.Property(m => m.EndDate).HasComment("The date the user finished playing the game.");
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ReplayCount < 0)
        {
            yield return new ValidationResult("The replay count must be greater than or equal to 0.", new[] { nameof(ReplayCount) });
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
