using System.Linq.Expressions;
using System.Reflection;
using AniNexus.Data.Entities;
using AniNexus.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AniNexus.Data.Conventions.Entities;

/// <summary>
/// A convention that automatically configures entities that implement <see cref="IHasSoftDelete"/>.
/// </summary>
internal sealed class HasSoftDeleteConvention : PrePostConfigureEntityConvention<IHasSoftDelete>
{
    private static MethodInfo? _softDeleteFilterMethod;

    protected override void OnPreConfigure(ModelBuilder builder, IMutableEntityType entityType)
    {
        var isSoftDeleteProperty = entityType.FindProperty(nameof(IHasSoftDelete.IsSoftDeleted))!;
        isSoftDeleteProperty.SetComment("Whether this entity is soft deleted.");
        isSoftDeleteProperty.IsNullable = false;
        isSoftDeleteProperty.SetDefaultValue(false);

        AddSoftDeleteFilter(entityType);
    }

    protected override void OnPostConfigure(ModelBuilder builder, IMutableEntityType entityType)
    {
        /**
         * This code attempts to fix the following error:
         *
         * Entity '{PrincipalModelName}' has a global query filter defined and is the required end of a relationship
         * with the entity '{DependentModelName}'. This may lead to unexpected results when the required entity is
         * filtered out. Either configure the navigation as optional, or define matching query filters for both entities
         * in the navigation. See https://go.microsoft.com/fwlink/?linkid=2131316 for more information.
         *
         * This logic does so by adding a query filter automagically to models that contain navigation properties
         * that implement <see cref="IHasSoftDelete"/>.
         */

        // If a query filter already exists for the model we will return early.
        // Entities can only have a single filter.
        //
        // In theory, since the query filter is an expression, we could combine it with Expression.And() to
        // combine our filters. In this case we would need to extract the parameter expression from the existing
        // lambda and use that instead of creating our own. This adds too much complexity right now, so we are
        // going to skip that.
        if (entityType.GetQueryFilter() is not null)
        {
            return;
        }

        /* We are creating the following expression:
         * 
         * (map) => !map.{NavigationProperty1}.IsSoftDeleted && !map.{NavigationProperty2}.IsSoftDeleted && ... && !map.{NavigationPropertyN}.IsSoftDeleted;                 * 
         */

        var isNotSoftDeletedExpressions = new List<Expression>();

        // (map)
        var lambdaParameter = Expression.Parameter(entityType.ClrType, "map");

        foreach (var navigationProperty in entityType.GetNavigations().Where(n => n.TargetEntityType.ClrType.IsTypeOf<IHasSoftDelete>()))
        {
            // map.{NavigationProperty}
            var navigationPropertyExpression = Expression.Property(lambdaParameter, navigationProperty.PropertyInfo!);

            // map.{NavigationProperty}.IsSoftDeleted
            var isSoftDeletedExpression = Expression.Property(navigationPropertyExpression, navigationProperty.ClrType, nameof(IHasSoftDelete.IsSoftDeleted));

            // !map.{NavigationProperty}.IsSoftDeleted
            var negationExpression = Expression.Not(isSoftDeletedExpression);

            // Add the check to the list.
            isNotSoftDeletedExpressions.Add(negationExpression);
        }

        // If there is at least 1 IHasSoftDelete navigation property...
        if (isNotSoftDeletedExpressions.Count != 0)
        {
            // Take the first one and assign it as the root.
            var expression = isNotSoftDeletedExpressions[0];

            // If we have more than 1, we need to && them together.
            if (isNotSoftDeletedExpressions.Count > 1)
            {
                foreach (var isNotSoftDeletedExpression in isNotSoftDeletedExpressions.Skip(1))
                {
                    // {ExistingExpression} && !map.{NavigationProperty#}.IsSoftDeleted
                    expression = Expression.AndAlso(expression, isNotSoftDeletedExpression);
                }
            }

            // Create the lambda.
            var lambda = Expression.Lambda(expression, lambdaParameter);

            // Set the query filter on the entity type.
            entityType.SetQueryFilter(lambda);
        }
    }

    private static void AddSoftDeleteFilter(IMutableEntityType entityType)
    {
        var method = _softDeleteFilterMethod ??= typeof(HasSoftDeleteConvention).GetMethod(nameof(SoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)!;

        var m = method.MakeGenericMethod(entityType.ClrType);
        var filter = m.Invoke(null, null) as LambdaExpression;
        entityType.SetQueryFilter(filter);
        entityType.AddIndex(entityType.FindProperty(nameof(IHasSoftDelete.IsSoftDeleted))!).SetIsClustered(false);
    }
    private static LambdaExpression SoftDeleteFilter<TEntity>()
        where TEntity : class, IHasSoftDelete
    {
        return (Expression<Func<TEntity, bool>>)(entity => !entity.IsSoftDeleted);
    }
}
