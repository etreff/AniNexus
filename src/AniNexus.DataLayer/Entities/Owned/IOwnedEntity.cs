namespace AniNexus.Data.Entities
{
    using AniNexus.Data.Validation;
    using Microsoft.Toolkit.Diagnostics;

    /// <summary>
    /// Defines an owned entity type.
    /// </summary>
    public interface IOwnedEntity<TOwnedEntity, TOwnerKey> : IEntity
        where TOwnedEntity : class, IOwnedEntity<TOwnedEntity, TOwnerKey>
        where TOwnerKey : struct, IComparable<TOwnerKey>, IEquatable<TOwnerKey>
    {
        /// <summary>
        /// The Id of this entity.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// The Id of the entity that owns this entity.
        /// </summary>
        TOwnerKey OwnerId { get; set; }

        /// <summary>
        /// Validates the <typeparamref name="TOwnedEntity"/> instance.
        /// </summary>
        /// <param name="validator">The entity validator.</param>
        void Validate(ValidationBuilder<TOwnedEntity> validator);
    }

    /// <summary>
    /// Defines an owned entity type.
    /// </summary>
    public interface IOwnedEntity<TOwnedEntity> : IOwnedEntity<TOwnedEntity, Guid>
        where TOwnedEntity : class, IOwnedEntity<TOwnedEntity>
    {
    }

    /// <summary>
    /// The base class for an owned entity.
    /// </summary>
    /// <typeparam name="TOwnedEntity">The owned entity type.</typeparam>
    /// <typeparam name="TKey">The type of the primary key of the owner entity.</typeparam>
    public abstract class OwnedEntity<TOwnedEntity, TKey> : IOwnedEntity<TOwnedEntity, TKey>
        where TOwnedEntity : OwnedEntity<TOwnedEntity, TKey>
        where TKey : struct, IComparable<TKey>, IEquatable<TKey>
    {
        /// <summary>
        /// The Id of this entity.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The Id of the entity that owns this entity.
        /// </summary>
        public TKey OwnerId { get; set; }

        /// <summary>
        /// Configures the <see cref="OwnedEntity{TOwnedEntity, TKey}"/>.
        /// </summary>
        /// <typeparam name="TOwnerEntity">The type of the owner entity.</typeparam>
        /// <param name="builder">The owned type builder.</param>
        /// <param name="createTable">Whether to create table.</param>
        /// <param name="tableName">The table name to use. If left <see langword="null"/>, one will be generated based off of the entity names.</param>
        public void Configure<TOwnerEntity>(OwnedNavigationBuilder<TOwnerEntity, TOwnedEntity> builder, bool createTable = true, string? tableName = null)
            where TOwnerEntity : class, IEntity
        {
            Guard.IsNotNull(builder, nameof(builder));

            if (createTable)
            {
                if (string.IsNullOrWhiteSpace(tableName))
                {
                    tableName = GetTableName(typeof(TOwnerEntity), Entity.GetDefaultTableName<TOwnerEntity>());
                }

                builder.ToTable(tableName);
            }

            builder.HasKey(m => m.Id);
            builder.HasIndex(m => m.OwnerId);
            builder.WithOwner().HasForeignKey(m => m.OwnerId);

            ConfigureEntity(builder);
        }

        /// <summary>
        /// Validates the <typeparamref name="TOwnedEntity"/> instance.
        /// </summary>
        /// <param name="validator">The entity validator.</param>
        public virtual void Validate(ValidationBuilder<TOwnedEntity> validator)
        {
        }

        /// <summary>
        /// Gets the table name for this owned entity if one had to be created.
        /// </summary>
        /// <param name="ownerType">The type of the owner.</param>
        /// <param name="ownerEntityTableName">The table name of the entity that owns this entity.</param>
        protected virtual string GetTableName(Type ownerType, string ownerEntityTableName)
        {
            return ownerEntityTableName + Entity.GetDefaultTableName<TOwnedEntity>();
        }

        /// <summary>
        /// Configures the owned entity.
        /// </summary>
        /// <param name="builder">The owned type builder.</param>
        protected abstract void ConfigureEntity<TOwnerEntity>(OwnedNavigationBuilder<TOwnerEntity, TOwnedEntity> builder)
            where TOwnerEntity : class, IEntity;
    }

    /// <summary>
    /// The base class for an owned entity.
    /// </summary>
    /// <typeparam name="TEntity">The owned entity type.</typeparam>
    public abstract class OwnedEntity<TEntity> : OwnedEntity<TEntity, Guid>, IOwnedEntity<TEntity>
        where TEntity : OwnedEntity<TEntity>
    {
    }

    /// <summary>
    /// <see cref="OwnedNavigationBuilder{TOwnerEntity, TDependentEntity}"/> extensions.
    /// </summary>
    public static partial class OwnedNavigationBuilderExtensions
    {
        /// <summary>
        /// Configures an owned entity.
        /// </summary>
        /// <typeparam name="TOwnerEntity">The owner type.</typeparam>
        /// <typeparam name="TOwnedEntity">The owned type.</typeparam>
        /// <param name="builder">The owned navigation builder.</param>
        /// <param name="createTable">Whether to create table.</param>
        /// <param name="tableName">The table name to use. If left <see langword="null"/>, one will be generated based off of the entity names.</param>
        public static OwnedNavigationBuilder<TOwnerEntity, TOwnedEntity> ConfigureOwnedEntity<TOwnerEntity, TOwnedEntity>(this OwnedNavigationBuilder<TOwnerEntity, TOwnedEntity> builder, bool createTable = true, string? tableName = null)
            where TOwnerEntity : class, IEntity
            where TOwnedEntity : OwnedEntity<TOwnedEntity>, new()
        {
            var configuration = new TOwnedEntity();
            configuration.Configure(builder, createTable, tableName);

            return builder;
        }
    }
}

namespace AniNexus.Data.Validation
{
    using AniNexus.Data.Entities;

    public static partial class PropertyValidatorBuilderMethods
    {
        /// <summary>
        /// Validates a <see cref="IOwnedEntity{T}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity being validated.</typeparam>
        /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
        /// <param name="builder">The property validator builder.</param>
        public static IPropertyValidatorBuilder<TEntity, TProperty?> ValidateOwnedEntity<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder)
            where TEntity : class, IEntity
            where TProperty : class, IOwnedEntity<TProperty>
        {
            return builder.AddValidationRule(context =>
            {
                var ownedEntity = context.Value!;
                var validator = new ValidationBuilder<TProperty>(ownedEntity, context.FullPropertyName);
                ownedEntity.Validate(validator);
            });
        }

        /// <summary>
        /// Validates a <see cref="IOwnedEntity{T}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity being validated.</typeparam>
        /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
        /// <param name="builder">The property validator builder.</param>
        public static IPropertyValidatorBuilder<TEntity, IList<TProperty?>?> ValidateOwnedEntities<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, IList<TProperty?>?> builder)
            where TEntity : class, IEntity
            where TProperty : class, IOwnedEntity<TProperty>
        {
            return builder.AddValidationRule(context => context.ForEach((v, e) => e.Validate(v)));
        }

        /// <summary>
        /// Validates a <see cref="IOwnedEntity{T}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity being validated.</typeparam>
        /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
        /// <param name="builder">The property validator builder.</param>
        public static IPropertyValidatorBuilder<TEntity, TProperty?[]?> ValidateOwnedEntities<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?[]?> builder)
            where TEntity : class, IEntity
            where TProperty : class, IOwnedEntity<TProperty>
        {
            return builder.AddValidationRule(context => context.ForEach((v, e) => e.Validate(v)));
        }
    }
}
