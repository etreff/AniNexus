using AniNexus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between a character and the game they star in.
/// </summary>
public class GameCharacterMapModel : IEntityTypeConfiguration<GameCharacterMapModel>
{
    /// <summary>
    /// The Id of the game.
    /// </summary>
    /// <seealso cref="GameModel"/>
    public int GameId { get; set; }

    /// <summary>
    /// The Id of the character.
    /// </summary>
    /// <seealso cref="MediaCharacterModel"/>
    public int CharacterId { get; set; }

    /// <summary>
    /// The Id of the role.
    /// </summary>
    /// <seealso cref="ECharacterRole"/>
    /// <seealso cref="CharacterRoleTypeModel"/>
    public int RoleId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The game that the character stars in.
    /// </summary>
    public GameModel Game { get; set; } = default!;

    /// <summary>
    /// The character that is in the media.
    /// </summary>
    public MediaCharacterModel Character { get; set; } = default!;

    /// <summary>
    /// The role this character plays in the media.
    /// </summary>
    public CharacterRoleTypeModel Role { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<GameCharacterMapModel> builder)
    {
        builder.ToTable("GameCharacterMap");

        builder.HasKey(m => new { m.GameId, m.CharacterId });
        builder.HasIndex(m => m.CharacterId);
        builder.HasIndex(m => m.RoleId);

        builder.HasOne(m => m.Game).WithMany(m => m.Characters).HasForeignKey(m => m.GameId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Character).WithMany(m => m.Games).HasForeignKey(m => m.CharacterId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Role).WithMany().HasForeignKey(m => m.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Game).AutoInclude();
        builder.Navigation(m => m.Character).AutoInclude();
        builder.Navigation(m => m.Role).AutoInclude();
    }
}
