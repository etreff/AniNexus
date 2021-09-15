using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a map of users and the game they have favorited.
/// </summary>
public class GameFavoriteMapModel : IEntityTypeConfiguration<GameFavoriteMapModel>
{
    /// <summary>
    /// The Id of the game.
    /// </summary>
    /// <seealso cref="GameModel"/>
    public int GameId { get; set; }

    /// <summary>
    /// The Id of the user who favorited the game.
    /// </summary>
    /// <seealso cref="ApplicationUserModel"/>
    public string UserId { get; set; } = default!;

    #region Navigation Properties
    /// <summary>
    /// The game that got favorited.
    /// </summary>
    public GameModel Game { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<GameFavoriteMapModel> builder)
    {
        builder.ToTable("GameFavoriteMap");

        builder.HasKey(m => new { m.GameId, m.UserId });
        builder.HasIndex(m => m.UserId);

        builder.HasOne(m => m.Game).WithMany(m => m.Favorites).HasForeignKey(m => m.GameId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Game).AutoInclude();
    }
}
