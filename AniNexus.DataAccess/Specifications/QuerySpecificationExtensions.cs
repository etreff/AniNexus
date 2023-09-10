using Microsoft.EntityFrameworkCore;
using AniNexus.DataAccess.Specifications;

namespace AniNexus.DataAccess.Specifications;

/// <summary>
/// Extensions for <see cref="IQueryable{T}"/> to enable the query specification pattern.
/// </summary>
public static class QuerySpecificationExtensions
{
    /// <summary>
    ///     Asynchronously creates a <see cref="List{T}" /> from an <see cref="IQueryable{T}" /> by enumerating it
    ///     asynchronously, applying the specification to filter results.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Multiple active operations on the same context instance are not supported. Use <see langword="await" /> to ensure
    ///         that any asynchronous operations have completed before calling another method on this context.
    ///         See <see href="https://aka.ms/efcore-docs-threading">Avoiding DbContext threading issues</see> for more information and examples.
    ///     </para>
    ///     <para>
    ///         See <see href="https://aka.ms/efcore-docs-async-linq">Querying data with EF Core</see> for more information and examples.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}" /> to create a list from.</param>
    /// <param name="specification">The specification to apply.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public static Task<List<TSource>> ToListAsync<TSource>(this IQueryable<TSource> source, IQuerySpecification<TSource> specification, CancellationToken cancellationToken = default)
        where TSource : class
    {
        ArgumentNullException.ThrowIfNull(source);

        return source.WithSpecification(specification).ToListAsync(cancellationToken);
    }

    /// <summary>
    ///     Asynchronously creates an array from an <see cref="IQueryable{T}" /> by enumerating it
    ///     asynchronously, applying the specification to filter results.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Multiple active operations on the same context instance are not supported. Use <see langword="await" /> to ensure
    ///         that any asynchronous operations have completed before calling another method on this context.
    ///         See <see href="https://aka.ms/efcore-docs-threading">Avoiding DbContext threading issues</see> for more information and examples.
    ///     </para>
    ///     <para>
    ///         See <see href="https://aka.ms/efcore-docs-async-linq">Querying data with EF Core</see> for more information and examples.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}" /> to create a list from.</param>
    /// <param name="specification">The specification to apply.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains an array that contains elements from the input sequence.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public static Task<TSource[]> ToArrayAsync<TSource>(this IQueryable<TSource> source, IQuerySpecification<TSource> specification, CancellationToken cancellationToken = default)
        where TSource : class
    {
        ArgumentNullException.ThrowIfNull(source);

        return source.WithSpecification(specification).ToArrayAsync(cancellationToken);
    }

    /// <summary>
    /// Returns a query that applies a specification to the data set.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}" /> to create a query from.</param>
    /// <param name="specification">The specification to apply.</param>
    /// <returns>A query built using the provided specification.</returns>
    public static IQueryable<TSource> WithSpecification<TSource>(this IQueryable<TSource> source, IQuerySpecification<TSource> specification)
        where TSource : class
    {
        return specification.ApplyTo(source);
    }
}
