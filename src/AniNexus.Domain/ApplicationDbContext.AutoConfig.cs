using AniNexus.Domain.Models;
using AniNexus.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.Linq.Expressions;
using System.Reflection;

namespace AniNexus.Domain
{
    public partial class ApplicationDbContext
    {
        private void AutoConfigureProperties(ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                // Find get-only properties and exclude them from navigation property discovery by convention.
                // If we do not do this the navigation builder will complain about multiple relationships.
                // There is likely some convention around get-only properties, but we do not use that convention
                // here. If a model wants to explicitly create a property over a get-only property they are
                // free to do so.
                foreach (var property in entityType.ClrType.GetProperties().Where(p => !p.CanWrite).ToArray())
                {
                    entityType.AddIgnored(property.Name);
                }

                // Configure interface types.
                if (entityType.ClrType.IsTypeOf<IHasGuidPK>())
                {
                    var idProperty = entityType.FindProperty(nameof(IHasGuidPK.Id))!;
                    idProperty.ValueGenerated = ValueGenerated.Never;
                    idProperty.SetValueGeneratorFactory(static (_, _) => new GuidValueGenerator());
                    entityType.SetPrimaryKey(idProperty)!.SetIsClustered(false);
                }

                if (entityType.ClrType.IsTypeOf<IHasAudit>())
                {
                    var dateAddedProperty = entityType.FindProperty(nameof(IHasAudit.DateAdded))!;
                    dateAddedProperty.SetComment("The date the entry was created.");
                    dateAddedProperty.IsNullable = false;
                    dateAddedProperty.SetComputedColumnSql("getutcdate()");
                    dateAddedProperty.ValueGenerated = ValueGenerated.OnAdd;

                    var dateUpdatedProperty = entityType.FindProperty(nameof(IHasAudit.DateUpdated))!;
                    dateUpdatedProperty.SetComment("The date the entry was last updated.");
                    dateUpdatedProperty.IsNullable = false;
                    dateUpdatedProperty.SetComputedColumnSql("getutcdate()");
                    dateUpdatedProperty.ValueGenerated = ValueGenerated.OnAddOrUpdate;
                }

                if (entityType.ClrType.IsTypeOf<IHasRowVersion>())
                {
                    var rowVersionProperty = entityType.FindProperty(nameof(IHasRowVersion.RowVersion))!;
                    rowVersionProperty.IsConcurrencyToken = true;
                    rowVersionProperty.ValueGenerated = ValueGenerated.OnAddOrUpdate;
                }

                if (entityType.ClrType.IsTypeOf<IHasSoftDelete>())
                {
                    var isSoftDeleteProperty = entityType.FindProperty(nameof(IHasSoftDelete.IsSoftDeleted))!;
                    isSoftDeleteProperty.SetComment("Whether this entity is soft deleted.");
                    isSoftDeleteProperty.IsNullable = false;

                    AddSoftDeleteFilter(entityType);
                }

                // Configure property-specific conventions.
                foreach (var entityProperty in entityType.GetProperties())
                {
                    // Configure nullability and ignored properties.
                    if (entityProperty.PropertyInfo is not null)
                    {
                        // Do a double check. The model creator seems to only pick up writeable properties,
                        // but that behavior may not be the case in the future.
                        bool isReadOnlyProperty = entityProperty.PropertyInfo.GetSetMethod() is null;
                        if (isReadOnlyProperty)
                        {
                            entityType.AddIgnored(entityProperty.PropertyInfo.Name);
                        }
                        else
                        {
                            entityProperty.IsNullable = entityProperty.PropertyInfo.IsNullable();
                        }
                    }

                    // Configure unicode and collation for string types.
                    if (entityProperty.ClrType == typeof(string))
                    {
                        // Default to English ASCII characters.
                        entityProperty.SetIsUnicode(false);

                        // Descriptors should be unicode since they can contain foreign characters.
                        if (entityProperty.Name.StartsWith("Native") ||
                            entityProperty.Name.Equals("Synopsis") ||
                            entityProperty.Name.Equals("Description") ||
                            entityProperty.Name.Equals("Comment"))
                        {
                            entityProperty.SetIsUnicode(true);
                            entityProperty.SetCollation(Collation.Japanese);
                        }
                    }
                }
            }

            static void AddSoftDeleteFilter(IMutableEntityType entityType)
            {
                var method = typeof(ApplicationDbContext)
                    .GetMethod(nameof(SoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(entityType.ClrType);
                var filter = method.Invoke(null, null) as LambdaExpression;
                entityType.SetQueryFilter(filter!);
                entityType.AddIndex(entityType.FindProperty(nameof(IHasSoftDelete.IsSoftDeleted))!);
            }
        }

        private void FixSoftDeleteQueryFiltersOnNavigationProperties(ModelBuilder builder)
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

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                // If a query filter already exists for the model we will return early.
                // Entities can only have a single filter.
                //
                // In theory, since the query filter is an expression, we could combine it with Expression.And() to
                // combine our filters. In this case we would need to extract the parameter expression from the existing
                // lambda and use that instead of creating our own. This adds too much complexity right now, so we are
                // going to skip that for now.
                if (entityType.GetQueryFilter() is not null)
                {
                    continue;
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
                    Expression expression = isNotSoftDeletedExpressions[0];

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
        }

        private static LambdaExpression SoftDeleteFilter<TEntity>()
            where TEntity : class, IHasSoftDelete
        {
            return (Expression<Func<TEntity, bool>>)(entity => !entity.IsSoftDeleted);
        }
    }
}
