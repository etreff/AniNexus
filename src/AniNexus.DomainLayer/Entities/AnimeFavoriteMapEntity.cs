namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a map of users and the anime they have favorited.
/// </summary>
public class AnimeFavoriteMapEntity : Entity<AnimeFavoriteMapEntity, long>
{
    /// <summary>
    /// The Id of the anime.
    /// </summary>
    public Guid AnimeId { get; set; }

    /// <summary>
    /// The Id of the user who favorited the anime.
    /// </summary>
    public Guid UserId { get; set; } = default!;

    /// <summary>
    /// The anime that got favorited.
    /// </summary>
    public AnimeEntity Anime { get; set; } = default!;

    /// <summary>
    /// The user that favorited the anime.
    /// </summary>
    public UserEntity User { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AnimeFavoriteMapEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => new { m.AnimeId, m.UserId }).IsUnique();
        builder.HasIndex(m => m.UserId);
        // 2. Navigation properties
        builder.HasOne(m => m.Anime).WithMany(m => m.Favorites).HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.User).WithMany(m => m.AnimeFavorites).HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 3. Propery specification
        // 4. Other
        builder.HasQueryFilter(m => !m.Anime.IsSoftDeleted && !m.User.IsSoftDeleted);
    }
}
