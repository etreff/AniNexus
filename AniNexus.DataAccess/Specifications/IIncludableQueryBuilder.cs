using System.Linq.Expressions;

namespace AniNexus.DataAccess.Specifications;

/// <summary>
/// Defines a builder that constructs queryable include statements.
/// </summary>
/// <typeparam name="TModel">The model type.</typeparam>
/// <typeparam name="TProperty">The property type to include.</typeparam>
public interface IIncludableQueryBuilder<TModel, TProperty>
{
    /// <summary>
    /// Includes a property in the query.
    /// </summary>
    /// <param name="include">The sub-property to include.</param>
    IIncludableQueryBuilder<TModel, TProperty> AddInclude(Expression<Func<TModel, TProperty>> include);

    /// <summary>
    /// Includes a property in the query.
    /// </summary>
    /// <param name="include">The sub-property to include.</param>
    IIncludableQueryBuilder<TModel, TProperty> AddInclude(Expression<Func<TModel, IList<TProperty>>> include);

    /// <summary>
    /// Includes a sub-property in the query.
    /// </summary>
    /// <typeparam name="TSubProperty">The type of the sub-property.</typeparam>
    /// <param name="include">The sub-property to include.</param>
    IIncludableQueryBuilder<TProperty, TSubProperty> ThenInclude<TSubProperty>(Expression<Func<TProperty, TSubProperty>> include);

    /// <summary>
    /// Includes a sub-property in the query.
    /// </summary>
    /// <typeparam name="TSubProperty">The type of the sub-property.</typeparam>
    /// <param name="include">The sub-property to include.</param>
    IIncludableQueryBuilder<TProperty, TSubProperty> ThenInclude<TSubProperty>(Expression<Func<TProperty, IList<TSubProperty>>> include);
}
