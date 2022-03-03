using Microsoft.Toolkit.Diagnostics;

namespace AniNexus.Collections;

/// <summary>
/// A wrapper around a <see cref="Span{T}"/> that treats it like a 2D array.
/// </summary>
/// <typeparam name="T">The underlying type contained within the <see cref="Span{T}"/>.</typeparam>
public ref struct Array2D<T>
{
    /// <summary>
    /// The width of the 2D array.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// The height of the 2D array.
    /// </summary>
    public int Height { get; }

    private readonly Span<T> _underlyingArray;

    /// <summary>
    /// Creates a new 2D array.
    /// </summary>
    /// <param name="span">The buffer.</param>
    /// <param name="width">The width of the array.</param>
    /// <param name="height">The height of the array.</param>
    /// <exception cref="ArgumentException">The length of the span does not equal <paramref name="width"/> * <paramref name="height"/>.</exception>
    public Array2D(Span<T> span, int width, int height)
    {
        Guard.IsTrue(span.Length == width * height, "Span length does not equal width * height.");

        _underlyingArray = span;
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Returns the element at the specified index.
    /// </summary>
    /// <param name="x">The X index.</param>
    /// <param name="y">The Y index.</param>
    public T this[int x, int y]
    {
        get => _underlyingArray[y * Width + x];
        set => _underlyingArray[y * Width + x] = value;
    }
}
