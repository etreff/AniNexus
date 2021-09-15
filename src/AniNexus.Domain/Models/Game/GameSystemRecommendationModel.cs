using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a game recommendation made by AniNexus.
/// </summary>
/// <remarks>
/// System recommendations intentionally do not have the ability to specify a reason.
/// </remarks>
public class GameSystemRecommendationModel : IEntityTypeConfiguration<GameSystemRecommendationModel>
{
    /// <summary>
    /// The Id of the game the recommendation is based off of.
    /// </summary>
    /// <seealso cref="GameModel"/>
    public int GameId { get; set; }

    /// <summary>
    /// The Id of the game being recommended.
    /// </summary>
    /// <seealso cref="GameModel"/>
    public int GameRecommendationId { get; set; }

    /// <summary>
    /// The order in which the recommendation is listed among other AniNexus recommendations for
    /// this game.
    /// </summary>
    /// <remarks>
    /// A lower order will be listed first.
    /// </remarks>
    public byte Order { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The game the recommendation is based off of.
    /// </summary>
    public GameModel Game { get; set; } = default!;

    /// <summary>
    /// The game being recommended as similar to <see cref="Game"/>.
    /// </summary>
    public GameModel Recommendation { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<GameSystemRecommendationModel> builder)
    {
        builder.ToTable("GameSysRec");

        builder.HasKey(m => new { m.GameId, m.GameRecommendationId });
        builder.HasIndex(m => m.GameRecommendationId);

        // Handled by EF trigger - SqlServer will falsely flag multiple cascade paths since the mapping
        // points to two entities in the same table.
        builder.HasOne(m => m.Game).WithMany().HasForeignKey(m => m.GameId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(m => m.Recommendation).WithMany().HasForeignKey(m => m.GameRecommendationId).IsRequired().OnDelete(DeleteBehavior.NoAction);

        // Justification - this is kind of a map, so the navigation properties are always included. In addition, recommendations are
        // generally displayed to a user and they need the name of both game.
        builder.Navigation(m => m.Game).AutoInclude();
        builder.Navigation(m => m.Recommendation).AutoInclude();

        builder.Property(m => m.GameRecommendationId).HasColumnName("GameRecId");
        builder.Property(m => m.Order).HasComment("The order in which the recommendation will be listed. Lower order will be listed first.").HasDefaultValue(10);
    }
}
