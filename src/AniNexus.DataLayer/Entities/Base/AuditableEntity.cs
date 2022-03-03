namespace AniNexus.Data.Entities;

/// <summary>
/// The base class for entities that are auditable.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TKey">The type of the primary key.</typeparam>
public abstract class AuditableEntity<TEntity, TKey> : Entity<TEntity, TKey>, IHasAudit
    where TEntity : AuditableEntity<TEntity, TKey>
    where TKey : struct, IComparable<TKey>, IEquatable<TKey>
{
    /// <summary>
    /// The UTC date and time this entity was added to the table.
    /// </summary>
    public DateTime DateAdded { get; set; }

    /// <summary>
    /// The UTC date and time this entity was last updated.
    /// </summary>
    public DateTime DateUpdated { get; set; }

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<TEntity> builder)
    {
        // 1. Primary key specification (if not Entity<>)
        // 2. Index specification
        // 3. Navigation properties
        // 4. Propery specification
        builder.Property(m => m.DateAdded).HasComputedColumnSql("getutcdate()").ValueGeneratedOnAdd().HasComment("The date the entity was created.");
        builder.Property(m => m.DateUpdated).HasComputedColumnSql("getutcdate()").ValueGeneratedOnAddOrUpdate().HasComment("The date the entity was last updated.");
    }
}

/// <summary>
/// The base class for entities that are auditable.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public abstract class AuditableEntity<TEntity> : AuditableEntity<TEntity, Guid>
    where TEntity : AuditableEntity<TEntity>
{
}
