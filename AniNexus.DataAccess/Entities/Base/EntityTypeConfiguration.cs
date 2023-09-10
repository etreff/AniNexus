using AniNexus.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace AniNexus.DataAccess.Entities;

/// <summary>
/// The base class for an entity configuration.
/// </summary>
/// <typeparam name="TEntity">The type of the entity being configured.</typeparam>
public abstract class EntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class, IEntity
{
    /// <summary>
    /// Configures the entity of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="builder">The type builder.</param>
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        string tableName = GetTableName();
        builder.ToTable(tableName);

        var seedData = GetSeedData()?.ToArray();
        if (seedData?.Length > 0)
        {
            builder.HasData(seedData);
        }
    }

    /// <summary>
    /// Gets seed data for this entity type for use in data migrations.
    /// </summary>
    /// <returns>The seed data for this entity type.</returns>
    protected virtual IEnumerable<TEntity> GetSeedData()
    {
        return Enumerable.Empty<TEntity>();
    }

    /// <summary>
    /// Gets the table name for this entity.
    /// </summary>
    protected virtual string GetTableName()
    {
        return Entity.GetDefaultTableName<TEntity>();
    }
}

/// <summary>
/// The base class for an entity configuration.
/// </summary>
/// <typeparam name="TEntity">The type of the entity being configured.</typeparam>
/// <typeparam name="TKey">The type of the primary key of the entity.</typeparam>
public abstract class EntityTypeConfiguration<TEntity, TKey> : EntityTypeConfiguration<TEntity>
    where TEntity : class, IEntity<TKey>
    where TKey : notnull
{
    /// <summary>
    /// Whether to generate the primary key on add. Defaults to <see langword="true"/>.
    /// </summary>
    /// <remarks>
    /// This property only applies to <see cref="Guid"/> and numeric primary keys.
    /// </remarks>
    protected virtual bool GenerateKeyOnAdd { get; } = true;

    /// <summary>
    /// Configures the entity of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="builder">The type builder.</param>
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);

        builder.HasKey(m => m.Id);

        var pk = builder.Property(m => m.Id);
        pk.UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction);

        if (GenerateKeyOnAdd)
        {
            pk.ValueGeneratedOnAdd();
        }
        else
        {
            pk.ValueGeneratedNever();
        }


        if (typeof(TKey) == typeof(Guid))
        {
            if (GenerateKeyOnAdd)
            {
                // The default value generation uses SequentialGuidValueGenerator. We don't want
                // sequential Guids.
                pk.HasValueGenerator<GuidValueGenerator>();
            }
        }
        else if (typeof(TKey) == typeof(string) ||
                 typeof(TEntity).IsTypeOf<IEnumEntity>())
        {
            pk.ValueGeneratedNever();
        }
        else
        {
            if (GenerateKeyOnAdd)
            {
                pk.UseIdentityColumn();
            }
        }
    }

    /// <inheritdoc />
    protected override IEnumerable<TEntity> GetSeedData()
    {
        foreach (var entity in GetSeedDataImpl())
        {
            entity.Instance.SetId(entity.Key);
            yield return entity.Instance;
        }
    }

    /// <summary>
    /// Gets seed data for this entity type.
    /// </summary>
    /// <returns>The seed data for this entity type.</returns>
    protected virtual IEnumerable<Entity> GetSeedDataImpl()
    {
        return Array.Empty<Entity>();
    }

    /// <summary>
    /// An entity container that allows assigning a key to an entity.
    /// </summary>
    /// <param name="Key">The primary key of the record.</param>
    /// <param name="Instance">The record to create.</param>
    /// <remarks>
    /// This structure should only be used within the context of <see cref="GetSeedDataImpl"/>.
    /// </remarks>
    protected readonly record struct Entity(TKey Key, TEntity Instance);
}

/// <summary>
/// <see cref="EntityTypeConfiguration{TEntity, TKey}"/> extensions.
/// </summary>
public static class BaseConfigurationExtensions
{
    /// <summary>
    /// Creates a new entity with the key set to the provided value.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <param name="_">The configuration.</param>
    /// <param name="key">The key.</param>
    public static TEntity CreateEntity<TEntity, TKey>(this EntityTypeConfiguration<TEntity> _, TKey key)
        where TEntity : Entity<TEntity, TKey>, new()
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        return Entity.Create<TEntity, TKey>(key);
    }

    /// <summary>
    /// Creates a new entity with the key set to the provided value.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <param name="_">The configuration.</param>
    /// <param name="key">The key.</param>
    /// <param name="configure">The action to configure the entity.</param>
    public static TEntity CreateEntity<TEntity, TKey>(this EntityTypeConfiguration<TEntity> _, TKey key, Action<TEntity> configure)
        where TEntity : Entity<TEntity, TKey>, new()
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        return Entity.Create(key, configure);
    }

    /// <summary>
    /// Creates a new entity with the key set to the provided value.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <param name="_">The configuration.</param>
    /// <param name="key">The key.</param>
    public static TEntity CreateEntity<TEntity, TKey>(this EntityTypeConfiguration<TEntity, TKey> _, TKey key)
        where TEntity : Entity<TEntity, TKey>, new()
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        return Entity.Create<TEntity, TKey>(key);
    }

    /// <summary>
    /// Creates a new entity with the key set to the provided value.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <param name="_">The configuration.</param>
    /// <param name="key">The key.</param>
    /// <param name="configure">The action to configure the entity.</param>
    public static TEntity CreateEntity<TEntity, TKey>(this EntityTypeConfiguration<TEntity, TKey> _, TKey key, Action<TEntity> configure)
        where TEntity : Entity<TEntity, TKey>, new()
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        return Entity.Create(key, configure);
    }
}
