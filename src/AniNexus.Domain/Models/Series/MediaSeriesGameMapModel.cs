
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping that associates a game with a series.
/// </summary>
public class MediaSeriesGameMapModel : IEntityTypeConfiguration<MediaSeriesGameMapModel>
{
    /// <summary>
    /// The series/IP Id.
    /// </summary>
    /// <seealso cref="MediaSeriesModel"/>
    public int SeriesId { get; set; }

    /// <summary>
    /// The game Id.
    /// </summary>
    /// <seealso cref="GameModel"/>
    public int GameId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The series.
    /// </summary>
    public MediaSeriesModel Series { get; set; } = default!;

    /// <summary>
    /// The game.
    /// </summary>
    public GameModel Game { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MediaSeriesGameMapModel> builder)
    {
        builder.ToTable("MediaSeriesGameMap");

        builder.HasKey(m => new { m.SeriesId, m.GameId });
        builder.HasIndex(m => m.GameId);

        builder.HasOne(m => m.Series).WithMany(m => m.Games).HasForeignKey(m => m.SeriesId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Game).WithMany(m => m.Series).HasForeignKey(m => m.GameId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Series).AutoInclude();
        builder.Navigation(m => m.Game).AutoInclude();
    }
}
