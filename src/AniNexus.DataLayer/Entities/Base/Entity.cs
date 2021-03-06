using System.ComponentModel.DataAnnotations;
using AniNexus.Data.Validation;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Toolkit.Diagnostics;

namespace AniNexus.Data.Entities;

/// <summary>
/// Defines a database entity.
/// </summary>
public interface IEntity
{
}

/// <summary>
/// Defines a database entity.
/// </summary>
public interface IEntity<TKey> : IEntity
    where TKey : struct, IComparable<TKey>, IEquatable<TKey>
{
    /// <summary>
    /// The Id of the entity.
    /// </summary>
    TKey Id { get; set; }
}

/// <summary>
/// The base class that all database entities inherit from.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TKey">The type of the primary key.</typeparam>
public abstract class Entity<TEntity, TKey> : IEntity<TKey>, IEntityTypeConfiguration<TEntity>, IValidatableObject
    where TEntity : Entity<TEntity, TKey>
    where TKey : struct, IComparable<TKey>, IEquatable<TKey>
{
    /// <summary>
    /// The Id of the entity.
    /// </summary>
    public TKey Id { get; set; }

    /// <summary>
    /// Configures the entity of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="builder">The type builder.</param>
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        string tableName = GetTableName();
        builder.ToTable(tableName);

        // 1. Index specification
        builder.HasKey(m => m.Id);
        // 2. Navigation properties
        // 3. Propery specification
        var pk = builder.Property(m => m.Id);
        if (typeof(TKey) == typeof(Guid))
        {
            // The default value generation uses SequentialGuidValueGenerator. We don't want
            // sequential Guids.
            pk.ValueGeneratedNever().HasValueGenerator<GuidValueGenerator>();
        }
        else
        {
            pk.ValueGeneratedOnAdd();
        }

        var seedData = GetSeedData()?.ToArray();
        if (seedData?.Length > 0)
        {
            builder.HasData(seedData);
        }

        ConfigureEntity(builder);

        // 4. Other
    }

    /// <summary>
    /// Determines whether the specified object is valid.
    /// </summary>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>A collection that holds failed-validation information.</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validator = new ValidationBuilder<TEntity>((TEntity)validationContext.ObjectInstance, null);
        Validate(validationContext, validator);
        return validator.Validate();
    }

    /// <summary>
    /// Determines whether the specified object is valid.
    /// </summary>
    /// <param name="validationContext">The validation context.</param>
    /// <param name="validator">The validator builder.</param>
    protected virtual void Validate(ValidationContext validationContext, ValidationBuilder<TEntity> validator)
    {
    }

    /// <summary>
    /// Gets seed data for this entity type. IT is used to generate data motion migrations.
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

    /// <summary>
    /// Configures the entity of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="builder">The type builder.</param>
    protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
    // 1. Index specification
    // 2. Navigation properties
    // 3. Propery specification
    // 4. Other
}

/// <summary>
/// The base class that all database entities inherit from.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public abstract class Entity<TEntity> : Entity<TEntity, Guid>
    where TEntity : Entity<TEntity>
{
}

/// <summary>
/// Metadata methods for an entity.
/// </summary>
public static class Entity
{
    /// <summary>
    /// Returns the default table name for an entity.
    /// </summary>
    public static string GetDefaultTableName<TEntity>()
        => GetDefaultTableName(typeof(TEntity));

    /// <summary>
    /// Returns the default table name for an entity.
    /// </summary>
    public static string GetDefaultTableName(Type type)
    {
        Guard.IsNotNull(type, nameof(type));

        const string entitySuffix = "Entity";

        string tableName = type.Name;
        return tableName.EndsWith(entitySuffix, StringComparison.Ordinal)
            ? tableName.Substring(0, tableName.Length - entitySuffix.Length)
            : tableName;
    }
}
