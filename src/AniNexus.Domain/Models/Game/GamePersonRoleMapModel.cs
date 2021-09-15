using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between a person and the roles that the person has played in an game.
/// </summary>
/// <remarks>
/// Voice actors or live-action actors are not included in this collection.
/// </remarks>
public class GamePersonRoleMapModel : IEntityTypeConfiguration<GamePersonRoleMapModel>
{
    /// <summary>
    /// The person Id.
    /// </summary>
    /// <seealso cref="MediaPersonModel"/>
    public int PersonId { get; set; }

    /// <summary>
    /// The game Id.
    /// </summary>
    /// <seealso cref="GameModel"/>
    public int GameId { get; set; }

    /// <summary>
    /// The role Id.
    /// </summary>
    /// <seealso cref="EPersonRole"/>
    /// <seealso cref="PersonRoleTypeModel"/>
    public int RoleId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The person who played a role in the creation of the game.
    /// </summary>
    public MediaPersonModel Person { get; set; } = default!;

    /// <summary>
    /// The game the person played a role in creating.
    /// </summary>
    public GameModel Game { get; set; } = default!;

    /// <summary>
    /// The role the person played in the creation of the game.
    /// </summary>
    public PersonRoleTypeModel Role { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<GamePersonRoleMapModel> builder)
    {
        builder.ToTable("GamePersonRoleMap");

        builder.HasKey(m => new { m.PersonId, m.GameId, m.RoleId });
        builder.HasIndex(m => m.GameId);
        builder.HasIndex(m => m.RoleId);

        builder.HasOne(m => m.Person).WithMany(m => m.GameRoles).HasForeignKey(m => m.PersonId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Game).WithMany(m => m.People).HasForeignKey(m => m.GameId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Role).WithMany().HasForeignKey(m => m.RoleId).IsRequired().OnDelete(DeleteBehavior.Restrict);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Game).AutoInclude();
        builder.Navigation(m => m.Person).AutoInclude();
        builder.Navigation(m => m.Role).AutoInclude();
    }
}
