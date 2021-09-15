using AniNexus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between related game entries.
/// </summary>
public class GameRelatedMapModel : IEntityTypeConfiguration<GameRelatedMapModel>
{
    /// <summary>
    /// The Id of the game.
    /// </summary>
    /// <seealso cref="GameModel"/>
    public int GameId { get; set; }

    /// <summary>
    /// The Id of the related game.
    /// </summary>
    /// <seealso cref="GameModel"/>
    public int RelatedGameId { get; set; }

    /// <summary>
    /// The Id of the relation type.
    /// </summary>
    /// <seealso cref="EMediaRelationType"/>
    /// <seealso cref="MediaRelationTypeModel"/>
    public int RelationTypeId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The game.
    /// </summary>
    public GameModel Game { get; set; } = default!;

    /// <summary>
    /// The game that is related.
    /// </summary>
    public GameModel Related { get; set; } = default!;

    /// <summary>
    /// How the game is related.
    /// </summary>
    public MediaRelationTypeModel RelationType { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<GameRelatedMapModel> builder)
    {
        builder.ToTable("GameRelatedMap");

        builder.HasKey(m => new { m.GameId, m.RelatedGameId });
        builder.HasIndex(m => m.RelatedGameId);
        builder.HasIndex(m => m.RelationTypeId);

        builder.HasOne(m => m.Game).WithMany(m => m.Related).HasForeignKey(m => m.GameId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        // Handled by EF trigger - SqlServer will falsely flag multiple cascade paths since the mapping
        // points to two entities in the same table.
        builder.HasOne(m => m.Related).WithMany().HasForeignKey(m => m.RelatedGameId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(m => m.RelationType).WithMany().HasForeignKey(m => m.RelatedGameId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Game).AutoInclude();
        builder.Navigation(m => m.Related).AutoInclude();
        builder.Navigation(m => m.RelationType).AutoInclude();
    }
}
