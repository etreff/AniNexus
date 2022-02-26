﻿using System.Collections;

namespace AniNexus;

/// <summary>
/// Allows an <see cref="IComparer{T}"/> to behave as an <see cref="IComparer"/>.
/// </summary>
public static class ComparerBridge
{
    /// <summary>
    /// Creates a <see cref="ComparerBridge{T}"/> using the provider <see cref="IComparer{T}"/>.
    /// </summary>
    /// <typeparam name="T">The underlying type being compared.</typeparam>
    /// <param name="comparer">The comparer to use.</param>
    /// <returns>An <see cref="IComparer"/> using <paramref name="comparer"/> as the underlying comparer.</returns>
    public static IComparer Create<T>(IComparer<T>? comparer)
    {
        return new ComparerBridge<T>(comparer);
    }
}

/// <summary>
/// Allows an <see cref="IComparer{T}"/> to behave as an <see cref="IComparer"/>.
/// </summary>
/// <typeparam name="T">The underlying type being compared.</typeparam>
public class ComparerBridge<T> : IComparer<T>, IComparer
{
    private readonly IComparer<T> _comparer;

    /// <inheritdoc />
    public ComparerBridge()
        : this(null)
    {
    }

    /// <inheritdoc />
    public ComparerBridge(IComparer<T>? comparer)
    {
        _comparer = comparer ?? Comparer<T>.Default;
    }

    /// <inheritdoc />
    public int Compare(T? x, T? y)
    {
        return _comparer.Compare(x!, y!);
    }

    /// <inheritdoc />
    public int Compare(object? x, object? y)
    {
        return _comparer.Compare(x is not null ? (T)x : default, y is not null ? (T)y : default);
    }
}
