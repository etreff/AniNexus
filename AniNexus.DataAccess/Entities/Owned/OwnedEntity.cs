using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace AniNexus.DataAccess.Entities.Owned;

/// <summary>
/// Utilities for configuring owned entities.
/// </summary>
/// <remarks>
/// https://learn.microsoft.com/en-us/ef/core/modeling/owned-entities
/// </remarks>
public static class OwnedEntity
{
    /// <summary>
    /// Configures the entity as an inline owned entity (the entity is stored in the same table as the owner).
    /// </summary>
    /// <typeparam name="TEntity">The owner entity type.</typeparam>
    /// <typeparam name="TRelatedEntity">The owned entity type.</typeparam>
    /// <param name="builder">The owned entity navigation builder.</param>
    public static void ConfigureInline<TEntity, TRelatedEntity>(OwnedNavigationBuilder<TEntity, TRelatedEntity> builder)
        where TEntity : class
        where TRelatedEntity : class, IOwnedEntity<TRelatedEntity>, new()
    {
        var entity = new TRelatedEntity();
        entity.ConfigureOwnership(builder);
    }

    /// <summary>
    /// Configures the entity as an owned entity in a different table.
    /// </summary>
    /// <typeparam name="TEntity">The owner entity type.</typeparam>
    /// <typeparam name="TRelatedEntity">The owned entity type.</typeparam>
    /// <typeparam name="TEntityKey">The type of the primary key of <typeparamref name="TEntity"/>.</typeparam>
    /// <param name="builder">The owned entity navigation builder.</param>
    public static void ConfigureMany<TEntity, TRelatedEntity, TEntityKey>(OwnedNavigationBuilder<TEntity, TRelatedEntity> builder)
        where TEntity : class
        where TRelatedEntity : class, IOwnedEntity<TRelatedEntity>, new()
        where TEntityKey : notnull
    {
        ConfigureMany<TEntity, TRelatedEntity, TEntityKey>(builder, Entity.GetDefaultTableName<TEntity>() + Entity.GetDefaultTableName<TRelatedEntity>());
    }

    /// <summary>
    /// Configures the entity as an owned entity in a different table.
    /// </summary>
    /// <typeparam name="TEntity">The owner entity type.</typeparam>
    /// <typeparam name="TRelatedEntity">The owned entity type.</typeparam>
    /// <typeparam name="TEntityKey">The type of the primary key of <typeparamref name="TEntity"/>.</typeparam>
    /// <param name="builder">The owned entity navigation builder.</param>
    /// <param name="tableName">The name of the table to save the owned entities to.</param>
    public static void ConfigureMany<TEntity, TRelatedEntity, TEntityKey>(OwnedNavigationBuilder<TEntity, TRelatedEntity> builder, string tableName)
        where TEntity : class
        where TRelatedEntity : class, IOwnedEntity<TRelatedEntity>, new()
        where TEntityKey : notnull
    {
        builder.ToTable(tableName);

        builder.Property<Guid>("Id")
            .ValueGeneratedNever()
            .HasValueGenerator<GuidValueGenerator>();
        builder.HasKey("Id");
        builder.Property<TEntityKey>("OwnerId");
        builder.HasIndex("OwnerId");
        builder.WithOwner().HasForeignKey("OwnerId");

        var entity = new TRelatedEntity();
        entity.ConfigureOwnership(builder);
    }
}
