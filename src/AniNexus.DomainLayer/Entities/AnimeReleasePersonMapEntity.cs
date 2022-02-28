namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a mapping between a person and the roles that the person has played in the creation of an anime release.
/// </summary>
/// <remarks>
/// Voice actors or live-action actors are not included in this collection.
/// </remarks>
public class AnimeReleasePersonMapEntity : Entity<AnimeReleasePersonMapEntity, long>
{
    /// <summary>
    /// The Id of the person who played a role in the creation of the anime.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// The Id of the anime release the person played a role in creating.
    /// </summary>
    public Guid ReleaseId { get; set; }

    /// <summary>
    /// The Id of the role the person played in the creation of the anime.
    /// </summary>
    /// <seealso cref="EPersonRole"/>
    /// <seealso cref="PersonRoleTypeEntity"/>
    public byte RoleId { get; set; }

    /// <summary>
    /// The person who played a role in the creation of the anime.
    /// </summary>
    public PersonEntity Person { get; set; } = default!;

    /// <summary>
    /// The anime release the person played a role in creating.
    /// </summary>
    public AnimeReleaseEntity Release { get; set; } = default!;

    /// <summary>
    /// The role the person played in the creation of the anime.
    /// </summary>
    public PersonRoleTypeEntity Role { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AnimeReleasePersonMapEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => new object[] { m.PersonId, m.ReleaseId, m.RoleId }).IsUnique();
        builder.HasIndex(m => m.ReleaseId);
        builder.HasIndex(m => m.RoleId);
        // 2. Navigation properties
        builder.HasOne(m => m.Person).WithMany(m => m.AnimeRoles).HasForeignKey(m => m.PersonId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Release).WithMany(m => m.People).HasForeignKey(m => m.ReleaseId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Role).WithMany().HasForeignKey(m => m.RoleId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        // 3. Propery specification
        // 4. Other
        builder.HasQueryFilter(m => !m.Person.IsSoftDeleted && !m.Release.IsSoftDeleted);
    }
}
