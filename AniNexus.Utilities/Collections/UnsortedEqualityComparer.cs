﻿namespace AniNexus.Collections;

/// <summary>
/// Compares equality of two unsorted collections.
/// </summary>
public static class UnsortedEqualityComparer
{
    /// <summary>
    /// Compares equality of two unsorted collections.
    /// </summary>
    /// <param name="a">The first collection.</param>
    /// <param name="b">The second collection.</param>
    public static bool Equals<T>(IEnumerable<T?>? a, IEnumerable<T?>? b)
    {
        return UnsortedEqualityComparer<T?>.Default.Equals(a, b);
    }

    /// <summary>
    /// Compares equality of two unsorted collections.
    /// </summary>
    /// <param name="a">The first collection.</param>
    /// <param name="b">The second collection.</param>
    /// <param name="equalityComparer">The equality comparer to use.</param>
    public static bool Equals<T>(IEnumerable<T?>? a, IEnumerable<T?>? b, IEqualityComparer<T?>? equalityComparer)
    {
        return new UnsortedEqualityComparer<T?>(equalityComparer).Equals(a, b);
    }
}

/// <summary>
/// Compares equality of two unsorted collections.
/// </summary>
/// <typeparam name="T"></typeparam>
public class UnsortedEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
{
    /// <summary>
    /// An <see cref="IEqualityComparer{T}"/> that utilizes <see cref="EqualityComparer{T}.Default"/>
    /// for unsorted equality comparisons.
    /// </summary>
    public static UnsortedEqualityComparer<T?> Default { get; } = new UnsortedEqualityComparer<T?>(EqualityComparer<T?>.Default);

    private readonly IEqualityComparer<T?> _comparer;

    /// <summary>
    /// Creates a new <see cref="UnsortedEqualityComparer{T}"/> instance.
    /// </summary>
    /// <param name="comparer">The comparer to use.</param>
    public UnsortedEqualityComparer(IEqualityComparer<T?>? comparer = null)
    {
        _comparer = comparer ?? EqualityComparer<T?>.Default;
    }

    /// <summary>
    /// Determines whether the specified objects are equal.
    /// </summary>
    /// <param name="x">The first object of type <typeparamref name="T"/> to compare.</param>
    /// <param name="y">The second object of type <typeparamref name="T"/> to compare.</param>
    /// <returns>
    /// <see langword="true" /> if the specified objects are equal; otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(IEnumerable<T?>? x, IEnumerable<T?>? y)
    {
        if (x is null)
        {
            return y is null;
        }

        if (y is null)
        {
            return false;
        }

        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x is ICollection<T?> firstCollection && y is ICollection<T?> secondCollection)
        {
            if (firstCollection.Count != secondCollection.Count)
            {
                return false;
            }

            if (firstCollection.Count == 0)
            {
                return true;
            }
        }

        return !HaveMismatchedElement(x, y);
    }

    private bool HaveMismatchedElement(IEnumerable<T?> first, IEnumerable<T?> second)
    {
        var firstElementCounts = GetElementCounts(first, out int firstNullCount);
        var secondElementCounts = GetElementCounts(second, out int secondNullCount);

        if (firstNullCount != secondNullCount || firstElementCounts.Count != secondElementCounts.Count)
        {
            return true;
        }

        if (firstElementCounts.Count == 0 && secondElementCounts.Count == 0 && firstNullCount == secondNullCount)
        {
            return false;
        }

        foreach (var kvp in firstElementCounts)
        {
            int firstElementCount = kvp.Value;
            if (!secondElementCounts.TryGetValue(kvp.Key, out int secondElementCount))
            {
                // The hash code has not been overridden, so similar keys may be considered
                // not-equal. Try again using a key defined by equality.
                try
                {
                    KeyValuePair<T, int>? newKey = null;

                    // This is a hot path, so LINQ allocations are not desired.
                    // newKey = secondElementCounts.Single(e => e.Key == kvp.Key);
                    using (var e = secondElementCounts.GetEnumerator())
                    {
                        while (e.MoveNext())
                        {
                            var result = e.Current;
                            if (_comparer.Equals(result.Key, kvp.Key))
                            {
                                bool dupFound = false;
                                while (e.MoveNext())
                                {
                                    if (_comparer.Equals(e.Current.Key, kvp.Key))
                                    {
                                        dupFound = true;
                                        break;
                                    }
                                }

                                if (dupFound)
                                {
                                    break;
                                }

                                newKey = result;
                            }
                        }
                    }

                    if (newKey.HasValue)
                    {
                        secondElementCounts.TryGetValue(newKey.Value.Key, out secondElementCount);
                    }
                    else
                    {
                        secondElementCount = 0;
                    }
                }
                catch
                {
                    secondElementCount = 0;
                }
            }

            if (firstElementCount != secondElementCount)
            {
                return true;
            }
        }

        return false;
    }

    // Suppression of CS8714 - null keys are excluded from the dictionary.
#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
    private Dictionary<T, int> GetElementCounts(IEnumerable<T?> enumerable, out int nullCount)
    {
        var dictionary = new Dictionary<T, int>(_comparer);
        nullCount = 0;

        foreach (var element in enumerable)
        {
            if (element is null)
            {
                ++nullCount;
            }
            else
            {
                dictionary.TryGetValue(element, out int num);
                ++num;
                dictionary[element] = num;
            }
        }

        return dictionary;
    }
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.

    /// <summary>
    /// Returns the hash code of the enumerable.
    /// </summary>
    /// <param name="enumerable">The enumerable.</param>
    public int GetHashCode(IEnumerable<T?>? enumerable)
    {
        if (enumerable is null)
        {
            return 0;
        }

        var hashCode = new HashCode();
        foreach (var element in enumerable.OrderBy(static x => x))
        {
            hashCode.Add(element);
        }

        return hashCode.ToHashCode();
    }
}
