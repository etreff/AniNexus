using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between an game and a third party tracker.
/// </summary>
public class GameThirdPartyMapModel : IEntityTypeConfiguration<GameThirdPartyMapModel>
{
    /// <summary>
    /// The AniNexus game Id.
    /// </summary>
    /// <seealso cref="GameModel"/>
    public int GameId { get; set; }

    /// <summary>
    /// The Id of the third party tracker.
    /// </summary>
    /// <seealso cref="ThirdPartyModel"/>
    public int ThirdPartyId { get; set; }

    /// <summary>
    /// The Id the third party tracker has assigned this game.
    /// </summary>
    public string ExternalMediaId { get; set; } = default!;

    #region Navigation Properties
    /// <summary>
    /// The game that the external media Id refers to.
    /// </summary>
    public GameModel Game { get; set; } = default!;

    /// <summary>
    /// The third party tracker.
    /// </summary>
    public ThirdPartyModel ThirdParty { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<GameThirdPartyMapModel> builder)
    {
        builder.ToTable("GameThirdPartyMap");

        builder.HasKey(m => new { m.GameId, m.ThirdPartyId });
        builder.HasIndex(m => m.ThirdPartyId);

        builder.HasOne(m => m.Game).WithMany(m => m.ExternalIds).HasForeignKey(m => m.GameId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.ThirdParty).WithMany().HasForeignKey(m => m.ThirdPartyId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Game).AutoInclude();
        builder.Navigation(m => m.ThirdParty).AutoInclude();

        builder.Property(m => m.ExternalMediaId).HasComment("The Id that the third party tracker has assigned to the game entry.");
    }
}
