namespace AniNexus.Data.Entities;

/// <summary>
/// Models a mapping between a character and the anime they star in.
/// </summary>
public class AnimeCharacterMapEntity : Entity<AnimeCharacterMapEntity>
{
    /// <summary>
    /// The Id of the anime the character stars in.
    /// </summary>
    public Guid AnimeId { get; set; }

    /// <summary>
    /// The Id of the character.
    /// </summary>
    public Guid CharacterId { get; set; }

    /// <summary>
    /// The Id of the role the character plays in the anime.
    /// </summary>
    /// <seealso cref="ECharacterRole"/>
    /// <seealso cref="CharacterRoleTypeEntity"/>
    public byte RoleId { get; set; }

    /// <summary>
    /// The anime that the character stars in.
    /// </summary>
    public AnimeEntity Anime { get; set; } = default!;

    /// <summary>
    /// The character.
    /// </summary>
    public CharacterEntity Character { get; set; } = default!;

    /// <summary>
    /// The role this character plays in the anime.
    /// </summary>
    public CharacterRoleTypeEntity Role { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AnimeCharacterMapEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => new[] { m.AnimeId, m.CharacterId }).IsUnique();
        builder.HasIndex(m => m.CharacterId);
        // 2. Navigation properties
        builder.HasOne(m => m.Anime).WithMany(m => m.Characters).HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Character).WithMany(m => m.Anime).HasForeignKey(m => m.CharacterId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Role).WithMany().HasForeignKey(m => m.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 3. Propery specification
        // 4. Other
        builder.HasQueryFilter(m => !m.Anime.IsSoftDeleted && !m.Character.IsSoftDeleted);
    }
}
