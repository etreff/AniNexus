using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between an game and its tags.
/// </summary>
public class GameTagMapModel : IEntityTypeConfiguration<GameTagMapModel>
{
    /// <summary>
    /// The Id of the game that the tag should be applied to.
    /// </summary>
    /// <seealso cref="GameModel"/>
    public int GameId { get; set; }

    /// <summary>
    /// The Id of the tag to apply to the game.
    /// </summary>
    /// <seealso cref="MediaTagModel"/>
    public int TagId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The game the tag should be applied to.
    /// </summary>
    public GameModel Game { get; set; } = default!;

    /// <summary>
    /// The tag to apply to the game.
    /// </summary>
    public MediaTagModel Tag { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<GameTagMapModel> builder)
    {
        builder.ToTable("GameTagMap");

        builder.HasKey(m => new { m.GameId, m.TagId });
        builder.HasIndex(m => m.TagId);

        builder.HasOne(m => m.Game).WithMany(m => m.Tags).HasForeignKey(m => m.GameId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Tag).WithMany(m => m.Games).HasForeignKey(m => m.TagId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Game).AutoInclude();
        builder.Navigation(m => m.Tag).AutoInclude();
    }
}
