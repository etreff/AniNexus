using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between an game and a Twitter hashtag.
/// </summary>
public class GameTwitterHashTagMapModel : IEntityTypeConfiguration<GameTwitterHashTagMapModel>
{
    /// <summary>
    /// The Id of the game.
    /// </summary>
    /// <seealso cref="GameModel"/>
    public int GameId { get; set; }

    /// <summary>
    /// The Id of the hashtag.
    /// </summary>
    /// <seealso cref="TwitterHashTagModel"/>
    public int TwitterHashTagId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The game the hashtag applies to.
    /// </summary>
    public GameModel Game { get; set; } = default!;

    /// <summary>
    /// The hashtag that is applied to the game.
    /// </summary>
    public TwitterHashTagModel TwitterHashTag { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<GameTwitterHashTagMapModel> builder)
    {
        builder.ToTable("GameTwitterHashTagMap");

        builder.HasKey(m => new { m.GameId, m.TwitterHashTagId });
        builder.HasIndex(m => m.TwitterHashTagId);

        builder.HasOne(m => m.Game).WithMany(m => m.TwitterHashtags).HasForeignKey(m => m.GameId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.TwitterHashTag).WithMany().HasForeignKey(m => m.TwitterHashTagId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Game).AutoInclude();
        builder.Navigation(m => m.TwitterHashTag).AutoInclude();
    }
}
