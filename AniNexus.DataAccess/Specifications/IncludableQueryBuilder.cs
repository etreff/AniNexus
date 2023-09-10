using System.Linq.Expressions;

namespace AniNexus.DataAccess.Specifications;

internal sealed class IncludableQueryBuilder<TModel, TProperty> : IIncludableQueryBuilder<TModel, TProperty>
{
    private readonly string _memberName;
    private readonly List<string> _includeStrings;

    public IncludableQueryBuilder(string memberName, List<string> includeStrings)
    {
        _memberName = memberName;
        _includeStrings = includeStrings;
    }

    public IIncludableQueryBuilder<TModel, TProperty> AddInclude(Expression<Func<TModel, TProperty>> include)
    {
        string memberName = ((MemberExpression)include.Body).Member.Name;
        string path = $"{_memberName}.{memberName}";

        _includeStrings.Add(path);

        return new IncludableQueryBuilder<TModel, TProperty>(_memberName, _includeStrings);
    }

    public IIncludableQueryBuilder<TModel, TProperty> AddInclude(Expression<Func<TModel, IList<TProperty>>> include)
    {
        string memberName = ((MemberExpression)include.Body).Member.Name;
        string path = $"{_memberName}.{memberName}";

        _includeStrings.Add(path);

        return new IncludableQueryBuilder<TModel, TProperty>(_memberName, _includeStrings);
    }

    public IIncludableQueryBuilder<TProperty, TSubProperty> ThenInclude<TSubProperty>(Expression<Func<TProperty, TSubProperty>> include)
    {
        string memberName = ((MemberExpression)include.Body).Member.Name;
        string path = $"{_memberName}.{memberName}";

        _includeStrings.Add(path);

        return new IncludableQueryBuilder<TProperty, TSubProperty>(path, _includeStrings);
    }

    public IIncludableQueryBuilder<TProperty, TSubProperty> ThenInclude<TSubProperty>(Expression<Func<TProperty, IList<TSubProperty>>> include)
    {
        string memberName = ((MemberExpression)include.Body).Member.Name;
        string path = $"{_memberName}.{memberName}";

        _includeStrings.Add(path);

        return new IncludableQueryBuilder<TProperty, TSubProperty>(path, _includeStrings);
    }
}
