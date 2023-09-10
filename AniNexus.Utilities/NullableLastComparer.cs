namespace AniNexus;

/// <summary>
/// An <see cref="IComparer{T}"/> that sorts <see langword="null"/> elements last.
/// </summary>
public sealed class NullableLastComparer<T> : IComparer<T>
{
    private readonly IComparer<T> _comparer;

    /// <summary>
    /// Creates a new <see cref="NullableLastComparer{T}"/> instance.
    /// </summary>
    /// <param name="comparer">The comparer to use for non-<see langword="null"/> elements.</param>
    public NullableLastComparer(IComparer<T>? comparer = null)
    {
        _comparer = comparer ?? Comparer<T>.Default;
    }

    /// <inheritdoc />
    public int Compare(T? x, T? y)
    {
        if (x == null)
        {
            return 1;
        }

        if (y == null)
        {
            return -1;
        }

        return _comparer.Compare(x, y);
    }
}
