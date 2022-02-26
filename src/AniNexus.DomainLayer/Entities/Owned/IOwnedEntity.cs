namespace AniNexus.Domain.Entities
{
    using AniNexus.Domain.Validation;

    /// <summary>
    /// Defines an owned entity type.
    /// </summary>
    public interface IOwnedEntity<TEntity> : IEntity
        where TEntity : class, IOwnedEntity<TEntity>
    {
        /// <summary>
        /// Validates the <typeparamref name="TEntity"/> instance.
        /// </summary>
        /// <param name="validator">The entity validator.</param>
        void Validate(ValidationBuilder<TEntity> validator);
    }
}

namespace AniNexus.Domain.Validation
{
    using AniNexus.Domain.Entities;

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
                if (context.CheckNullability() || context.Value is null)
                {
                    return;
                }

                var ownedEntity = context.Value;
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
