namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a mapping of tags and an anime.
/// </summary>
public class AnimeTagMapEntity : Entity<AnimeTagMapEntity>
{
    /// <summary>
    /// The Id of the anime the tag applies to.
    /// </summary>
    public Guid AnimeId { get; set; }

    /// <summary>
    /// The Id of the tag to apply to the anime.
    /// </summary>
    public int TagId { get; set; }

    /// <summary>
    /// The anime the tag applies to.
    /// </summary>
    public AnimeEntity Anime { get; set; } = default!;

    /// <summary>
    /// The tag to apply to the anime.
    /// </summary>
    public UserTagEntity Tag { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AnimeTagMapEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => new { m.AnimeId, m.TagId }).IsUnique();
        builder.HasIndex(m => m.TagId);
        // 2. Navigation properties
        builder.HasOne(m => m.Anime).WithMany(m => m.Tags).HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Tag).WithMany(m => m.Anime).HasForeignKey(m => m.TagId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 3. Propery specification
        // 4. Other
        builder.HasQueryFilter(m => !m.Anime.IsSoftDeleted);
    }
}

/// <summary>
/// Models a mapping of tags and an anime that are waiting for enough user votes
/// to be applied.
/// </summary>
public class AnimeTagMapPendingEntity : Entity<AnimeTagMapPendingEntity>
{
    /// <summary>
    /// The Id of the anime the tag applies to.
    /// </summary>
    public Guid AnimeId { get; set; }

    /// <summary>
    /// The Id of the tag to apply to the anime.
    /// </summary>
    public int TagId { get; set; }

    /// <summary>
    /// The Id of the user who applied the tag.
    /// </summary>
    public Guid UserId { get; set; }

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AnimeTagMapPendingEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => new { m.AnimeId, m.TagId, m.UserId }).IsUnique();
        builder.HasIndex(m => m.TagId);
        builder.HasIndex(m => m.UserId);
        // 2. Navigation properties
        builder.HasOne<AnimeEntity>().WithMany().HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<AnimeTagMapEntity>().WithMany().HasForeignKey(m => m.TagId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<UserEntity>().WithMany(m => m.PendingAnimeTags).HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 3. Propery specification
        // 4. Other
    }
}
