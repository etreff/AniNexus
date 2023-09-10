using System.Linq.Expressions;

namespace AniNexus.DataAccess.Specifications;

/// <summary>
/// Defines a query specification.
/// </summary>
public interface IQuerySpecification<TModel>
	where TModel : class
{
	/// <summary>
	/// The search criteria - the WHERE clause.
	/// </summary>
	Expression<Func<TModel, bool>> Criteria { get; }

	/// <summary>
	/// The sub-objects to include in the result.
	/// </summary>
	IReadOnlyList<Expression<Func<TModel, object>>> Includes { get; }

	/// <summary>
	/// The sub-object paths to include in the result.
	/// </summary>
	IReadOnlyList<string> IncludeStrings { get; }

	/// <summary>
	/// Applies the specification to the specified query.
	/// </summary>
	/// <param name="query">The query to apply the specification to.</param>
	/// <returns>A new query with the specification applied.</returns>
	IQueryable<TModel> ApplyTo(IQueryable<TModel> query);

	/// <summary>
	/// Returns whether the model satisfies the specification.
	/// </summary>
	/// <param name="model">The model to check.</param>
	bool Satisfies(TModel model);

	/// <summary>
	/// Creates a query specification which applies a logical AND to this
	/// and the specified specification.
	/// </summary>
	/// <param name="other">The specification to apply the logical AND to.</param>
	IQuerySpecification<TModel> And(IQuerySpecification<TModel> other);

	/// <summary>
	/// Creates a query specification which applies a logical OR to this
	/// and the specified specification.
	/// </summary>
	/// <param name="other">The specification to apply the logical OR to.</param>
	IQuerySpecification<TModel> Or(IQuerySpecification<TModel> other);
}