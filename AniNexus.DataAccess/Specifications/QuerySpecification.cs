using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace AniNexus.DataAccess.Specifications;

/// <summary>
/// The base class for a query specification.
/// </summary>
/// <typeparam name="TModel">The entity type.</typeparam>
public abstract class QuerySpecification<TModel> : IQuerySpecification<TModel>
    where TModel : class
{
    /// <inheritdoc/>
    public Expression<Func<TModel, bool>> Criteria { get; }

    /// <inheritdoc/>
    public IReadOnlyList<Expression<Func<TModel, object>>> Includes { get; }

    /// <inheritdoc/>
    public IReadOnlyList<string> IncludeStrings { get; }

    /// <summary>
    /// Whether to ignore global query filters.
    /// </summary>
    protected bool IgnoreQueryFilters { get; set; }

    /// <summary>
    /// Whether to split the query.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Split queries should <strong>not</strong> be enabled if the sort is not on a unique column
    /// since the order is not guaranteed between the queries.
    /// </para>
    /// <para>
    /// https://learn.microsoft.com/en-us/ef/core/querying/single-split-queries#split-queries
    /// </para>
    /// </remarks>
    protected bool SplitQuery { get; set; }

    private readonly List<Expression<Func<TModel, object>>> _includes = new();
    private readonly List<string> _includeStrings = new();

    /// <summary>
    /// Creates a new <see cref="QuerySpecification{TModel}"/> instance.
    /// </summary>
    /// <param name="criteria">The contents of the WHERE clause for the specification.</param>
    protected QuerySpecification(Expression<Func<TModel, bool>> criteria)
    {
        Criteria = criteria;
        Includes = _includes.AsReadOnly();
        IncludeStrings = _includeStrings.AsReadOnly();
    }

    /// <summary>
    /// Adds a navigation property to the results.
    /// </summary>
    /// <param name="include">The property to include in the results.</param>
    protected IIncludableQueryBuilder<TModel, TProperty> AddInclude<TProperty>(Expression<Func<TModel, TProperty>> include)
    {
        string memberName = ((MemberExpression)include.Body).Member.Name;
        AddInclude(memberName, typeof(TProperty));

        return new IncludableQueryBuilder<TModel, TProperty>(memberName, _includeStrings);
    }

    /// <summary>
    /// Adds a navigation property to the results.
    /// </summary>
    /// <param name="include">The property to include in the results.</param>
    protected IIncludableQueryBuilder<TModel, TProperty> AddInclude<TProperty>(Expression<Func<TModel, IList<TProperty>>> include)
    {
        string memberName = ((MemberExpression)include.Body).Member.Name;
        AddInclude(memberName, typeof(TProperty));

        return new IncludableQueryBuilder<TModel, TProperty>(memberName, _includeStrings);
    }

    private void AddInclude(string memberName, Type propertyType)
    {
        var param = Expression.Parameter(propertyType);
        var property = Expression.Property(param, memberName);

        Expression expression = property;
        // If ValueType, we need to convert.
        if (propertyType.IsValueType)
        {
            expression = Expression.Convert(property, typeof(object));
        }

        var i = Expression.Lambda<Func<TModel, object>>(expression, param);

        _includes.Add(i);
    }

    /// <summary>
    /// Adds a navigation property to the results.
    /// </summary>
    /// <param name="include">The navigation path to include in the results.</param>
    protected void AddInclude(string include)
    {
        _includeStrings.Add(include);
    }

    /// <inheritdoc />
    public IQueryable<TModel> ApplyTo(IQueryable<TModel> query)
    {
        // Add includes.
        var q = Includes.Aggregate(query, (current, include) => current.Include(include));

        // Add string includes.
        q = IncludeStrings.Aggregate(q, (current, include) => current.Include(include));

        // Check if we should ignore query filters.
        if (IgnoreQueryFilters)
        {
            q = q.IgnoreQueryFilters();
        }

        // Return the result using the criteria expression.
        return q.Where(Criteria);
    }

    /// <inheritdoc />
    public bool Satisfies(TModel model)
    {
        var predicate = Criteria.Compile();
        return predicate(model);
    }

    /// <inheritdoc />
    public IQuerySpecification<TModel> And(IQuerySpecification<TModel> other)
    {
        return And(this, other);
    }

    /// <summary>
    /// Creates a query specification which applies a logical AND to two specifications.
    /// </summary>
    /// <param name="left">The first specification to apply the logical AND to.</param>
    /// <param name="right">The second specification to apply the logical AND to.</param>
    public static IQuerySpecification<TModel> And(IQuerySpecification<TModel> left, IQuerySpecification<TModel> right)
    {
        var l = left as QuerySpecification<TModel>;
        var r = right as QuerySpecification<TModel>;

        var specification = new AndSpecification(left, right)
        {
            IgnoreQueryFilters = l?.IgnoreQueryFilters == true || r?.IgnoreQueryFilters == true,
            SplitQuery = l?.SplitQuery == true || r?.SplitQuery == true
        };

        specification._includes.AddRange(left.Includes);
        specification._includes.AddRange(right.Includes);

        specification._includeStrings.AddRange(left.IncludeStrings);
        specification._includeStrings.AddRange(right.IncludeStrings);

        return specification;
    }

    /// <inheritdoc />
    public IQuerySpecification<TModel> Or(IQuerySpecification<TModel> other)
    {
        return Or(this, other);
    }

    /// <summary>
    /// Creates a query specification which applies a logical OR to two specifications.
    /// </summary>
    /// <param name="left">The first specification to apply the logical OR to.</param>
    /// <param name="right">The second specification to apply the logical OR to.</param>
    public static IQuerySpecification<TModel> Or(IQuerySpecification<TModel> left, IQuerySpecification<TModel> right)
    {
        var l = left as QuerySpecification<TModel>;
        var r = right as QuerySpecification<TModel>;

        var specification = new OrSpecification(left, right)
        {
            IgnoreQueryFilters = l?.IgnoreQueryFilters == true || r?.IgnoreQueryFilters == true,
            SplitQuery = l?.SplitQuery == true || r?.SplitQuery == true
        };

        specification._includes.AddRange(left.Includes);
        specification._includes.AddRange(right.Includes);

        specification._includeStrings.AddRange(left.IncludeStrings);
        specification._includeStrings.AddRange(right.IncludeStrings);

        return specification;
    }

    /// <summary>
    /// A parameter rewriter that will avoid exceptions if parameters are not replaced correctly.
    /// </summary>
    private sealed class ParameterReplacer(ParameterExpression parameterExpression) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return base.VisitParameter(parameterExpression);
        }
    }

    private class AndSpecification : QuerySpecification<TModel>
    {
        public AndSpecification(IQuerySpecification<TModel> left, IQuerySpecification<TModel> right)
            : base(CreateExpression(left, right))
        {
        }

        private static Expression<Func<TModel, bool>> CreateExpression(IQuerySpecification<TModel> left, IQuerySpecification<TModel> right)
        {
            var leftCriteria = left.Criteria;
            var rightCriteria = right.Criteria;

            var paramExpression = Expression.Parameter(typeof(TModel));
            var expressionBody = new ParameterReplacer(paramExpression).Visit(Expression.AndAlso(leftCriteria, rightCriteria));

            return Expression.Lambda<Func<TModel, bool>>(expressionBody, paramExpression);
        }
    }

    private class OrSpecification : QuerySpecification<TModel>
    {
        public OrSpecification(IQuerySpecification<TModel> left, IQuerySpecification<TModel> right)
            : base(CreateExpression(left, right))
        {
        }

        private static Expression<Func<TModel, bool>> CreateExpression(IQuerySpecification<TModel> left, IQuerySpecification<TModel> right)
        {
            var leftCriteria = left.Criteria;
            var rightCriteria = right.Criteria;

            var paramExpression = Expression.Parameter(typeof(TModel));
            var expressionBody = new ParameterReplacer(paramExpression).Visit(Expression.OrElse(leftCriteria, rightCriteria));

            return Expression.Lambda<Func<TModel, bool>>(expressionBody, paramExpression);
        }
    }
}
