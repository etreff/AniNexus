using AniNexus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between a character and the manga they star in.
/// </summary>
public class MangaCharacterMapModel : IEntityTypeConfiguration<MangaCharacterMapModel>
{
    /// <summary>
    /// The Id of the manga.
    /// </summary>
    /// <seealso cref="MangaModel"/>
    public int MangaId { get; set; }

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
    /// The manga that the character stars in.
    /// </summary>
    public MangaModel Manga { get; set; } = default!;

    /// <summary>
    /// The character that is in the media.
    /// </summary>
    public MediaCharacterModel Character { get; set; } = default!;

    /// <summary>
    /// The role this character plays in the media.
    /// </summary>
    public CharacterRoleTypeModel Role { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MangaCharacterMapModel> builder)
    {
        builder.ToTable("MangaCharacterMap");

        builder.HasKey(m => new { m.MangaId, m.CharacterId });
        builder.HasIndex(m => m.CharacterId);
        builder.HasIndex(m => m.RoleId);

        builder.HasOne(m => m.Manga).WithMany(m => m.Characters).HasForeignKey(m => m.MangaId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Character).WithMany(m => m.Manga).HasForeignKey(m => m.CharacterId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Role).WithMany().HasForeignKey(m => m.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Manga).AutoInclude();
        builder.Navigation(m => m.Character).AutoInclude();
        builder.Navigation(m => m.Role).AutoInclude();
    }
}
