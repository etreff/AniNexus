using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Dataflow;
using AniNexus;

namespace System.Collections.Generic;

/// <summary>
/// Collection extensions.
/// </summary>
public static partial class CollectionExtensions
{
    /// <summary>
    /// A random number generator to use for extension methods.
    /// </summary>
    private static readonly IRandomNumberProvider _randomNumberGenerator = new MersenneTwisterRandom();

    /// <summary>
    /// Adds a collection of elements to the collection.
    /// </summary>
    /// <typeparam name="T">The type of the element to add.</typeparam>
    /// <param name="collection">The collection to add to.</param>
    /// <param name="elements">The elements to add to the collection.</param>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/></exception>
    /// <exception cref="NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</exception>
    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T>? elements)
    {
        Guard.IsNotNull(collection, nameof(collection));

        if (elements is null)
        {
            return;
        }

        foreach (var element in elements)
        {
            collection.Add(element);
        }
    }

    /// <summary>
    /// Adds a collection of elements to the collection where the element matches the specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of the element to add.</typeparam>
    /// <param name="collection">The collection to add to.</param>
    /// <param name="elements">The elements to add to the collection.</param>
    /// <param name="predicate">The predicate that must be passed for the item to be added to the collection. If null, all elements will be added.</param>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/></exception>
    /// <exception cref="NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</exception>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T>? elements, Predicate<T> predicate)
    {
        Guard.IsNotNull(collection, nameof(collection));
        Guard.IsNotNull(predicate, nameof(predicate));

        if (elements is null)
        {
            return;
        }

        foreach (var element in elements)
        {
            if (predicate(element))
            {
                collection.Add(element);
            }
        }
    }

    /// <summary>
    /// Adds the specified elements to the collection as long as the collection does not already contain the elements.
    /// </summary>
    /// <typeparam name="T">The type of the element to add.</typeparam>
    /// <param name="collection">The collection to add to.</param>
    /// <param name="values">The values to add.</param>
    /// <returns>The number of elements added.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/></exception>
    /// <exception cref="NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</exception>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AddRangeUnique<T>(this ICollection<T> collection, IEnumerable<T>? values)
    {
        return AddRangeUnique(collection, values?.ToArray());
    }

    /// <summary>
    /// Adds the specified elements to the collection as long as the collection does not already contain the elements.
    /// </summary>
    /// <typeparam name="T">The type of the element to add.</typeparam>
    /// <param name="collection">The collection to add to.</param>
    /// <param name="values">The values to add.</param>
    /// <returns>The number of elements added.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/></exception>
    /// <exception cref="NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</exception>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AddRangeUnique<T>(this ICollection<T> collection, params T[]? values)
    {
        return AddRangeUnique(collection, values, (Func<T, T, bool>?)null);
    }

    /// <summary>
    /// Adds the specified elements to the collection as long as the collection does not already contain the elements.
    /// </summary>
    /// <typeparam name="T">The type of the element to add.</typeparam>
    /// <param name="collection">The collection to add to.</param>
    /// <param name="values">The values to add.</param>
    /// <param name="equalityComparer">The <see cref="IEqualityComparer{T}"/> to use for object comparison. If null, the default <see cref="IEqualityComparer{T}"/> will be used.</param>
    /// <returns><see langword="true" /> if the element was added, <see langword="false" /> otherwise.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/> -or- <paramref name="equalityComparer"/> is <see langword="null" /></exception>
    /// <exception cref="NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</exception>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    public static int AddRangeUnique<T>(this ICollection<T> collection, IEnumerable<T>? values, IEqualityComparer<T>? equalityComparer)
    {
        Guard.IsNotNull(collection, nameof(collection));

        if (values is null)
        {
            return 0;
        }

        equalityComparer ??= EqualityComparer<T>.Default;

        int result = 0;

        foreach (var value in values)
        {
            bool exists = false;
            foreach (var element in collection)
            {
                if (equalityComparer.Equals(element, value))
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                collection.Add(value);
                ++result;
            }
        }

        return result;
    }

    /// <summary>
    /// Adds the specified elements to the collection as long as the collection does not already contain the elements.
    /// </summary>
    /// <typeparam name="T">The type of the element to add.</typeparam>
    /// <param name="collection">The collection to add to.</param>
    /// <param name="values">The values to add.</param>
    /// <param name="equalityComparer">The equality comparer function to use for object comparison. If null, the default <see cref="IEqualityComparer{T}"/> is used.</param>
    /// <returns><see langword="true" /> if the element was added, <see langword="false" /> otherwise.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/></exception>
    /// <exception cref="NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</exception>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    public static int AddRangeUnique<T>(this ICollection<T> collection, IEnumerable<T>? values, Func<T, T, bool>? equalityComparer)
    {
        Guard.IsNotNull(collection, nameof(collection));

        if (values is null)
        {
            return 0;
        }

        equalityComparer ??= EqualityComparer<T>.Default.Equals;

        int result = 0;

        foreach (var value in values)
        {
            bool exists = false;
            foreach (var element in collection)
            {
                if (equalityComparer(element, value))
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                collection.Add(value);
                ++result;
            }
        }

        return result;
    }

    /// <summary>
    /// Adds an item to the collection, ensuring the result is sorted using the specified
    /// comparer. If the comparer is <see langword="null" />, a default comparer is used.
    /// </summary>
    /// <typeparam name="T">The type of the element to add.</typeparam>
    /// <param name="collection">The collection to add to.</param>
    /// <param name="item">The item to add to the collection.</param>
    /// <param name="comparer">The comparer to use.</param>
    /// <returns>The index at which the element was added.</returns>
    /// <remarks>
    /// This method will only generate a single event in the case of an <see cref="ObservableCollection{T}"/>.
    /// This method also assumes the collection is already sorted.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/></exception>
    /// <exception cref="NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
    public static int AddSorted<T>(this IList<T> collection, T? item, IComparer<T>? comparer = null)
    {
        Guard.IsNotNull(collection, nameof(collection));

        comparer ??= Comparer<T>.Default;
        int i = 0;
        while (i < collection.Count && comparer.Compare(collection[i], item!) < 0)
        {
            ++i;
        }

        collection.Insert(i, item!);

        return i;
    }

    /// <summary>
    /// Adds the specified element to the collection as long as the collection does not already contain the element.
    /// </summary>
    /// <typeparam name="T">The type of the element to add.</typeparam>
    /// <param name="collection">The collection to add to.</param>
    /// <param name="value">The value to add.</param>
    /// <param name="equalityComparer">The <see cref="IEqualityComparer{T}"/> to use for object comparison.</param>
    /// <returns><see langword="true" /> if the element was added, <see langword="false" /> otherwise.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/></exception>
    /// <exception cref="NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</exception>
    public static bool AddUnique<T>(this ICollection<T> collection, T? value, IEqualityComparer<T>? equalityComparer = null)
    {
        Guard.IsNotNull(collection, nameof(collection));

        if (!collection.Contains(value!, equalityComparer ?? EqualityComparer<T>.Default))
        {
            collection.Add(value!);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Returns this collection wrapped as a <see cref="IReadOnlyCollection{T}"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/></exception>
    public static IReadOnlyCollection<T> AsReadOnly<T>(this IList<T> collection)
    {
        Guard.IsNotNull(collection, nameof(collection));

        return new ReadOnlyCollection<T>(collection);
    }

    /// <summary>
    /// Returns this collection wrapped as a <see cref="IReadOnlyList{T}"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/></exception>
    public static IReadOnlyList<T> AsReadOnlyList<T>(this IList<T> collection)
    {
        Guard.IsNotNull(collection, nameof(collection));

        return new ReadOnlyCollection<T>(collection);
    }

    /// <summary>
    /// Performs an operation on each element of the source enumerable in parallel.
    /// </summary>
    /// <param name="collection">The collection.</param>
    /// <param name="action">The action to perform.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public static Task ForEachParallelAsync<T>(this IEnumerable<T> collection, Func<T, Task> action, CancellationToken cancellationToken = default)
        => ForEachParallelAsync(collection, action, DataflowBlockOptions.Unbounded, null, cancellationToken);

    /// <summary>
    /// Performs an operation on each element of the source enumerable in parallel.
    /// </summary>
    /// <param name="collection">The collection.</param>
    /// <param name="action">The action to perform.</param>
    /// <param name="degreeOfParallelism">The number of elements to operate on at once.</param>
    /// <param name="scheduler">The scheduler to run the actions on.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public static Task ForEachParallelAsync<T>(this IEnumerable<T> collection, Func<T, Task> action, int degreeOfParallelism = DataflowBlockOptions.Unbounded, TaskScheduler? scheduler = null, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(collection, nameof(collection));
        Guard.IsNotNull(action, nameof(action));

        var options = new ExecutionDataflowBlockOptions
        {
            MaxDegreeOfParallelism = degreeOfParallelism,
            CancellationToken = cancellationToken
        };

        if (scheduler is not null)
        {
            options.TaskScheduler = scheduler;
        }

        var block = new ActionBlock<T>(action, options);
        foreach (var element in collection)
        {
            block.Post(element);
        }

        block.Complete();
        return block.Completion;
    }

    /// <summary>
    /// Performs an operation on each element of the source enumerable in parallel.
    /// </summary>
    /// <param name="collection">The collection.</param>
    /// <param name="action">The action to perform.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public static Task ForEachParallelAsync<T>(this IAsyncEnumerable<T> collection, Func<T, Task> action, CancellationToken cancellationToken = default)
        => ForEachParallelAsync(collection, action, DataflowBlockOptions.Unbounded, null, cancellationToken);

    /// <summary>
    /// Performs an operation on each element of the source enumerable in parallel.
    /// </summary>
    /// <param name="collection">The collection.</param>
    /// <param name="action">The action to perform.</param>
    /// <param name="degreeOfParallelism">The number of elements to operate on at once.</param>
    /// <param name="scheduler">The scheduler to run the actions on.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public static async Task ForEachParallelAsync<T>(this IAsyncEnumerable<T> collection, Func<T, Task> action, int degreeOfParallelism = DataflowBlockOptions.Unbounded, TaskScheduler? scheduler = null, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(collection, nameof(collection));
        Guard.IsNotNull(action, nameof(action));

        var options = new ExecutionDataflowBlockOptions
        {
            MaxDegreeOfParallelism = degreeOfParallelism,
            CancellationToken = cancellationToken
        };

        if (scheduler is not null)
        {
            options.TaskScheduler = scheduler;
        }

        var block = new ActionBlock<T>(action, options);
        await foreach (var element in collection)
        {
            block.Post(element);
        }

        block.Complete();
        await block.Completion;
    }

    /// <summary>
    /// Returns the index of the first occurrence of specified string element using the specified search rules.
    /// </summary>
    /// <param name="collection">The collection to search.</param>
    /// <param name="element">The element to find the index of.</param>
    /// <param name="comparisonType">The comparison rules to use.</param>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/></exception>
    public static int IndexOf(this IList<string?> collection, string? element, StringComparison comparisonType = StringComparison.Ordinal)
    {
        Guard.IsNotNull(collection, nameof(collection));

        for (int i = 0; i < collection.Count; ++i)
        {
            if (string.Equals(collection[i], element, comparisonType))
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Shuffles a collection using a Fisher Yates shuffle.
    /// </summary>
    /// <typeparam name="T">The type of the element to add.</typeparam>
    /// <param name="collection">The collection to shuffle.</param>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/></exception>
    /// <exception cref="NotSupportedException"><paramref name="collection"/> is read-only.</exception>
    public static void Shuffle<T>(this IList<T> collection)
    {
        Guard.IsNotNull(collection, nameof(collection));

        if (collection.IsReadOnly)
        {
            throw new NotSupportedException($"{nameof(collection)} is read-only.");
        }

        int count = collection.Count;
        for (int i = count - 1; i > 0; --i)
        {
            // We use i instead of i-1 since our RNG interface dictates an inclusive upper.
            int randomIndex = _randomNumberGenerator.NextInt32(i);
            (collection[i], collection[randomIndex]) = (collection[randomIndex], collection[i]);
        }
    }
}
