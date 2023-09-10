using AniNexus.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace AniNexus.DataAccess.Entities.Configuration;

/// <summary>
/// The base class for an entity configuration.
/// </summary>
/// <typeparam name="TEntity">The type of the entity being configured.</typeparam>
internal abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class
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
}

/// <summary>
/// The base class for an entity configuration.
/// </summary>
/// <typeparam name="TEntity">The type of the entity being configured.</typeparam>
/// <typeparam name="TKey">The type of the primary key of the entity.</typeparam>
internal abstract class BaseConfiguration<TEntity, TKey> : BaseConfiguration<TEntity>
    where TEntity : class, IEntity<TKey>
    where TKey : notnull
{
    /// <summary>
    /// Configures the entity of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="builder">The type builder.</param>
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(m => m.Id);

        var pk = builder.Property(m => m.Id);
        pk.UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction);

        if (typeof(TKey) == typeof(Guid))
        {
            // The default value generation uses SequentialGuidValueGenerator. We don't want
            // sequential Guids.
            pk.ValueGeneratedNever().HasValueGenerator<GuidValueGenerator>();
        }
        else if (typeof(TKey) == typeof(string) ||
                 typeof(TEntity).IsTypeOf<IEnumEntity>())
        {
            pk.ValueGeneratedNever();
        }
        else
        {
            pk.UseIdentityColumn();
        }
    }
}
