using AniNexus.Models;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between a character and the anime they star in.
/// </summary>
public class AnimeCharacterMapModel : IEntityTypeConfiguration<AnimeCharacterMapModel>
{
    /// <summary>
    /// The Id of the anime the character stars in.
    /// </summary>
    /// <seealso cref="AnimeModel"/>
    public int AnimeId { get; set; }

    /// <summary>
    /// The Id of the character.
    /// </summary>
    /// <seealso cref="MediaCharacterModel"/>
    public int CharacterId { get; set; }

    /// <summary>
    /// The Id of the role the character plays in the anime.
    /// </summary>
    /// <seealso cref="ECharacterRole"/>
    /// <seealso cref="CharacterRoleTypeEntity"/>
    public int RoleId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The anime that the character stars in.
    /// </summary>
    public AnimeModel Anime { get; set; } = default!;

    /// <summary>
    /// The character.
    /// </summary>
    public MediaCharacterModel Character { get; set; } = default!;

    /// <summary>
    /// The role this character plays in the anime.
    /// </summary>
    public CharacterRoleTypeEntity Role { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<AnimeCharacterMapModel> builder)
    {
        builder.ToTable("AnimeCharacterMap");

        builder.HasKey(m => new { m.AnimeId, m.CharacterId });
        builder.HasIndex(m => m.CharacterId);
        builder.HasIndex(m => m.RoleId);

        builder.HasOne(m => m.Anime).WithMany(m => m.Characters).HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Character).WithMany(m => m.Anime).HasForeignKey(m => m.CharacterId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Role).WithMany().HasForeignKey(m => m.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Anime).AutoInclude();
        builder.Navigation(m => m.Character).AutoInclude();
        builder.Navigation(m => m.Role).AutoInclude();

        builder.HasQueryFilter(m => !m.Anime.IsSoftDeleted && !m.Character.IsSoftDeleted);
    }
}
