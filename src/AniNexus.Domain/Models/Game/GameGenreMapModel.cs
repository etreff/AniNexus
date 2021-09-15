using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping of genres and an game.
/// </summary>
public class GameGenreMapModel : IEntityTypeConfiguration<GameGenreMapModel>
{
    /// <summary>
    /// The game Id.
    /// </summary>
    /// <seealso cref="GameModel"/>
    public int GameId { get; set; }

    /// <summary>
    /// The genre Id.
    /// </summary>
    /// <seealso cref="MediaGenreModel"/>
    public int GenreId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The game the genre applies to.
    /// </summary>
    public GameModel Game { get; set; } = default!;

    /// <summary>
    /// The genre to apply to the game.
    /// </summary>
    public MediaGenreModel Genre { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<GameGenreMapModel> builder)
    {
        builder.ToTable("GameGenreMap");

        builder.HasKey(m => new { m.GameId, m.GenreId });
        builder.HasIndex(m => m.GenreId);

        builder.HasOne(m => m.Game).WithMany(m => m.Genres).HasForeignKey(m => m.GameId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Genre).WithMany(m => m.Games).HasForeignKey(m => m.GenreId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Game).AutoInclude();
        builder.Navigation(m => m.Genre).AutoInclude();
    }
}
